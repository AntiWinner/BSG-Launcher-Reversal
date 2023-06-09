using Eft.Launcher.Services.BackendService;

namespace Eft.Launcher.Gui.Wpf.Views;

public interface IMatchingConfigurationWindowDelegate : IWindowDelegate
{
	bool? DialogResult { get; set; }

	string MatchingConfiguration { get; set; }

	void RenderForm(DatacenterDto[] datacenters);

	void SetPing(string datacenter, int ping);
}
