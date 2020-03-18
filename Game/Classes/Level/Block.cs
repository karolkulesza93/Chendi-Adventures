using SFML.Audio;
using SFML.Graphics;
using SFML.System;

namespace ChendiAdventures
{
    public class Block : Entity
    {
        public static Sound sCrush = new Sound(new SoundBuffer(@"sfx/crush.wav"));
        public static Sound sPurify = new Sound(new SoundBuffer(@"sfx/petrifier.wav"));
        public static Sound sHard = new Sound(new SoundBuffer(@"sfx/hard.wav"));
        public static Sound sDestroy = new Sound(new SoundBuffer(@"sfx/destroyed.wav")) {Volume = 60};
        public static Sound sLever = new Sound(new SoundBuffer(@"sfx/lever.wav"));
        public static Sound sShatter = new Sound(new SoundBuffer(@"sfx/shatter.wav")) {Volume = 70};
        public static Clock LeverTimer;
        public Clock DefaultTimer;

        public Block(float x, float y, Texture texture, BlockType type = BlockType.None, int hintNumber = 0) : base(x,
            y, texture)
        {
            OriginalPos = new Vector2f(x, y);
            Type = type;
            IsDestroyed = false;
            HintNumber = hintNumber;
            SetBlock(type);
        }

        public Vector2f OriginalPos { get; }
        public Animation BlockAnimation { get; private set; }
        public TextLine Hint { get; set; }
        public int HintNumber { get; set; }
        public bool IsDestroyed { get; private set; }
        public bool IsShattered { get; private set; }
        public bool IsStomped { get; set; }
        public int Health { get; set; }
        public BlockType Type { get; set; }

