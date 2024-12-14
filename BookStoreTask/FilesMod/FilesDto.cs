using BookStoreTask.Utli;

namespace BookStoreTask.FilesMod;

public class FilesDto:BaseDto<Guid>
{
    public string? FileName { get; set; }
    public string? FilePath { get; set; }
}