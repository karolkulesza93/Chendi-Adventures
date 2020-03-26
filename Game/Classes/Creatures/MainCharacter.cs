using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace ChendiAdventures
{
    public sealed class MainCharacter : Creature
    {
        //steering
        public static Keyboard.Key KeyUP = Keyboard.Key.Up;
        public static Keyboard.Key KeyDOWN = Keyboard.Key.Down;
        public static Keyboard.Key KeyLEFT = Keyboard.Key.Left;
        public static Keyboard.Key KeyRIGHT = Keyboard.Key.Right;
        public static Keyboard.Key KeyJUMP = Keyboard.Key.Z;
        public static Keyboard.Key KeyATTACK = Keyboard.Key.X;
        public static Keyboard.Key KeyARROW = Keyboard.Key.C;
        public static Keyboard.Key KeyTHUNDER = Keyboard.Key.D;
        public static Keyboard.Key KeyIMMORTALITY = Keyboard.Key.S;
        
        public readonly Clock DefaultClock;

        public MainCharacter(float x, float y, Texture texture) : base(x, y, texture)
        {
            SetTextureRectangle(32, 64);
            Lives = 3;
            Continues = 2;
            OutOfLives = false;
            DefaultClock = new Clock();
            IsStandingOnBlocks = false;
            Score = 0;
            LivesGranted = 0;

            MaxSpeedX = 4f;
            MaxSpeedY = 10.15f;

            dX = 0.4f;
            GravityForce = 0.5f;

            SafePosition = new Vector2f(x,y);
            SafePositionClock = new Clock();
            JustRespawned = false;

            Sword = new Sword(this);
            Arrow = new Arrow(-100, -100, ArrowTexture, Movement.Right);
            ArrowAmount = 3;
            Mana = 1;
            IsJumping = false;
            IsAttacking = false;
            IsUpAttacking = false;
            IsShooting = false;
            IsVulnerable = true;
            _immortalityAnimationCounter = 0;
            _immortalityAnimationFlag = false;

            HasSilverKey = false;
            HasGoldenKey = false;
            HasCrystalKey = false;

            GotExit = false;

            //animations
            _standing = new Animation(this, 0.2f,
                new Vector2i(32, 64),
                new Vector2i(128, 0)
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
                new Vector2i(0, 160),
                new Vector2i(32, 160),
                new Vector2i(64, 160),
                new Vector2i(96, 160),
                new Vector2i(128, 160)
            );
            _attackRight = new Animation(this, 0.05f,
                new Vector2i(0, 128),
                new Vector2i(32, 128),
                new Vector2i(64, 128),
                new Vector2i(96, 128),
                new Vector2i(128, 128)
            );
            _attackDown = new Animation(this, 0.05f,
                new Vector2i(0, 224),
                new Vector2i(32, 224),
                new Vector2i(64, 224)
            );

            _jumpUp = new Animation(this, 0.05f,
                new Vector2i(0, 192),
                new Vector2i(32, 192)
            );
            _jumpDown = new Animation(this, 0.05f,
                new Vector2i(64, 192),
                new Vector2i(96, 192)
            );
            _jumpBack = new Animation(this, 0.05f,
                new Vector2i(64, 64),
                new Vector2i(96, 64)
            );
            _jumpLeft = new Animation(this, 0.05f,
                new Vector2i(64, 96),
                new Vector2i(96, 96)
            );
            _jumpRight = new Animation(this, 0.05f,
                new Vector2i(0, 96),
                new Vector2i(32, 96)
            );

            _victoryAnimation = new Animation(this, 0.1f,
                new Vector2i(128, 64),
                new Vector2i(128, 192),
                new Vector2i(128, 96),
                new Vector2i(128, 64),
                new Vector2i(128, 192),
                new Vector2i(128, 32)
            );
            //

            sJump = new Sound(new SoundBuffer(@"sfx/jump.wav"));
            sStep = new Sound(new SoundBuffer(@"sfx/step.wav")) {Volume = 15};
            sLand = new Sound(new SoundBuffer(@"sfx/land.wav")) {Volume = 30};
            sTramp = new Sound(new SoundBuffer(@"sfx/trampoline.wav"));
            sCoin = new Sound(new SoundBuffer(@"sfx/coin.wav")) {Volume = 40};
            sAtk = new Sound(new SoundBuffer(@"sfx/sword.wav"));
            sDie = new Sound(new SoundBuffer(@"sfx/death.wav"));
            sTp = new Sound(new SoundBuffer(@"sfx/teleport.wav"));
            sKey = new Sound(new SoundBuffer(@"sfx/key.wav"));
            sLife = new Sound(new SoundBuffer(@"sfx/life.wav"));
            sPickup = new Sound(new SoundBuffer(@"sfx/pickup.wav"));
            sImmortality = new Sound(new SoundBuffer(@"sfx/immortality.wav")) {Volume = 30, Loop = true};
            sError = new Sound(new SoundBuffer(@"sfx/error.wav")) {Volume = 10};
        }

        public Sword Sword { get; }
        public Arrow Arrow { get; }
        public bool JustRespawned { get; set; }
        public int Coins { get; set; }
        public int ArrowAmount { get; set; }
        public int Mana { get; set; }
        public int Lives { get; set; }
        public int Continues { get; set; }
        public int Score { get; set; }
        public bool IsJumping { get; private set; }
        public bool IsAttacking { get; set; }
        public bool IsUpAttacking { get; set; }
        public bool IsDownAttacking { get; set; }
        public bool IsShooting { get; private set; }
        public bool IsVulnerable { get; private set; }
        public bool HasSilverKey { get; set; }
        public bool HasGoldenKey { get; set; }
        public bool HasCrystalKey { get; set; }
        public bool OutOfLives { get; set; }
        public bool GotExit { get; set; }
        public int LivesGranted { get; set; }
        public Vector2f SafePosition { get; set; }
        public Clock SafePositionClock { get; }


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
        public Sound sError { get; }

        public void MainCharacterSteering(Level level)
        {
            if (!IsDead && !GotExit)
            {
                //jump
                if ((Keyboard.IsKeyPressed(KeyJUMP) || Joystick.IsButtonPressed(0, 0)) && !IsShooting && !IsAttacking && !IsJumping) Jump();
                IsJumping = Keyboard.IsKeyPressed(KeyJUMP) || Joystick.IsButtonPressed(0, 0);
                //attack
                if ((Keyboard.IsKeyPressed(KeyATTACK) || Joystick.IsButtonPressed(0, 2)) && DefaultClock.ElapsedTime.AsMilliseconds() > 500 &&
                    IsVulnerable && !IsAttacking && !IsShooting)
                {
                    if (sAtk.Status != SoundStatus.Playing) sAtk.Play();
                    Sword.LastMove = _lastMove;
                    IsAttacking = true;
                    sJump.Stop();
                    if ((Keyboard.IsKeyPressed(Keyboard.Key.Up) || Joystick.GetAxisPosition(0, Joystick.Axis.Y) < -50 || Joystick.GetAxisPosition(0, Joystick.Axis.PovY) > 5) && !IsDownAttacking) IsUpAttacking = true;
                    else if ((Keyboard.IsKeyPressed(KeyDOWN) || Joystick.GetAxisPosition(0, Joystick.Axis.Y) > 50 || Joystick.GetAxisPosition(0, Joystick.Axis.PovY) < -5) && !IsUpAttacking && !IsStandingOnBlocks && SpeedY >= -5f)
                        IsDownAttacking = true;
                    DefaultClock.Restart();
                }

                //arrow
                if ((Keyboard.IsKeyPressed(KeyARROW) || Joystick.IsButtonPressed(0, 3)) && DefaultClock.ElapsedTime.AsMilliseconds() > 700 &&
                    ArrowAmount > 0 && IsVulnerable && Arrow.X < 0 && IsStandingOnBlocks && !IsShooting && !IsAttacking)
                {
                    IsShooting = true;
                    Projectile.sShoot.Play();
                    ArrowAmount--;
                    Arrow.isEnergized = false;
                    Arrow.ArrowDirectionDefine(_lastMove);
                    DefaultClock.Restart();
                    Arrow.SetPosition(X, Y + 12);
                }
                else if ((Keyboard.IsKeyPressed(KeyARROW) || Joystick.IsButtonPressed(0, 3)) && DefaultClock.ElapsedTime.AsMilliseconds() > 700 &&
                         IsStandingOnBlocks && !JustRespawned && !IsShooting && !IsAttacking && IsVulnerable)
                {
                    level.ScoreAdditionEffects.Add(new ScoreAdditionEffect(0, X + 2, Y, "ARROW NEEDED"));
                    DefaultClock.Restart();
                    sError.Play();
                }

                //energized arrow
                if ((Keyboard.IsKeyPressed(KeyTHUNDER) || Joystick.IsButtonPressed(0, 1)) && DefaultClock.ElapsedTime.AsMilliseconds() > 1200 &&
                    ArrowAmount > 0 && Mana > 0 && IsVulnerable && IsStandingOnBlocks)
                {
                    IsShooting = true;
                    Projectile.sEnergyShoot.Play();
                    ArrowAmount--;
                    Mana--;
                    Arrow.isEnergized = true;
                    Arrow.ArrowDirectionDefine(_lastMove);
                    DefaultClock.Restart();
                    Arrow.SetPosition(X, Y + 12);
                }
                else if ((Keyboard.IsKeyPressed(KeyTHUNDER) || Joystick.IsButtonPressed(0, 1)) && DefaultClock.ElapsedTime.AsMilliseconds() > 1200 &&
                         IsStandingOnBlocks && !IsShooting && !IsAttacking && !JustRespawned && IsVulnerable)
                {
                    if (Mana == 0 && ArrowAmount == 0)
                        level.ScoreAdditionEffects.Add(new ScoreAdditionEffect(0, X - 10, Y,
                            "POTION AND ARROW NEEDED"));
                    else if (Mana == 0)
                        level.ScoreAdditionEffects.Add(new ScoreAdditionEffect(0, X, Y, "POTION NEEDED"));
                    else
                        level.ScoreAdditionEffects.Add(new ScoreAdditionEffect(0, X, Y, "ARROW NEEDED"));
                    DefaultClock.Restart();
                    sError.Play();
                }

                //immortality
                if ((Keyboard.IsKeyPressed(KeyIMMORTALITY) || Joystick.IsButtonPressed(0, 5) || Joystick.IsButtonPressed(0, 4)) && DefaultClock.ElapsedTime.AsMilliseconds() > 500 &&
                    Mana >= 3 && IsVulnerable)
                {
                    Mana -= 3;
                    DefaultClock.Restart();
                    IsVulnerable = false;
                    sImmortality.Play();
                }
                else if ((Keyboard.IsKeyPressed(KeyIMMORTALITY) || Joystick.IsButtonPressed(0, 5) || Joystick.IsButtonPressed(0, 4)) && DefaultClock.ElapsedTime.AsMilliseconds() > 500 && IsVulnerable && !JustRespawned)
                {
                    level.ScoreAdditionEffects.Add(new ScoreAdditionEffect(0, X, Y, "MORE POTIONS NEEDED"));
                    DefaultClock.Restart();
                    sError.Play();
                }

                //movement left
                if ((Keyboard.IsKeyPressed(KeyLEFT) || Joystick.GetAxisPosition(0, Joystick.Axis.X) < -50 || Joystick.GetAxisPosition(0, Joystick.Axis.PovX) < -5) &&
                    !(Keyboard.IsKeyPressed(KeyRIGHT) || Joystick.GetAxisPosition(0, Joystick.Axis.X) > 50 || Joystick.GetAxisPosition(0, Joystick.Axis.PovX) > 5) && !IsStandingOnBlocks && !IsDownAttacking)
                {
                    MoveLeft();
                    if (SpeedX <= 0 && !IsAttacking)
                    {
                        _lastMove = Movement.Left;
                    }
                }
                else if ((Keyboard.IsKeyPressed(KeyLEFT) || Joystick.GetAxisPosition(0, Joystick.Axis.X) <  -50 || Joystick.GetAxisPosition(0, Joystick.Axis.PovX) < -5) && 
                         !(Keyboard.IsKeyPressed(KeyRIGHT) || Joystick.GetAxisPosition(0, Joystick.Axis.X) > 50 || Joystick.GetAxisPosition(0, Joystick.Axis.PovX) > 5) && !IsAttacking && !IsShooting)
                {
                    MoveLeft();
                    if (SpeedX <= 0)
                    {
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
                if ((Keyboard.IsKeyPressed(KeyRIGHT) || Joystick.GetAxisPosition(0, Joystick.Axis.X) > 50 || Joystick.GetAxisPosition(0, Joystick.Axis.PovX) > 5) && 
                    !(Keyboard.IsKeyPressed(KeyLEFT) || Joystick.GetAxisPosition(0, Joystick.Axis.X) < -50 || Joystick.GetAxisPosition(0, Joystick.Axis.PovX) < -5) && !IsStandingOnBlocks && !IsDownAttacking)
                {
                    MoveRight();
                    if (SpeedX >= 0 && !IsAttacking)
                    {
                        _lastMove = Movement.Right;
                    }
                }
                else if ((Keyboard.IsKeyPressed(KeyRIGHT) || Joystick.GetAxisPosition(0, Joystick.Axis.X) > 50 || Joystick.GetAxisPosition(0, Joystick.Axis.PovX) > 5) &&
                         !(Keyboard.IsKeyPressed(KeyLEFT) || Joystick.GetAxisPosition(0, Joystick.Axis.X) < -50 || Joystick.GetAxisPosition(0, Joystick.Axis.PovX) < -5) && !IsAttacking && !IsShooting)
                {
                    MoveRight();
                    if (SpeedX >= 0)
                    {
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
                    SpeedX -= dX / 4;
                    if (SpeedX < 0) SpeedX = 0f;
                }
                else if (SpeedX < 0f)
                {
                    SpeedX += dX / 4;
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
                IsAttacking = false;
                IsVulnerable = true;
                sImmortality.Stop();
                
                SpeedX = 0f;
                SpeedY = 0f;
            }

            GrantAdditionalLifeDependingOnScore();

            CollisionDependence(level);

            if (SpeedX != 0 && IsStandingOnBlocks && sStep.Status != SoundStatus.Playing) sStep.Play();

            if (!IsVulnerable)
            {
                if (JustRespawned && DefaultClock.ElapsedTime.AsSeconds() > 3)
                {
                    IsVulnerable = true;
                    JustRespawned = false;
                    SetColor(Color.White);
                }
                else if (DefaultClock.ElapsedTime.AsSeconds() > 6)
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
                if (DefaultClock.ElapsedTime.AsSeconds() > 0.25f && !IsDownAttacking)
                {
                    IsAttacking = false;
                    IsUpAttacking = false;
                }
                else if (IsStandingOnBlocks && IsDownAttacking)
                {
                    IsAttacking = false;
                    IsDownAttacking = false;
                }
                else if (IsUpAttacking)
                {
                    Sword.AttackUp();
                }
                else if (IsDownAttacking && !IsStandingOnBlocks)
                {
                    Sword.AttackDown();
                }
                else if (!IsUpAttacking && !IsDownAttacking)
                {
                    Sword.Attack();
                }
            }
            else
            {
                Sword.Reset();
            }

            if (IsShooting && DefaultClock.ElapsedTime.AsSeconds() > 0.7f)
            {
                IsShooting = false;
            }

            UpdateTextures();
        }

        public override void UpdateTextures()
        {
            if (SpeedX < 0)
                MovementDirection = Movement.Left;
            else if (SpeedX > 0)
                MovementDirection = Movement.Right;
            else MovementDirection = Movement.None;

            if (IsStandingOnBlocks)
            {
                if (IsShooting)
                {
                    if (_lastMove == Movement.Left)
                        SetTextureRectangle(64, 32);
                    else if (_lastMove == Movement.Right) SetTextureRectangle(96, 32);
                }
                else if (IsAttacking)
                {
                    if (IsUpAttacking)
                        SetTextureRectangle(0, 64);
                    else if (_lastMove == Movement.Left)
                        _attackLeft.Animate(32, Sword.AnimLeftFrameNumber());
                    else if (_lastMove == Movement.Right) _attackRight.Animate(32, Sword.AnimRightFrameNumber());
                }
                else
                {
                    if (MovementDirection == Movement.Right) _animRight.Animate();
                    else if (MovementDirection == Movement.Left) _animLeft.Animate();
                    else if (Keyboard.IsKeyPressed(Keyboard.Key.Up) || Joystick.GetAxisPosition(0, Joystick.Axis.Y) < -50 || Joystick.GetAxisPosition(0, Joystick.Axis.PovY) > 5) SetTextureRectangle(0, 64);
                    else _standing.Animate();
                }
            }
            else
            {
                if (IsAttacking)
                {
                    if (IsUpAttacking)
                        SetTextureRectangle(64, 64);
                    else if (IsDownAttacking)
                        _attackDown.Animate();
                    else if (_lastMove == Movement.Left)
                        _attackLeft.Animate(32, Sword.AnimLeftFrameNumber());
                    else if (_lastMove == Movement.Right) _attackRight.Animate(32, Sword.AnimRightFrameNumber());
                }
                else if (IsShooting)
                {
                    if (_lastMove == Movement.Left)
                        SetTextureRectangle(64, 32);
                    else if (_lastMove == Movement.Right) SetTextureRectangle(96, 32);
                }
                else if (SpeedY != 0)
                {
                    if (MovementDirection == Movement.Left)
                    {
                        _jumpLeft.Animate();
                    }
                    else if (MovementDirection == Movement.Right)
                    {
                        _jumpRight.Animate();
                    }
                    else if (Keyboard.IsKeyPressed(Keyboard.Key.Up) || Joystick.GetAxisPosition(0, Joystick.Axis.Y) < -50 || Joystick.GetAxisPosition(0, Joystick.Axis.PovY) > 5)
                    {
                        _jumpBack.Animate();
                    }
                    else
                    {
                        if (SpeedY < 0) _jumpUp.Animate();
                        else _jumpDown.Animate();
                    }
                }
            }

            if (IsDead)
            {
                if (IsStandingOnBlocks) SetTextureRectangle(96, 0);
                else SetTextureRectangle(64, 0);
            }

            if (GotExit) _victoryAnimation.Animate();
        }

        public void CollisionDependence(Level level)
        {
            var NewX = Get32NextPosition().X;
            var NewY = Get32NextPosition().Y;
            Block obstacle;

            // HORIZONTAL
            if (SpeedX < 0) // moving left
            {
                if (level.UnpassableContains(
                        (obstacle = level.GetObstacle(Get32NextPosition().X + 0.01f, Get32Position().Y + 0.15f))
                        .Type) ||
                    level.UnpassableContains((obstacle =
                        level.GetObstacle(Get32NextPosition().X + 0.01f, Get32Position().Y + 0.85f)).Type))
                {
                    NewX = (int)Get32NextPosition().X + 1;
                    SpeedX = 0;
                    //Console.WriteLine("from RIGHT side");
                }
            }
            else if (SpeedX > 0) //moving right
            {
                if (level.UnpassableContains(
                        (obstacle = level.GetObstacle(Get32NextPosition().X + 0.99f, Get32Position().Y + 0.15f))
                        .Type) ||
                    level.UnpassableContains((obstacle =
                        level.GetObstacle(Get32NextPosition().X + 0.99f, Get32Position().Y + 0.85f)).Type))
                {
                    NewX = (int)Get32NextPosition().X;
                    SpeedX = 0;
                    //Console.WriteLine("from LEFT side");
                }
            }

            // VERTICAL
            IsStandingOnBlocks = false;

            if (SpeedY < 0) // moving up
            {
                if (level.UnpassableContains((obstacle =
                        level.GetObstacle(Get32NextPosition().X + 0.15f, Get32NextPosition().Y + 0.1f)).Type) ||
                    level.UnpassableContains((obstacle =
                        level.GetObstacle(Get32NextPosition().X + 0.85f, Get32NextPosition().Y + 0.1f)).Type))
                {
                    NewY = (int)Get32NextPosition().Y + 1;
                    if (SpeedY < -10f) sJump.Stop();
                    SpeedY = 0;
                    //Console.WriteLine("from BOTTOM side");
                }
            }
            else //moving down
            {
                if (level.UnpassableContains((obstacle =
                        level.GetObstacle(Get32NextPosition().X + 0.15f, Get32NextPosition().Y + 0.99f)).Type) ||
                    level.UnpassableContains((obstacle =
                        level.GetObstacle(Get32NextPosition().X + 0.85f, Get32NextPosition().Y + 0.99f)).Type))
                {
                    if (obstacle.Type == BlockType.Stone) obstacle.Stomp();
                    if ((obstacle =
                            level.GetObstacle(Get32NextPosition().X + 0.85f, Get32NextPosition().Y + 0.99f)).Type ==
                        BlockType.Stone)
                        obstacle.Stomp();
                    NewY = (int)Get32NextPosition().Y;
                    IsStandingOnBlocks = true;

                    if (SafePositionClock.ElapsedTime.AsSeconds() > 5 && (obstacle.Type == BlockType.Brick || obstacle.Type == BlockType.Dirt) && !IsDead && IsVulnerable)
                    {
                        SafePosition = new Vector2f(obstacle.X, obstacle.Y - 32);
                        SafePositionClock.Restart();
                    }

                    if (SpeedY > 1f) sLand.Play();
                    SpeedY = 0;
                }
            }

            Set32Position(NewX, NewY);

            if (!IsDead) ObstaclesCollision(level);
        }

        public void ObstaclesCollision(Level level)
        {
            Block obstacle = null;
            for (var i = 0; i < 4; i++)
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
                        obstacle = level.GetObstacle(Get32Position().X + 0.9f, Get32Position().Y + 0.1f);
                        break;
                    }
                    case 2:
                    {
                        obstacle = level.GetObstacle(Get32Position().X + 0.9f, Get32Position().Y + 0.9f);
                        break;
                    }
                    case 3:
                    {
                        obstacle = level.GetObstacle(Get32Position().X + 0.1f, Get32Position().Y + 0.9f);
                        break;
                    }
                }

                switch (obstacle.Type)
                {
                    case BlockType.Exit:
                    {
                        if ((Keyboard.IsKeyPressed(Keyboard.Key.Up) || Joystick.GetAxisPosition(0, Joystick.Axis.Y) < -50 || Joystick.GetAxisPosition(0, Joystick.Axis.PovY) > 5) && IsStandingOnBlocks && !IsDead && SpeedX == 0)
                        {
                            SpeedX = 0;
                            SetTextureRectangle(128, 64);
                            GotExit = true;
                        }

                        break;
                    }
                    case BlockType.Shop:
                    {
                        if ((Keyboard.IsKeyPressed(Keyboard.Key.Up) || Joystick.GetAxisPosition(0, Joystick.Axis.Y) < -50 || Joystick.GetAxisPosition(0, Joystick.Axis.PovY) > 5) && IsStandingOnBlocks && !IsDead && SpeedX == 0)
                            level.isShopOpened = true;

                        break;
                    }
                    case BlockType.Lever:
                    {
                        if ((Keyboard.IsKeyPressed(Keyboard.Key.Up) || Joystick.GetAxisPosition(0, Joystick.Axis.Y) < -50 || Joystick.GetAxisPosition(0, Joystick.Axis.PovY) > 5) && IsStandingOnBlocks && !IsDead && SpeedX == 0)
                            Block.FlipLever();
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
                    case BlockType.CrystalKey:
                    {
                        HasCrystalKey = true;
                        level.UnableToPassl.Remove(BlockType.CrystalDoor);
                        AddToScore(level, 1000, obstacle.X, obstacle.Y);
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
                    case BlockType.CrystalDoor:
                    {
                        if (HasCrystalKey)
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
                    case BlockType.WoodenSpike:
                    {
                        Die(level);
                        break;
                    }
                    case BlockType.Trampoline:
                    {
                        if (SpeedY > 4f && !IsDead &&
                            (!Keyboard.IsKeyPressed(KeyDOWN) && Joystick.GetAxisPosition(0, Joystick.Axis.Y) < 50 && Joystick.GetAxisPosition(0, Joystick.Axis.PovY) > -5))
                        {
                            SetPosition(X, obstacle.Y - Height);
                            IsDownAttacking = false;
                            IsAttacking = false;
                            obstacle.SetTextureRectangle(96, 32);
                            obstacle.DefaultTimer.Restart();

                            if (Keyboard.IsKeyPressed(KeyJUMP) || Joystick.IsButtonPressed(0, 0))
                            {
                                SpeedY *= -1.2f;
                                if (SpeedY > -1 * MaxSpeedY - 1.5f) 
                                    SpeedY = -1 * MaxSpeedY - 1.5f;
                                sJump.Play();
                            }
                            else
                            {
                                SpeedY = -1 * MaxSpeedY - 1.5f;
                            }

                            if (SpeedY < -17.3f) SpeedY = -17.3f;

                            sTramp.Play();
                        }

                        break;
                    }
                    //teleports/////////////////
                    case BlockType.Teleport1:
                    {
                        if ((Keyboard.IsKeyPressed(Keyboard.Key.Up) || Joystick.GetAxisPosition(0, Joystick.Axis.Y) < -50 || Joystick.GetAxisPosition(0, Joystick.Axis.PovY) > 5) && DefaultClock.ElapsedTime.AsSeconds() > 1 &&
                            IsStandingOnBlocks && !IsDead && SpeedX == 0)
                        {
                            sTp.Play();
                            SetPosition(level.tp2Position.X, level.tp2Position.Y);
                            DefaultClock.Restart();
                        }

                        break;
                    }
                    case BlockType.Teleport2:
                    {
                        if ((Keyboard.IsKeyPressed(Keyboard.Key.Up) || Joystick.GetAxisPosition(0, Joystick.Axis.Y) < -50 || Joystick.GetAxisPosition(0, Joystick.Axis.PovY) > 5) && DefaultClock.ElapsedTime.AsSeconds() > 1 &&
                            IsStandingOnBlocks && !IsDead && SpeedX == 0)
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
                        if ((Keyboard.IsKeyPressed(Keyboard.Key.Up) || Joystick.GetAxisPosition(0, Joystick.Axis.Y) < -50 || Joystick.GetAxisPosition(0, Joystick.Axis.PovY) > 5) && DefaultClock.ElapsedTime.AsSeconds() > 1 &&
                            IsStandingOnBlocks && !IsDead && SpeedX == 0)
                        {
                            sTp.Play();
                            SetPosition(level.tp4Position.X, level.tp4Position.Y);
                            DefaultClock.Restart();
                        }

                        break;
                    }
                    case BlockType.Teleport4:
                    {
                        if ((Keyboard.IsKeyPressed(Keyboard.Key.Up) || Joystick.GetAxisPosition(0, Joystick.Axis.Y) < -50 || Joystick.GetAxisPosition(0, Joystick.Axis.PovY) > 5) && DefaultClock.ElapsedTime.AsSeconds() > 1 &&
                            IsStandingOnBlocks && !IsDead && SpeedX == 0)
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
                    else if (trap.Type == TrapType.Crusher || trap.Type == TrapType.Spikes)
                    {
                        Die(level);
                    }
                    else if (trap.Type == TrapType.BlowerLeft)
                    {
                        if (trap.IsBlowing)
                        {
                            SpeedY = -5f;
                            SpeedX = -25f;
                        }
                    }
                    else if (trap.Type == TrapType.BlowerRight)
                    {
                        if (trap.IsBlowing)
                        {
                            SpeedY = -5f;
                            SpeedX = 25f; 
                        }
                    }
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

            foreach (var golem in level.Golems)
            {
                if (GetBoundingBox().Intersects(golem.GetBoundingBox())) Die(level);
                if (GetBoundingBox().Intersects(golem.Boulder.GetBoundingBox()))
                {
                    Die(level);
                    golem.Boulder.ResetBoulder(level);
                }
            }

            foreach (var walker in level.Walkers)
            {
                if (GetBoundingBox().Intersects(walker.GetBoundingBox())) Die(level);
                if (GetBoundingBox().Intersects(walker.Laser.GetBoundingBox()))
                {
                    Die(level);
                    walker.Laser.DeleteArrow();
                }
            }
        }

        public void Die(Level level)
        {
            if (!IsDead && IsVulnerable && !GotExit)
            {
                sJump.Stop();
                sAtk.Stop();

                sDie.Play();
                sKill.Play();

                level.AddParticleEffect(new ParticleEffect(X, Y, Color.Red));
                Sword.Reset();
                DefaultClock.Restart();
                IsDead = true;
                SpeedY = -6f;

                IsAttacking = false;
                IsDownAttacking = false;
                IsUpAttacking = false;

                //Lives--;

                level.isShopOpened = false;

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

        public void Respawn()
        {
            IsDead = false;
            IsVulnerable = false;
            JustRespawned = true;
            ApplySafePosition();
            DefaultClock.Restart();
        }

        public void SetStartingPosition(Level level)
        {
            SetPosition(level.EnterancePosition.X, level.EnterancePosition.Y);
        }

        public void ApplySafePosition()
        {
            X = SafePosition.X;
            Y = SafePosition.Y;
            JustRespawned = true;
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
            Mana = 1;
            Score = 0;
            LivesGranted = 0;
            Coins = 0;

            SpeedY = 0;
            SpeedX = 0;

            _immortalityAnimationCounter = 0;
            _immortalityAnimationFlag = false;

            HasSilverKey = false;
            HasGoldenKey = false;
            HasCrystalKey = false;
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
            target.Draw(Arrow);
            target.Draw(Sword);
        }

        public new FloatRect GetBoundingBox()
        {
            return new FloatRect(X + 2, Y + 2, 30, 30);
        }

        private byte _immortalityAnimationCounter;
        private bool _immortalityAnimationFlag;
        private readonly Animation _attackDown;
        private readonly Animation _attackLeft;
        private readonly Animation _attackRight;
        private readonly Animation _jumpBack;
        private readonly Animation _jumpDown;
        private readonly Animation _jumpLeft;
        private readonly Animation _jumpRight;
        private readonly Animation _jumpUp;
        private readonly Animation _standing;
        private readonly Animation _victoryAnimation;
        private Movement _lastMove;

    }
}