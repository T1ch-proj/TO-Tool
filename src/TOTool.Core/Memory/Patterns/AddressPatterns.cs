namespace TOTool.Core.Memory.Patterns
{
    public static class AddressPatterns
    {
        public static class Player
        {
            public static readonly int[] HealthOffsets = { 0x123, 0x456, 0x789 };
            public static readonly int[] ManaOffsets = { 0x123, 0x456, 0x78A };
            public static readonly int[] ExpOffsets = { 0x123, 0x456, 0x78B };
            public static readonly int[] PositionOffsets = { 0x123, 0x456, 0x78C };
        }

        public static class Game
        {
            public const string ProcessName = "Trickster";
            public const string ModuleName = "Trickster.exe";
            public static readonly int[] BaseOffsets = { 0x400000, 0x1000 };
        }
    }
} 