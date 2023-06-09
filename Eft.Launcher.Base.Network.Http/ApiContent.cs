using System;
using System.IO;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Eft.Launcher.Base.Network.Http;

public class ApiContent : JsonContent
{
	[CompilerGenerated]
	private new readonly int _E000;

	[CompilerGenerated]
	private readonly string _E001;

	[CompilerGenerated]
	private readonly JToken _E002;

	[CompilerGenerated]
	private readonly JToken _E003;

	private readonly JsonContent _E004;

	public int Code
	{
		[CompilerGenerated]
		get
		{
			return _E000;
		}
	}

	public string Message
	{
		[CompilerGenerated]
		get
		{
			return _E001;
		}
	}

	public JToken Data
	{
		[CompilerGenerated]
		get
		{
			return _E002;
		}
	}

	public JToken Args
	{
		[CompilerGenerated]
		get
		{
			return _E003;
		}
	}

	internal ApiContent(int code, string message, JToken data, JToken args, JsonContent jsonContent)
		: base(jsonContent.Json)
	{
		_E004 = jsonContent ?? throw new ArgumentNullException(_E05B._E000(15667));
		_E000 = code;
		_E001 = message;
		_E002 = data;
		_E003 = args;
	}

	protected override Task SerializeToStreamAsync(Stream stream, TransportContext context)
	{
		return _E004._E000(stream, context);
	}

	protected override bool TryComputeLength(out long length)
	{
		return _E004._E000(out length);
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			_E004.Dispose();
		}
		base.Dispose(disposing);
	}
}
