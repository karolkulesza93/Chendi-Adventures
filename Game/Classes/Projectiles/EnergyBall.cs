using SFML.Audio;
using SFML.Graphics;
using SFML.System;

namespace ChendiAdventures
{
    public class EnergyBall : Projectile
    {
        private readonly Animation _anim;
        public Clock DefaultTimer;
        private readonly Sound sAtk;

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
                if (GetCenterPosition().X >= character.GetCenterPosition().X) SpeedX = -1 * Speed;
                else SpeedX = Speed;

                if (GetCenterPosition().Y >= character.GetCenterPosition().Y) SpeedY = -1 * Speed;
                else SpeedY = Speed;

                X += SpeedX;
                Y += SpeedY;

                if (DefaultTimer.ElapsedTime.AsSeconds() > 5)
                {
                    ResetEnergyBall(level);
                }

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
    }
}