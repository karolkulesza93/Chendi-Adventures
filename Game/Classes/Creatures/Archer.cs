using SFML.Graphics;
using SFML.System;

namespace ChendiAdventures
{
    public sealed class Archer : Creature
    {
        private int _shootInterval;
        public Archer(float x, float y, Texture texture, Movement dir) : base(x, y, texture)
        {
            DefaultClock = new Clock();
            _shootInterval = 5;

            SpeedX = 13f;
            Arrow = new EnemyArrow(-100, -100, ArrowTexture, dir);
            Direction = dir;
            isDrawing = false;

            IsDead = false;

            switch (Direction)
            {
                case Movement.Left:
                {
                    SetTextureRectanlge(0, 32, 32, 32);
                    Arrow.SetTextureRectanlge(0, 14, 32, 7);

                    Arrow.SpeedX = -1 * Arrow.SpeedX;
                    break;
                }
                case Movement.Right:
                {
                    SetTextureRectanlge(0, 0, 32, 32);
                    Arrow.SetTextureRectanlge(0, 21, 32, 7);
                    break;
                }
            }

            ApplyDifficulty();
        }

        public Clock DefaultClock { get; set; }
        public EnemyArrow Arrow { get; set; }
        public Movement Direction { get; set; }
        public bool isDrawing { get; private set; }

        public void UpdateCreature(Level level)
        {
            //arrow
            Arrow.ProjectileUpdate(level);
            //textures
            if (DefaultClock.ElapsedTime.AsSeconds() > _shootInterval && !isDrawing && !IsDead)
            {
                isDrawing = true;
                Projectile.sDraw.Play();
                switch (Direction)
                {
                    case Movement.Left:
                    {
                        SetTextureRectanlge(32, 32, 32, 32);
                        break;
                    }
                    case Movement.Right:
                    {
                        SetTextureRectanlge(32, 0, 32, 32);
                        break;
                    }
                }
            }
            else if (DefaultClock.ElapsedTime.AsSeconds() > _shootInterval + 2 && !IsDead)
            {
                DefaultClock.Restart();
                isDrawing = false;
                Projectile.sShoot.Play();
                Arrow.SetPosition(X, Y + 12);

                switch (Direction)
                {
                    case Movement.Left:
                    {
                        SetTextureRectanlge(0, 32, 32, 32);
                        break;
                    }
                    case Movement.Right:
                    {
                        SetTextureRectanlge(0, 0, 32, 32);
                        break;
                    }
                }
            }
        }

        public override void Draw(RenderTarget target, RenderStates states)
        {
            base.Draw(target, states);
            target.Draw(Arrow, states);
        }

        public override void Die(Level level)
        {
            IsDead = true;
            sKill.Play();
            level.Particles.Add(new ParticleEffect(X, Y, Color.Red));
            Projectile.sDraw.Stop();
            SetPosition(500, -100);
        }

        public override void ApplyDifficulty()
        {
            switch (MainGameWindow.GameDifficulty)
            {
                case Difficulty.Easy:
                {
                    _shootInterval = 7;
                    Points = 300;
                    break;
                }
                case Difficulty.Medium:
                {
                    _shootInterval = 5;
                    Points = 500;
                    break;
                }
                case Difficulty.Hard:
                {
                    _shootInterval = 3;
                    Points = 700;
                    break;
                }
            }
        }
    }
}