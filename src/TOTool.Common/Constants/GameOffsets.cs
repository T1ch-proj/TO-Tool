namespace TOTool.Common.Constants
{
    public static class GameOffsets
    {
        public static class Player
        {
            public const int HP = 0x123;
            public const int MP = 0x127;
            public const int Level = 0x12B;
            public const int Experience = 0x12F;
            public const int PositionX = 0x133;
            public const int PositionY = 0x137;
        }

        public static class Inventory
        {
            public const int BaseOffset = 0x200;
            public const int ItemCount = 0x204;
            public const int FirstItem = 0x208;
        }

        public static class Status
        {
            public const int BaseOffset = 0x300;
            public const int BuffCount = 0x304;
            public const int FirstBuff = 0x308;
        }
    }
} 