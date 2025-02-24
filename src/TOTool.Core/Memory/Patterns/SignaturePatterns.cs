namespace TOTool.Core.Memory.Patterns
{
    public static class SignaturePatterns
    {
        public static class Player
        {
            public const string BaseAddress = "48 8B 05 ?? ?? ?? ?? 48 85 C0 74 ?? 48 8B 40";
            public const string BaseMask = "xxx????xxxx?xxx";
            
            public const string Health = "89 0D ?? ?? ?? ?? 8B 0D ?? ?? ?? ??";
            public const string HealthMask = "xx????xx????";
        }

        public static class Game
        {
            public const string BasePattern = "48 8B 05 ?? ?? ?? ?? 48 85 C0 74 ?? 48 8B 40";
            public const string BaseMask = "xxx????xxxx?xxx";
        }
    }
} 