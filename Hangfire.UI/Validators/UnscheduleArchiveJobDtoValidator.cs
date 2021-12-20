using FluentValidation;
using Hangfire.UI.Dtos;

namespace Hangfire.UI.Validators
{
    public class UnscheduleArchiveJobDtoValidator : AbstractValidator<UnscheduleArchiveJobDto>
    {
        public UnscheduleArchiveJobDtoValidator()
        {
            RuleFor(u => u.JobId)
                .NotEmpty()
                .NotNull();
        } 
    }
}