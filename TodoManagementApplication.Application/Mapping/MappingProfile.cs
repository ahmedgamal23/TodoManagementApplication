using AutoMapper;
using TodoManagementApplication.Domain.Models;
using TodoManagementApplication.Domain.ViewModels;

namespace TodoManagementApplication.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {

            CreateMap<TodoDto, Todo>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) =>
                                    srcMember != null && !string.IsNullOrWhiteSpace(srcMember.ToString())));


        }
    }
}
