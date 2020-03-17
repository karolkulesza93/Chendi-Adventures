using System;
using SFML.Audio;
using SFML.Graphics;
using SFML.System;

namespace ChendiAdventures
{
    public class EnergyBall : Projectile
    {
        public Clock DefaultTimer;

        public EnergyBall(float x, float y, Texture texture, Movement dir = Movement.None) : base(x, y, texture, dir)
        {
            Speed = 1.5f;
            SpeedX = 0f;
            SpeedY = 0f;
            isAttacking = false;

            DefaultTimer = new Clock();

            _anim = new Animation(this, 0.1f,
                new Vector2i(0, 64),
                new Vector2i(16, 64),
                new Vector2i(32, 64),
                new Vector2i(48, 64),
                new Vector2i(32, 64),
                new Vector2i(16, 64)
            );

            sAtk = new Sound(new SoundBuffer("sfx/spell.wav"));
        }

        public float Speed { get; }
        public float SpeedX { get; private set; }
        public float SpeedY { get; private set; }
        public bool isAttacking { get; private set; }

        public void UpdateEnergyBall(MainCharacter character, Level level)
        {
            if (isAttacking)
            {
                _speed = new Vector2f(character.X - X, character.Y - Y);
                float wersor = (float)1 / (float)Math.Sqrt(_speed.X * _speed.X + _speed.Y * _speed.Y);
                SpeedX = _speed.X * wersor * Speed;
                SpeedY = _speed.Y * wersor * Speed;

                X += SpeedX;
                Y += SpeedY;

                if (DefaultTimer.ElapsedTime.AsSeconds() > 5) ResetEnergyBall(level);

                _anim.Animate(16);
            }
        }

        public void Attack(float x, float y)
        {
            sAtk.Play();
            isAttacking = true;
            DefaultTimer.Restart();
            SetPosition(x, y);
        }

        public void ResetEnergyBall(Level level)
        {
            isAttacking = false;
            level.Particles.Add(new ParticleEffect(X - 8, Y - 8, Color.Yellow));
            SetPosition(-100, -100);
        }

        private readonly Animation _anim;
        private readonly Sound sAtk;
        private Vector2f _speed;
    }
}