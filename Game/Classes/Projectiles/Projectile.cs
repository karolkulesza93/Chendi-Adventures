using SFML.Audio;
using SFML.Graphics;
using SFML.System;

namespace ChendiAdventures
{
    public abstract class Projectile : Entity
    {
        public static Sound sHit = new Sound(new SoundBuffer(@"sfx/hit.wav"));
        public static Sound sEnergyHit = new Sound(new SoundBuffer(@"sfx/energyhit.wav"));
        public static Sound sDraw = new Sound(new SoundBuffer(@"sfx/draw.wav"));
        public static Sound sEnergyShoot = new Sound(new SoundBuffer(@"sfx/energyshoot.wav"));

        public Projectile(float x, float y, Texture texture, Movement dir) : base(x, y, texture)
        {
            SpeedX = 15f;
            _direction = dir;

            ApplyDifficulty();
        }

        public float SpeedX { get; set; }
        public Vector2f TipPosition { get; set; }
        public static Sound sShoot { get; } = new Sound(new SoundBuffer(@"sfx/shoot.wav"));

        public virtual void ProjectileUpdate(Level level)
        {
            switch (_direction)
            {
                case Movement.Left:
                {
                    TipPosition = new Vector2f(X, Y + Height / 2);
                    break;
                }
                case Movement.Right:
                {
                    TipPosition = new Vector2f(X + Width, Y + Height / 2);
                    break;
                }
            }

            Block obstacle;
            if (TipPosition.X > 0 && TipPosition.X < level.LevelWidth * 32 &&
                TipPosition.Y > 0 && TipPosition.Y < level.LevelHeight * 32)
            {
                X += SpeedX;
                obstacle = level.GetObstacle(TipPosition.X / 32, TipPosition.Y / 32);
                if (level.UnpassableContains(obstacle.Type) && obstacle.Type != BlockType.BrokenBrick) DeleteArrow();
            }
        }

        public void DeleteArrow()
        {
            sHit.Play();
            X = -100;
            Y = 400;
        }

        public void ApplyDifficulty()
        {
            switch (MainGameWindow.GameDifficulty)
            {
                case Difficulty.Easy:
                {
                    SpeedX = 12f;
                    break;
                }
                case Difficulty.Medium:
                {
                    SpeedX = 15f;
                    break;
                }
                case Difficulty.Hard:
                {
                    SpeedX = 17f;
                    break;
                }
            }
        }

        private readonly Movement _direction;
    }
}