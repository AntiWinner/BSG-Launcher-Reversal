using System.IO;
using System.Runtime.CompilerServices;
using Bsg.Launcher.Gears;

namespace Bsg.Launcher.Services.BugReportService;

public class AttachmentStream : WrappedStream
{
	[CompilerGenerated]
	private readonly string _E001;

	public string FileName
	{
		[CompilerGenerated]
		get
		{
			return _E001;
		}
	}

	public AttachmentStream(Stream source, string fileName)
		: base(source)
	{
		_E001 = fileName;
	}

	public AttachmentStream(string fileName)
		: this(new MemoryStream(), fileName)
	{
	}
}
