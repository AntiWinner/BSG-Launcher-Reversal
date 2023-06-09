namespace Bsg.Launcher.Updating;

public interface IPatchAlgorithmProvider
{
	IPatchAlgorithm ChooseAnAlgorithm(long oldFileSize, long newFileSize);

	IPatchAlgorithm GetAlgorithm(byte algorithmId);
}
