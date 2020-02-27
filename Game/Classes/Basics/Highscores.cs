using System.IO;
using System.Collections.Generic;
using System.Text;
using SFML.Graphics;

namespace Game
{
    public class HighscoreRecord
    {
        public int Score { get; set; }
        public int Level { get; set; }
        public HighscoreRecord(int score, int level)
        {
            this.Score = score;
            this.Level = level;
        }
        public override string ToString()
        {
            var str = new StringBuilder();

            str.Append(this.Score);
            for (int i = 0; i < 8 - Score.ToString().Length; i++)
                str.Append(" ");
            str.Append("LEVEL ");
            str.Append(this.Level);

            return str.ToString();
        }
    }
    public class Highscores : Drawable
    {
        private string _path;
        private int _maxAmountOfRecords;
        private TextLine _highscores;

        public readonly List<HighscoreRecord> Scores;
        public Highscores()
        {
            this._path = @"highscores.dat";
            this._maxAmountOfRecords = 20;
            this._highscores = new TextLine("", 50, 470, 70, Color.White);

            this.Scores = new List<HighscoreRecord>();
            this.Scores.Clear();
            this.LoadHighscores();
        }
        public void AddNewRecord(HighscoreRecord record)
        {
            for (int i = 0; i < this.Scores.Count; i++)
            {
                if (record.Score > this.Scores[i].Score)
                {
                    this.Scores.Insert(i, record);
                    break;
                }
            }

            while (this.Scores.Count > this._maxAmountOfRecords)
                this.Scores.RemoveAt(this.Scores.Count - 1);

            this.SaveHighscores();
            this._highscores.EditText(this.GetHighscores());
        }
        public void LoadHighscores()
        {
            string[] content = File.ReadAllLines(this._path);
            this.Scores.Clear();

            foreach (string line in content)
            {
                if (line == "\n" || line == "" || line == " " || line == "\r") break;
                string[] tmp = line.Split(' ');
                this.Scores.Add(new HighscoreRecord(int.Parse(tmp[0]), int.Parse(tmp[1])));
            }

            this._highscores.EditText(this.GetHighscores());
        }
        public string GetHighscores()
        {
            var str = new StringBuilder();

            for (int i = 0; i < this.Scores.Count; i++)
            {
                str.Append(i + 1);
                if (i + 1 < 10) str.Append(".  ");
                else str.Append(". ");
                str.Append(this.Scores[i].ToString());
                str.Append("\n");
            }

            return str.ToString();
        }
        public void SaveHighscores()
        {
            var str = new StringBuilder();

            foreach (HighscoreRecord record in this.Scores)
            {
                str.Append(record.Score + " " + record.Level + "\n");
            }

            File.WriteAllText(this._path, str.ToString());
        }
        public void Draw(RenderTarget target, RenderStates states)
        {
            target.Draw(this._highscores, states);
        }
    }
}
