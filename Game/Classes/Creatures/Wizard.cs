using SFML.Graphics;
using SFML.System;

namespace ChendiAdventures
{
    public sealed class Wizard : Creature
    {
        public readonly Clock DefaultClock;
        private int _shootInterval;
        public float XMaxPos;
        public float XMinPos;

        public Wizard(float x, float y, Texture texture) : base(x, y, texture)
        {
            DefaultClock = new Clock();
            EnergyBall = new EnergyBall(-100, -100, WizardTexture);

            XMinPos = x - 96;
            XMaxPos = x + 128;

            SpeedX = 1f;
            SpeedY = 0f;

            _shootInterval = 10;

            _animLeft = new Animation(this, 0.1f,
                new Vector2i(0, 0),
                new Vector2i(32, 0)
            );
            _animRight = new Animation(this, 0.1f,
                new Vector2i(0, 32),
                new Vector2i(32, 32)
            );

            SetTextureRectanlge(0, 0, 32, 32);
            IsDead = false;

            ApplyDifficulty();
        }

        public EnergyBall EnergyBall { get; }

        public override void UpdateTextures()
        {
            if (SpeedX > 0) _animRight.Animate();
            else _animLeft.Animate();
        }

        public new void UpdateCreature(MainCharacter character)
        {
            if (!IsDead)
            {
                X += SpeedX;
                if (Left < XMinPos || Right > XMaxPos) SpeedX *= -1;

                if (DefaultClock.ElapsedTime.AsSeconds() > _shootInterval)
                {
                    EnergyBall.Attack(X + 8, Y + 8);
                    DefaultClock.Restart();
                }
            }

            EnergyBall.UpdateEnergyBall(character);

            UpdateTextures();
        }

        public void Die(Level level)
        {
            sKill.Play();
            level.Particles.Add(new ParticleEffect(X, Y, Color.Red));
            SetPosition(400, -100);
            IsDead = true;
            DefaultClock.Dispose();
        }

        public override void Draw(RenderTarget target, RenderStates states)
        {
            base.Draw(target, states);
            target.Draw(EnergyBall, states);
        }

        public override void ApplyDifficulty()
        {
            switch (MainGameWindow.GameDifficulty)
            {
                case Difficulty.Easy:
                {
                    _shootInterval = 12;
                    SpeedX = 0.5f;
                    Points = 1000;
                    break;
                }
                case Difficulty.Medium:
                {
                    _shootInterval = 10;
                    SpeedX = 1f;
                    Points = 1300;
                    break;
                }
                case Difficulty.Hard:
                {
                    _shootInterval = 8;
                    SpeedX = 2f;
                    Points = 1600;
                    break;
                }
            }
        }
    }
}