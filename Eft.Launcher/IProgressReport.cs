namespace Eft.Launcher;

public interface IProgressReport
{
	double BytesTransferred { get; }

	double FileSize { get; }
}
