using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace EmojiTranslator
{
    public static class Translator
    {
        internal static List<string> SplitString(string source)
        {
            List<string> result = new List<string>();
            for (int sourceindex = 0; sourceindex < source.Length;)
            {
                string addmeaning = "" + source[sourceindex];
                int max_match_count = 0;
                int match_count = 0;
                List<string> meaninglist = Database.MeaningList[GetWordHash(source[sourceindex] + "", Encoding.UTF8)];
                for (int meaninglist_index = 0; meaninglist_index < meaninglist.Count; meaninglist_index++)
                {
                    int meaning_index = 0;
                    match_count = 0;
                    string match = null;
                    while (sourceindex < source.Length
                        && meaning_index < meaninglist[meaninglist_index].Length
                        && meaninglist[meaninglist_index][meaning_index] == source[sourceindex + match_count])
                    {
                        match += meaninglist[meaninglist_index][meaning_index];
                        match_count++;
                        meaning_index++;
                        if (sourceindex + match_count == source.Length)
                            break;
                    }
                    if (match_count > max_match_count)
                    {
                        addmeaning = match;
                        max_match_count = match_count;
                    }
                }
                sourceindex += max_match_count;
                if (max_match_count == 0)
                    sourceindex++;
                result.Add(addmeaning);
            }
            return result;
        }
        public static string GetEmojiFromWord(string word)
        {
            byte y = GetWordHash(word, Encoding.UTF8);
            for (int x = 0; x < Database.MainDatabase[y].Length - 1; x += 2)
            {
                if (Database.MainDatabase[y][x] == word)
                    return Database.MainDatabase[y][x + 1];
            }
            return word;
        }

        public static string GetEmojiFromChar(string source)
        {
            string pinyin = GetPinyin(source);
            try
            {
                if (Database.SecondaryDatabase[pinyin] == "")
                    return null;
                return Database.SecondaryDatabase[pinyin];
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private static string GetPinyin(string character)
        {
            byte charhash = GetWordHash(character, Encoding.UTF8);
            foreach(string comb in Database.CharDatabase[charhash])
                if (comb.Split('@')[0] == character)
                    return comb.Split('@')[1];
            return null;
        }

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