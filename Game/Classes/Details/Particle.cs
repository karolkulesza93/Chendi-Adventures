using System;
using SFML.Graphics;
using SFML.System;

namespace Game
{
    public class Particle : Drawable
    {
        private static readonly Random _rnd = new Random();
        private readonly CircleShape _circle;
        public float GravityForce = 0.5f;

        public Particle(float speedx, float speedy, float x, float y, Color color)
        {
            _circle = new CircleShape((float) _rnd.Next(30, 200) / 100);
            _circle.FillColor = color;
            _circle.Position = new Vector2f(x, y);
            SpeedX = speedx;
            SpeedY = speedy;
        }

        public float X
        {
            get => _circle.Position.X;
            set => _circle.Position = new Vector2f(value, _circle.Position.Y);
        }

        public float Y
        {
            get => _circle.Position.Y;
            set => _circle.Position = new Vector2f(_circle.Position.X, value);
        }

        public float SpeedX { get; }
        public float SpeedY { get; private set; }

        public void Draw(RenderTarget target, RenderStates states)
        {
            target.Draw(_circle, states);
        }

        public void Update()
        {
            X += SpeedX;
            Y += SpeedY;
            if (SpeedY < 10) SpeedY += GravityForce;
        }
    }
}