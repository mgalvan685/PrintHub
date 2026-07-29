using AutoMapper;
using PrintHub.Database.Models;
using PrintHub.DTOs;

namespace PrintHub.Mappers;

public class ProjectProfile : Profile
{
    public ProjectProfile()
    {
        CreateMap<Project, ProjectDto>()
            .ForMember(dest => dest.Printer_Name, opt => opt.MapFrom(src => src.Printer.Name));

        CreateMap<ProjectFilament, ProjectFilamentDto>()
            .ForMember(dest => dest.Filament_ID, opt => opt.MapFrom(src => src.Filament_ID))
            .ForMember(dest => dest.Brand, opt => opt.MapFrom(src => src.Filament.Brand))
            .ForMember(dest => dest.Material, opt => opt.MapFrom(src => src.Filament.Material))
            .ForMember(dest => dest.Texture, opt => opt.MapFrom(src => src.Filament.Texture))
            .ForMember(dest => dest.Color, opt => opt.MapFrom(src => src.Filament.Color));

        CreateMap<ProjectMaterial, ProjectMaterialDto>()
            .ForMember(dest => dest.Material_ID, opt => opt.MapFrom(src => src.Material_ID))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Material.Name))
            .ForMember(dest => dest.Units, opt => opt.MapFrom(src => src.Material.Units));
    }
}
