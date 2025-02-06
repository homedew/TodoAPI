using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using TodoAPI.Dtos;
using TodoAPI.Entity;

namespace TodoAPI.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<TodoItem, TodoDto>().ReverseMap();
        }
    }
}