        public void SetBlock(BlockType type)
        {
            switch (type)
            {
                case BlockType.Brick:
                {
                    SetTextureRectangle(0, 0);
                    SetColor(Level.LevelColor);
                    break;
                }
                case BlockType.TransparentBrick:
                {
                    SetTextureRectangle(0, 160);
                    SetColor(Level.LevelColor);
                    break;
                }
                case BlockType.HardBlock:
                {
                    SetTextureRectangle(0, 192);
                    Health = 200;
                    break;
                }
                case BlockType.SteelGate:
                {
                    SetTextureRectangle(96, 160);
                    Level.SteelGates.Add(this);
                    break;
                }
                case BlockType.Lever:
                {
                    SetTextureRectangle(32, 160);
                    LeverTimer = new Clock();
                    Level.IsLeverOn = false;
                    Level.Levers.Add(this);
                    break;
                }
                case BlockType.EnergyBall:
                {
                    BlockAnimation = new Animation(this, 0.05f,
                        new Vector2i(0, 224),
                        new Vector2i(32, 224),
                        new Vector2i(64, 224),
                        new Vector2i(96, 224),
                        new Vector2i(64, 224),
                        new Vector2i(32, 224)
                    );
                    SetTextureRectangle(0, 224);
                    break;
                }
                case BlockType.Spike:
                {
                    SetTextureRectangle(0, 32);
                    break;
                }
                case BlockType.WoodenSpike:
                {
                    SetTextureRectangle(32, 288);
                    break;
                    }
                case BlockType.Enterance:
                {
                    SetTextureRectangle(32, 64);
                    break;
                }
                case BlockType.Shop:
                {
                    SetTextureRectangle(32, 32);
                    break;
                }
                case BlockType.Purifier:
                {
                    BlockAnimation = new Animation(this, 0.05f,
                        new Vector2i(64, 64),
                        new Vector2i(96, 64)
                    );
                    SetTextureRectangle(64, 64);
                    break;
                }
                case BlockType.BrokenBrick:
                {
                    SetTextureRectangle(0,288);
                    SetColor(Level.LevelColor);
                    break;
                }
                case BlockType.Coin:
                {
                    BlockAnimation = new Animation(this, 0.05f,
                        new Vector2i(0, 0),
                        new Vector2i(32, 0),
                        new Vector2i(64, 0),
                        new Vector2i(96, 0),
                        new Vector2i(128, 0),
                        new Vector2i(160, 0)
                    );
                    SetTextureRectangle(0, 0);
                    break;
                }
                case BlockType.SackOfGold:
                {
                    BlockAnimation = new Animation(this, 0.05f,
                        new Vector2i(0, 320),
                        new Vector2i(32, 320),
                        new Vector2i(64, 320),
                        new Vector2i(96, 320),
                        new Vector2i(64, 320),
                        new Vector2i(32, 320)
                    );
                    SetTextureRectangle(0, 320);
                    break;
                }
                case BlockType.Life:
                {
                    BlockAnimation = new Animation(this, 0.05f,
                        new Vector2i(64, 96),
                        new Vector2i(32, 96),
                        new Vector2i(0, 96),
                        new Vector2i(0, 96),
                        new Vector2i(0, 96),
                        new Vector2i(0, 96),
                        new Vector2i(0, 96),
                        new Vector2i(32, 96),
                        new Vector2i(64, 96),
                        new Vector2i(96, 192),
                        new Vector2i(96, 160),
                        new Vector2i(96, 128),
                        new Vector2i(96, 96),
                        new Vector2i(96, 128),
                        new Vector2i(96, 160),
                        new Vector2i(96, 192)
                    );
                    SetTextureRectangle(64, 96);
                    break;
                }
                case BlockType.Arrow:
                {
                    BlockAnimation = new Animation(this, 0.05f,
                        new Vector2i(64, 160),
                        new Vector2i(32, 160),
                        new Vector2i(0, 160),
                        new Vector2i(0, 160),
                        new Vector2i(0, 160),
                        new Vector2i(0, 160),
                        new Vector2i(0, 160),
                        new Vector2i(32, 160),
                        new Vector2i(64, 160),
                        new Vector2i(96, 192),
                        new Vector2i(96, 160),
                        new Vector2i(96, 128),
                        new Vector2i(96, 96),
                        new Vector2i(96, 128),
                        new Vector2i(96, 160),
                        new Vector2i(96, 192)
                    );
                    SetTextureRectangle(64, 160);
                    break;
                }
                case BlockType.TripleArrow:
                {
                    BlockAnimation = new Animation(this, 0.05f,
                        new Vector2i(64, 192),
                        new Vector2i(32, 192),
                        new Vector2i(0, 192),
                        new Vector2i(0, 192),
                        new Vector2i(0, 192),
                        new Vector2i(0, 192),
                        new Vector2i(0, 192),
                        new Vector2i(32, 192),
                        new Vector2i(64, 192),
                        new Vector2i(96, 192),
                        new Vector2i(96, 160),
                        new Vector2i(96, 128),
                        new Vector2i(96, 96),
                        new Vector2i(96, 128),
                        new Vector2i(96, 160),
                        new Vector2i(96, 192)
                    );
                    SetTextureRectangle(64, 192);
                    break;
                }
                case BlockType.TripleMana:
                {
                    BlockAnimation = new Animation(this, 0.05f,
                        new Vector2i(64, 288),
                        new Vector2i(32, 288),
                        new Vector2i(0, 288),
                        new Vector2i(0, 288),
                        new Vector2i(0, 288),
                        new Vector2i(0, 288),
                        new Vector2i(0, 288),
                        new Vector2i(32, 288),
                        new Vector2i(64, 288),
                        new Vector2i(96, 192),
                        new Vector2i(96, 160),
                        new Vector2i(96, 128),
                        new Vector2i(96, 96),
                        new Vector2i(96, 128),
                        new Vector2i(96, 160),
                        new Vector2i(96, 192)
                    );
                    SetTextureRectangle(64, 288);
                    break;
                }
                case BlockType.Score1000:
                {
                    BlockAnimation = new Animation(this, 0.05f,
                        new Vector2i(64, 224),
                        new Vector2i(32, 224),
                        new Vector2i(0, 224),
                        new Vector2i(0, 224),
                        new Vector2i(0, 224),
                        new Vector2i(0, 224),
                        new Vector2i(0, 224),
                        new Vector2i(32, 224),
                        new Vector2i(64, 224),
                        new Vector2i(96, 192),
                        new Vector2i(96, 160),
                        new Vector2i(96, 128),
                        new Vector2i(96, 96),
                        new Vector2i(96, 128),
                        new Vector2i(96, 160),
                        new Vector2i(96, 192)
                    );
                    SetTextureRectangle(64, 224);
                    break;
                }
                case BlockType.Mana:
                {
                    BlockAnimation = new Animation(this, 0.05f,
                        new Vector2i(64, 256),
                        new Vector2i(32, 256),
                        new Vector2i(0, 256),
                        new Vector2i(0, 256),
                        new Vector2i(0, 256),
                        new Vector2i(0, 256),
                        new Vector2i(0, 256),
                        new Vector2i(32, 256),
                        new Vector2i(64, 256),
                        new Vector2i(96, 192),
                        new Vector2i(96, 160),
                        new Vector2i(96, 128),
                        new Vector2i(96, 96),
                        new Vector2i(96, 128),
                        new Vector2i(96, 160),
                        new Vector2i(96, 192)
                    );
                    SetTextureRectangle(64, 256);
                    break;
                }
                case BlockType.Score5000:
                {
                    BlockAnimation = new Animation(this, 0.05f,
                        new Vector2i(64, 128),
                        new Vector2i(32, 128),
                        new Vector2i(0, 128),
                        new Vector2i(0, 128),
                        new Vector2i(0, 128),
                        new Vector2i(0, 128),
                        new Vector2i(0, 128),
                        new Vector2i(32, 128),
                        new Vector2i(64, 128),
                        new Vector2i(96, 192),
                        new Vector2i(96, 160),
                        new Vector2i(96, 128),
                        new Vector2i(96, 96),
                        new Vector2i(96, 128),
                        new Vector2i(96, 160),
                        new Vector2i(96, 192)
                    );
                    SetTextureRectangle(64, 128);
                    break;
                }
                case BlockType.Stone:
                {
                    SetTextureRectangle(64, 0);
                    SetColor(Level.LevelColor);
                    break;
                }
                case BlockType.Illusion:
                {
                    SetTextureRectangle(0, 0);
                    SetColor(Level.LevelColor);
                    break;
                }
                case BlockType.Wood:
                {
                    SetTextureRectangle(96, 0);
                    IsDestroyed = false;
                    break;
                }
                case BlockType.Trampoline:
                {
                    DefaultTimer = new Clock();
                    SetTextureRectangle(64, 32);
                    break;
                }
                case BlockType.Exit:
                {
                    BlockAnimation = new Animation(this, 0.05f,
                        new Vector2i(0, 64),
                        new Vector2i(0, 96)
                    );
                    SetTextureRectangle(0, 64);
                    break;
                }
                //doors
                case BlockType.SilverDoor:
                {
                    SetTextureRectangle(64, 96);
                    break;
                }
                case BlockType.SilverKey:
                {
                    BlockAnimation = new Animation(this, 0.05f,
                        new Vector2i(0, 32),
                        new Vector2i(32, 32),
                        new Vector2i(64, 32),
                        new Vector2i(96, 32),
                        new Vector2i(64, 32),
                        new Vector2i(32, 32)
                    );
                    SetTextureRectangle(0, 32);
                    break;
                }
                case BlockType.GoldDoor:
                {
                    SetTextureRectangle(96, 96);
                    break;
                }
                case BlockType.GoldenKey:
                {
                    BlockAnimation = new Animation(this, 0.05f,
                        new Vector2i(0, 64),
                        new Vector2i(32, 64),
                        new Vector2i(64, 64),
                        new Vector2i(96, 64),
                        new Vector2i(64, 64),
                        new Vector2i(32, 64)
                    );
                    SetTextureRectangle(0, 64);
                    break;
                }
                case BlockType.CrystalDoor:
                {
                    BlockAnimation = new Animation(this, 0.05f,
                        new Vector2i(0, 256),
                        new Vector2i(32, 256),
                        new Vector2i(64, 256),
                        new Vector2i(96, 256)
                    );
                    SetTextureRectangle(0, 256);
                    break;
                }
                case BlockType.CrystalKey:
                {
                    BlockAnimation = new Animation(this, 0.05f,
                        new Vector2i(0, 352),
                        new Vector2i(32, 352),
                        new Vector2i(64, 352),
                        new Vector2i(96, 352),
                        new Vector2i(64, 352),
                        new Vector2i(32, 352)
                    );
                    SetTextureRectangle(0, 352);
                    break;
                }
                //teleports
                case BlockType.Teleport1:
                {
                    BlockAnimation = new Animation(this, 0.05f,
                        new Vector2i(0, 128),
                        new Vector2i(32, 128),
                        new Vector2i(64, 128),
                        new Vector2i(96, 128),
                        new Vector2i(64, 128),
                        new Vector2i(32, 128)
                    );
                    SetTextureRectangle(0, 128);
                    break;
                }
                case BlockType.Teleport2:
                {
                    BlockAnimation = new Animation(this, 0.05f,
                        new Vector2i(0, 128),
                        new Vector2i(32, 128),
                        new Vector2i(64, 128),
                        new Vector2i(96, 128),
                        new Vector2i(64, 128),
                        new Vector2i(32, 128)
                    );
                    SetTextureRectangle(0, 128);
                    break;
                }
                case BlockType.Teleport3:
                {
                    BlockAnimation = new Animation(this, 0.05f,
                        new Vector2i(0, 128),
                        new Vector2i(32, 128),
                        new Vector2i(64, 128),
                        new Vector2i(96, 128),
                        new Vector2i(64, 128),
                        new Vector2i(32, 128)
                    );
                    SetTextureRectangle(0, 128);
                    break;
                }
                case BlockType.Teleport4:
                {
                    BlockAnimation = new Animation(this, 0.05f,
                        new Vector2i(0, 128),
                        new Vector2i(32, 128),
                        new Vector2i(64, 128),
                        new Vector2i(96, 128),
                        new Vector2i(64, 128),
                        new Vector2i(32, 128)
                    );
                    SetTextureRectangle(0, 128);
                    break;
                }
                ///////
                case BlockType.Warning:
                {
                    SetTextureRectangle(0, 0);
                    break;
                }
                case BlockType.Hint:
                {
                    Hint = new TextLine("", 8, -100, -100, new Color(255, 255, 255, 0));
                    Hint.SetOutlineThickness(0.8f);
                    SetTextureRectangle(32, 0);
                    break;
                }
                case BlockType.LSpiderweb:
                {
                    SetTextureRectangle(64, 0);
                    break;
                }
                case BlockType.RSpiderweb:
                {
                    SetTextureRectangle(96, 0);
                    break;
                }
                case BlockType.Torch:
                {
                    BlockAnimation = new Animation(this, 0.1f,
                        new Vector2i(0, 32),
                        new Vector2i(32, 32),
                        new Vector2i(64, 32),
                        new Vector2i(32, 32)
                    );
                    SetTextureRectangle(0, 32);
                    break;
                }
                case BlockType.EvilEyes:
                {
                    BlockAnimation = new Animation(this, 3f,
                        new Vector2i(96, 32),
                        new Vector2i(128, 0)
                    );
                    SetTextureRectangle(96, 32);
                    break;
                }
                case BlockType.Grass:
                {
                    SetTextureRectangle(128, 32);
                    break;
                }
                case BlockType.None:
                {
                    SetTextureRectangle(32, 96);
                    break;
                }
                default:
                {
                    SetTextureRectangle(32, 96);
                    break;
                }
            }
        }

