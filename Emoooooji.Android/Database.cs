using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using Android.App;
using Android.Content.PM;
using Android.Content.Res;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using EmojiTranslator;

namespace EmojiTranslator.Droid
{
    [Activity(Label = "EmojiTranslator", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public static class DatabaseIO
    {
        internal static string Path = "emojilist.dat";
        internal static AssetManager Manager;

        internal static void LoadDatabase()
        {
            for (int i = 0; i < 256; i++)
                Database.MeaningList[i] = new List<string>();
            Database.MainDatabase = new string[256][];
            Stream ipts = Manager.Open("MainDatabase.dat");
            StreamReader reader = new StreamReader(ipts);
            for (int y = 0; y < 256; y++){
                string line = reader.ReadLine();
                if (line != null)
                    Database.MainDatabase[y] = line.Split(' ');
                else
                    Database.MainDatabase[y] = new string[1] {""};
                for (int x = 0; x < Database.MainDatabase[y].Length - 1; x += 2)
                {
                    byte meaninghash = GetWordHash(Database.MainDatabase[y][x][0] + "", Encoding.UTF8);
                    Database.MeaningList[meaninghash].Add(Database.MainDatabase[y][x]);
                }
                    
            }
            reader.Close();
            reader.Dispose();
            ipts.Dispose();

            ipts = Manager.Open("SecondaryDatabase.dat");
            reader = new StreamReader(ipts);
            for (string line = reader.ReadLine(); line != null; line = reader.ReadLine())
                Database.SecondaryDatabase.Add(line.Split(' ')[0],line.Split(' ')[1]);
            reader.Close();
            reader.Dispose();
            ipts.Dispose();

            ipts = Manager.Open("CharDatabase.dat");
            reader = new StreamReader(ipts);
            for(int y = 0; y < 256; y++)
                EmojiTranslator.Database.CharDatabase[y] = reader.ReadLine().Split(' ');
            reader.Close();
            reader.Dispose();
            ipts.Dispose();
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