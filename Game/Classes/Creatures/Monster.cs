using SFML.Graphics;
using SFML.System;

namespace Game
{
    public sealed class Monster : Creature
    {
        public float XMaxPos;
        public float XMinPos;

        public Monster(float x, float y, Texture texture) : base(x, y, texture)
        {
            XMinPos = x - 100;
            XMaxPos = x + 100;

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

            SetTextureRectanlge(0, 0, 32, 32);
            IsDead = false;
        }

        public override void UpdateTextures()
        {
            if (SpeedX > 0) _animRight.Animate();
            else _animLeft.Animate();
        }

        public void MonsterUpdate()
        {
            X += SpeedX;
            if (Left < XMinPos || Right > XMaxPos) SpeedX *= -1;
            UpdateTextures();
        }

        public void Die(Level level)
        {
            sKill.Play();
            level.Particles.Add(new ParticleEffect(X, Y, Color.Red));

            SetPosition(400, -100);
            IsDead = true;
        }
    }
}