namespace Application.Interfaces.Files
{

    public interface IFileService
    {
        //Task<List<ExcelOrderDto>> GetSingleOrderFilesAsync(IProgress<ProgressValue>? progress = null, CancellationToken cancellationToken = default);

        string GetRootFolder();

        string GetFailedFolder();

        string GetSuccessFolder();
        string GetLogFolder();

        List<string> GetLocalFiles();

        bool IsLocked(string filePath);

        void MoveOrderFiles(List<string> files, bool success);

    }
}
