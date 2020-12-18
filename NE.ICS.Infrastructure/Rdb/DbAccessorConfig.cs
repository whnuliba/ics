namespace NE.ICS.Infrastructure
{
    public sealed class DbAccessorConfig
    {
        // DbAccessor<TConnection>.AutoDetectReadOnlyQuery
        public bool AutoDetectReadOnlyQuery { get; set; }

        // IInjectionDefensePolicy.DetectInjectionWithParameters
        public bool DelectInjectionWithParameters { get; set; }

        // IReadWritePolicy.DefaultConnectionIntent
        public ReadWriteIntent DefaultConnectionIntent { get; set; } = ReadWriteIntent.ReadOnly;

        // IReadWritePolicy.DefaultActionIntent
        public ReadWriteIntent DefaultActionIntent { get; set; } = ReadWriteIntent.ReadOnly;

        public SqlRegex SqlRegex { get; set; }
                        
    }
}
