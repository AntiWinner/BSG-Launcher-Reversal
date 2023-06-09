using System.Globalization;
using System.Runtime.CompilerServices;

namespace Eft.Launcher.Core;

public class ProgressReport : IProgressReport
{
	[CompilerGenerated]
	private readonly double _E000;

	[CompilerGenerated]
	private readonly double _E001;

	public double Percent
	{
		get
		{
			if (!(FileSize > 0.0))
			{
				return 0.0;
			}
			return BytesTransferred * 100.0 / FileSize;
		}
	}

	public double BytesTransferred
	{
		[CompilerGenerated]
		get
		{
			return _E000;
		}
	}

	public double FileSize
	{
		[CompilerGenerated]
		get
		{
			return _E001;
		}
	}

	public ProgressReport(double current, double total)
	{
		_E000 = current;
		_E001 = total;
	}

	public override string ToString()
	{
		return Percent.ToString(_E05B._E000(60774), CultureInfo.InvariantCulture);
	}
}
