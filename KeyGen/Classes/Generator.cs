using System;
using System.Collections.Generic;
using System.Text;

namespace KeyGen
{
    public static class Generator
    {
        public static List<List<char>> Generate()
        {
            List<char> part1 = new List<char>();
            List<char> part2 = new List<char>();
            List<char> part3 = new List<char>();
            List<char> part4 = new List<char>();

            Random rnd = new Random();

            char[] chars = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

            // generate
            var indx1 = rnd.Next(chars.Length);
            var indx2 = rnd.Next(chars.Length);
            var indx3 = rnd.Next(chars.Length);
            var indx4 = rnd.Next(chars.Length);
            part1.Add(chars[indx1]);
            part2.Add(chars[indx2]);
            part3.Add(chars[indx3]);
            part4.Add(chars[indx4]);

            indx1 += indx3; indx1 = indx1 >= chars.Length ? indx1 - chars.Length : indx1;
            indx2 -= indx4; indx2 = indx2 < 0 ? indx2 + chars.Length : indx2;
            indx3 += indx1; indx3 = indx3 >= chars.Length ? indx3 - chars.Length : indx3;
            indx4 -= indx2; indx4 = indx4 < 0 ? indx4 + chars.Length : indx4;
            part1.Add(chars[indx1]);
            part2.Add(chars[indx2]);
            part3.Add(chars[indx3]);
            part4.Add(chars[indx4]);

            indx1 += indx2; indx1 = indx1 >= chars.Length ? indx1 - chars.Length : indx1;
            indx2 += indx3; indx2 = indx2 >= chars.Length ? indx2 - chars.Length : indx2;
            indx3 += indx4; indx3 = indx3 >= chars.Length ? indx3 - chars.Length : indx3;
            indx4 += indx1; indx4 = indx4 >= chars.Length ? indx4 - chars.Length : indx4;
            part1.Add(chars[indx1]);
            part2.Add(chars[indx2]);
            part3.Add(chars[indx3]);
            part4.Add(chars[indx4]);

            indx1 -= indx3; indx1 = indx1 < 0 ? indx1 + chars.Length : indx1;
            indx2 += indx4; indx2 = indx2 >= chars.Length ? indx2 - chars.Length : indx2;
            indx3 -= indx1; indx3 = indx3 < 0 ? indx3 + chars.Length : indx3;
            indx4 += indx2; indx4 = indx4 >= chars.Length ? indx4 - chars.Length : indx4;
            part1.Add(chars[indx1]);
            part2.Add(chars[indx2]);
            part3.Add(chars[indx3]);
            part4.Add(chars[indx4]);

            indx1 -= indx2; indx1 = indx1 < 0 ? indx1 + chars.Length : indx1;
            indx2 -= indx3; indx2 = indx2 < 0 ? indx2 + chars.Length : indx2;
            indx3 -= indx4; indx3 = indx3 < 0 ? indx3 + chars.Length : indx3;
            indx4 -= indx1; indx4 = indx4 < 0 ? indx4 + chars.Length : indx4;
            part1.Add(chars[indx1]);
            part2.Add(chars[indx2]);
            part3.Add(chars[indx3]);
            part4.Add(chars[indx4]);
            //

            List<List<char>> key = new List<List<char>>();
            key.Add(part1);
            key.Add(part2);
            key.Add(part3);
            key.Add(part4);
            return key;
        }

        public static bool Validate(string key)
        {
            try
            {
                var parts = key.Split('-');
                List<char> part1 = new List<char>();
                List<char> part2 = new List<char>();
                List<char> part3 = new List<char>();
                List<char> part4 = new List<char>();

                List<char> chars = new List<char>() { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

                var indx1 = chars.IndexOf(parts[0][0]);
                var indx2 = chars.IndexOf(parts[1][0]);
                var indx3 = chars.IndexOf(parts[2][0]);
                var indx4 = chars.IndexOf(parts[3][0]);

                part1.Add(chars[indx1]);
                part2.Add(chars[indx2]);
                part3.Add(chars[indx3]);
                part4.Add(chars[indx4]);

                indx1 += indx3; indx1 = indx1 >= chars.Count ? indx1 - chars.Count : indx1;
                indx2 -= indx4; indx2 = indx2 < 0 ? indx2 + chars.Count : indx2;
                indx3 += indx1; indx3 = indx3 >= chars.Count ? indx3 - chars.Count : indx3;
                indx4 -= indx2; indx4 = indx4 < 0 ? indx4 + chars.Count : indx4;
                part1.Add(chars[indx1]);
                part2.Add(chars[indx2]);
                part3.Add(chars[indx3]);
                part4.Add(chars[indx4]);

                indx1 += indx2; indx1 = indx1 >= chars.Count ? indx1 - chars.Count : indx1;
                indx2 += indx3; indx2 = indx2 >= chars.Count ? indx2 - chars.Count : indx2;
                indx3 += indx4; indx3 = indx3 >= chars.Count ? indx3 - chars.Count : indx3;
                indx4 += indx1; indx4 = indx4 >= chars.Count ? indx4 - chars.Count : indx4;
                part1.Add(chars[indx1]);
                part2.Add(chars[indx2]);
                part3.Add(chars[indx3]);
                part4.Add(chars[indx4]);

                indx1 -= indx3; indx1 = indx1 < 0 ? indx1 + chars.Count : indx1;
                indx2 += indx4; indx2 = indx2 >= chars.Count ? indx2 - chars.Count : indx2;
                indx3 -= indx1; indx3 = indx3 < 0 ? indx3 + chars.Count : indx3;
                indx4 += indx2; indx4 = indx4 >= chars.Count ? indx4 - chars.Count : indx4;
                part1.Add(chars[indx1]);
                part2.Add(chars[indx2]);
                part3.Add(chars[indx3]);
                part4.Add(chars[indx4]);

                indx1 -= indx2; indx1 = indx1 < 0 ? indx1 + chars.Count : indx1;
                indx2 -= indx3; indx2 = indx2 < 0 ? indx2 + chars.Count : indx2;
                indx3 -= indx4; indx3 = indx3 < 0 ? indx3 + chars.Count : indx3;
                indx4 -= indx1; indx4 = indx4 < 0 ? indx4 + chars.Count : indx4;
                part1.Add(chars[indx1]);
                part2.Add(chars[indx2]);
                part3.Add(chars[indx3]);
                part4.Add(chars[indx4]);

                for (int i = 0; i < 5; i++)
                {
                    if (part1[i] != parts[0][i]) return false;
                    if (part2[i] != parts[1][i]) return false;
                    if (part3[i] != parts[2][i]) return false;
                    if (part4[i] != parts[3][i]) return false;
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public new static string ToString(List<List<char>> key)
        {
            var str = new StringBuilder();

            foreach (var part in key)
            {
                foreach (var ch in part)
                {
                    str.Append(ch);
                }

                str.Append("-");
            }

            str.Remove(str.Length - 1, 1);
            return str.ToString();
        }
    }
}
