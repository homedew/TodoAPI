using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using TodoApp.Application.Dto;
using TodoApp.Infrastructure.Entity;

namespace TodoApp.Application.Mapping
{
    public class TodoMappingProfile : Profile
    {
        public TodoMappingProfile()
        {
            CreateMap<TodoItem, TodoDto>().ReverseMap();
        }
    }
}