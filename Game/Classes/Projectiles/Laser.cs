using System;
using SFML.Graphics;
using SFML.System;

namespace ChendiAdventures
{
    public sealed class Laser : Projectile
    {
        public float Speed { get; set; }
        public float SpeedY { get; set; }

        public Laser(float x, float y, Texture texture, Movement dir) : base(x, y, texture, dir)
        {
            Speed = 20f;
            SetTextureRectangle(0,64,6,6);
            ApplyDifficulty();
        }

        public void LaserUpdate(Level level, MainCharacter character)
        {
            Block obstacle = level.GetObstacle((X+3)/32, (Y+3)/32);
            if (X+3 > 0 && X+3 < level.LevelWidth * 32 &&
                Y+3 > 0 && Y+3 < level.LevelHeight * 32)
            {
                X += SpeedX;
                Y += SpeedY;
                if (level.UnpassableContains(obstacle.Type))
                {
                    level.Particles.Add(new ParticleEffect(X-16,Y-16,Color.Green, 5));
                    DeleteArrow();
                    if (obstacle.Type == BlockType.Wood)
                    {
                        character.Sword.sWood.Play();
                        level.Particles.Add(new ParticleEffect(obstacle.OriginalPos.X, obstacle.OriginalPos.Y,
                            new Color(193, 97, 0)));
                        obstacle.DeleteObstacle();
                    }
                }
            }
        }

        public void Attack(MainCharacter character)
        {
            _speed = new Vector2f(character.X - X, character.Y - Y);
            float wersor = (float)1 / (float)Math.Sqrt(_speed.X * _speed.X + _speed.Y * _speed.Y);
            SpeedX = _speed.X * wersor * Speed;
            SpeedY = _speed.Y * wersor * Speed;
        }

        public void ApplyDifficulty()
        {
            switch (MainGameWindow.GameDifficulty)
            {
                case Difficulty.Easy:
                {
                    Speed = 8f;
                    break;
                }
                case Difficulty.Medium:
                {
                    Speed = 12f;
                    break;
                }
                case Difficulty.Hard:
                {
                    Speed = 16f;
                    break;
                }
            }
        }
        private Vector2f _speed;
    }
}
