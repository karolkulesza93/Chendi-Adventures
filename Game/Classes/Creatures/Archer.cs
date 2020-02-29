using SFML.System;
using SFML.Graphics;

namespace Game
{
    public class Archer : Creature
    {
        public Clock DefaultClock { get; set; }
        public EnemyArrow Arrow { get; set; }
        public Movement Direction { get; set; }
        public bool isDrawing { get; private set; }
        public Archer(float x, float y, Texture texture, Movement dir) : base(x,y,texture)
        {
            this.DefaultClock = new Clock();

            this.SpeedX = 13f;
            this.Arrow = new EnemyArrow(-100, -100, ArrowTexture, dir);
            this.Direction = dir;
            this.isDrawing = false;

            this.IsDead = false;

            switch (this.Direction)
            {
                case Movement.Left:
                {
                    this.SetTextureRectanlge(0, 32, 32, 32);
                    this.Arrow.SetTextureRectanlge(0, 14, 32, 7);

                    this.Arrow.SpeedX = -1 * this.Arrow.SpeedX;
                    break;
                }
                case Movement.Right:
                {
                    this.SetTextureRectanlge(0, 0, 32, 32);
                    this.Arrow.SetTextureRectanlge(0, 21, 32, 7);
                    break;
                }
            }
        }
        public void UpdateArcher(Level level)
        {
            //arrow
            this.Arrow.ProjectileUpdate(level);
            //textures
            if (this.DefaultClock.ElapsedTime.AsSeconds() > 5 && !this.isDrawing && !this.IsDead)
            {
                this.isDrawing = true;
                Projectile.sDraw.Play();
                switch (this.Direction)
                {
                    case Movement.Left:
                    {
                            this.SetTextureRectanlge(32, 32, 32, 32);
                            break;
                    }
                    case Movement.Right:
                    {
                            this.SetTextureRectanlge(32, 0, 32, 32);
                            break;
                    }
                }
            }
            else if (this.DefaultClock.ElapsedTime.AsSeconds() > 7 && !this.IsDead)
            {
                this.DefaultClock.Restart();
                this.isDrawing = false;
                Projectile.sShoot.Play();
                this.Arrow.SetPosition(this.X, this.Y + 12);

                switch (this.Direction)
                {
                    case Movement.Left:
                        {
                            this.SetTextureRectanlge(0, 32, 32, 32);
                            break;
                        }
                    case Movement.Right:
                        {
                            this.SetTextureRectanlge(0, 0, 32, 32);
                            break;
                        }
                }
            }
        }
        public override void Draw(RenderTarget target, RenderStates states)
        {
            base.Draw(target, states);
            target.Draw(this.Arrow, states);
        }
        public void Die(Level level)
        {
            this.IsDead = true;
            sKill.Play();
            level.Particles.Add(new ParticleEffect(this.X, this.Y, Color.Red));
            Projectile.sDraw.Stop();
            this.SetPosition(500, -100);
        }
    }
}
