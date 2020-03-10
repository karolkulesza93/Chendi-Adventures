using System;
using SFML.Graphics;
using SFML.System;

namespace ChendiAdventures
{
    public abstract class Entity : Drawable
    {
        //textures
        public static Texture ArcherTexture = new Texture(@"img/archer.png");
        public static Texture ArrowTexture = new Texture(@"img/arrow.png");
        public static Texture DetailsTexture = new Texture(@"img/details.png");
        public static Texture GhostTexture = new Texture(@"img/ghost.png");
        public static Texture KnightTexture = new Texture(@"img/knight.png");
        public static Texture MainCharacterTexture = new Texture(@"img/MainCharacter.png");
        public static Texture PickupsTexture = new Texture(@"img/pickups.png");
        public static Texture SwordTexture = new Texture(@"img/sword.png");
        public static Texture TilesTexture = new Texture(@"img/tiles.png");
        public static Texture TrapsTexture = new Texture(@"img/traps.png");
        public static Texture WizardTexture = new Texture(@"img/wizard.png");
        public static Texture GameMachineTexture = new Texture(@"img/machine.png");
        public static Texture RewardsTexture = new Texture(@"img/rewards.png");
        public static Texture ShopTexture = new Texture(@"img/merchant.png");
        private readonly Sprite _entitySprite; //głowny obiekt na ekranie

        private IntRect _textureRectangle;

        public Entity(float x = 0, float y = 0, Texture texture = null)
        {
            try
            {
                LoadedTexture = texture;
            } //załagodwanie textury
            catch (Exception)
            {
                Console.WriteLine("Something went wrong, as always :)");
            }

            _textureRectangle = new IntRect(0, 0, 0, 0);
            _entitySprite = new Sprite(LoadedTexture, _textureRectangle); //przypisanie textury
            SetPosition(x, y);
        }

        public Texture LoadedTexture { get; set; } //tekstura ladowana z pliku

        public float X //pozycja X
        {
            get => _entitySprite.Position.X;
            set => _entitySprite.Position = new Vector2f(value, _entitySprite.Position.Y);
        }

        public float Y //pozycja Y
        {
            get => _entitySprite.Position.Y;
            set => _entitySprite.Position = new Vector2f(_entitySprite.Position.X, value);
        }

        public float Left => X;
        public float Right => X + Width;
        public float Top => Y;
        public float Bottom => Y + Height;
        public float Width => _entitySprite.TextureRect.Width; //szerokość
        public float Height => _entitySprite.TextureRect.Height; //wysokość

        public virtual void Draw(RenderTarget target, RenderStates states)
        {
            target.Draw(_entitySprite, states);
        } 

        public virtual void SetPosition(float x, float y)
        {
            X = x;
            Y = y;
        } 

        public FloatRect GetBoundingBox()
        {
            return _entitySprite.GetGlobalBounds();
        }

        public void SetTextureRectanlge(int x, int y, int width = 32, int height = 32)
        {
            _textureRectangle.Left = x;
            _textureRectangle.Top = y;
            _textureRectangle.Width = width;
            _textureRectangle.Height = height;
            SetTexture();
        }

        private void SetTexture()
        {
            _entitySprite.TextureRect = _textureRectangle;
        }

        public void UseTexture()
        {
            _entitySprite.Texture = LoadedTexture;
        }

        public Vector2f Get32Position()
        {
            return new Vector2f(X / 32, Y / 32);
        }

        public void Set32Position(float x, float y)
        {
            SetPosition(32 * x, 32 * y);
        }

        public Vector2f GetCenterPosition()
        {
            return new Vector2f(X + (float) 0.5 * Width, Y + (float) 0.5 * Height);
        }

        public void SetColor(Color color)
        {
            _entitySprite.Color = color;
        }
    }
}