using FluentValidation;
using SproomInbox.API.Domain.Models;
using SproomInbox.WebApp.Shared.Resources.Parametrization;

namespace RentalAPI.ValidationFilters
{
    public class QueryParametersValidator: AbstractValidator<DocumentsQueryParameters>
    {
        public QueryParametersValidator()
        {
            RuleFor(param => param.UserName).NotEmpty()
                                            .NotNull()
                                            .MaximumLength(100)
                                            .WithMessage("UserName is mandatory.");

            RuleFor(param => param.State).Must(state =>
                                                string.IsNullOrEmpty(state) ||
                                                (int.TryParse(state, out _) == false && 
                                                 Enum.TryParse<State>(state, true, out var stateId) &&
                                                 Enum.IsDefined<State>(stateId)))
                                         .WithMessage("Invalid State value. Please use: " +
                                                       String.Join(", ", Enum.GetNames<State>()) + ".");
            
            RuleFor(param => param.Type).Must(type => 
                                                 string.IsNullOrEmpty(type) ||
                                                (int.TryParse(type, out _) == false && 
                                                 Enum.TryParse<DocumentType>(type, true, out var typeId) &&
                                                 Enum.IsDefined<DocumentType>(typeId)))
                                          .WithMessage("Invalid Document Type value. Please use: " +
                                                       String.Join(", ", Enum.GetNames<DocumentType>()) + ".");

            RuleFor(param => param.CurrentPage).GreaterThan(0)
                                                      .WithMessage("Current page must be grater than 0.");

            RuleFor(param => param.PageSize).GreaterThan(0)
                                                   .LessThanOrEqualTo(30);
        }
    }
}
