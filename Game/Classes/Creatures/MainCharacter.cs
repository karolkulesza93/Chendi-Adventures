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

        //mian character animations
        private Movement _lastMove;
        private Animation _standing;
        private Animation _attackLeft;
        private Animation _attackRight;
        private Animation _jumpLeft;
        private Animation _jumpRight;
        private Animation _jumpUp;
        private Animation _jumpDown;
        private Animation _jumpBack;
        private Animation _victoryAnimation;
        //
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
            IsJumping = false;
            IsAttacking = false;
            IsUpAttacking = false;
            IsShooting = false;
            IsVulnerable = true;
            _immortalityAnimationCounter = 0;
            _immortalityAnimationFlag = false;

            HasSilverKey = false;
            HasGoldenKey = false;

            GotExit = false;

            //animations
            _standing = new Animation(this, 0.2f,
                new Vector2i(32,64),
                new Vector2i(128,0)
                );
            _animLeft = new Animation(this, 0.05f,
                new Vector2i(0, 32),
                new Vector2i(32, 32));
            _animRight = new Animation(this, 0.05f,
                new Vector2i(0, 0),
                new Vector2i(32, 0));
            //main character animations
            _lastMove = Movement.Right;

            _attackLeft = new Animation(this, 0.05f,
                new Vector2i(0,160),
                new Vector2i(32, 160),
                new Vector2i(64, 160),
                new Vector2i(96, 160),
                new Vector2i(128, 160)
                );
            _attackRight = new Animation(this,0.05f,
                new Vector2i(0, 128),
                new Vector2i(32, 128),
                new Vector2i(64, 128),
                new Vector2i(96, 128),
                new Vector2i(128, 128)
                );

            _jumpUp = new Animation(this, 0.05f,
                new Vector2i(0,192),
                new Vector2i(32, 192)
                );
            _jumpDown = new Animation(this, 0.05f,
                new Vector2i(64,192),
                new Vector2i(96, 192)
                );
            _jumpBack = new Animation(this, 0.05f,
                new Vector2i(64,64),
                new Vector2i(96,64)
                );
            _jumpLeft = new Animation(this, 0.05f,
                new Vector2i(64,96),
                new Vector2i(96,96)
                );
            _jumpRight = new Animation(this, 0.05f,
                new Vector2i(0, 96),
                new Vector2i(32,96)
                );

            _victoryAnimation = new Animation(this, 0.1f,
                new Vector2i(128,64),
                new Vector2i(128, 192),
                new Vector2i(128,96),
                new Vector2i(128, 64),
                new Vector2i(128, 192),
                new Vector2i(128, 32)
                );
            //

            sJump = new Sound(new SoundBuffer(@"sfx/jump.wav"));
            sTramp = new Sound(new SoundBuffer(@"sfx/trampoline.wav"));
            sCoin = new Sound(new SoundBuffer(@"sfx/coin.wav"));
            sCoin.Volume = 30;
            sAtk = new Sound(new SoundBuffer(@"sfx/sword.wav"));
            sDie = new Sound(new SoundBuffer(@"sfx/death.wav"));
            sTp = new Sound(new SoundBuffer(@"sfx/teleport.wav"));
            sKey = new Sound(new SoundBuffer(@"sfx/key.wav"));
            sLife = new Sound(new SoundBuffer(@"sfx/life.wav"));
            sPickup = new Sound(new SoundBuffer(@"sfx/pickup.wav"));
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
        public bool IsJumping { get; private set; }
        public bool IsAttacking { get; private set; }
        public bool IsUpAttacking { get; private set; }
        public bool IsShooting { get; private set; }
        public bool IsVulnerable { get; private set; }
        public bool HasSilverKey { get; set; }
        public bool HasGoldenKey { get; set; }
        public bool OutOfLives { get; set; }
        public bool GotExit { get; set; }
        public int LivesGranted { get; set; }

        //steering
        public static Keyboard.Key KeyUP = Keyboard.Key.Up;
        public static Keyboard.Key KeyLEFT = Keyboard.Key.Left;
        public static Keyboard.Key KeyRIGHT = Keyboard.Key.Right;
        public static Keyboard.Key KeyJUMP = Keyboard.Key.Z;
        public static Keyboard.Key KeyATTACK = Keyboard.Key.X;
        public static Keyboard.Key KeyARROW = Keyboard.Key.C;
        public static Keyboard.Key KeyTHUNDER = Keyboard.Key.D;
        public static Keyboard.Key KeyDIE = Keyboard.Key.U;
        public static Keyboard.Key KeyIMMORTALITY = Keyboard.Key.S;

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
            if (!IsDead && !GotExit)
            {
                //just die
                if (Keyboard.IsKeyPressed(KeyDIE)) Die(level);
                //jump
                if (Keyboard.IsKeyPressed(KeyJUMP) && !IsShooting && !IsAttacking && !IsJumping) Jump();
                IsJumping = Keyboard.IsKeyPressed(KeyJUMP);
                //attack
                if (Keyboard.IsKeyPressed(KeyATTACK) && DefaultClock.ElapsedTime.AsMilliseconds() > 500 && IsVulnerable && !IsAttacking && !IsShooting)
                {
                    IsAttacking = true;
                    sJump.Stop();
                    if (Keyboard.IsKeyPressed(Keyboard.Key.Up)) IsUpAttacking = true;
                    DefaultClock.Restart();
                }
                //arrow
                if (Keyboard.IsKeyPressed(KeyARROW) && DefaultClock.ElapsedTime.AsMilliseconds() > 700 &&
                    ArrowAmount > 0 && IsVulnerable && Arrow.X < 0 && IsStandingOnBlocks && !IsShooting && !IsAttacking)
                {
                    IsShooting = true;
                    Projectile.sShoot.Play();
                    ArrowAmount--;
                    Arrow.ArrowDirectionDefine();
                    Arrow.isEnergized = false;
                    DefaultClock.Restart();
                    Arrow.SetPosition(X, Y + 12);
                }
                //energized arrow
                if (Keyboard.IsKeyPressed(KeyTHUNDER) && DefaultClock.ElapsedTime.AsMilliseconds() > 1200 &&
                    ArrowAmount > 0 && Mana > 0 && IsVulnerable && IsStandingOnBlocks && !IsShooting && !IsAttacking)
                {
                    IsShooting = true;
                    Projectile.sEnergyShoot.Play();
                    ArrowAmount--;
                    Mana--;
                    Arrow.ArrowDirectionDefine();
                    Arrow.isEnergized = true;
                    DefaultClock.Restart();
                    Arrow.SetPosition(X, Y + 12);
                }
                //immortality
                if (Keyboard.IsKeyPressed(KeyIMMORTALITY) && DefaultClock.ElapsedTime.AsMilliseconds() > 500 &&
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
                    if (DefaultClock.ElapsedTime.AsSeconds() > 0.25f) {IsAttacking = false; IsUpAttacking = false; }
                    else if (IsUpAttacking)
                        Sword.AttackUp();
                    else if (!IsUpAttacking) Sword.Attack();
                }
                else
                {
                    Sword.Reset();
                }

                if (IsShooting)
                {
                    if (!Arrow.isEnergized)
                    {
                        if (Arrow.LastMove == Movement.Left) Arrow.SetTextureRectanlge(0, 0, 32, 7);
                        else Arrow.SetTextureRectanlge(0, 7, 32, 7);
                    }
                    else
                    {
                        if (Arrow.LastMove == Movement.Left) Arrow.SetTextureRectanlge(0, 28, 32, 7);
                        else Arrow.SetTextureRectanlge(0, 35, 32, 7);
                    }

                    if (DefaultClock.ElapsedTime.AsSeconds() > 0.5f) IsShooting = false;
                }

                //movement left
                if (Keyboard.IsKeyPressed(KeyLEFT) && !Keyboard.IsKeyPressed(KeyRIGHT) && !IsStandingOnBlocks)
                {
                    MoveLeft();
                    if (SpeedX <= 0)
                    {
                        Sword.LastMove = Movement.Left;
                        Arrow.LastMove = Movement.Left;
                        _lastMove = Movement.Left;
                    }
                }
                else if (Keyboard.IsKeyPressed(KeyLEFT) && !Keyboard.IsKeyPressed(KeyRIGHT) && !IsAttacking && !IsShooting)
                {
                    MoveLeft();
                    if (SpeedX <= 0)
                    {
                        Sword.LastMove = Movement.Left;
                        Arrow.LastMove = Movement.Left;
                        _lastMove = Movement.Left;
                    }
                }
                else if (SpeedX < 0)
                {
                    SpeedX += dX;
                    _lastMove = Movement.Left;
                    if (SpeedX > 0) SpeedX = 0;
                }
                
                //movement right
                if (Keyboard.IsKeyPressed(KeyRIGHT) && !Keyboard.IsKeyPressed(KeyLEFT) && !IsStandingOnBlocks)
                {
                    MoveRight();
                    if (SpeedX >= 0)
                    {
                        Sword.LastMove = Movement.Right;
                        Arrow.LastMove = Movement.Right;
                        _lastMove = Movement.Right;
                    }
                }
                else if (Keyboard.IsKeyPressed(KeyRIGHT) && !Keyboard.IsKeyPressed(KeyLEFT) && !IsAttacking && !IsShooting)
                {
                    MoveRight();
                    if (SpeedX >= 0)
                    {
                        Sword.LastMove = Movement.Right;
                        Arrow.LastMove = Movement.Right;
                        _lastMove = Movement.Right;
                    }
                }
                else if (SpeedX > 0)
                {
                    SpeedX -= dX;
                    _lastMove = Movement.Right;
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

        public void MainCharacterUpdate(Level level)
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
            if (SpeedX < 0)
            {
                MovementDirection = Movement.Left;
            }
            else if (SpeedX > 0)
            {
                MovementDirection = Movement.Right;
            }
            else MovementDirection = Movement.None;

            //==//==//==//==//==//==//==//==//==//==//==//==//==//==//==//==//==//==//

            if (IsStandingOnBlocks)
            {
                if (IsShooting)
                {
                    if (_lastMove == Movement.Left)
                    {
                        SetTextureRectanlge(64, 32, 32, 32);
                    }
                    else if (_lastMove == Movement.Right)
                    {
                        SetTextureRectanlge(96, 32, 32, 32);
                    }
                }
                else if (IsAttacking)
                {
                    if (IsUpAttacking)
                    {
                        SetTextureRectanlge(0, 64, 32, 32);
                    }
                    else if (_lastMove == Movement.Left)
                    {
                        _attackLeft.Animate();
                    }
                    else if (_lastMove == Movement.Right)
                    {
                        _attackRight.Animate();
                    }
                }
                else
                {
                    if (MovementDirection == Movement.Right) _animRight.Animate();
                    else if (MovementDirection == Movement.Left) _animLeft.Animate();
                    else if (Keyboard.IsKeyPressed(Keyboard.Key.Up)) SetTextureRectanlge(0, 64, 32, 32);
                    else _standing.Animate();
                }
            }
            else
            {
                if (IsAttacking)
                {
                    if (IsUpAttacking)
                    {
                        SetTextureRectanlge(64, 64, 32, 32);
                    }
                    else if (_lastMove == Movement.Left)
                    {
                        _attackLeft.Animate();
                    }
                    else if (_lastMove == Movement.Right)
                    {
                        _attackRight.Animate();
                    }
                }
                else if (IsShooting)
                {
                    if (_lastMove == Movement.Left)
                    {
                        SetTextureRectanlge(64, 32, 32, 32);
                    }
                    else if (_lastMove == Movement.Right)
                    {
                        SetTextureRectanlge(96, 32, 32, 32);
                    }
                }
                else if (SpeedY != 0)
                {
                    if (MovementDirection == Movement.Left) _jumpLeft.Animate();
                    else if (MovementDirection == Movement.Right) _jumpRight.Animate();
                    else if (Keyboard.IsKeyPressed(Keyboard.Key.Up)) _jumpBack.Animate();
                    else
                    {
                        if (SpeedY < 0) _jumpUp.Animate(); 
                        else _jumpDown.Animate();
                    }
                }
            }

            //==//==//==//==//==//==//==//==//==//==//==//==//==//==//==//==//==//==//

            if (IsDead)
            {
                if (IsStandingOnBlocks) SetTextureRectanlge(96, 0, 32, 32);
                else SetTextureRectanlge(64, 0, 32, 32);
            }
            if (GotExit) _victoryAnimation.Animate();
        }

        public override void CollisionDependence(Level level)
        {
            base.CollisionDependence(level);
            ObstaclesCollision(level);
        }

        public void ObstaclesCollision(Level level)
        {
            Block obstacle = null;
            for (int i = 0; i < 4; i++)
            {
                switch (i)
                {
                    case 0:
                    {
                        obstacle = level.GetObstacle(Get32Position().X + 0.1f, Get32Position().Y + 0.1f);
                        break;
                    }
                    case 1: 
                    {
                        obstacle = level.GetObstacle(Get32Position().X+0.9f, Get32Position().Y+0.1f);
                        break;
                    }
                    case 2: 
                    {
                        obstacle = level.GetObstacle(Get32Position().X+0.9f, Get32Position().Y+0.9f);
                        break;
                    }
                    case 3: 
                    {
                        obstacle = level.GetObstacle(Get32Position().X + 0.1f, Get32Position().Y+0.9f);
                        break;
                    }
                }
                switch (obstacle.Type)
                {
                    case BlockType.Enterance:
                        {
                            break;
                        }
                    case BlockType.Exit:
                        {
                            if (Keyboard.IsKeyPressed(KeyUP) && IsStandingOnBlocks && !IsDead)
                            {
                                SpeedX = 0;
                                SetTextureRectanlge(128,64);
                                GotExit = true;
                            }

                            break;
                        }
                    case BlockType.Shop:
                        {
                            if (Keyboard.IsKeyPressed(KeyUP) && IsStandingOnBlocks)
                            {
                                level.isShopOpened = true;
                            }

                            break;
                        }
                    case BlockType.Lever:
                        {
                            if (Keyboard.IsKeyPressed(KeyUP) && IsStandingOnBlocks)
                            {
                                Block.FlipLever();
                            }
                            break;
                        }
                    case BlockType.Purifier:
                        {
                            if (Mana > 0 || ArrowAmount > 0 || !IsVulnerable)
                            {
                                Block.sPurify.Play();
                                level.AddParticleEffect(new ParticleEffect(obstacle.X, obstacle.Y, Color.Magenta));
                                ArrowAmount = 0;
                                Mana = 0;
                                IsVulnerable = true;
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
                            Coins += MainGameWindow.Randomizer.Next(30) + 11;
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
                    //
                    case BlockType.Teleport3:
                        {
                            if (Keyboard.IsKeyPressed(KeyUP) && DefaultClock.ElapsedTime.AsSeconds() > 1)
                            {
                                sTp.Play();
                                SetPosition(level.tp4Position.X, level.tp4Position.Y);
                                DefaultClock.Restart();
                            }

                            break;
                        }
                    case BlockType.Teleport4:
                        {
                            if (Keyboard.IsKeyPressed(KeyUP) && DefaultClock.ElapsedTime.AsSeconds() > 1)
                            {
                                sTp.Play();
                                SetPosition(level.tp3Position.X, level.tp3Position.Y);
                                DefaultClock.Restart();
                            }

                            break;
                        }
                    //////////////////
                    case BlockType.Hint:
                        {
                            level.SetHints(obstacle);
                            break;
                        }
                }
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
                if (GetBoundingBox().Intersects(wizard.EnergyBall.GetBoundingBox()))
                {
                    Die(level);
                    wizard.EnergyBall.ResetEnergyBall(level);
                }
            }
        }

        public void Die(Level level)
        {
            if (!IsDead && IsVulnerable && !GotExit)
            {
                sDie.Play();
                sKill.Play();
                level.AddParticleEffect(new ParticleEffect(X, Y, Color.Red));
                Sword.Reset();
                DefaultClock.Restart();
                IsDead = true;
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
            IsJumping = false;
            IsAttacking = false;
            IsDead = false;
            OutOfLives = false;
            IsVulnerable = true;
            ArrowAmount = 3;
            Lives = 3;
            Mana = 0;
            Score = 0;
            LivesGranted = 0;
            Coins = 0;

            SpeedY = 0;
            SpeedX = 0;

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

        public new FloatRect GetBoundingBox()
        {
            return new FloatRect(X +2, Y+2, 30,30 );
        }
    }
}