using SFML.Graphics;
using SFML.System;

namespace ChendiAdventures
{
    public sealed class Arrow : Projectile
    {
        public Arrow(float x, float y, Texture texture, Movement dir) : base(x, y, texture, dir)
        {
            isEnergized = false;
            LastMove = Movement.Right;
        }

        public bool isEnergized { get; set; }
        public Movement LastMove { get; set; }

        public void ArrowUpdate(MainCharacter character, Level level)
        {
            switch (LastMove)
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

            Block obstacle = level.GetObstacle(TipPosition.X / 32, TipPosition.Y / 32);
            if (TipPosition.X > 0 && TipPosition.X < level.LevelWidth * 32 &&
                TipPosition.Y > 0 && TipPosition.Y < level.LevelHeight * 32)
            {
                X += SpeedX;
                if (obstacle.Type == BlockType.EnergyBall && isEnergized)
                {
                    character.AddToScore(level, 250, obstacle.X, obstacle.Y);
                    obstacle.Shatter();
                    level.Particles.Add(new ParticleEffect(obstacle.OriginalPos.X, obstacle.OriginalPos.Y,
                        Color.Yellow, 10));
                    DeleteArrow();
                }

                if (level.UnpassableContains(obstacle.Type) && obstacle.Type != BlockType.BrokenBrick) DeleteArrow();
            }

            foreach (var monster in level.Monsters)
                if (GetBoundingBox().Intersects(monster.GetBoundingBox()))
                {
                    character.AddToScore(level, monster.Points, monster.X, monster.Y);
                    monster.Die(level);
                    if (!isEnergized) DeleteArrow();
                }

            foreach (var archer in level.Archers)
                if (GetBoundingBox().Intersects(archer.GetBoundingBox()))
                {
                    character.AddToScore(level, archer.Points, archer.X, archer.Y);
                    archer.Die(level);
                    if (!isEnergized) DeleteArrow();
                }

            foreach (var wizard in level.Wizards)
                if (GetBoundingBox().Intersects(wizard.GetBoundingBox()))
                {
                    character.AddToScore(level, wizard.Points, wizard.X, wizard.Y);
                    wizard.Die(level);
                    if (!isEnergized) DeleteArrow();
                }

            foreach (var golem in level.Golems)
                if (GetBoundingBox().Intersects(golem.GetBoundingBox()))
                {
                    if (!isEnergized)
                    {
                        DeleteArrow();
                    }
                    else
                    {
                        level.Particles.Add(new ParticleEffect(golem.X, golem.Y,
                            new Color(100, 100, 100), 10));
                        level.Particles.Add(new ParticleEffect(golem.X, golem.Y,
                            Color.Yellow, 5));
                        golem.Health -= 2;
                        sEnergyHit.Play();
                        DeleteArrow();
                    }
                }

            if (isEnergized)
                foreach (var ghost in level.Ghosts)
                    if (GetBoundingBox().Intersects(ghost.GetBoundingBox()))
                    {
                        character.AddToScore(level, ghost.Points, ghost.X, ghost.Y);
                        ghost.Die(level);
                        DeleteArrow();
                    }
        }

        public void ArrowDirectionDefine(Movement dir)
        {
            LastMove = dir;
            switch (LastMove)
            {
                case Movement.Left:
                {
                    TipPosition = new Vector2f(X, Y + Height / 2);
                    SpeedX = -13f;
                    SetTextureRectangle(0, !isEnergized ? 0 : 28, 32, 7);
                    break;
                }
                case Movement.Right:
                {
                    TipPosition = new Vector2f(X + Width, Y + Height / 2);
                    SpeedX = 13f;
                    SetTextureRectangle(0, !isEnergized ? 7 : 35, 32, 7);
                    break;
                }
            }
        }
    }
}