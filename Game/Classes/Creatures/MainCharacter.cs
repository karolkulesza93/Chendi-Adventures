using System.Net.Configuration;
using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace ChendiAdventures
{
    public sealed class MainCharacter : Creature
    {
        public readonly Clock DefaultClock;
        private byte _immortalityAnimationCounter;
        private bool _immortalityAnimationFlag;

        public MainCharacter(float x, float y, Texture texture) : base(x, y, texture)
        {
            SetTextureRectanlge(32, 64, 32, 32);
            Lives = 3;
            Continues = 2;
            OutOfLives = false;
            DefaultClock = new Clock();
            IsStandingOnBlocks = false;
            Score = 0;
            LivesGranted = 0;

            Sword = new Sword(this);
            Arrow = new Arrow(-100, -100, ArrowTexture, Movement.Right);
            ArrowAmount = 3;
            Mana = 0;
            IsAttacking = false;
            IsShooting = false;
            IsVulnerable = true;
            _immortalityAnimationCounter = 0;
            _immortalityAnimationFlag = false;

            HasSilverKey = false;
            HasGoldenKey = false;

            GotExit = false;

            _animLeft = new Animation(this, 0.05f,
                new Vector2i(0, 32),
                new Vector2i(32, 32));
            _animRight = new Animation(this, 0.05f,
                new Vector2i(0, 0),
                new Vector2i(32, 0));

            KeyUP = Keyboard.Key.Up;
            KeyLEFT = Keyboard.Key.Left;
            KeyRIGHT = Keyboard.Key.Right;
            KeyJUMP = Keyboard.Key.Z;
            KeyATTACK = Keyboard.Key.X;
            KeyARROW = Keyboard.Key.C;
            KeyTHUNDER = Keyboard.Key.D;
            KeyIMMORTALITY = Keyboard.Key.S;
            KeyDIE = Keyboard.Key.U;

            sJump = new Sound(new SoundBuffer(@"sfx/jump.wav"));
            sTramp = new Sound(new SoundBuffer(@"sfx/trampoline.wav"));
            sCoin = new Sound(new SoundBuffer(@"sfx/coin.wav"));
            sCoin.Volume = 40;
            sAtk = new Sound(new SoundBuffer(@"sfx/sword.wav"));
            sAtk.Volume = 30;
            sDie = new Sound(new SoundBuffer(@"sfx/death.wav"));
            sTp = new Sound(new SoundBuffer(@"sfx/teleport.wav"));
            sKey = new Sound(new SoundBuffer(@"sfx/key.wav"));
            sLife = new Sound(new SoundBuffer(@"sfx/life.wav"));
            sLife.Volume = 50;
            sPickup = new Sound(new SoundBuffer(@"sfx/pickup.wav"));
            sPickup.Volume = 40;
            sImmortality = new Sound(new SoundBuffer(@"sfx/immortality.wav"));
            sImmortality.Volume = 30;
            sImmortality.Loop = true;
        }

        public Sword Sword { get; }
        public Arrow Arrow { get; }
        public int Coins { get; set; }
        public int ArrowAmount { get; set; }
        public int Mana { get; set; }
        public int Lives { get; set; }
        public int Continues { get; set; }
        public int Score { get; set; }
        public bool IsAttacking { get; private set; }
        public bool IsShooting { get; private set; }
        public bool IsVulnerable { get; private set; }
        public bool HasSilverKey { get; set; }
        public bool HasGoldenKey { get; set; }
        public bool OutOfLives { get; set; }
        public bool GotExit { get; set; }
        public int LivesGranted { get; set; }

        //steering
        public Keyboard.Key KeyUP { get; set; }
        public Keyboard.Key KeyLEFT { get; set; }
        public Keyboard.Key KeyRIGHT { get; set; }
        public Keyboard.Key KeyJUMP { get; set; }
        public Keyboard.Key KeyATTACK { get; set; }
        public Keyboard.Key KeyARROW { get; set; }
        public Keyboard.Key KeyTHUNDER { get; set; }
        public Keyboard.Key KeyDIE { get; set; }
        public Keyboard.Key KeyIMMORTALITY { get; set; }

        //sounds
        public Sound sTramp { get; }
        public Sound sAtk { get; }
        public Sound sDie { get; }
        public Sound sCoin { get; }
        public Sound sTp { get; }
        public Sound sKey { get; }
        public Sound sLife { get; }
        public Sound sImmortality { get; }
        public Sound sPickup { get; }

        public void MainCharacterSteering(Level level)
        {
            if (!IsDead)
            {
                //just die
                if (Keyboard.IsKeyPressed(KeyDIE)) Die(level);
                //jump
                if (Keyboard.IsKeyPressed(KeyJUMP)) Jump();
                //attack
                if (Keyboard.IsKeyPressed(KeyATTACK) && DefaultClock.ElapsedTime.AsMilliseconds() > 500 && IsVulnerable)
                {
                    IsAttacking = true;
                    DefaultClock.Restart();
                }

                if (Keyboard.IsKeyPressed(KeyARROW) && DefaultClock.ElapsedTime.AsMilliseconds() > 1000 &&
                    ArrowAmount > 0 && IsVulnerable && Arrow.X < 0)
                {
                    IsShooting = true;
                    Arrow.ArrowDirectionDefine();
                    Arrow.isEnergized = false;
                    DefaultClock.Restart();
                }

                if (Keyboard.IsKeyPressed(KeyTHUNDER) && DefaultClock.ElapsedTime.AsMilliseconds() > 1500 &&
                    ArrowAmount > 0 && Mana > 0 && IsVulnerable)
                {
                    IsShooting = true;
                    Arrow.ArrowDirectionDefine();
                    Arrow.isEnergized = true;
                    DefaultClock.Restart();
                }

                //immortality
                if (Keyboard.IsKeyPressed(KeyIMMORTALITY) && DefaultClock.ElapsedTime.AsMilliseconds() > 1000 &&
                    Mana > 0 && IsVulnerable)
                {
                    Mana--;
                    DefaultClock.Restart();
                    IsVulnerable = false;
                    sImmortality.Play();
                }

                if (!IsVulnerable)
                {
                    if (DefaultClock.ElapsedTime.AsSeconds() > 5)
                    {
                        IsVulnerable = true;
                        SetColor(Color.White);
                    }
                    else
                    {
                        ImmortalityEffect();
                    }
                }
                else
                {
                    sImmortality.Stop();
                }


                if (IsAttacking)
                {
                    if (sAtk.Status != SoundStatus.Playing) sAtk.Play();
                    if (DefaultClock.ElapsedTime.AsSeconds() > 0.035f) IsAttacking = false;
                    if (Keyboard.IsKeyPressed(KeyUP))
                        Sword.AttackUp();
                    else Sword.Attack();
                }
                else
                {
                    Sword.Reset();
                }

                if (IsShooting)
                {
                    ArrowAmount--;

                    if (!Arrow.isEnergized)
                    {
                        Projectile.sShoot.Play();
                        if (Arrow.LastMove == Movement.Left) Arrow.SetTextureRectanlge(0, 0, 32, 7);
                        else Arrow.SetTextureRectanlge(0, 7, 32, 7);
                    }
                    else
                    {
                        Mana--;
                        Projectile.sEnergyShoot.Play();
                        if (Arrow.LastMove == Movement.Left) Arrow.SetTextureRectanlge(0, 28, 32, 7);
                        else Arrow.SetTextureRectanlge(0, 35, 32, 7);
                    }

                    IsShooting = false;
                    Arrow.SetPosition(X, Y + 12);
                }

                //movement
                if (Keyboard.IsKeyPressed(KeyLEFT))
                {
                    MoveLeft();
                    Sword.LastMove = Movement.Left;
                    Arrow.LastMove = Movement.Left;
                }
                else if (SpeedX < 0)
                {
                    SpeedX += dX;
                    if (SpeedX > 0) SpeedX = 0;
                }

                if (Keyboard.IsKeyPressed(KeyRIGHT))
                {
                    MoveRight();
                    Sword.LastMove = Movement.Right;
                    Arrow.LastMove = Movement.Right;
                }
                else if (SpeedX > 0)
                {
                    SpeedX -= dX;
                    if (SpeedX < 0) SpeedX = 0;
                }
            }
            else
            {
                if (SpeedX > 0)
                {
                    SpeedX -= dX/4;
                    if (SpeedX < 0) SpeedX = 0f;
                }
                else if (SpeedX < 0f)
                {
                    SpeedX += dX/4;
                    if (SpeedX > 0) SpeedX = 0f;
                }
            }
        }

        public void MainCharactereUpdate(Level level)
        {
            MainCharacterSteering(level);

            SpeedY += GravityForce;

            Arrow.ArrowUpdate(this, level);
            Sword.SwordCollisionCheck(level);

            if (GotExit)
            {
                sImmortality.Stop();
                SpeedX = 0f;
                SpeedY = 0f;
            }

            GrantAdditionalLifeDependingOnScore();

            CollisionDependence(level);
            UpdateTextures();
        }

        public override void UpdateTextures()
        {
            if (SpeedX < 0) MovementDirection = Movement.Left;
            else if (SpeedX > 0) MovementDirection = Movement.Right;
            else MovementDirection = Movement.None;

            if (IsStandingOnBlocks)
            {
                if (MovementDirection == Movement.Right) _animRight.Animate();
                else if (MovementDirection == Movement.Left) _animLeft.Animate();
                else if (Keyboard.IsKeyPressed(Keyboard.Key.Up)) SetTextureRectanlge(0, 64, 32, 32);
                else SetTextureRectanlge(32, 64, 32, 32);
            }
            else if (!IsStandingOnBlocks && SpeedY != 0)
            {
                if (MovementDirection == Movement.Left) SetTextureRectanlge(32, 96, 32, 32);
                else if (MovementDirection == Movement.Right) SetTextureRectanlge(0, 96, 32, 32);
                else if (Keyboard.IsKeyPressed(Keyboard.Key.Up)) SetTextureRectanlge(64, 64, 32, 32);
                else SetTextureRectanlge(64, 96, 32, 32);
            }

            if (IsDead)
            {
                if (IsStandingOnBlocks) SetTextureRectanlge(96, 0, 32, 32);
                else SetTextureRectanlge(64, 0, 32, 32);
            }

            if (GotExit) SetTextureRectanlge(96, 96, 32, 32);
        }

        public override void CollisionDependence(Level level)
        {
            base.CollisionDependence(level);
            ObstaclesCollision(level);
        }

        public void ObstaclesCollision(Level level)
        {
            foreach (var obstacle in level.LevelObstacles)
                if (obstacle.GetBoundingBox().Intersects(GetBoundingBox()))
                {
                    switch (obstacle.Type)
                    {
                        case BlockType.Enterance:
                        {
                            break;
                        }
                        case BlockType.Exit:
                        {
                            if (Keyboard.IsKeyPressed(KeyUP) && IsStandingOnBlocks)
                            {
                                SpeedX = 0;
                                GotExit = true;
                            }

                            break;
                        }
                        case BlockType.Coin:
                        {
                            AddToScore(level, 30, obstacle.X, obstacle.Y);
                            sCoin.Play();
                            Coins++;
                            obstacle.DeletePickup();
                            break;
                        }
                        case BlockType.SackOfGold:
                        {
                            AddToScore(level, 300, obstacle.X, obstacle.Y);
                            sCoin.Play();
                            Coins += 10;
                            obstacle.DeletePickup();
                            break;
                        }
                        case BlockType.Life:
                        {
                            AddToScore(level, 500, obstacle.X, obstacle.Y);
                            Lives++;
                            sLife.Play();
                            obstacle.DeletePickup();
                            break;
                        }
                        case BlockType.Mana:
                        {
                            AddToScore(level, 300, obstacle.X, obstacle.Y);
                            Mana++;
                            sPickup.Play();
                            obstacle.DeletePickup();
                            break;
                        }
                        case BlockType.TripleMana:
                        {
                            AddToScore(level, 900, obstacle.X, obstacle.Y);
                            Mana += 3;
                            sPickup.Play();
                                obstacle.DeletePickup();
                            break;
                        }
                        case BlockType.Score1000:
                        {
                            AddToScore(level, 1000, obstacle.X, obstacle.Y);
                            sPickup.Play();
                                obstacle.DeletePickup();
                            break;
                        }
                        case BlockType.Score5000:
                        {
                            AddToScore(level, 5000, obstacle.X, obstacle.Y);
                            sPickup.Play();
                                obstacle.DeletePickup();
                            break;
                        }
                        case BlockType.Arrow:
                        {
                            AddToScore(level, 100, obstacle.X, obstacle.Y);
                            ArrowAmount++;
                            sPickup.Play();
                                obstacle.DeletePickup();
                            break;
                        }
                        case BlockType.TripleArrow:
                        {
                            AddToScore(level, 300, obstacle.X, obstacle.Y);
                            ArrowAmount += 3;
                            sPickup.Play();
                                obstacle.DeletePickup();
                            break;
                        }
                        case BlockType.SilverKey:
                        {
                            HasSilverKey = true;
                            level.UnableToPassl.Remove(BlockType.SilverDoor);
                            AddToScore(level, 250, obstacle.X, obstacle.Y);
                            sKey.Play();
                            obstacle.DeletePickup();
                            break;
                        }
                        case BlockType.GoldenKey:
                        {
                            HasGoldenKey = true;
                            level.UnableToPassl.Remove(BlockType.GoldDoor);
                            AddToScore(level, 500, obstacle.X, obstacle.Y);
                            sKey.Play();
                            obstacle.DeletePickup();
                            break;
                        }
                        case BlockType.SilverDoor:
                        {
                            if (HasSilverKey)
                            {
                                obstacle.DeleteObstacle();
                                sKey.Play();
                            }

                            break;
                        }
                        case BlockType.GoldDoor:
                        {
                            if (HasGoldenKey)
                            {
                                obstacle.DeleteObstacle();
                                sKey.Play();
                            }

                            break;
                        }
                        case BlockType.Spike:
                        {
                            Die(level);
                            break;
                        }
                        case BlockType.Trampoline:
                        {
                            if (SpeedY > 2 && !IsDead)
                            {
                                obstacle.SetTextureRectanlge(96, 32, 32, 32);
                                obstacle.DefaultTimer.Restart();
                                SpeedY = -20;
                                sTramp.Play();
                            }

                            break;
                        }
                        //teleports/////////////////
                        case BlockType.Teleport1:
                        {
                            if (Keyboard.IsKeyPressed(KeyUP) && DefaultClock.ElapsedTime.AsSeconds() > 1)
                            {
                                sTp.Play();
                                SetPosition(level.tp2Position.X, level.tp2Position.Y);
                                DefaultClock.Restart();
                            }

                            break;
                        }
                        case BlockType.Teleport2:
                        {
                            if (Keyboard.IsKeyPressed(KeyUP) && DefaultClock.ElapsedTime.AsSeconds() > 1)
                            {
                                sTp.Play();
                                SetPosition(level.tp1Position.X, level.tp1Position.Y);
                                DefaultClock.Restart();
                            }

                            break;
                        }
                        //////////////////
                        case BlockType.Hint:
                        {
                            level.SetHints(obstacle, this);
                            break;
                        }
                    }
                }
                else
                {
                    if (obstacle.Type == BlockType.Hint) level.HideHint(obstacle);
                }

            foreach (var trap in level.Traps)
                if (GetBoundingBox().Intersects(trap.GetBoundingBox()))
                {
                    if ((trap.Type == TrapType.BlowTorchLeft || trap.Type == TrapType.BlowTorchRight) &&
                        trap.IsBlowing) Die(level);
                    else if (trap.Type == TrapType.Crusher || trap.Type == TrapType.Spikes) Die(level);
                }

            foreach (var monster in level.Monsters)
                if (GetBoundingBox().Intersects(monster.GetBoundingBox()))
                    Die(level);
            foreach (var archer in level.Archers)
            {
                if (GetBoundingBox().Intersects(archer.GetBoundingBox())) Die(level);
                if (GetBoundingBox().Intersects(archer.Arrow.GetBoundingBox()))
                {
                    Die(level);
                    archer.Arrow.DeleteArrow();
                }
            }

            foreach (var ghost in level.Ghosts)
                if (GetBoundingBox().Intersects(ghost.GetBoundingBox()))
                    Die(level);
            foreach (var wizard in level.Wizards)
            {
                if (GetBoundingBox().Intersects(wizard.GetBoundingBox())) Die(level);
                if (GetBoundingBox().Intersects(wizard.EnergyBall.GetBoundingBox())) Die(level);
            }
        }

        public void Die(Level level)
        {
            if (!IsDead && IsVulnerable)
            {
                sDie.Play();
                level.AddParticleEffect(new ParticleEffect(X, Y, Color.Red));
                Sword.Reset();
                DefaultClock.Restart();
                IsDead = true;
                SpeedX *= -1f;
                SpeedY = -10f;
                this.Lives--;

                HasSilverKey = false;
                HasGoldenKey = false;

                if (Lives <= 0)
                {
                    OutOfLives = true;
                    Lives = 0;
                }
            }
        }

        public void ImmortalityEffect()
        {
            if (_immortalityAnimationFlag)
            {
                if (_immortalityAnimationCounter > 255) _immortalityAnimationFlag = false;
                _immortalityAnimationCounter += 20;
                SetColor(new Color(255, 255, 255, _immortalityAnimationCounter));
            }
            else
            {
                if (_immortalityAnimationCounter < 0) _immortalityAnimationFlag = true;
                _immortalityAnimationCounter -= 20;
                SetColor(new Color(255, 255, 255, _immortalityAnimationCounter));
            }
        }

        public void Respawn(Level level)
        {
            IsDead = false;
            SetTextureRectanlge(32, 64, 32, 32);
            SetStartingPosition(level);
            DefaultClock.Restart();
        }

        public void SetStartingPosition(Level level)
        {
            SetPosition(level.EnterancePosition.X, level.EnterancePosition.Y);
        }

        public void AddToScore(Level level, int value, float x, float y)
        {
            Score += value;
            level.ScoreAdditionEffects.Add(new ScoreAdditionEffect(value, x, y));
        }

        public void ResetMainCharacter()
        {
            GotExit = false;
            IsAttacking = false;
            IsDead = false;
            OutOfLives = false;
            IsVulnerable = true;
            ArrowAmount = 3;
            Lives = 3;
            Mana = 0;
            Score = 0;
            LivesGranted = 0;

            _immortalityAnimationCounter = 0;
            _immortalityAnimationFlag = false;

            HasSilverKey = false;
            HasGoldenKey = false;
        }

        public void GrantAdditionalLifeDependingOnScore()
        {
            if (Score / 100000 == LivesGranted + 1 && Score > 100000)
            {
                LivesGranted++;
                Lives++;
                sLife.Play();
            }
        }

        public override void Draw(RenderTarget target, RenderStates states)
        {
            base.Draw(target, states);
            target.Draw(Arrow, states);
            target.Draw(Sword, states);
        }
    }
}