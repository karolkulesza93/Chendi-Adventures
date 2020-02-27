using SFML.System;
using SFML.Audio;
using SFML.Graphics;

namespace Game
{
    public abstract class Projectile : Entity
    {
        public float SpeedX { get; set; }
        public Vector2f TipPosition { get; set; }
        public static Sound sHit = new Sound(new SoundBuffer(@"sfx/hit.wav"));
        public static Sound sEnergyHit = new Sound(new SoundBuffer(@"sfx/energyhit.wav"));
        public static Sound sDraw = new Sound(new SoundBuffer(@"sfx/draw.wav"));
        public static Sound sShoot { get; private set; } = new Sound(new SoundBuffer(@"sfx/shoot.wav"));
        public static Sound sEnergyShoot = new Sound(new SoundBuffer(@"sfx/energyshoot.wav"));
        private Movement _direction;
        public Projectile(float x, float y, Texture texture, Movement dir) : base(x, y, texture)
        { 
            this.SpeedX = 13f;
            this._direction = dir;
        }
        public virtual void ProjectileUpdate(Level level)
        {
            switch (this._direction)
            {
                case Movement.Left:
                    {
                        this.TipPosition = new Vector2f(this.X, this.Y + this.Height / 2);
                        break;
                    }
                case Movement.Right:
                    {
                        this.TipPosition = new Vector2f(this.X + this.Width, this.Y + this.Height / 2);
                        break;
                    }
            }
            Block obstacle;
            if (this.TipPosition.X > 0 && this.TipPosition.X < level.LevelWidth * 32 &&
                this.TipPosition.Y > 0 && this.TipPosition.Y < level.LevelHeight * 32)
            {
                this.X += this.SpeedX;
                if (level.UnpassableContains((obstacle = level.GetObstacle(this.TipPosition.X / 32, this.TipPosition.Y / 32)).Type))
                {
                    this.DeleteArrow();
                }
            }
        }
        public void DeleteArrow()
        {
            sHit.Play();
            this.X = -100;
            this.Y = 400;
        }
            
    }
}
