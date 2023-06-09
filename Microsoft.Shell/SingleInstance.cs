using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Ipc;
using System.Runtime.Serialization.Formatters;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace Microsoft.Shell;

public static class SingleInstance<TApplication> where TApplication : Application, ISingleInstanceApp
{
	private class _E000 : MarshalByRefObject
	{
		public void _E000(IList<string> args)
		{
			if (Application.Current != null)
			{
				Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new DispatcherOperationCallback(SingleInstance<TApplication>._E000), args);
			}
		}

		public override object InitializeLifetimeService()
		{
			return null;
		}
	}

	private const string m__E000 = ":";

	private const string _E001 = "SingeInstanceIPCChannel";

	private const string _E002 = "SingleInstanceApplicationService";

	private const string _E003 = "ipc://";

	private static Mutex _E004;

	private static IpcServerChannel _E005;

	private static IList<string> _E006;

	public static IList<string> CommandLineArgs => _E006;

	public static bool InitializeAsFirstInstance(string uniqueName)
	{
		_E006 = SingleInstance<TApplication>._E000(uniqueName);
		string text = uniqueName + Environment.UserName;
		string channelName = text + _E05B._E000(26481) + _E05B._E000(26487);
		_E004 = new Mutex(initiallyOwned: true, text, out var createdNew);
		if (createdNew)
		{
			SingleInstance<TApplication>._E000(channelName);
		}
		else
		{
			_E000(channelName, _E006);
		}
		return createdNew;
	}

	public static void Cleanup()
	{
		if (_E004 != null)
		{
			_E004.Close();
			_E004 = null;
		}
		if (_E005 != null)
		{
			ChannelServices.UnregisterChannel(_E005);
			_E005 = null;
		}
	}

	private static IList<string> _E000(string uniqueApplicationName)
	{
		string[] array = null;
		if (AppDomain.CurrentDomain.ActivationContext == null)
		{
			array = Environment.GetCommandLineArgs();
		}
		else
		{
			string path = Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), uniqueApplicationName), _E05B._E000(26447));
			if (File.Exists(path))
			{
				try
				{
					using (TextReader textReader = new StreamReader(path, Encoding.Unicode))
					{
						array = _E007._E000(textReader.ReadToEnd());
					}
					File.Delete(path);
				}
				catch (IOException)
				{
				}
			}
		}
		if (array == null)
		{
			array = new string[0];
		}
		return new List<string>(array);
	}

	private static void _E000(string channelName)
	{
		BinaryServerFormatterSinkProvider sinkProvider = new BinaryServerFormatterSinkProvider
		{
			TypeFilterLevel = TypeFilterLevel.Full
		};
		_E005 = new IpcServerChannel(new Dictionary<string, string>
		{
			{
				_E05B._E000(26459),
				channelName
			},
			{
				_E05B._E000(26462),
				channelName
			},
			{
				_E05B._E000(26533),
				_E05B._E000(26553)
			}
		}, sinkProvider);
		ChannelServices.RegisterChannel(_E005, ensureSecurity: true);
		RemotingServices.Marshal(new _E000(), _E05B._E000(26499));
	}

	private static void _E000(string channelName, IList<string> args)
	{
		ChannelServices.RegisterChannel(new IpcClientChannel(), ensureSecurity: true);
		string url = _E05B._E000(26594) + channelName + _E05B._E000(26603);
		((_E000)RemotingServices.Connect(typeof(_E000), url))?._E000(args);
	}

	private static object _E000(object arg)
	{
		_E000(arg as IList<string>);
		return null;
	}

	private static void _E000(IList<string> args)
	{
		if (Application.Current != null)
		{
			((TApplication)Application.Current).SignalExternalCommandLineArgs(args);
		}
	}
}
