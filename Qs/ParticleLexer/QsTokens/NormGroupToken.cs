using ParticleLexer.StandardTokens;

namespace ParticleLexer.QsTokens;

public class NormGroupToken() : GroupTokenClass(new LeftNormToken(), new RightNormToken());