namespace a2p.Application.Services
{

    public interface IFileService
    {
        //Task<List<OrderEntry>> GetSingleOrderFilesAsync(IProgress<ProgressValue>? progress = null, CancellationToken cancellationToken = default);

        string GetRootFolder();

        string GetFailedFolder();

        string GetSuccessFolder();
        string GetLogFolder();


        List<string>? GetFiles();

        List<a2p.Domain.Models.File> GetOrderFiles(string order);

        bool IsLocked(string filePath);

        void MoveOrderFiles(List<string> files, bool success);



    }
}
