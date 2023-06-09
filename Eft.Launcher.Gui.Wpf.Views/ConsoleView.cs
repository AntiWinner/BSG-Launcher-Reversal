using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;

namespace Eft.Launcher.Gui.Wpf.Views;

public class ConsoleView : UserControl, IComponentConnector
{
	private bool m__E000;

	private void _E000(object sender, MouseButtonEventArgs e)
	{
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	public void InitializeComponent()
	{
		if (!this.m__E000)
		{
			this.m__E000 = true;
			Uri resourceLocator = new Uri(_E05B._E000(24835), UriKind.Relative);
			Application.LoadComponent(this, resourceLocator);
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	void IComponentConnector.Connect(int connectionId, object target)
	{
		if (connectionId == 1)
		{
			((TextBlock)target).MouseDown += _E000;
		}
		else
		{
			this.m__E000 = true;
		}
	}
}
