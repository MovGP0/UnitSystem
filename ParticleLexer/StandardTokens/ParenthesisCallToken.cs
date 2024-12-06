namespace ParticleLexer.StandardTokens;

/// <summary>
/// fifo(p1, p2, ..., pn)    whole token
/// </summary>
public class ParenthesisCallToken() : CallTokenClass(new ParenthesisGroupToken(), new CommaToken());