        public void DeleteObstacle()
        {
            Type = BlockType.None;
            IsDestroyed = true;
            SetTextureRectangle(32, 96);
        }

        public void DeletePickup()
        {
            Type = BlockType.None;
            SetTextureRectangle(128, 128);
        }

        public void BlockUpdate(Level level)
        {
            if (Type == BlockType.Stone && IsStomped)
                if (DefaultTimer.ElapsedTime.AsSeconds() > 1)
                {
                    DeleteObstacle();
                    sCrush.Play();
                    DefaultTimer.Dispose();
                    level.Particles.Add(new ParticleEffect(OriginalPos.X, OriginalPos.Y,
                        new Color(150, 150, 150)));
                }
            if (Type == BlockType.EnergyBall && IsShattered)
                if (DefaultTimer.ElapsedTime.AsSeconds() > 0.17f)
                {
                    Block obstacle;
                    if ((obstacle = level.GetObstacle(X/32 - 0.5f, Y/32)).Type == BlockType.EnergyBall) obstacle.Shatter();
                    if ((obstacle = level.GetObstacle( X/32+ 1.5f, Y/32)).Type == BlockType.EnergyBall) obstacle.Shatter();
                    if ((obstacle = level.GetObstacle(X/32, Y/32- 0.5f)).Type == BlockType.EnergyBall) obstacle.Shatter();
                    if ((obstacle = level.GetObstacle(X/32, Y/32+ 1.5f)).Type == BlockType.EnergyBall) obstacle.Shatter();

                    SetColor(Color.White);
                    DeleteObstacle();
                    sShatter.Play();
                    DefaultTimer.Dispose();
                    level.Particles.Add(new ParticleEffect(OriginalPos.X, OriginalPos.Y,
                        Color.Cyan,  15));
                }
        }

