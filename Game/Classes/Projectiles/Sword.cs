using SFML.Audio;
using SFML.Graphics;
using SFML.System;

namespace ChendiAdventures
{
    public sealed class Sword : Entity
    {
        private readonly Animation _animLeft;
        private readonly Animation _animRight;
        private readonly Animation _animUp;
        private readonly MainCharacter _character;
        private readonly float _frameTime;
        public Movement LastMove;

        public Sword(MainCharacter character) : base(-400, -400, SwordTexture)
        {
            _character = character;
            LastMove = Movement.Right;
            _frameTime = 0.05f;

            _animLeft = new Animation(this, _frameTime,
                new Vector2i(0, 30),
                new Vector2i(30, 30),
                new Vector2i(60, 30),
                new Vector2i(90, 30),
                new Vector2i(120, 30));
            _animRight = new Animation(this, _frameTime,
                new Vector2i(0, 0),
                new Vector2i(30, 0),
                new Vector2i(60, 0),
                new Vector2i(90, 0),
                new Vector2i(120, 0));
            _animUp = new Animation(this, _frameTime,
                new Vector2i(0, 60),
                new Vector2i(30, 60),
                new Vector2i(60, 60),
                new Vector2i(90, 60),
                new Vector2i(120, 60));

            sWood = new Sound(new SoundBuffer(@"sfx/wood.wav"));
        }

        public Sound sWood { get; }

        public void SwordCollisionCheck(Level level)
        {
            if (X > 0 && Y > 0)
            {
                Block obstacle;
                if ((obstacle = level.GetObstacle(Get32Position().X, Get32Position().Y)).Type == BlockType.Wood ||
                    (obstacle = level.GetObstacle(Get32Position().X + 0.9375f, Get32Position().Y)).Type ==
                    BlockType.Wood ||
                    (obstacle = level.GetObstacle(Get32Position().X + 0.9375f, Get32Position().Y + 0.9375f)).Type ==
                    BlockType.Wood ||
                    (obstacle = level.GetObstacle(Get32Position().X, Get32Position().Y + 0.9375f)).Type ==
                    BlockType.Wood)
                {
                    obstacle.DeleteObstacle();
                    level.Particles.Add(new ParticleEffect(obstacle.OriginalPos.X, obstacle.OriginalPos.Y,
                        new Color(193, 97, 0)));
                    sWood.Play();
                    _character.AddToScore(level, 10, obstacle.X, obstacle.Y);
                }

                if ((obstacle = level.GetObstacle(Get32Position().X, Get32Position().Y)).Type == BlockType.HardBlock ||
                    (obstacle = level.GetObstacle(Get32Position().X + 0.9375f, Get32Position().Y)).Type ==
                    BlockType.HardBlock ||
                    (obstacle = level.GetObstacle(Get32Position().X + 0.9375f, Get32Position().Y + 0.9375f)).Type ==
                    BlockType.HardBlock ||
                    (obstacle = level.GetObstacle(Get32Position().X, Get32Position().Y + 0.9375f)).Type ==
                    BlockType.HardBlock)
                {
                    level.Particles.Add(new ParticleEffect(obstacle.OriginalPos.X, obstacle.OriginalPos.Y,
                        new Color(57, 65, 81), 1));
                    obstacle.HitHardblock();
                    if (obstacle.Health <= 0)
                    {
                        obstacle.DeleteObstacle();
                        Block.sDestroy.Play();
                        level.Particles.Add(new ParticleEffect(obstacle.OriginalPos.X, obstacle.OriginalPos.Y,
                            new Color(57, 65, 81)));
                        _character.AddToScore(level, 100, obstacle.X, obstacle.Y);
                    }
                }





                foreach (var monster in level.Monsters)
                    if (GetBoundingBox().Intersects(monster.GetBoundingBox()))
                    {
                        _character.AddToScore(level, monster.Points, monster.X, monster.Y);
                        level.Particles.Add(new ParticleEffect(monster.X, monster.Y, Color.Red));
                        monster.Die(level);
                    }

                foreach (var archer in level.Archers)
                    if (GetBoundingBox().Intersects(archer.GetBoundingBox()))
                    {
                        _character.AddToScore(level, archer.Points, archer.X, archer.Y);
                        level.Particles.Add(new ParticleEffect(archer.X, archer.Y, Color.Red));
                        archer.Die(level);
                    }

                foreach (var wizard in level.Wizards)
                    if (GetBoundingBox().Intersects(wizard.GetBoundingBox()))
                    {
                        _character.AddToScore(level, wizard.Points, wizard.X, wizard.Y);
                        level.Particles.Add(new ParticleEffect(wizard.X, wizard.Y, Color.Red));
                        wizard.Die(level);
                    }
            }
        }

        public void Attack()
        {
            if (LastMove == Movement.Left)
            {
                SetPosition(_character.X - Width, _character.Y + 1f);
                _animLeft.Animate(30);
            }
            else if (LastMove == Movement.Right)
            {
                SetPosition(_character.X + _character.Width, _character.Y + 1f);
                _animRight.Animate(30);
            }
        }

        public void AttackUp()
        {
            SetPosition(_character.X + 1f, _character.Y - Height);
            _animUp.Animate(30);
        }

        public void Reset()
        {
            _animLeft.ResetAnimation();
            _animRight.ResetAnimation();
            _animUp.ResetAnimation();
            SetPosition(-400, -400);
        }
    }
}