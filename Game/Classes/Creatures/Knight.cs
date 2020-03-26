using SFML.Graphics;
using SFML.System;

namespace ChendiAdventures
{
    public sealed class Knight : Creature
    {
        public float XMaxPos;
        public float XMinPos;

        public Knight(float x, float y, Texture texture) : base(x, y, texture)
        {
            XMinPos = x - 96;
            XMaxPos = x + 128;

            SpeedX = 2f;
            SpeedY = 0f;

            _animLeft = new Animation(this, 0.05f,
                new Vector2i(0, 0),
                new Vector2i(32, 0)
            );
            _animRight = new Animation(this, 0.05f,
                new Vector2i(0, 32),
                new Vector2i(32, 32)
            );

            SetTextureRectangle(0, 0);
            IsDead = false;

            ApplyDifficulty();
        }

        public override void UpdateTextures()
        {
            if (SpeedX > 0) _animRight.Animate();
            else _animLeft.Animate();
        }

        public override void UpdateCreature()
        {
            X += SpeedX;
            if (Left < XMinPos || Right > XMaxPos) SpeedX *= -1;
            UpdateTextures();
        }

        public override void Die(Level level)
        {
            sKill.Play();
            level.Particles.Add(new ParticleEffect(X, Y, Color.Red));
            SetPosition(400, -100);
            IsDead = true;
        }

        public override void ApplyDifficulty()
        {
            switch (MainGameWindow.GameDifficulty)
            {
                case Difficulty.Easy:
                {
                    SpeedX = 1f;
                    Points = 250;
                    break;
                }
                case Difficulty.Medium:
                {
                    SpeedX = 2f;
                    Points = 350;
                    break;
                }
                case Difficulty.Hard:
                {
                    SpeedX = 3f;
                    Points = 500;
                    break;
                }
            }
        }
    }
}