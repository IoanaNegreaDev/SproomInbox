using FluentValidation;
using SproomInbox.API.Parametrization;

namespace SproomInbox.API.Utils.Validation
{

    public class FindDocumentsByIdValidator : AbstractValidator<DocumentsFindByIdParameters>
    {
        public FindDocumentsByIdValidator()
        {
            RuleFor(param => param.UserName).NotEmpty()
                                            .NotNull()
                                            .MaximumLength(100)
                                            .WithMessage("UserName is mandatory.");

            RuleFor(param => param.Id).NotEmpty()
                                      .NotNull()
                                      .WithMessage("Document Id is mandatory.");
        }
    }
}
