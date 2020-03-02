using SFML.Graphics;
using SFML.System;

namespace Game
{
    public class Wizard : Creature
    {
        private readonly Clock _defaultClock;
        public float XMaxPos;
        public float XMinPos;

        public Wizard(float x, float y, Texture texture) : base(x, y, texture)
        {
            _defaultClock = new Clock();
            EnergyBall = new EnergyBall(-100, -100, WizardTexture);

            XMinPos = x - 100;
            XMaxPos = x + 100;

            SpeedX = 1f;
            SpeedY = 0f;

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
        }

        public EnergyBall EnergyBall { get; }

        public override void UpdateTextures()
        {
            if (SpeedX > 0) _animRight.Animate();
            else _animLeft.Animate();
        }

        public void WizardUpdate(MainCharacter character)
        {
            if (!IsDead)
            {
                X += SpeedX;
                if (Left < XMinPos || Right > XMaxPos) SpeedX *= -1;

                if (_defaultClock.ElapsedTime.AsSeconds() > 10)
                {
                    EnergyBall.Attack(X + 8, Y + 8);
                    _defaultClock.Restart();
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
            _defaultClock.Dispose();
        }

        public override void Draw(RenderTarget target, RenderStates states)
        {
            base.Draw(target, states);
            target.Draw(EnergyBall, states);
        }
    }
}