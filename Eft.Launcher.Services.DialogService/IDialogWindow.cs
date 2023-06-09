using System.Threading.Tasks;

namespace Eft.Launcher.Services.DialogService;

public interface IDialogWindow
{
	Task<DialogResult> ShowDialog();

	void Close();
}
