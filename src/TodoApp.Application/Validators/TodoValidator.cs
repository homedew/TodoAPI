using FluentValidation;
using TodoApp.Application.Dto;
namespace TodoApp.Application.Validators
{
    public class TodoValidator : AbstractValidator<TodoDto>
    {
        public TodoValidator()
        {
            RuleFor(x=> x.Title)
                .NotEmpty().WithMessage("Title is required")
                .MaximumLength(10).WithMessage("Title cannot exceed 100 characters");

            RuleFor(x=>x.Description)
                .MaximumLength(500).WithMessage("Description too long");

        }
        
    }
}