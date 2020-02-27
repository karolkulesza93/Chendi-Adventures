using SFML.System;
using SFML.Audio;
using SFML.Graphics;

namespace Game
{
    public class Block : Entity
    {
        public Vector2f OriginalPos { get; private set; }
        public Animation BlockAnimation { get; private set; }
        public TextLine Hint { get; set; }
        public int HintNumber { get; set; }
        public bool IsDestroyed { get; private set; }
        public bool IsStomped { get; set; }
        public Clock DefaultTimer;
        public BlockType Type { get; set; }
        public static Sound sCrush = new Sound(new SoundBuffer(@"sfx/crush.wav"));
        public Block(float x, float y, Texture texture, BlockType type = BlockType.None, int hintNumber= 0) : base(x, y, texture)
        {
            this.OriginalPos = new Vector2f(x, y);
            this.Type = type;
            this.IsDestroyed = false;
            this.HintNumber = hintNumber;

            switch (type)
            {
                case BlockType.Brick:
                    {
                        this.SetTextureRectanlge(0, 0, 32, 32);
                        break;
                    }
                case BlockType.Spike:
                    {
                        this.SetTextureRectanlge(0, 32, 32, 32);
                        break;
                    }
                case BlockType.Enterance:
                    {
                        this.SetTextureRectanlge(32, 64, 32, 32);
                        break;
                    }
                case BlockType.Coin:
                    {
                        this.BlockAnimation = new Animation(this, 0.05f,
                            new Vector2i(0, 0),
                            new Vector2i(32, 0),
                            new Vector2i(64, 0),
                            new Vector2i(96, 0),
                            new Vector2i(64, 0),
                            new Vector2i(32, 0)
                            );
                        this.SetTextureRectanlge(0, 0, 32, 32);
                        break;
                    }
                case BlockType.Life:
                    {
                        this.BlockAnimation = new Animation(this, 0.05f,
                            new Vector2i(0, 96),
                            new Vector2i(32, 96),
                            new Vector2i(64, 96),
                            new Vector2i(96, 96),
                            new Vector2i(64, 96),
                            new Vector2i(32, 96)
                            );
                        this.SetTextureRectanlge(0, 96, 32, 32);
                        break;
                    }
                case BlockType.Arrow:
                    {
                        this.BlockAnimation = new Animation(this, 0.05f,
                            new Vector2i(0, 160),
                            new Vector2i(32, 160),
                            new Vector2i(64, 160),
                            new Vector2i(96, 160),
                            new Vector2i(64, 160),
                            new Vector2i(32, 160)
                            );
                        this.SetTextureRectanlge(0, 128, 32, 32);
                        break;
                    }
                case BlockType.TripleArrow:
                    {
                        this.BlockAnimation = new Animation(this, 0.05f,
                            new Vector2i(0, 192),
                            new Vector2i(32, 192),
                            new Vector2i(64, 192),
                            new Vector2i(96, 192),
                            new Vector2i(64, 192),
                            new Vector2i(32, 192)
                            );
                        this.SetTextureRectanlge(0, 128, 32, 32);
                        break;
                    }
                case BlockType.Score1000:
                    {
                        this.BlockAnimation = new Animation(this, 0.05f,
                            new Vector2i(0, 224),
                            new Vector2i(32, 224),
                            new Vector2i(64, 224),
                            new Vector2i(96, 224),
                            new Vector2i(64, 224),
                            new Vector2i(32, 224)
                            );
                        this.SetTextureRectanlge(0, 128, 32, 32);
                        break;
                    }
                case BlockType.Mana:
                    {
                        this.BlockAnimation = new Animation(this, 0.05f,
                            new Vector2i(0, 256),
                            new Vector2i(32, 256),
                            new Vector2i(64, 256),
                            new Vector2i(96, 256),
                            new Vector2i(64, 256),
                            new Vector2i(32, 256)
                            );
                        this.SetTextureRectanlge(0, 256, 32, 32);
                        break;
                    }
                case BlockType.Score5000:
                    {
                        this.BlockAnimation = new Animation(this, 0.05f,
                            new Vector2i(0, 128),
                            new Vector2i(32, 128),
                            new Vector2i(64, 128),
                            new Vector2i(96, 128),
                            new Vector2i(64, 128),
                            new Vector2i(32, 128)
                            );
                        this.SetTextureRectanlge(0, 128, 32, 32);
                        break;
                    }
                case BlockType.Stone:
                    {
                        this.SetTextureRectanlge(64, 0, 32, 32);
                        break;
                    }
                case BlockType.Illusion:
                    {
                        this.SetTextureRectanlge(0, 0, 32, 32);
                        break;
                    }
                case BlockType.Wood:
                    {
                        this.SetTextureRectanlge(96, 0, 32, 32);
                        this.IsDestroyed = false;
                        break;
                    }
                case BlockType.Trampoline:
                    {
                        this.DefaultTimer = new Clock();
                        this.SetTextureRectanlge(64, 32, 32, 32);
                        break;
                    }
                case BlockType.Exit:
                    {
                        this.SetTextureRectanlge(0, 64, 32, 32);
                        break;
                    }
                //doors
                case BlockType.SilverDoor:
                    {
                        this.SetTextureRectanlge(64, 96, 32, 32);
                        break;
                    }
                case BlockType.SilverKey:
                    {
                        this.BlockAnimation = new Animation(this, 0.05f,
                            new Vector2i(0, 32),
                            new Vector2i(32, 32),
                            new Vector2i(64, 32),
                            new Vector2i(96, 32)
                            );
                        this.SetTextureRectanlge(0, 32, 32, 32);
                        break;
                    }
                case BlockType.GoldDoor:
                    {
                        this.SetTextureRectanlge(96, 96, 32, 32);
                        break;
                    }
                case BlockType.GoldenKey:
                    {
                        this.BlockAnimation = new Animation(this, 0.05f,
                            new Vector2i(0, 64),
                            new Vector2i(32, 64),
                            new Vector2i(64, 64),
                            new Vector2i(96, 64)
                            );
                        this.SetTextureRectanlge(0, 64, 32, 32);
                        break;
                    }
                //teleports
                case BlockType.Teleport1:
                    {
                        this.SetTextureRectanlge(64, 64, 32, 32);
                        break;
                    }
                case BlockType.Teleport2:
                    {
                        this.SetTextureRectanlge(96, 64, 32, 32);
                        break;
                    }
                ///////
                case BlockType.Warning:
                    {
                        this.SetTextureRectanlge(0, 0, 32, 32);
                        break;
                    }
                case BlockType.Hint:
                    {
                        this.Hint = new TextLine("", 8, -100, -100, new SFML.Graphics.Color(255,255,255,0));
                        this.Hint.SetOutlineThickness(0.8f);
                        this.SetTextureRectanlge(32, 0, 32, 32);
                        break;
                    }
                case BlockType.LSpiderweb:
                    {
                        this.SetTextureRectanlge(64, 0, 32, 32);
                        break;
                    }
                case BlockType.RSpiderweb:
                    {
                        this.SetTextureRectanlge(96, 0, 32, 32);
                        break;
                    }
                case BlockType.Torch:
                    {
                        this.BlockAnimation = new Animation(this, 0.1f,
                            new Vector2i(0, 32),
                            new Vector2i(32, 32),
                            new Vector2i(64, 32),
                            new Vector2i(32, 32)
                            );
                        this.SetTextureRectanlge(0, 32, 32, 32);
                        break;
                    }
                case BlockType.EvilEyes:
                    {
                        this.SetTextureRectanlge(96, 32, 32, 16);
                        break;
                    }
                case BlockType.None:
                    {
                        this.SetTextureRectanlge(32, 96, 32, 32);
                        break;
                    }
                default:
                    {
                        this.SetTextureRectanlge(32, 96, 32, 32);
                        break;
                    }
            }
        }
        public void DeleteObstacle()
        {
            if (this.Type == BlockType.Stone)
            {
                sCrush.Play();
                this.DefaultTimer.Dispose();
            }
            this.Type = BlockType.None;
            this.IsDestroyed = true;
            this.SetTextureRectanlge(0, 96, 32, 32);
        }
        public void DeletePickup()
        {
            this.Type = BlockType.None;
            this.SetTextureRectanlge(128, 128, 32, 32);
        }
        public void StoneUpdate()
        {
            if (this.Type == BlockType.Stone && this.IsStomped)
            {
                if (this.DefaultTimer.ElapsedTime.AsSeconds() > 1)
                    this.DeleteObstacle();
            }
        }
        public void Stomp()
        {
            if (this.Type == BlockType.Stone && !this.IsStomped)
            {
                this.IsStomped = true;
                this.DefaultTimer = new Clock();
            }
        }
    }
}
