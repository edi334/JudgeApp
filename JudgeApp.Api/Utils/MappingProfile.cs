using AutoMapper;
using JudgeApp.Api.DTOs;
using JudgeApp.Core.Entities;

namespace JudgeApp.API.Utils;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Status, StatusDto>().ReverseMap();
        CreateMap<Project, ProjectDto>().ReverseMap();
        CreateMap<Judging, JudgingDto>().ReverseMap();
    }
}