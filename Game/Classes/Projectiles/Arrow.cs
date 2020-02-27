using SFML.System;
using SFML.Graphics;

namespace Game
{
    public sealed class Arrow : Projectile
    {
        public bool isEnergized { get; set; }
        public Movement LastMove { get; set; }
        public Arrow(float x, float y, Texture texture, Movement dir) : base(x,y,texture,dir)
        {
            this.isEnergized = false;
        }
        public void ArrowUpdate(MainCahracter character, Level level)
        {
            foreach (Monster Monster in level.Monsters)
            {
                if (this.GetBoundingBox().Intersects(Monster.GetBoundingBox()))
                {
                    character.AddToScore(level, 300, Monster.X, Monster.Y);
                    Monster.Die(level);
                    sHit.Play();
                    if (!this.isEnergized) this.DeleteArrow();
                }
            }
            foreach (Archer archer in level.Archers)
            {
                if (this.GetBoundingBox().Intersects(archer.GetBoundingBox()))
                {
                    character.AddToScore(level, 600, archer.X, archer.Y);
                    archer.Die(level);
                    sHit.Play();
                    if (!this.isEnergized) this.DeleteArrow();
                }
            }
            foreach (Wizard wizard in level.Wizards)
            {
                if (this.GetBoundingBox().Intersects(wizard.GetBoundingBox()))
                {
                    character.AddToScore(level, 1800, wizard.X, wizard.Y);
                    wizard.Die(level);
                    sHit.Play();
                    if (!this.isEnergized) this.DeleteArrow();
                }
            }
            if (this.isEnergized)
            {
                foreach (Ghost ghost in level.Ghosts)
                {
                    if (this.GetBoundingBox().Intersects(ghost.GetBoundingBox()))
                    {
                        character.AddToScore(level, 1000, ghost.X, ghost.Y);
                        ghost.Die(level);
                        this.DeleteArrow();
                    }
                }
            }
        }
        public void ArrowDirectionDefine()
        {
            switch (this.LastMove)
            {
                case Movement.Left:
                    {
                        this.TipPosition = new Vector2f(this.X, this.Y + this.Height / 2);
                        this.SpeedX = -13f;
                        break;
                    }
                default:
                    {
                        this.TipPosition = new Vector2f(this.X + this.Width, this.Y + this.Height / 2);
                        this.SpeedX = 13f;
                        break;
                    }
            }
        }
        
    }
}
