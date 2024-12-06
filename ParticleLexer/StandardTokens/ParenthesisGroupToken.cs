namespace ParticleLexer.StandardTokens;

/// <summary>
/// () groups
/// </summary>
public class ParenthesisGroupToken() : GroupTokenClass(new LeftParenthesisToken(), new RightParenthesisToken());