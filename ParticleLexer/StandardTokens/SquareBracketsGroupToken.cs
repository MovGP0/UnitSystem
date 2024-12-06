namespace ParticleLexer.StandardTokens;

/// <summary>
/// [] Groups
/// </summary>
public class SquareBracketsGroupToken() : GroupTokenClass(new LeftSquareBracketToken(), new RightSquareBracketToken());