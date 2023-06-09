using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Serialization;

namespace Bsg.Launcher.WebSockets;

public class WebSocketDefaultSerializationBinder : ISerializationBinder
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public WebSocketDefaultSerializationBinder _E000;

		public string[] _E001;

		internal bool _E000(Type t)
		{
			_E001 CS_0024_003C_003E8__locals0 = new _E001
			{
				_E000 = t
			};
			if (CS_0024_003C_003E8__locals0._E000.IsClass && CS_0024_003C_003E8__locals0._E000.IsVisible && CS_0024_003C_003E8__locals0._E000.Namespace != null && (!CS_0024_003C_003E8__locals0._E000.IsAbstract || !CS_0024_003C_003E8__locals0._E000.IsSealed) && CS_0024_003C_003E8__locals0._E000.Name != this._E000.m__E001)
			{
				return _E001.Any((string n) => CS_0024_003C_003E8__locals0._E000.Namespace.StartsWith(n));
			}
			return false;
		}

		internal string _E000(Type t)
		{
			return t.Name.Replace(this._E000.m__E001, "").ToLowerInvariant();
		}
	}

	[CompilerGenerated]
	private sealed class _E001
	{
		public Type _E000;

		internal bool _E000(string n)
		{
			return this._E000.Namespace.StartsWith(n);
		}
	}

	private readonly Dictionary<string, Type> m__E000;

	private readonly string m__E001;

	public WebSocketDefaultSerializationBinder(string[] messageNamespaces, string postfix)
	{
		WebSocketDefaultSerializationBinder webSocketDefaultSerializationBinder = this;
		if (messageNamespaces == null)
		{
			throw new ArgumentNullException(_E05B._E000(3667));
		}
		this.m__E001 = postfix;
		this.m__E000 = (from t in AppDomain.CurrentDomain.GetAssemblies().SelectMany((Assembly asm) => asm.GetTypes())
			where t.IsClass && t.IsVisible && t.Namespace != null && (!t.IsAbstract || !t.IsSealed) && t.Name != webSocketDefaultSerializationBinder.m__E001 && messageNamespaces.Any((string n) => t.Namespace.StartsWith(n))
			select t).ToDictionary((Type t) => t.Name.Replace(webSocketDefaultSerializationBinder.m__E001, "").ToLowerInvariant());
	}

	public void BindToName(Type serializedType, out string assemblyName, out string typeName)
	{
		assemblyName = null;
		string name = serializedType.Name;
		typeName = ((name != null && name.Length > 0) ? (serializedType.Name.First().ToString().ToLowerInvariant() + serializedType.Name.Substring(1).Replace(this.m__E001, "")) : "");
	}

	public Type BindToType(string assemblyName, string typeName)
	{
		if (!this.m__E000.TryGetValue(typeName.ToLowerInvariant(), out var value))
		{
			throw new Exception(_E05B._E000(3745) + typeName + _E05B._E000(27975));
		}
		return value;
	}
}
