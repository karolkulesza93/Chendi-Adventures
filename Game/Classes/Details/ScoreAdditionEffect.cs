using SFML.System;
using SFML.Graphics;

namespace Game
{
    public class ScoreAdditionEffect : Drawable
    {
        private int counter;
        public bool toDestroy { get; set; }
        public TextLine Line { get; set; }
        public ScoreAdditionEffect(int value, float x, float y)
        {
            this.counter = 0;
            this.toDestroy = false;
            this.Line = new TextLine(value.ToString(), 10, x, y, Color.White);
        }
        public void UpdateScoreAdditionEffect()
        {
            if (this.counter < 30)
            {
                this.Line.MoveText(this.Line.X, this.Line.Y - 0.5f);
                this.counter++;
            }
            else
            {
                this.toDestroy = true;
            }
        }
        public void Draw(RenderTarget target, RenderStates states)
        {
            if (this.counter < 60)
                target.Draw(this.Line, states);
        }
    }
}
