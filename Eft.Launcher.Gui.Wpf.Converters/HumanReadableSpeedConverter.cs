using System;
using System.Globalization;
using System.Windows.Data;
using Bsg.Network.MultichannelDownloading;

namespace Eft.Launcher.Gui.Wpf.Converters;

public class HumanReadableSpeedConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value is int size)
		{
			return size.ToHumanReadableSize() + _E05B._E000(31146);
		}
		if (value is long size2)
		{
			return size2.ToHumanReadableSize() + _E05B._E000(31146);
		}
		if (value is double size3)
		{
			return size3.ToHumanReadableSize() + _E05B._E000(31146);
		}
		throw new NotSupportedException();
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
