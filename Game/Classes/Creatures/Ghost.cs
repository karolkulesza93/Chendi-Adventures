using System;
using SFML.System;
using SFML.Audio;
using SFML.Graphics;

namespace Game
{
    public class Ghost : Creature
    {
        public float Speed { get; private set; }
        public float ProcsDistance { get; private set; }
        public Sound sGhost { get; private set; }
        public Clock DefaultClock { get; private set; }
        public Ghost(float x, float y, Texture texture) : base(x,y,texture)
        {
            this.Speed = 1f;
            this.ProcsDistance = 500f;

            this._animLeft = new Animation(this, 0.2f,
                new Vector2i(0,0),
                new Vector2i(32,0)
                );

            this._animRight = new Animation(this, 0.2f,
                new Vector2i(0,32),
                new Vector2i(32,32));

            this.DefaultClock = new Clock();

            this.IsDead = false;
            this.SetTextureRectanlge(0, 0, 32, 32);
            this.sGhost = new Sound(new SoundBuffer(@"sfx/ghost.wav"));
            this.sGhost.Volume = 40;
        }
        public void UpdateGhostTexture()
        {
            if (this.SpeedX < 0) this._animLeft.Animate();
            else this._animRight.Animate();
        }
        public void UpdateGhost(Level level, MainCahracter character)
        {
            this.UpdateGhostTexture();

            if (!this.IsDead && (float)Math.Sqrt(Math.Pow(this.GetCenterPosition().X - character.GetCenterPosition().X, 2) + Math.Pow(this.GetCenterPosition().Y - character.GetCenterPosition().Y , 2)) < this.ProcsDistance)
            {
                if (this.GetCenterPosition().X > character.GetCenterPosition().X) this.SpeedX = -1 * this.Speed;
                else this.SpeedX = this.Speed;

                if (this.GetCenterPosition().Y > character.GetCenterPosition().Y) this.SpeedY = -1 * this.Speed;
                else this.SpeedY = this.Speed;

                this.X += this.SpeedX;
                this.Y += this.SpeedY;
            }
            if (!this.IsDead && this.DefaultClock.ElapsedTime.AsSeconds() > 10)
            {
                this.sGhost.Play();
                this.DefaultClock.Restart();
            }
        }
        public void Die(Level level)
        {
            sKill.Play();
            level.Particles.Add(new ParticleEffect(this.X, this.Y, Color.Cyan));
            this.SetPosition(400, -100);
            this.IsDead = true;
            this.DefaultClock.Dispose();
        }
    }
}
