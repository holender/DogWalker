using GraphQL.Services;
using GraphQL.Types.User;

namespace GraphQL.Types.Operations.Mutations;

[ExtendObjectType(OperationTypeNames.Mutation)]
public sealed class UserMutation
{
    public Token GenerateToken(
        TokenRequestInput input, 
        [Service] ITokenGeneratorService tokenGeneratorService)
    {
        var token = tokenGeneratorService.GenerateToken(input.Username, input.Role);
        
        return new Token(token!);
    }
}

public class Token
{
    private readonly string _value;

    public Token(string value)
    {
        _value = value;
    }

    public string Value() => _value;
}