        public void Stomp()
        {
            if (Type == BlockType.Stone && !IsStomped)
            {
                IsStomped = true;
                DefaultTimer = new Clock();
            }
        }

        public void Shatter()
        {
            if (Type == BlockType.EnergyBall && !IsShattered)
            {
                IsShattered = true;
                SetColor(Color.Cyan);
                DefaultTimer = new Clock();
            }
        }

        public void HitHardblock(MainCharacter character)
        {
            if (character.IsDownAttacking)
            {
                Health -= (int) character.SpeedY * 3;
            }
            else
                Health--;

            if (sHard.Status != SoundStatus.Playing) sHard.Play();

            if (Health < 150) SetTextureRectangle(32, 192);
            if (Health < 100) SetTextureRectangle(64, 192);
            if (Health < 50) SetTextureRectangle(96, 192);
        }

        public static void FlipLever()
        {
            if (Level.IsLeverOn == false)
            {
                Level.IsLeverOn = true;
                LeverTimer.Restart();
                sLever.Play();

                foreach (var gate in Level.SteelGates) gate.Type = BlockType.None;

                foreach (var lever in Level.Levers) lever.SetTextureRectangle(64, 160);
            }
        }

        public static void LeverMechanismUpdate()
        {
            if (Level.IsLeverOn == false)
            {
                foreach (var gate in Level.SteelGates)
                    if (gate.Y < gate.OriginalPos.Y)
                        gate.Y += 0.002f;
            }
            else if (Level.IsLeverOn && LeverTimer.ElapsedTime.AsSeconds() < Level.LeverInterval)
            {
                foreach (var gate in Level.SteelGates)
                    if (gate.Y > gate.OriginalPos.Y - 32)
                        gate.Y -= 0.002f;
            }
            else
            {
                Level.IsLeverOn = false;
                sLever.Play();
                foreach (var gate in Level.SteelGates) gate.Type = BlockType.SteelGate;
                foreach (var lever in Level.Levers) lever.SetTextureRectangle(32, 160);
            }
        }
    }
}