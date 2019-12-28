using System;
using System.IO;
using System.Collections.Generic;

namespace DatabaseBuilder
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("输入原库地址");
            string sourcepath = Console.ReadLine();
            Console.WriteLine("输入转换库地址");
            string charpath = Console.ReadLine();

            Database.ConvertPinyin(sourcepath, charpath);
        }
    }
}
