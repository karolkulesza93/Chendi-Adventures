
using SFML.System;
using SFML.Graphics;

namespace Game
{
    public class Wizard : Creature
    {
        private Clock _defaultClock;
        public EnergyBall EnergyBall { get; private set; }
        public float XMaxPos;
        public float XMinPos;
        public Wizard(float x, float y, Texture texture) : base(x,y,texture)
        {
            this._defaultClock = new Clock();
            this.EnergyBall = new EnergyBall(-100, -100, WizardTexture);

            this.XMinPos = x - 100;
            this.XMaxPos = x + 100;

            this.SpeedX = 1f;
            this.SpeedY = 0f;

            this._animLeft = new Animation(this, 0.1f,
                new Vector2i(0, 0),
                new Vector2i(32, 0)
                );
            this._animRight = new Animation(this, 0.1f,
                new Vector2i(0, 32),
                new Vector2i(32, 32)
                );

            this.SetTextureRectanlge(0, 0, 32, 32);
            this.IsDead = false;
        }
        public override void UpdateTextures()
        {
            if (this.SpeedX > 0) this._animRight.Animate();
            else this._animLeft.Animate();
        }
        public void WizardUpdate(MainCahracter character)
        {
            if (!this.IsDead)
            {
                this.X += this.SpeedX;
                if (this.Left < this.XMinPos || this.Right > this.XMaxPos) this.SpeedX *= -1;

                if (this._defaultClock.ElapsedTime.AsSeconds() > 10)
                {
                    this.EnergyBall.Attack(this.X + 8, this.Y + 8);
                    this._defaultClock.Restart();
                }
            }

            this.EnergyBall.UpdateEnergyBall(character);

            this.UpdateTextures();
        }
        public void Die(Level level)
        {
            sKill.Play();
            level.Particles.Add(new ParticleEffect(this.X, this.Y, Color.Red));
            this.SetPosition(400, -100);
            this.IsDead = true;
            this._defaultClock.Dispose();
        }

        public override void Draw(RenderTarget target, RenderStates states)
        {
            base.Draw(target, states);
            target.Draw(this.EnergyBall, states);
        }
    }
}
