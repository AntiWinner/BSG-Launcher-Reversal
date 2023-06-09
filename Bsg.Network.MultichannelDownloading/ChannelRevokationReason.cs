using System;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;

namespace Bsg.Network.MultichannelDownloading;

public class ChannelRevokationReason
{
	[CompilerGenerated]
	private static readonly ChannelRevokationReason _E000 = new ChannelRevokationReason(LogLevel.Warning, _E05B._E000(15308));

	[CompilerGenerated]
	private static readonly ChannelRevokationReason _E001 = new ChannelRevokationReason(LogLevel.Trace, _E05B._E000(2080));

	[CompilerGenerated]
	private static readonly ChannelRevokationReason _E002 = new ChannelRevokationReason(LogLevel.Debug, _E05B._E000(2106));

	[CompilerGenerated]
	private static readonly ChannelRevokationReason _E003 = new ChannelRevokationReason(LogLevel.Debug, _E05B._E000(2062));

	[CompilerGenerated]
	private static readonly ChannelRevokationReason _E004 = new ChannelRevokationReason(LogLevel.Debug, _E05B._E000(2158));

	[CompilerGenerated]
	private static readonly ChannelRevokationReason _E005 = new ChannelRevokationReason(LogLevel.Debug, _E05B._E000(2127));

	[CompilerGenerated]
	private readonly LogLevel _E006;

	[CompilerGenerated]
	private readonly string _E007;

	[CompilerGenerated]
	private readonly object[] _E008;

	public static ChannelRevokationReason Unknown
	{
		[CompilerGenerated]
		get
		{
			return _E000;
		}
	}

	public static ChannelRevokationReason TheNeedIsExhausted
	{
		[CompilerGenerated]
		get
		{
			return _E001;
		}
	}

	public static ChannelRevokationReason ChannelDestruction
	{
		[CompilerGenerated]
		get
		{
			return _E002;
		}
	}

	public static ChannelRevokationReason FileSizeSuccessfullyReceived
	{
		[CompilerGenerated]
		get
		{
			return _E003;
		}
	}

	public static ChannelRevokationReason MetadataSuccessfullyReceived
	{
		[CompilerGenerated]
		get
		{
			return _E004;
		}
	}

	public static ChannelRevokationReason SpareNodeActivationThresholdRestored
	{
		[CompilerGenerated]
		get
		{
			return _E005;
		}
	}

	public LogLevel Severity
	{
		[CompilerGenerated]
		get
		{
			return _E006;
		}
	}

	public Exception Exception { get; }

	public string MessageTemplate
	{
		[CompilerGenerated]
		get
		{
			return _E007;
		}
	}

	public object[] MessageArgs
	{
		[CompilerGenerated]
		get
		{
			return _E008;
		}
	}

	public ChannelRevokationReason(LogLevel severity, Exception exception, string messageTemplate, params object[] messageArgs)
	{
		_E006 = severity;
		Exception = exception;
		_E007 = messageTemplate;
		_E008 = messageArgs;
	}

	public ChannelRevokationReason(LogLevel severity, string messageTemplate, params object[] messageArgs)
		: this(severity, null, messageTemplate, messageArgs)
	{
	}

	public override string ToString()
	{
		return string.Format(MessageTemplate, MessageArgs);
	}
}
