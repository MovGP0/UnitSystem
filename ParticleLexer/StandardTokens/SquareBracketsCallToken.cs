namespace ParticleLexer.StandardTokens;

public class SquareBracketsCallToken() : CallTokenClass(new SquareBracketsGroupToken(), new CommaToken());