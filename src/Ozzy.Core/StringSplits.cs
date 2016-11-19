namespace Ozzy.Core
{
    /// <summary>
    /// StringSplits should be used in String.Split method calls to avoid unnecessary allocations (especially in loops) 
    /// </summary>
    public class StringSplits
    {
        public static readonly char[] Semicolon = { ';' };
        public static readonly char[] Colon = { ':' };
        public static readonly char[] Dot = { '.' };
        public static readonly char[] Comma = { ',' };
        public static readonly char[] Hyphen = { '-' };
        public static readonly char[] Slash = { '/' };
        public static readonly char[] BackSlash = { '\\' };
    }
}
