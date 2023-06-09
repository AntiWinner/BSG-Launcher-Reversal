using System.Buffers;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Eft.Launcher.Base.Network.Http;

public class JsonContent : HttpContent
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public byte[] _E000;

		internal void _E000(Task t)
		{
			_E005.Return(this._E000);
		}
	}

	private static readonly ArrayPool<byte> _E005 = ArrayPool<byte>.Shared;

	[CompilerGenerated]
	private readonly JContainer _E006;

	private readonly HttpContent _E007;

	public JContainer Json
	{
		[CompilerGenerated]
		get
		{
			return _E006;
		}
	}

	public JsonContent(JContainer json)
	{
		_E006 = json;
	}

	public JsonContent(HttpContent httpContent)
	{
		_E007 = httpContent;
		_E006 = JObject.Parse(httpContent.ReadAsStringAsync().Result);
	}

	internal Task _E000(Stream stream, TransportContext context)
	{
		if (_E007 != null)
		{
			return _E007.CopyToAsync(stream, context);
		}
		string text = Json.ToString(Formatting.None);
		byte[] array = _E005.Rent(Encoding.UTF8.GetMaxByteCount(text.Length));
		int bytes = Encoding.UTF8.GetBytes(text, 0, text.Length, array, 0);
		return stream.WriteAsync(array, 0, bytes).ContinueWith(delegate
		{
			_E005.Return(array);
		});
	}

	internal bool _E000(out long length)
	{
		length = 0L;
		return false;
	}

	protected override Task SerializeToStreamAsync(Stream stream, TransportContext context)
	{
		return _E000(stream, context);
	}

	protected override bool TryComputeLength(out long length)
	{
		return _E000(out length);
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && _E007 != null)
		{
			_E007.Dispose();
		}
		base.Dispose(disposing);
	}
}
