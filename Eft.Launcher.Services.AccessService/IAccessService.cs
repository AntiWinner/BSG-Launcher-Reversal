using System.Threading.Tasks;
using Microsoft.Win32;

namespace Eft.Launcher.Services.AccessService;

public interface IAccessService
{
	void AddEntryToRegistry(string subkey, string name, object value, RegistryValueKind valueKind);

	void ModifyRegistryEntry(string subkey, string name, object value, RegistryValueKind valueKind);

	void DeleteSectionFromRegistry(string subkey);

	Task<bool> AssignFullPermissions(string folderPath);

	bool CheckPermissions(string folderPath);

	Task<int> RunProcess(string fileName, string arguments, string workingDirectory);

	void StopService();
}
