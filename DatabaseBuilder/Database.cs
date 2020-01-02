using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DatabaseBuilder
{
    public static class Database
    {
        public static string Path = null;
        public static void ConvertPinyin(string sourcepath,string outputpath)
        {
            string[] sourcelines = File.ReadAllLines(sourcepath, Encoding.UTF8);
            string[] charlist = new string[256];
            foreach(string sourceline in sourcelines)
            {
                string[] infos = sourceline.Split(' ');
                foreach(char character in infos[1].ToCharArray())
                {
                    charlist[GetWordHash(character + "", Encoding.UTF8)]
                        += character + "@" + infos[0] + " ";
                }
            }

            if(File.Exists(outputpath))
                outputpath = outputpath.Split('.')[0] + "_1."
                    + outputpath.Split('.')[outputpath.Split('.').Length - 1];

            StreamWriter writer = new StreamWriter(outputpath, true, Encoding.UTF8);
            StreamWriter infowriter = new StreamWriter(outputpath + "_info.txt", true, Encoding.UTF8);
            foreach (string line in charlist)
            {
                if (line != null)
                {
                    writer.WriteLine(line.Remove(line.Length - 1));
                    infowriter.Write(line.Split(' ').Length + ",");
                }
                else
                {
                    writer.WriteLine("");
                    infowriter.Write("0,");
                }
            }
            writer.Close();
            writer.Dispose();
            infowriter.Close();
            infowriter.Dispose();
        }

        public static void Convert(string sourcepath,string outputpath)
        {
            string[] result = new string[256];
            string[] sourcelines = File.ReadAllLines(sourcepath, Encoding.UTF8);
            foreach(string sourceline in sourcelines)
            {
                string[] infos = sourceline.Split(' ');
                for (int meaningindex = 1; meaningindex < infos.Length; meaningindex++)
                {
                    string meaning = infos[meaningindex];
                    result[GetWordHash(meaning, Encoding.UTF8)]
                        += meaning + " " + infos[0] + " ";
                }
            }

            if (File.Exists(outputpath))
                outputpath = outputpath.Split('.')[0] + "_1."
                    + outputpath.Split('.')[outputpath.Split('.').Length - 1];

            StreamWriter writer = new StreamWriter(outputpath, true, Encoding.UTF8);
            StreamWriter infowriter = new StreamWriter(outputpath + "_info.txt", true, Encoding.UTF8);
            for (int i = 0; i < result.Length; i++)
            {
                if (result[i] != null)
                    writer.WriteLine(result[i].Remove(result[i].Length - 1));
                else
                    writer.WriteLine("");
                infowriter.Write(result[i].Length + ",");
            }
            writer.Close();
            writer.Dispose();
            infowriter.Close();
            infowriter.Dispose();
        }

        public static byte GetWordHash(string word, Encoding encoding)
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