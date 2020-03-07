using SFML.Graphics;

namespace ChendiAdventures
{
    public class ScoreAdditionEffect : Drawable
    {
        private int counter;

        public ScoreAdditionEffect(int value, float x, float y)
        {
            counter = 0;
            toDestroy = false;
            Line = new TextLine(value.ToString(), 10, x, y, Color.White);
        }

        public bool toDestroy { get; set; }
        public TextLine Line { get; set; }

        public void Draw(RenderTarget target, RenderStates states)
        {
            if (counter < 60)
                target.Draw(Line, states);
        }

        public void UpdateScoreAdditionEffect()
        {
            if (counter < 30)
            {
                Line.MoveText(Line.X, Line.Y - 0.5f);
                counter++;
            }
            else
            {
                toDestroy = true;
            }
        }
    }
}