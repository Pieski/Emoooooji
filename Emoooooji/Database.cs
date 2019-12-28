using System;
using System.Collections.Generic;
using System.Text;

namespace EmojiTranslator
{
    public static class Database
    {
        public static string[][] MainDatabase = new string[256][];
        public static List<string>[] MeaningList = new List<string>[256];
        public static Dictionary<string, string> SecondaryDatabase = new Dictionary<string, string>();
        public static string[][] CharDatabase = new string[256][];

        private static byte GetWordHash(string word, Encoding encoding)
        {
            int hash32 = 1;
            byte[] wordbytes = encoding.GetBytes(word);
            foreach (byte wordbyte in wordbytes)
                hash32 *= wordbyte;
            hash32 *= hash32;
            return BitConverter.GetBytes(hash32)[BitConverter.GetBytes(hash32).Length - 2];
        }
    }
}
