using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using Eft.Launcher.Services.SettingsService;
using Microsoft.Win32;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Eft.Launcher.Services.AccessService;

public static class AccessProvider
{
	private static NamedPipeClientStream m__E000;

	private static readonly UTF8Encoding m__E001 = new UTF8Encoding();

	private static readonly Dictionary<string, Action<JToken>> m__E002 = new Dictionary<string, Action<JToken>>
	{
		{
			_E05B._E000(12337),
			_E000
		},
		{
			_E05B._E000(12299),
			_E001
		},
		{
			_E05B._E000(12308),
			_E002
		},
		{
			_E05B._E000(16367),
			_E003
		},
		{
			_E05B._E000(16014),
			_E004
		},
		{
			_E05B._E000(12501),
			_E005
		}
	};

	public static int Main(string[] args)
	{
		if (!args.Contains(_E05B._E000(25585)))
		{
			return 0;
		}
		try
		{
			AccessProvider.m__E000 = new NamedPipeClientStream(_E05B._E000(27969), AppConfig.Instance.SubfolderName, PipeDirection.InOut, PipeOptions.None, TokenImpersonationLevel.Impersonation);
			Console.WriteLine(_E05B._E000(61143));
			AccessProvider.m__E000.Connect();
			while (AccessProvider.m__E000 != null && AccessProvider.m__E000.IsConnected)
			{
				Console.WriteLine(_E05B._E000(60718));
				string text = _E000();
				JObject jObject = JObject.Parse(text);
				string name = (jObject.First as JProperty).Name;
				JToken value = (jObject.First as JProperty).Value;
				if (AccessProvider.m__E002.TryGetValue(name, out var value2))
				{
					value2(value);
					continue;
				}
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine(_E05B._E000(60683) + text);
				Console.ResetColor();
			}
		}
		catch
		{
			return 1;
		}
		return 0;
	}

	private static void _E000(JToken json)
	{
		string text = json.Value<string>(_E05B._E000(12349));
		string name = json.Value<string>(_E05B._E000(12290));
		object value = json.Value<object>(_E05B._E000(12288));
		RegistryValueKind valueKind = (RegistryValueKind)json.Value<int>(_E05B._E000(12294));
		RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(text, writable: true);
		if (registryKey == null)
		{
			registryKey = Registry.LocalMachine.CreateSubKey(text);
		}
		registryKey.SetValue(name, value, valueKind);
	}

	private static void _E001(JToken json)
	{
		string name = json.Value<string>(_E05B._E000(12349));
		string name2 = json.Value<string>(_E05B._E000(12290));
		object value = json.Value<object>(_E05B._E000(12288));
		RegistryValueKind valueKind = (RegistryValueKind)json.Value<int>(_E05B._E000(12294));
		using RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(name, writable: true);
		registryKey?.SetValue(name2, value, valueKind);
	}

	private static void _E002(JToken json)
	{
		string subkey = json.Value<string>(_E05B._E000(12349));
		Registry.LocalMachine.DeleteSubKeyTree(subkey, throwOnMissingSubKey: false);
	}

	private static void _E003(JToken json)
	{
		string text = "";
		bool flag = false;
		try
		{
			text = json.Value<string>(_E05B._E000(16321));
			Directory.CreateDirectory(text);
			DirectoryInfo directoryInfo = new DirectoryInfo(text);
			DirectorySecurity accessControl = directoryInfo.GetAccessControl();
			FileSystemAccessRule accessRule = new FileSystemAccessRule(new SecurityIdentifier(WellKnownSidType.BuiltinUsersSid, null), FileSystemRights.FullControl, InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow);
			accessControl.SetAccessRule(accessRule);
			directoryInfo.SetAccessControl(accessControl);
			flag = true;
		}
		catch (Exception ex)
		{
			Console.Error.WriteLine(_E05B._E000(60697) + ex.Message);
		}
		_E000(new JObject { 
		{
			_E05B._E000(16327),
			new JObject
			{
				{
					_E05B._E000(16321),
					text
				},
				{
					_E05B._E000(15905),
					flag
				}
			}
		} }.ToString(Formatting.None));
	}

	private static void _E004(JToken json)
	{
		Process process = Process.Start(new ProcessStartInfo
		{
			FileName = json.Value<string>(_E05B._E000(16025)),
			Arguments = json.Value<string>(_E05B._E000(16096)),
			WorkingDirectory = json.Value<string>(_E05B._E000(16110)),
			Verb = _E05B._E000(12365)
		});
		process.WaitForExit();
		_E000(new JObject { 
		{
			_E05B._E000(16125),
			new JObject { 
			{
				_E05B._E000(12546),
				process.ExitCode
			} }
		} }.ToString(Formatting.None));
	}

	private static void _E005(JToken json)
	{
		if (AccessProvider.m__E000 != null)
		{
			if (AccessProvider.m__E000.IsConnected)
			{
				AccessProvider.m__E000.Close();
			}
			AccessProvider.m__E000 = null;
		}
	}

	private static string _E000()
	{
		byte[] array = new byte[4];
		if (AccessProvider.m__E000.Read(array, 0, array.Length) != array.Length)
		{
			throw new Exception(_E05B._E000(16202));
		}
		int num = BitConverter.ToInt32(array, 0);
		byte[] array2 = new byte[num];
		AccessProvider.m__E000.Read(array2, 0, num);
		string @string = AccessProvider.m__E001.GetString(array2);
		Console.ForegroundColor = ConsoleColor.Cyan;
		Console.WriteLine(@string);
		Console.ResetColor();
		return @string;
	}

	private static int _E000(string outString)
	{
		byte[] bytes = AccessProvider.m__E001.GetBytes(outString);
		int num = bytes.Length;
		byte[] bytes2 = BitConverter.GetBytes(num);
		AccessProvider.m__E000.Write(bytes2, 0, bytes2.Length);
		AccessProvider.m__E000.Write(bytes, 0, num);
		AccessProvider.m__E000.Flush();
		Console.ForegroundColor = ConsoleColor.Magenta;
		Console.WriteLine(outString);
		Console.ResetColor();
		return bytes2.Length + num;
	}
}
