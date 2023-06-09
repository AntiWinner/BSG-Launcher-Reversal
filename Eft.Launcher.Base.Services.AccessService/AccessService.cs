using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using Eft.Launcher.Services.AccessService;
using Eft.Launcher.Services.SettingsService;
using Microsoft.Extensions.Logging;
using Microsoft.Win32;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Eft.Launcher.Base.Services.AccessService;

public sealed class AccessService : IAccessService, IDisposable
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public AccessService _E000;

		public string _E001;

		internal bool _E000()
		{
			try
			{
				this._E000._E000();
			}
			catch
			{
				return false;
			}
			try
			{
				this._E000.m__E000.LogDebug(_E05B._E000(16303), _E001);
				string outString = new JObject(new JProperty(_E05B._E000(16367), new JObject(new JProperty(_E05B._E000(16321), _E001)))).ToString(Formatting.None);
				this._E000._E000(outString);
				JToken jToken = JObject.Parse(this._E000._E000())[_E05B._E000(16327)];
				string text = jToken[_E05B._E000(16321)].Value<string>();
				if (jToken[_E05B._E000(15905)].Value<bool>() && _E001 == text)
				{
					this._E000.m__E000.LogDebug(_E05B._E000(15911), _E001);
					return true;
				}
				this._E000.m__E000.LogWarning(_E05B._E000(15981), _E001);
				return false;
			}
			catch (Exception exception)
			{
				this._E000.m__E000.LogError(exception, _E05B._E000(16050));
				throw;
			}
		}
	}

	[CompilerGenerated]
	private sealed class _E001
	{
		public AccessService _E000;

		public string _E001;

		public string _E002;

		public string _E003;

		internal int _E000()
		{
			this._E000._E000();
			string outString = new JObject { 
			{
				_E05B._E000(16014),
				new JObject
				{
					{
						_E05B._E000(16025),
						_E001
					},
					{
						_E05B._E000(16096),
						_E002
					},
					{
						_E05B._E000(16110),
						_E003
					}
				}
			} }.ToString(Formatting.None);
			this._E000._E000(outString);
			return JObject.Parse(this._E000._E000())[_E05B._E000(16125)].Value<int>(_E05B._E000(12546));
		}
	}

	private readonly ILogger m__E000;

	private Process m__E001;

	private NamedPipeServerStream _E002;

	private readonly UTF8Encoding _E003;

	private readonly object _E004 = new object();

	public AccessService(ILogger<AccessService> logger)
	{
		this.m__E000 = logger;
		_E003 = new UTF8Encoding();
	}

	public void AddEntryToRegistry(string subkey, string name, object value, RegistryValueKind valueKind)
	{
		this._E000();
		string outString = new JObject(new JProperty(_E05B._E000(12337), new JObject(new JProperty(_E05B._E000(12349), subkey), new JProperty(_E05B._E000(12290), name), new JProperty(_E05B._E000(12288), value), new JProperty(_E05B._E000(12294), (int)valueKind)))).ToString(Formatting.None);
		_E000(outString);
	}

	public void ModifyRegistryEntry(string subkey, string name, object value, RegistryValueKind valueKind)
	{
		this._E000();
		string outString = new JObject(new JProperty(_E05B._E000(12299), new JObject(new JProperty(_E05B._E000(12349), subkey), new JProperty(_E05B._E000(12290), name), new JProperty(_E05B._E000(12288), value), new JProperty(_E05B._E000(12294), (int)valueKind)))).ToString(Formatting.None);
		_E000(outString);
	}

	public void DeleteSectionFromRegistry(string subkey)
	{
		this._E000();
		string outString = new JObject(new JProperty(_E05B._E000(12308), new JObject(new JProperty(_E05B._E000(12349), subkey)))).ToString(Formatting.None);
		_E000(outString);
	}

	public Task<bool> AssignFullPermissions(string directory)
	{
		return Task.Run(delegate
		{
			try
			{
				this._E000();
			}
			catch
			{
				return false;
			}
			try
			{
				this.m__E000.LogDebug(_E05B._E000(16303), directory);
				string outString = new JObject(new JProperty(_E05B._E000(16367), new JObject(new JProperty(_E05B._E000(16321), directory)))).ToString(Formatting.None);
				_E000(outString);
				JToken jToken = JObject.Parse(this._E000())[_E05B._E000(16327)];
				string text = jToken[_E05B._E000(16321)].Value<string>();
				if (jToken[_E05B._E000(15905)].Value<bool>() && directory == text)
				{
					this.m__E000.LogDebug(_E05B._E000(15911), directory);
					return true;
				}
				this.m__E000.LogWarning(_E05B._E000(15981), directory);
				return false;
			}
			catch (Exception exception)
			{
				this.m__E000.LogError(exception, _E05B._E000(16050));
				throw;
			}
		});
	}

	public bool CheckPermissions(string folderPath)
	{
		string text = Path.Combine(folderPath, Guid.NewGuid().ToString());
		string path = Path.Combine(text, Guid.NewGuid().ToString());
		try
		{
			if (!Directory.Exists(text))
			{
				Directory.CreateDirectory(text);
			}
			using (new FileStream(path, FileMode.Create, FileSystemRights.FullControl, FileShare.None, 4096, FileOptions.DeleteOnClose))
			{
				return true;
			}
		}
		catch
		{
			return false;
		}
		finally
		{
			if (Directory.Exists(text))
			{
				Directory.Delete(text, recursive: true);
			}
		}
	}

	public Task<int> RunProcess(string fileName, string arguments, string workingDirectory)
	{
		return Task.Run(delegate
		{
			this._E000();
			string outString = new JObject { 
			{
				_E05B._E000(16014),
				new JObject
				{
					{
						_E05B._E000(16025),
						fileName
					},
					{
						_E05B._E000(16096),
						arguments
					},
					{
						_E05B._E000(16110),
						workingDirectory
					}
				}
			} }.ToString(Formatting.None);
			_E000(outString);
			return JObject.Parse(this._E000())[_E05B._E000(16125)].Value<int>(_E05B._E000(12546));
		});
	}

	public void StopService()
	{
		_E001();
	}

	private void _E000()
	{
		try
		{
			lock (_E004)
			{
				if (this.m__E001 == null)
				{
					this.m__E000.LogDebug(_E05B._E000(12384));
					_E002 = new NamedPipeServerStream(AppConfig.Instance.SubfolderName, PipeDirection.InOut);
					this.m__E001 = new Process();
					this.m__E001.StartInfo.FileName = Assembly.GetEntryAssembly().Location;
					this.m__E001.StartInfo.Verb = _E05B._E000(12365);
					this.m__E001.StartInfo.Arguments = _E05B._E000(25585);
					this.m__E001.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
					this.m__E001.Exited += delegate
					{
						_E001();
					};
					this.m__E001.EnableRaisingEvents = true;
					if (!this.m__E001.Start())
					{
						throw new Exception(_E05B._E000(12375));
					}
					_E002.WaitForConnection();
				}
			}
		}
		catch (Win32Exception exception)
		{
			this.m__E000.LogWarning(exception, _E05B._E000(12469));
			_E001();
			throw;
		}
		catch (Exception exception2)
		{
			_E001();
			this.m__E000.LogError(exception2, _E05B._E000(12526));
			throw;
		}
	}

	private void _E001()
	{
		lock (_E004)
		{
			try
			{
				if (_E002 != null)
				{
					if (_E002.IsConnected)
					{
						try
						{
							_E000(_E05B._E000(12501));
						}
						catch (IOException)
						{
						}
						_E002.Close();
					}
					_E002 = null;
				}
				if (this.m__E001 != null)
				{
					try
					{
						this.m__E001.Kill();
					}
					catch (Win32Exception ex2)
					{
						this.m__E000.LogWarning(_E05B._E000(12508), ex2.NativeErrorCode);
					}
					catch (InvalidOperationException)
					{
					}
					this.m__E001 = null;
				}
			}
			catch (Exception exception)
			{
				this.m__E000.LogError(exception, _E05B._E000(16229));
				throw;
			}
		}
	}

	private string _E000()
	{
		byte[] array = new byte[4];
		if (_E002.Read(array, 0, array.Length) != array.Length)
		{
			throw new Exception(_E05B._E000(16202));
		}
		int num = BitConverter.ToInt32(array, 0);
		byte[] array2 = new byte[num];
		_E002.Read(array2, 0, num);
		return _E003.GetString(array2);
	}

	private int _E000(string outString)
	{
		byte[] bytes = _E003.GetBytes(outString);
		int num = bytes.Length;
		byte[] bytes2 = BitConverter.GetBytes(num);
		_E002.Write(bytes2, 0, bytes2.Length);
		_E002.Write(bytes, 0, num);
		_E002.Flush();
		return bytes2.Length + num;
	}

	public void Dispose()
	{
		_E001();
	}

	[CompilerGenerated]
	private void _E000(object sender, EventArgs e)
	{
		_E001();
	}
}
