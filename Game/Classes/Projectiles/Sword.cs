using SFML.System;
using SFML.Graphics;
using SFML.Audio;

namespace Game
{
    public sealed class Sword : Entity
    {
        private Animation _animLeft;
        private Animation _animRight;
        private Animation _animUp;
        private float _frameTime;
        public Movement LastMove;
        private MainCahracter _character;
        public Sound sWood { get; private set; }

        public Sword(MainCahracter character) : base(-400, -400, SwordTexture)
        {
            this._character = character;
            this.LastMove = Movement.Right;
            this._frameTime = 0.01f;

            this._animLeft = new Animation(this, this._frameTime,
                new Vector2i(0, 30),
                new Vector2i(30, 30),
                new Vector2i(60, 30),
                new Vector2i(90, 30),
                new Vector2i(120, 30));
            this._animRight = new Animation(this, this._frameTime,
                new Vector2i(0, 0),
                new Vector2i(30, 0),
                new Vector2i(60, 0),
                new Vector2i(90,0),
                new Vector2i(120,0));
            this._animUp = new Animation(this, this._frameTime,
                new Vector2i(0, 60),
                new Vector2i(30, 60),
                new Vector2i(60, 60),
                new Vector2i(90, 60),
                new Vector2i(120, 60));

            this.sWood = new Sound(new SoundBuffer(@"sfx/wood.wav"));
        }
        public void SwordCollisionCheck(Level level)
        {
            if (this.X > 0 && this.Y > 0)
            {
                Block obstacle;
                if ((obstacle = level.GetObstacle(this.Get32Position().X, this.Get32Position().Y)).Type == BlockType.Wood ||
                    (obstacle = level.GetObstacle(this.Get32Position().X + 0.9375f, this.Get32Position().Y)).Type == BlockType.Wood ||
                    (obstacle = level.GetObstacle(this.Get32Position().X + 0.9375f, this.Get32Position().Y + 0.9375f)).Type == BlockType.Wood ||
                    (obstacle = level.GetObstacle(this.Get32Position().X, this.Get32Position().Y + 0.9375f)).Type == BlockType.Wood)
                {
                    obstacle.DeleteObstacle();
                    level.Particles.Add(new ParticleEffect(obstacle.OriginalPos.X, obstacle.OriginalPos.Y, new Color(193, 97, 0)));
                    this.sWood.Play();
                    this._character.AddToScore(level, 10, obstacle.X, obstacle.Y);
                }
                foreach (Monster monster in level.Monsters)
                {
                    if (this.GetBoundingBox().Intersects(monster.GetBoundingBox()))
                    {
                        this._character.AddToScore(level, 250, monster.X, monster.Y);
                        level.Particles.Add(new ParticleEffect(monster.X, monster.Y, Color.Red));
                        monster.Die(level);
                    }
                }
                foreach (Archer archer in level.Archers)
                {
                    if (this.GetBoundingBox().Intersects(archer.GetBoundingBox()))
                    {
                        this._character.AddToScore(level, 500, archer.X, archer.Y);
                        level.Particles.Add(new ParticleEffect(archer.X, archer.Y, Color.Red));
                        archer.Die(level);
                    }
                }
                foreach (Wizard wizard in level.Wizards)
                {
                    if (this.GetBoundingBox().Intersects(wizard.GetBoundingBox()))
                    {
                        this._character.AddToScore(level, 1500, wizard.X, wizard.Y);
                        level.Particles.Add(new ParticleEffect(wizard.X, wizard.Y, Color.Red));
                        wizard.Die(level);
                    }
                }
            }
        }
        public void Attack()
        {
            if (this.LastMove == Movement.Left)
            {
                this.SetPosition(this._character.X - this.Width, this._character.Y + 1f);
                _animLeft.Animate(30);
            }
            else if (this.LastMove == Movement.Right)
            {
                this.SetPosition(this._character.X + this._character.Width, this._character.Y + 1f);
                _animRight.Animate(30);
            }
        }
        public void AttackUp()
        {
            this.SetPosition(this._character.X + 1f, this._character.Y - this.Height);
            _animUp.Animate(30);
        }
        public void Reset()
        {
            this._animLeft.ResetAnimation();
            this._animRight.ResetAnimation();
            this._animUp.ResetAnimation();
            this.SetPosition(-400, -400);
        }
    }
}
