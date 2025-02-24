using System;

namespace TOTool.Core.Patterns
{
    public static class GamePatterns
    {
        // 遊戲基礎特徵碼
        public const string BaseAddress = "48 8B 05 ?? ?? ?? ?? 48 85 C0 74 ?? 48 8B 40";
        public const string BaseMask = "xxx????xxxx?xxx";

        // 玩家特徵碼
        public static class Player
        {
            public const string Health = "89 0D ?? ?? ?? ?? 8B 0D ?? ?? ?? ??";
            public const string Mana = "89 0D ?? ?? ?? ?? 8B 15 ?? ?? ?? ??";
            public const string Position = "F3 0F 11 05 ?? ?? ?? ?? F3 0F 10 05";
            public const string Status = "8B 0D ?? ?? ?? ?? 85 C9 74 ?? 8B 01";
        }

        // 背包特徵碼
        public static class Inventory
        {
            public const string Base = "48 8B 0D ?? ?? ?? ?? 48 85 C9 74 ?? 48 8B 01";
            public const string Items = "4C 8B 05 ?? ?? ?? ?? 48 8B D9 48 85 C0";
        }
    }
} 