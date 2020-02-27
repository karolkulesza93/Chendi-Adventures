using SFML.Graphics;
using SFML.System;
using System;

namespace Game
{
    public class Particle : Drawable
    {
        private CircleShape _circle;
        private static Random _rnd = new Random();
        public float X
        {
            get
            {
                return this._circle.Position.X;
            }
            set
            {
                this._circle.Position = new Vector2f(value, this._circle.Position.Y);
            }
        }
        public float Y
        {
            get
            {
                return this._circle.Position.Y;
            }
            set
            {
                this._circle.Position = new Vector2f(this._circle.Position.X, value);
            }
        }
        public float SpeedX { get; private set; }
        public float SpeedY { get; private set; }
        public float GravityForce = 0.5f;
        public Particle(float speedx, float speedy, float x, float y, Color color)
        {
            this._circle = new CircleShape((float)_rnd.Next(30,200)/100);
            this._circle.FillColor = color;
            this._circle.Position = new Vector2f(x, y);
            this.SpeedX = speedx;
            this.SpeedY = speedy;
        }
        public void Update()
        {
            this.X += this.SpeedX;
            this.Y += this.SpeedY;
            if (this.SpeedY < 10) this.SpeedY += this.GravityForce;
        }
        public void Draw(RenderTarget target, RenderStates states)
        {
            target.Draw(this._circle, states);
        }
    }
}
