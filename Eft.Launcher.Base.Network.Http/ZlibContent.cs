using System.IO;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Ionic.Zlib;

namespace Eft.Launcher.Base.Network.Http;

public class ZlibContent : HttpContent
{
	[CompilerGenerated]
	private readonly HttpContent _E000;

	private readonly CompressionMode _E001;

	public HttpContent OriginalContent
	{
		[CompilerGenerated]
		get
		{
			return this._E000;
		}
	}

	public ZlibContent(HttpContent originalContent, CompressionMode compressionMode)
	{
		this._E000 = originalContent;
		_E001 = compressionMode;
	}

	protected override async Task SerializeToStreamAsync(Stream stream, TransportContext context)
	{
		if (_E001 == CompressionMode.Compress)
		{
			using (ZlibStream stream2 = new ZlibStream(stream, _E001, leaveOpen: true))
			{
				await OriginalContent.CopyToAsync(stream2);
			}
			return;
		}
		using ZlibStream zlibStream = new ZlibStream(await OriginalContent.ReadAsStreamAsync(), _E001);
		try
		{
			zlibStream.CopyTo(stream);
		}
		catch (ZlibException innerException)
		{
			throw new ZlibContentException(_E05B._E000(15847), innerException);
		}
	}

	protected override bool TryComputeLength(out long length)
	{
		length = 0L;
		return false;
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			OriginalContent.Dispose();
		}
		base.Dispose(disposing);
	}
}
