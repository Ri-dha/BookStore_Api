using AutoMapper;

namespace BookStoreTask.FilesMod;

public class ProjectFilesMapper:Profile
{
    
    public ProjectFilesMapper()
    {
        CreateMap<ProjectFiles, FilesDto>()
            .ForMember(dest => dest.FileName, opt => opt.MapFrom(src => src.FileName))
            .ForMember(dest => dest.FilePath, opt => opt.MapFrom(src => src.FilePath));
        
    }
    
}