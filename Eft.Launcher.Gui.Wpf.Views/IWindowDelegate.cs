using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;

namespace Eft.Launcher.Gui.Wpf.Views;

public interface IWindowDelegate
{
	bool IsBrowserReady { get; }

	event EventHandler OnClosingWindow;

	void DisplayBrowser();

	void SetWindowState(WindowState newWindowState);

	void Close();

	Task LoadAsync(string url);

	void NotifyAboutSettingsUpdate(string settingsJson);

	void SetNetworkAvailability(bool isAvailable);

	string[] OpenFileDialog(string title, bool multiselect, bool isFolderPicker, string initialDir = null, KeyValuePair<string, string>[] filters = null);

	void ShowLoader();

	void ShowError(Exception exc);

	void HideLoader();

	void RestoreWindow();

	Task<bool> RequestRestartForUpdate();

	void Alert(string message);

	void SetSize(int? width, int? height);
}
