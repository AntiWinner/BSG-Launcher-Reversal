using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace Eft.Launcher.Gui.Wpf.Properties;

[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
[DebuggerNonUserCode]
[CompilerGenerated]
internal class Resources
{
	private static ResourceManager m__E000;

	private static CultureInfo _E001;

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	internal static ResourceManager _E000
	{
		get
		{
			if (Resources.m__E000 == null)
			{
				Resources.m__E000 = new ResourceManager(_E05B._E000(25319), typeof(Resources).Assembly);
			}
			return Resources.m__E000;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	internal static CultureInfo _E000
	{
		get
		{
			return _E001;
		}
		set
		{
			_E001 = value;
		}
	}

	internal Resources()
	{
	}
}
