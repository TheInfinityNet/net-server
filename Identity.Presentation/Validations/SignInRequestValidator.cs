using InfinityNetServer.Services.Identity.Application.DTOs.Requests;
using FluentValidation;

namespace InfinityNetServer.Services.Identity.Presentation.Validations
{
    public class SignInRequestValidator : AbstractValidator<SignInRequest>
    {
        public SignInRequestValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("null_email")
                .EmailAddress().WithMessage("invalid_email");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("null_password")
                .Length(6, 20).WithMessage("size_password");
        }
    }
}
