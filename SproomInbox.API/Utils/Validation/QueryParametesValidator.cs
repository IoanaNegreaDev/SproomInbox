using FluentValidation;
using SproomInbox.API.Domain.Models;
using SproomInbox.API.Utils.Parametrization;

namespace RentalAPI.ValidationFilters
{
    public class QueryParametesValidator: AbstractValidator<QueryParameters>
    {
        public QueryParametesValidator()
        {
            RuleFor(param => param.UserName).NotEmpty()
                                            .NotNull()
                                            .MaximumLength(100)
                                            .WithMessage("UserName is mandatory.");

            RuleFor(param => param.State).Must(state =>
                                                string.IsNullOrEmpty(state) ||
                                                Enum.TryParse<State>(state, true, out var stateId) == true)
                                         .WithMessage("Invalid State value. Please use: " +
                                                       String.Join(", ", Enum.GetNames<State>()) + ".");
            
            RuleFor(param => param.Type).Must(type => 
                                                 string.IsNullOrEmpty(type) ||
                                                 Enum.TryParse<DocumentType>(type, true, out var typeId) == true)
                                          .WithMessage("Invalid Document Type value. Please use: " +
                                                       String.Join(", ", Enum.GetNames<DocumentType>()) + ".");
        }
    }
}
