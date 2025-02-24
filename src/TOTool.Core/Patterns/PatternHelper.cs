using System;

namespace TOTool.Core.Patterns
{
    public static class PatternHelper
    {
        public static byte[] PatternToBytes(string pattern)
        {
            string[] parts = pattern.Split(' ');
            byte[] bytes = new byte[parts.Length];

            for (int i = 0; i < parts.Length; i++)
            {
                if (parts[i] == "??")
                {
                    bytes[i] = 0;
                }
                else
                {
                    bytes[i] = Convert.ToByte(parts[i], 16);
                }
            }

            return bytes;
        }

        public static bool[] CreateMask(string pattern)
        {
            string[] parts = pattern.Split(' ');
            bool[] mask = new bool[parts.Length];

            for (int i = 0; i < parts.Length; i++)
            {
                mask[i] = parts[i] != "??";
            }

            return mask;
        }
    }
} 