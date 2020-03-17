using SFML.Audio;
using SFML.Graphics;
using SFML.System;

namespace ChendiAdventures
{
    public sealed class Sword : Entity
    {
        public Movement LastMove { get; set; }
        public float BounceSpeed { get; set; }
        public Sound sBroke = new Sound(new SoundBuffer(@"sfx/broke.wav"));
        public Sound sWood = new Sound(new SoundBuffer(@"sfx/wood.wav")) {Volume = 50};

        public Sword(MainCharacter character) : base(-400, -400, SwordTexture)
        {
            _character = character;
            LastMove = Movement.Right;
            _frameTime = 0.05f;
            BounceSpeed = -3f;

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
            _animDown = new Animation(this, _frameTime,
                new Vector2i(150, 0),
                new Vector2i(150, 30),
                new Vector2i(150, 60),
                new Vector2i(150, 30)
            );
        }

        public void SwordCollisionCheck(Level level)
        {
            if (_character.IsAttacking)
            {
                Block obstacle = null;
                for (var i = 0; i < 4; i++)
                {
                    switch (i)
                    {
                        case 0:
                        {
                            obstacle = level.GetObstacle(Get32Position().X, Get32Position().Y);
                            break;
                        }
                        case 1:
                        {
                            obstacle = level.GetObstacle(Get32Position().X + 0.9375f, Get32Position().Y);
                            break;
                        }
                        case 2:
                        {
                            obstacle = level.GetObstacle(Get32Position().X + 0.9375f, Get32Position().Y + 0.9375f);
                            break;
                        }
                        case 3:
                        {
                            obstacle = level.GetObstacle(Get32Position().X, Get32Position().Y + 0.9375f);
                            break;
                        }
                    }

                    switch (obstacle.Type)
                    {
                        case BlockType.Wood:
                        {
                            obstacle.DeleteObstacle();
                            if (!_character.IsDownAttacking)
                                level.Particles.Add(new ParticleEffect(obstacle.OriginalPos.X, obstacle.OriginalPos.Y,
                                    new Color(193, 97, 0)));
                            else
                                level.Particles.Add(new ParticleEffect(obstacle.OriginalPos.X, obstacle.OriginalPos.Y,
                                    new Color(193, 97, 0), 10));
                            sWood.Play();
                            _character.AddToScore(level, 10, obstacle.X, obstacle.Y);
                            if (_character.IsDownAttacking) _character.SpeedY -= 1.2f;
                            break;
                        }
                        case BlockType.HardBlock:
                        {
                            level.Particles.Add(new ParticleEffect(obstacle.OriginalPos.X, obstacle.OriginalPos.Y,
                                new Color(57, 65, 81), 1));
                            obstacle.HitHardblock(_character);
                            if (obstacle.Health <= 0)
                            {
                                obstacle.DeleteObstacle();
                                Block.sDestroy.Play();
                                level.Particles.Add(new ParticleEffect(obstacle.OriginalPos.X, obstacle.OriginalPos.Y,
                                    new Color(57, 65, 81)));
                                _character.AddToScore(level, 100, obstacle.X, obstacle.Y);
                            }

                            break;
                        }
                        case BlockType.EnergyBall:
                        {
                            level.Particles.Add(new ParticleEffect(obstacle.OriginalPos.X, obstacle.OriginalPos.Y,
                                Color.Cyan, 10));
                            if (!_character.IsDownAttacking)
                            {
                                _character.SpeedX =
                                    obstacle.GetCenterPosition().X - _character.GetCenterPosition().X < 0
                                        ? 15f
                                        : -15f;
                                _character.SpeedY = BounceSpeed;
                            }
                            else
                            {
                                _character.IsDownAttacking = false;
                                _character.SpeedY = BounceSpeed * 4;
                            }

                            _character.IsAttacking = false;
                            Block.sHard.Play();
                            break;
                        }
                    }
                }

                foreach (var monster in level.Monsters)
                    if (GetBoundingBox().Intersects(monster.GetBoundingBox()))
                    {
                        _character.AddToScore(level, monster.Points, monster.X, monster.Y);
                        level.Particles.Add(new ParticleEffect(monster.X, monster.Y, Color.Red));
                        monster.Die(level);
                        if (_character.IsDownAttacking)
                        {
                            _character.SpeedY = BounceSpeed;
                            _character.IsDownAttacking = false;
                            _character.IsAttacking = false;
                        }
                    }

                foreach (var archer in level.Archers)
                {
                    if (GetBoundingBox().Intersects(archer.GetBoundingBox()))
                    {
                        _character.AddToScore(level, archer.Points, archer.X, archer.Y);
                        level.Particles.Add(new ParticleEffect(archer.X, archer.Y, Color.Red));
                        archer.Die(level);
                        if (_character.IsDownAttacking)
                        {
                            _character.SpeedY = BounceSpeed;
                            _character.IsDownAttacking = false;
                            _character.IsAttacking = false;
                        }
                    }

                    if (GetBoundingBox().Intersects(archer.Arrow.GetBoundingBox()))
                    {
                        level.Particles.Add(new ParticleEffect(archer.Arrow.X, archer.Arrow.Y,
                            new Color(193, 97, 0), 10));
                        archer.Arrow.DeleteArrow();
                        sBroke.Play();
                    }
                }


                foreach (var wizard in level.Wizards)
                    if (GetBoundingBox().Intersects(wizard.GetBoundingBox()))
                    {
                        _character.AddToScore(level, wizard.Points, wizard.X, wizard.Y);
                        level.Particles.Add(new ParticleEffect(wizard.X, wizard.Y, Color.Red));
                        wizard.Die(level);
                        if (_character.IsDownAttacking)
                        {
                            _character.SpeedY = BounceSpeed;
                            _character.IsDownAttacking = false;
                            _character.IsAttacking = false;
                        }
                    }

                foreach (var golem in level.Golems)
                {
                    if (GetBoundingBox().Intersects(golem.GetBoundingBox()))
                    {
                        golem.Health--;
                        level.Particles.Add(new ParticleEffect(golem.X, golem.Y,
                            new Color(100, 100, 100), 10));

                        if (!_character.IsDownAttacking)
                        {
                            _character.SpeedX = golem.GetCenterPosition().X - _character.GetCenterPosition().X < 0
                                ? 10f
                                : -10f;
                            _character.SpeedY = BounceSpeed;
                        }
                        else
                        {
                            _character.SpeedY = BounceSpeed * 2.5f;
                            _character.IsDownAttacking = false;
                        }

                        _character.IsAttacking = false;
                        Block.sHard.Play();
                    }

                    if (GetBoundingBox().Intersects(golem.Boulder.GetBoundingBox())) golem.Boulder.ResetBoulder(level);
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

        public void AttackDown()
        {
            SetPosition(_character.X + 1f, _character.Y + 32);
            _animDown.Animate(30);
        }

        public void Reset()
        {
            _animLeft.ResetAnimation();
            _animRight.ResetAnimation();
            _animUp.ResetAnimation();
            _animDown.ResetAnimation();
            SetPosition(-400, -400);
        }

        public int AnimLeftFrameNumber()
        {
            return _animLeft.Frame;
        }

        public int AnimRightFrameNumber()
        {
            return _animRight.Frame;
        }

        private readonly Animation _animDown;
        private readonly Animation _animLeft;
        private readonly Animation _animRight;
        private readonly Animation _animUp;
        private readonly MainCharacter _character;
        private readonly float _frameTime;

    }
}