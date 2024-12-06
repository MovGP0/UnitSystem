namespace ParticleLexer.StandardTokens;

/// <summary>
/// {} Groups
/// </summary>
public class CurlyBracketGroupToken() : GroupTokenClass(new LeftCurlyBracketToken(), new RightCurlyBracketToken());