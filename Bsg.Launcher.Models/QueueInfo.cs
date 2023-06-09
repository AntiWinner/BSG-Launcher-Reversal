using System;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;

namespace Bsg.Launcher.Models;

public class QueueInfo
{
	[CompilerGenerated]
	private int _E000;

	[CompilerGenerated]
	private Uri _E001;

	[CompilerGenerated]
	private Uri _E002;

	[JsonProperty("position")]
	public int Position
	{
		[CompilerGenerated]
		get
		{
			return _E000;
		}
		[CompilerGenerated]
		set
		{
			_E000 = value;
		}
	}

	[JsonProperty("url_private")]
	public Uri PrivateUrl
	{
		[CompilerGenerated]
		get
		{
			return _E001;
		}
		[CompilerGenerated]
		set
		{
			_E001 = value;
		}
	}

	[JsonProperty("url_public")]
	public Uri PublicUrl
	{
		[CompilerGenerated]
		get
		{
			return _E002;
		}
		[CompilerGenerated]
		set
		{
			_E002 = value;
		}
	}
}
