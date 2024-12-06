using ParticleLexer.StandardTokens;

namespace ParticleLexer.QsTokens;

public sealed class AbsoluteGroupToken() : GroupTokenClass(new LeftAbsoluteToken(), new RightAbsoluteToken());