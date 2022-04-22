using SproomInbox.API.Domain.Models;

namespace SproomInbox.API.Utils.Validation
{
    static public class StateValidityChecker
    {
        static public bool IsValid(string state)
        {
            if (!string.IsNullOrEmpty(state) &&
                  int.TryParse(state, out _) == false &&
                  Enum.TryParse<State>(state, true, out var stateId) &&
                  Enum.IsDefined<State>(stateId))
                return true;

            return false;
        }
    }
}
