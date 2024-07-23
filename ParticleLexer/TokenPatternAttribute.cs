namespace ParticleLexer
{

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class TokenPatternAttribute : Attribute
    {
        /// <summary>
        /// The regex pattern that will be used to compare when tokenizing the target.
        /// </summary>
        public string RegexPattern { get; set; }

        /// <summary>
        /// Indicates that Regex Pattern is a whole exact complete word without special regex classes.
        /// </summary>
        public bool ExactWord { get; set; }


        /// <summary>
        /// Condition for the comparing process to make sure that the target should begin with this value. otherwise the code will bypass the current process.
        /// </summary>
        public string ShouldBeginWith { get; set; }

        /// <summary>
        /// Condition that let the parser continue parsing until it found the End with charachter
        /// </summary>
        public string ShouldEndWith { get; set; }


        /// <summary>
        /// Indicates if parser should continue test after success and consume other tokens or not.
        /// This property by default is false.
        /// </summary>
        public bool ContinueTestAfterSuccess { get; set; }


        /// <summary>
        /// Indicates that the token is a whole continous unit, which can't be discrete or strictly speaking when mathcing fail for
        /// an extra text character the test shouldn't search for another success tokens after that.
        /// </summary>
        public bool ContinousToken { get; set; }
    }
}
