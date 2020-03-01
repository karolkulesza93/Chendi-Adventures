using SFML.Window;
using SFML.System;
using SFML.Audio;
using SFML.Graphics;

namespace Game
{
    public sealed class MainCahracter : Creature
    {
        /*signleton field*/ private static MainCahracter _instance = null;
        /*signleton field*/ private static readonly object _padlock = new object();
        public Sword Sword { get; private set; }
        public Arrow Arrow { get; private set; }
        public int Coins { get; set; }
        public int ArrowAmount { get; set; }
        public int Mana { get; set; }
        public readonly Clock DefaultClock;
        public int Lives { get; set; }
        public int Score { get; set; }
        public bool IsAttacking { get; private set; }
        public bool IsShooting { get; private set; }
        public bool IsVulnerable { get; private set; }
        private byte _immortalityAnimationCounter;
        private bool _immortalityAnimationFlag;
        public bool HasSilverKey { get; set; }
        public bool HasGoldenKey { get; set; }
        public bool OutOfLives { get; set; }
        public bool GotExit { get; set; }
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
        public Sound sTramp { get; private set; }
        public Sound sAtk { get; private set; }
        public Sound sDie { get; private set; }
        public Sound sCoin { get; private set; }
        public Sound sTp { get; private set; }
        public Sound sKey { get; private set; }
        public Sound sLife { get; private set; }
        public Sound sImmortality { get; private set; }
        /*signleton property*/
        public static MainCahracter Instance
        {
            get
            {
                if (_instance == null)
                    lock (_padlock)
                    {
                        if (_instance == null)
                            _instance = new MainCahracter(0, 0, null);
                    }
                return _instance;
            }
        }
        public MainCahracter(float x, float y, Texture texture) : base(x, y, texture)
        {
            this.SetTextureRectanlge(32, 64, 32, 32);
            this.Lives = 3;
            this.OutOfLives = false;
            this.DefaultClock = new Clock();
            this.IsStandingOnBlocks = false;
            this.Score = 0;

            this.Sword = new Sword(this);
            this.Arrow = new Arrow(-100, -100, ArrowTexture, Movement.Right);
            this.ArrowAmount = 3;
            this.Mana = 0;
            this.IsAttacking = false;
            this.IsShooting = false;
            this.IsVulnerable = true;
            this._immortalityAnimationCounter = 0;
            this._immortalityAnimationFlag = false;

            this.HasSilverKey = false;
            this.HasGoldenKey = false;

            this.GotExit = false;

            this._animLeft = new Animation(this, 0.05f,
                new Vector2i(0, 32),
                new Vector2i(32, 32));
            this._animRight = new Animation(this, 0.05f,
                new Vector2i(0, 0),
                new Vector2i(32, 0));

            this.KeyUP = Keyboard.Key.Up;
            this.KeyLEFT = Keyboard.Key.Left;
            this.KeyRIGHT = Keyboard.Key.Right;
            this.KeyJUMP = Keyboard.Key.Z;
            this.KeyATTACK = Keyboard.Key.X;
            this.KeyARROW = Keyboard.Key.C;
            this.KeyTHUNDER = Keyboard.Key.D;
            this.KeyIMMORTALITY = Keyboard.Key.S;
            this.KeyDIE = Keyboard.Key.U;

            sJump = new Sound(new SoundBuffer(@"sfx/jump.wav"));

            this.sTramp = new Sound(new SoundBuffer(@"sfx/trampoline.wav"));
            this.sCoin = new Sound(new SoundBuffer(@"sfx/coin.wav")); this.sCoin.Volume = 40;
            this.sAtk = new Sound(new SoundBuffer(@"sfx/sword.wav")); this.sAtk.Volume = 30;
            this.sDie = new Sound(new SoundBuffer(@"sfx/death.wav"));
            this.sTp = new Sound(new SoundBuffer(@"sfx/teleport.wav"));
            this.sKey = new Sound(new SoundBuffer(@"sfx/key.wav"));
            this.sLife = new Sound(new SoundBuffer(@"sfx/life.wav"));
            this.sImmortality = new Sound(new SoundBuffer(@"sfx/immortality.wav")); this.sImmortality.Volume = 30; this.sImmortality.Loop = true;
        }
        public void MainCharacterSteering(Level level)
        {
            if (!this.IsDead)
            {
                //just die
                if (Keyboard.IsKeyPressed(this.KeyDIE)) this.Die(level);
                //jump
                if (Keyboard.IsKeyPressed(this.KeyJUMP))
                {
                    this.Jump();            
                }  
                //attack
                if (Keyboard.IsKeyPressed(this.KeyATTACK) && this.DefaultClock.ElapsedTime.AsMilliseconds() > 500 && this.IsVulnerable)
                {
                    this.IsAttacking = true;
                    this.DefaultClock.Restart();
                }
                if (Keyboard.IsKeyPressed(this.KeyARROW) && this.DefaultClock.ElapsedTime.AsMilliseconds() > 1000 && this.ArrowAmount > 0 && this.IsVulnerable && this.Arrow.X < 0)
                {
                    this.IsShooting = true;
                    this.Arrow.ArrowDirectionDefine();
                    this.Arrow.isEnergized = false;
                    this.DefaultClock.Restart();
                }
                if (Keyboard.IsKeyPressed(this.KeyTHUNDER) && this.DefaultClock.ElapsedTime.AsMilliseconds() > 1500 && this.ArrowAmount > 0 && this.Mana > 0 && this.IsVulnerable)
                {
                    this.IsShooting = true;
                    this.Arrow.ArrowDirectionDefine();
                    this.Arrow.isEnergized = true;
                    this.DefaultClock.Restart();
                }
                //immortality
                if (Keyboard.IsKeyPressed(this.KeyIMMORTALITY) && this.DefaultClock.ElapsedTime.AsMilliseconds() > 1000 && this.Mana > 0 && this.IsVulnerable)
                {
                    this.Mana--;
                    this.DefaultClock.Restart();
                    this.IsVulnerable = false;
                    this.sImmortality.Play();
                }

                if (!this.IsVulnerable)
                {
                    if (this.DefaultClock.ElapsedTime.AsSeconds() > 5)
                    {
                        this.IsVulnerable = true;
                        this.SetColor(Color.White);
                    }
                    else
                    {
                        this.ImmortalityEffect();
                    }
                }
                else
                {
                    this.sImmortality.Stop();
                }


                if (this.IsAttacking)
                {
                    if (this.sAtk.Status != SoundStatus.Playing) this.sAtk.Play();
                    if (this.DefaultClock.ElapsedTime.AsSeconds() > 0.035f) { this.IsAttacking = false; }
                    if (Keyboard.IsKeyPressed(this.KeyUP))
                    {
                        this.Sword.AttackUp();
                    }
                    else this.Sword.Attack();
                }
                else this.Sword.Reset();

                if (this.IsShooting)
                {
                    this.ArrowAmount--;

                    if (!this.Arrow.isEnergized)
                    {
                        Projectile.sShoot.Play();
                        if (this.Arrow.LastMove == Movement.Left) this.Arrow.SetTextureRectanlge(0, 0, 32, 7);
                        else this.Arrow.SetTextureRectanlge(0, 7, 32, 7);
                    }
                    else
                    {
                        this.Mana--;
                        Projectile.sEnergyShoot.Play();
                        if (this.Arrow.LastMove == Movement.Left) this.Arrow.SetTextureRectanlge(0, 28, 32, 7);
                        else this.Arrow.SetTextureRectanlge(0, 35, 32, 7);
                    }

                    this.IsShooting = false;
                    this.Arrow.SetPosition(this.X, this.Y + 12);
                }
                //movement
                if (Keyboard.IsKeyPressed(this.KeyLEFT))
                {
                    this.MoveLeft();
                    this.Sword.LastMove = Movement.Left;
                    this.Arrow.LastMove = Movement.Left;
                }
                else if (this.SpeedX < 0)
                {
                    this.SpeedX += this.dX;
                    if (this.SpeedX > 0) this.SpeedX = 0;
                }

                if (Keyboard.IsKeyPressed(this.KeyRIGHT))
                {
                    this.MoveRight();
                    this.Sword.LastMove = Movement.Right;
                    this.Arrow.LastMove = Movement.Right;
                }
                else if (this.SpeedX > 0)
                {
                    this.SpeedX -= this.dX;
                    if (this.SpeedX < 0) this.SpeedX = 0;
                } 
            }
        }
        public void MainCharactereUpdate(Level level)
        {
            this.MainCharacterSteering(level);

            this.SpeedY += GravityForce;

            this.Arrow.ArrowUpdate(this, level);
            this.Arrow.ProjectileUpdate(level);
            this.Sword.SwordCollisionCheck(level);

            if (this.GotExit)
            {
                this.sImmortality.Stop();
                this.SpeedX = 0f;
                this.SpeedY = 0f;
            }

            this.CollisionDependence(level);
            this.UpdateTextures();
        }
        public override void UpdateTextures()
        {
            if (this.SpeedX < 0) this.MovementDirection = Movement.Left;
            else if (this.SpeedX > 0) this.MovementDirection = Movement.Right;
            else this.MovementDirection = Movement.None;

            if (this.IsStandingOnBlocks)
            {
                if (this.MovementDirection == Movement.Right) this._animRight.Animate();
                else if (this.MovementDirection == Movement.Left) this._animLeft.Animate();
                else if (Keyboard.IsKeyPressed(Keyboard.Key.Up)) this.SetTextureRectanlge(0, 64, 32, 32);
                else this.SetTextureRectanlge(32, 64, 32, 32);
            }
            else if (!this.IsStandingOnBlocks && this.SpeedY != 0)
            {
                if (this.MovementDirection == Movement.Left) this.SetTextureRectanlge(32, 96, 32, 32);
                else if (this.MovementDirection == Movement.Right) this.SetTextureRectanlge(0, 96, 32, 32);
                else if (Keyboard.IsKeyPressed(Keyboard.Key.Up)) this.SetTextureRectanlge(64, 64, 32, 32);
                else this.SetTextureRectanlge(64, 96, 32, 32);
            }

            if (this.IsDead)
            {
                if (this.IsStandingOnBlocks) this.SetTextureRectanlge(96, 0, 32, 32);
                else this.SetTextureRectanlge(64, 0, 32, 32);
            }

            if (this.GotExit)
            {
                this.SetTextureRectanlge(96, 96, 32, 32);
            }
        }
        public override void CollisionDependence(Level level)
        {
            base.CollisionDependence(level);
            this.ObstaclesCollision(level);
        }
        public void ObstaclesCollision(Level level)
        {
            foreach (var obstacle in level.LevelObstacles)
            {
                if (obstacle.GetBoundingBox().Intersects(this.GetBoundingBox()))
                {
                    switch (obstacle.Type)
                    {
                        case BlockType.Enterance:
                            {
                                break;
                            }
                        case BlockType.Exit:
                            {
                                if (Keyboard.IsKeyPressed(this.KeyUP))
                                {
                                    this.SpeedX = 0;
                                    this.GotExit = true;
                                }
                                break;
                            }
                        case BlockType.Coin:
                            {
                                this.AddToScore(level, 100, obstacle.X, obstacle.Y);
                                this.sCoin.Play();
                                this.Coins++;
                                obstacle.DeletePickup();
                                break;
                            }
                        case BlockType.SackOfGold:
                            {
                                this.AddToScore(level, 1000, obstacle.X, obstacle.Y);
                                this.sCoin.Play();
                                this.Coins += 10;
                                obstacle.DeletePickup();
                                break;
                            }
                        case BlockType.Life:
                            {
                                this.AddToScore(level, 500, obstacle.X, obstacle.Y);
                                this.Lives++;
                                this.sLife.Play();
                                obstacle.DeletePickup();
                                break;
                            }
                        case BlockType.Mana:
                            {
                                this.AddToScore(level, 400, obstacle.X, obstacle.Y);
                                this.Mana++;
                                this.sCoin.Play();
                                obstacle.DeletePickup();
                                break;
                            }
                        case BlockType.TripleMana:
                            {
                                this.AddToScore(level, 1000, obstacle.X, obstacle.Y);
                                this.Mana += 3;
                                this.sCoin.Play();
                                obstacle.DeletePickup();
                                break;
                            }
                        case BlockType.Score1000:
                            {
                                this.AddToScore(level, 1000, obstacle.X, obstacle.Y);
                                this.sCoin.Play();
                                obstacle.DeletePickup();
                                break;
                            }
                        case BlockType.Score5000:
                            {
                                this.AddToScore(level, 5000, obstacle.X, obstacle.Y);
                                this.sCoin.Play();
                                obstacle.DeletePickup();
                                break;
                            }
                        case BlockType.Arrow:
                            {
                                this.AddToScore(level, 200, obstacle.X, obstacle.Y);
                                this.ArrowAmount++;
                                this.sCoin.Play();
                                obstacle.DeletePickup();
                                break;
                            }
                        case BlockType.TripleArrow:
                            {
                                this.AddToScore(level, 600, obstacle.X, obstacle.Y);
                                this.ArrowAmount += 3;
                                this.sCoin.Play();
                                obstacle.DeletePickup();
                                break;
                            }
                        case BlockType.SilverKey:
                            {
                                this.HasSilverKey = true;
                                level.UnableToPassl.Remove(BlockType.SilverDoor);
                                this.AddToScore(level, 500, obstacle.X, obstacle.Y);
                                this.sKey.Play();
                                obstacle.DeletePickup();
                                break;
                            }
                        case BlockType.GoldenKey:
                            {
                                this.HasGoldenKey = true;
                                level.UnableToPassl.Remove(BlockType.GoldDoor);
                                this.AddToScore(level, 1000, obstacle.X, obstacle.Y);
                                this.sKey.Play();
                                obstacle.DeletePickup();
                                break;
                            }
                        case BlockType.SilverDoor:
                            {
                                if (this.HasSilverKey)
                                {
                                    obstacle.DeleteObstacle();
                                    sKey.Play();
                                }
                                break;
                            }
                        case BlockType.GoldDoor:
                            {
                                if (this.HasGoldenKey)
                                {
                                    obstacle.DeleteObstacle();
                                    sKey.Play();
                                }
                                break;
                            }
                        case BlockType.Spike:
                            {
                                this.Die(level);
                                break;
                            }
                        case BlockType.Trampoline:
                            {
                                if (this.SpeedY > 2 && !this.IsDead) 
                                {
                                    obstacle.SetTextureRectanlge(96, 32, 32, 32);
                                    obstacle.DefaultTimer.Restart();
                                    this.SpeedY = -20; 
                                    this.sTramp.Play(); 
                                } 
                                break;
                            }
                            //teleports/////////////////
                        case BlockType.Teleport1:
                            {
                                if (Keyboard.IsKeyPressed(this.KeyUP) && this.DefaultClock.ElapsedTime.AsSeconds() > 1)
                                {
                                    this.sTp.Play();
                                    this.SetPosition(level.tp2Position.X, level.tp2Position.Y);
                                    this.DefaultClock.Restart();
                                }
                                break;
                            }
                        case BlockType.Teleport2:
                            {
                                if (Keyboard.IsKeyPressed(this.KeyUP) && this.DefaultClock.ElapsedTime.AsSeconds() > 1)
                                {
                                    this.sTp.Play();
                                    this.SetPosition(level.tp1Position.X, level.tp1Position.Y);
                                    this.DefaultClock.Restart();
                                }
                                break;
                            }
                        //////////////////
                        case BlockType.Hint:
                        {
                            level.SetHints(obstacle, this);
                            break;
                        }
                        default: { break; }
                    }
                }
                else
                {
                    if (obstacle.Type == BlockType.Hint)
                    {
                        level.HideHint(obstacle);
                    }
                }
            }

            foreach (var trap in level.Traps)
            {
                if (this.GetBoundingBox().Intersects(trap.GetBoundingBox()))
                {
                    if ((trap.Type == TrapType.BlowTorchLeft || trap.Type == TrapType.BlowTorchRight) && trap.IsBlowing) this.Die(level);
                    else if (trap.Type == TrapType.Crusher || trap.Type == TrapType.Spikes) this.Die(level);
                }
            }

            foreach (Monster monster in level.Monsters)
            {
                if (this.GetBoundingBox().Intersects(monster.GetBoundingBox()))
                {
                    this.Die(level);
                }
            }
            foreach (Archer archer in level.Archers)
            {
                if (this.GetBoundingBox().Intersects(archer.GetBoundingBox()))
                {
                    this.Die(level);
                }
                if (this.GetBoundingBox().Intersects(archer.Arrow.GetBoundingBox()))
                {
                    this.Die(level);
                    archer.Arrow.DeleteArrow();
                }
            }
            foreach (Ghost ghost in level.Ghosts)
            {
                if (this.GetBoundingBox().Intersects(ghost.GetBoundingBox()))
                {
                    this.Die(level);
                }
            }
            foreach (Wizard wizard in level.Wizards)
            {
                if (this.GetBoundingBox().Intersects(wizard.GetBoundingBox()))
                {
                    this.Die(level);
                }
                if (this.GetBoundingBox().Intersects(wizard.EnergyBall.GetBoundingBox()))
                {
                    this.Die(level);
                }
            }
        }
        public void Die(Level level)
        {
            if (!this.IsDead && this.IsVulnerable)
            {
                this.sDie.Play();
                level.AddParticleEffect(new ParticleEffect(this.X, this.Y, Color.Red));
                this.Sword.Reset();
                this.DefaultClock.Restart();
                this.IsDead = true;
                this.SpeedX = 0f;
                this.SpeedY = -10f;
                //this.Lives--;

                this.HasSilverKey = false;
                this.HasGoldenKey = false;
                
                if (this.Lives <= 0)
                {
                    this.OutOfLives = true;
                    this.Lives = 0;
                }
            }
        }
        public void ImmortalityEffect()
        {
            if (this._immortalityAnimationFlag)
            {
                if (this._immortalityAnimationCounter > 255) this._immortalityAnimationFlag = false;
                this._immortalityAnimationCounter += 20;
                this.SetColor(new Color(255, 255, 255, this._immortalityAnimationCounter));
            }
            else
            {
                if (this._immortalityAnimationCounter < 0) this._immortalityAnimationFlag = true;
                this._immortalityAnimationCounter -= 20;
                this.SetColor(new Color(255, 255, 255, this._immortalityAnimationCounter));
            }
        }
        public void Respawn(Level level)
        {
            this.IsDead = false;
            this.SetTextureRectanlge(32, 64, 32, 32);
            this.SetStartingPosition(level);
            this.DefaultClock.Restart();
        }
        public void SetStartingPosition(Level level)
        {
            this.SetPosition(level.EnterancePosition.X, level.EnterancePosition.Y);
        }
        public void AddToScore(Level level, int value, float x, float y)
        {
            this.Score += value;
            level.ScoreAdditionEffects.Add(new ScoreAdditionEffect(value, x, y));
        }
        public void ResetMainCharacter()
        {
            this.GotExit = false;
            this.IsAttacking = false;
            this.IsDead = false;
            this.OutOfLives = false;
            this.IsVulnerable = true;
            this.ArrowAmount = 3;
            this.Lives = 3;
            this.Mana = 0;
            this.Score = 0;

            this._immortalityAnimationCounter = 0;
            this._immortalityAnimationFlag = false;

            this.HasSilverKey = false;
            this.HasGoldenKey = false;
        }
        public override void Draw(RenderTarget target, RenderStates states)
        {
            base.Draw(target, states);
            target.Draw(this.Arrow, states);
            target.Draw(this.Sword, states);
        }
    }
}
