using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace KeyGen.Classes
{
    public static class Generator
    {
        public static List<List<char>> Generate()
        {
            List<char> part1 = new List<char>();
            List<char> part2 = new List<char>();
            List<char> part3 = new List<char>();
            List<char> part4 = new List<char>();

            Random rnd = new Random(DateTime.Now.Second % 5);

            char[] chars = {'A', 'B', 'C', 'D', 'E', 'F', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9'}; //16
            var x = rnd.Next(chars.Length); 



            List<List<char>> key = new List<List<char>>();
            key.Add(part1);
            key.Add(part2);
            key.Add(part3);
            key.Add(part4);
            return key;
        }

        public static void Validate(string key)
        {

        }
    }
}
