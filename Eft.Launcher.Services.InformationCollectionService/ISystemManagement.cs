namespace Eft.Launcher.Services.InformationCollectionService;

public interface ISystemManagement
{
	SmSystemFirmwareInfo GetSystemFirmwareInfo();

	SmBiosInfo GetBiosInfo();

	SmSystemInfo GetSystemInfo();

	SmBaseboardInfo GetBaseboardInfo();

	SmProcessorInfo GetProcessorInfo();
}
