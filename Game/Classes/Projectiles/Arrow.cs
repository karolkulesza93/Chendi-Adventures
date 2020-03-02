using SFML.Graphics;
using SFML.System;

namespace Game
{
    public sealed class Arrow : Projectile
    {
        public Arrow(float x, float y, Texture texture, Movement dir) : base(x, y, texture, dir)
        {
            isEnergized = false;
        }

        public bool isEnergized { get; set; }
        public Movement LastMove { get; set; }

        public void ArrowUpdate(MainCharacter character, Level level)
        {
            foreach (var Monster in level.Monsters)
                if (GetBoundingBox().Intersects(Monster.GetBoundingBox()))
                {
                    character.AddToScore(level, 300, Monster.X, Monster.Y);
                    Monster.Die(level);
                    sHit.Play();
                    if (!isEnergized) DeleteArrow();
                }

            foreach (var archer in level.Archers)
                if (GetBoundingBox().Intersects(archer.GetBoundingBox()))
                {
                    character.AddToScore(level, 600, archer.X, archer.Y);
                    archer.Die(level);
                    sHit.Play();
                    if (!isEnergized) DeleteArrow();
                }

            foreach (var wizard in level.Wizards)
                if (GetBoundingBox().Intersects(wizard.GetBoundingBox()))
                {
                    character.AddToScore(level, 1800, wizard.X, wizard.Y);
                    wizard.Die(level);
                    sHit.Play();
                    if (!isEnergized) DeleteArrow();
                }

            if (isEnergized)
                foreach (var ghost in level.Ghosts)
                    if (GetBoundingBox().Intersects(ghost.GetBoundingBox()))
                    {
                        character.AddToScore(level, 1000, ghost.X, ghost.Y);
                        ghost.Die(level);
                        DeleteArrow();
                    }
        }

        public void ArrowDirectionDefine()
        {
            switch (LastMove)
            {
                case Movement.Left:
                {
                    TipPosition = new Vector2f(X, Y + Height / 2);
                    SpeedX = -13f;
                    break;
                }
                default:
                {
                    TipPosition = new Vector2f(X + Width, Y + Height / 2);
                    SpeedX = 13f;
                    break;
                }
            }
        }
    }
}