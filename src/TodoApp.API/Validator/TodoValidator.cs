using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using TodoAPI.Dtos;
using TodoAPI.Entity;
namespace TodoAPI.Validator
{
    public class TodoValidator : AbstractValidator<TodoDto>
    {
        public TodoValidator()
        {
            RuleFor(x=> x.Title)
                .NotEmpty().WithMessage("Title is required")
                .MaximumLength(10).WithMessage("Title cannot exceed 100 characters");
        }
        
    }
}