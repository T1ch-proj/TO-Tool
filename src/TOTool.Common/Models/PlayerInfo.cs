namespace TOTool.Common.Models
{
    public class PlayerInfo
    {
        public int HP { get; set; }
        public int MaxHP { get; set; }
        public int MP { get; set; }
        public int MaxMP { get; set; }
        public int Level { get; set; }
        public long Experience { get; set; }
        public long MaxExperience { get; set; }
        public float PositionX { get; set; }
        public float PositionY { get; set; }
        public int MapID { get; set; }
        public bool IsAlive => HP > 0;
        public float HPPercentage => MaxHP == 0 ? 0 : (float)HP / MaxHP * 100;
        public float MPPercentage => MaxMP == 0 ? 0 : (float)MP / MaxMP * 100;
    }
} 