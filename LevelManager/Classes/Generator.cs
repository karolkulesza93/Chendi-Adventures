using System.IO;
using System.Text;
using System.Collections.Generic;
using System;

namespace LevelManager
{
    public sealed class Generator
    {
        public static string Title { get; set; }

        public static string GenerateTemplate(int width, int height, bool edit = false)
        {
            StringBuilder generatedTemplate = new StringBuilder();
            StringBuilder voidLine = new StringBuilder();
            StringBuilder brickLine = new StringBuilder();

            for (int i = 0; i < width; i++)
            {
                brickLine.Append("W");
            }
            brickLine.Append("\r\n");

            voidLine.Append("W");
            for (int i = 0; i < width - 2; i++)
            {
                voidLine.Append(".");
            }
            voidLine.Append("W\r\n");

            generatedTemplate.Append(brickLine.ToString());
            for (int i = 0; i < height - 2; i++)
            {
                generatedTemplate.Append(voidLine.ToString());
            }
            generatedTemplate.Append(brickLine.ToString());

            generatedTemplate.Remove(generatedTemplate.Length - 2, 2);


            if (!edit)
            {
                Title = string.Format("{0}x{1}", width, height);
                File.WriteAllText(string.Format(@"C:\Users\Karol Kulesza\source\repos\Game\Game\bin\Chendi Adventures\levels\templates\{0}.dat", Title), generatedTemplate.ToString());
            }
            else
            {
                Title = "edit";
                File.WriteAllText(string.Format(@"C:\Users\Karol Kulesza\source\repos\Game\Game\bin\Chendi Adventures\levels\{0}.dat", Title), generatedTemplate.ToString());
            }


            return generatedTemplate.ToString();
        }

        public static string GenerateLevel(string title, int width, int height,
            bool knights = false, bool archers = false, bool ghosts = false, bool wizards = false,
            bool crushers = false, bool spikes = false, bool blowtorches = false,
            bool silverDoor = false, bool goldenDoor = false, bool teleports = false, bool shop = false, bool spikeballs = true, bool trampolines = true
        )
        {
            List<List<char>> level = new List<List<char>>();
            Random rnd = new Random();

            width *= 8;
            height *= 8;

            for (int i = 0; i < height; i++)
            {
                level.Add(new List<char>());
                if (i == 0 || i == height - 1)
                {
                    for (int j = 0; j < width; j++)
                    {
                        level[i].Add('D');
                    }
                }
                else
                {
                    for (int j = 0; j < width; j++)
                    {
                        if (j == 0 || j == width - 1)
                        {
                            level[i].Add('D');
                        }
                        else
                        {
                            level[i].Add('.');
                        }
                    }
                }
            }

            //==//==//==//==//==//==//==//==//==//==//==//==//

            int R = rnd.Next(2);

            //enterance and exit
















            //==//==//==//==//==//==//==//==//==//==//==//==//

            Title = string.Format("{0}_{1}x{2}", title, width, height);
            File.WriteAllText(string.Format(@"C:\Users\Karol Kulesza\source\repos\Game\Game\bin\Chendi Adventures\levels\generated\{0}.dat", Title), List2dToString(level));
            return List2dToString(level);
        }

        private static string List2dToString(List<List<char>> matrix)
        {
            StringBuilder str = new StringBuilder();

            foreach (var row in matrix)
            {
                foreach (var cell in row)
                {
                    str.Append(cell);
                }
                str.Append("\r\n");
            }
            str.Remove(str.Length - 2, 2);

            return str.ToString();
        }
    }
}
