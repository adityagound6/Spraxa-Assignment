using AssignmentSpraxa.API.Model;

namespace AssignmentSpraxa.API
{
    public interface IJwtTokenService
    {
        Task<(string Token, System.DateTime Expires)> GenerateTokenAsync(User user, IEnumerable<string> roles);
    }
}
