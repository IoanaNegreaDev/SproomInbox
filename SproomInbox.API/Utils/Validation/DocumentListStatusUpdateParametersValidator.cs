using FluentValidation;
using SproomInbox.API.Domain.Models;
using SproomInbox.WebApp.Shared.Resources.Parametrization;

namespace SproomInbox.API.Utils.Validation
{
    public class DocumentListStatusUpdateParametersValidator : AbstractValidator<DocumentsUpdateStatusParameters>
    {
        public DocumentListStatusUpdateParametersValidator()
        {
            RuleFor(param => param.UserName).NotEmpty()
                                            .NotNull()
                                            .MaximumLength(100)
                                            .WithMessage("UserName is mandatory.");

            RuleFor(param => param.NewState).NotNull()
                                                .Must(state =>
                                                string.IsNullOrEmpty(state) ||
                                                (StateValidityChecker.IsValid(state) &&
                                                 Enum.Parse<State>(state) != State.Received))
                                         .WithMessage($"Invalid State value. Must be not null or { Enum.GetName<State>(State.Received)}. " +
                                                      $"Allowed update states: { Enum.GetName<State>(State.Approved)},{ Enum.GetName<State>(State.Approved)}.");

            RuleFor(param => param.DocumentIds).NotNull();
            RuleForEach(param => param.DocumentIds).NotNull();                     
        }
    }
}
