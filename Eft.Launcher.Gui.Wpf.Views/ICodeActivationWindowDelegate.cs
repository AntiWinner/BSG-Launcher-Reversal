using Newtonsoft.Json.Linq;

namespace Eft.Launcher.Gui.Wpf.Views;

public interface ICodeActivationWindowDelegate : IWindowDelegate
{
	bool? DialogResult { get; set; }

	void Success(JToken data);
}
