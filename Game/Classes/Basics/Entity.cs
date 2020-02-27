using SFML.Graphics;
using SFML.System;

namespace Game
{
    public abstract class Entity : Drawable
    {
        private Sprite _EntitySprite; //głowny obiekt na ekranie

        //textyres
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
           
        private IntRect _TextureRectangle;
        public Texture LoadedTexture { get; set; } //tekstura ladowana z pliku
        public float X //pozycja X
        {
            get { return this._EntitySprite.Position.X; }
            set { this._EntitySprite.Position = new Vector2f(value, this._EntitySprite.Position.Y); }
        }
        public float Y //pozycja Y
        {
            get { return this._EntitySprite.Position.Y; }
            set { this._EntitySprite.Position = new Vector2f(this._EntitySprite.Position.X, value); }
        }
        public float Left { get { return this.X; } }
        public float Right { get { return this.X + this.Width; } }
        public float Top { get { return this.Y; } }
        public float Bottom { get { return this.Y + this.Height; } }
        public float Width { get { return this._EntitySprite.TextureRect.Width; } } //szerokość
        public float Height { get { return this._EntitySprite.TextureRect.Height; } } //wysokość
        public virtual void Draw(RenderTarget target, RenderStates states) { target.Draw(this._EntitySprite, states); } //implementacja interfejsu drawable
        public virtual void SetPosition(float x, float y) { this.X = x; this.Y = y; } //ustawienie pozycji
        public FloatRect GetBoundingBox() { return this._EntitySprite.GetGlobalBounds(); }
        public void SetTextureRectanlge(int x, int y, int width, int height)
        {
            this._TextureRectangle.Left = x;
            this._TextureRectangle.Top = y;
            this._TextureRectangle.Width = width;
            this._TextureRectangle.Height = height;
            this.SetTexture();
        }
        public void SetTexture()
        {
            this._EntitySprite.TextureRect = this._TextureRectangle;
        }
        public Entity(float x = 0, float y = 0, Texture texture = null)
        {
            try { this.LoadedTexture = texture; } //załagodwanie textury
            catch (System.Exception) { System.Console.WriteLine("Something went wrong, as always :)"); }
            this._TextureRectangle = new IntRect(0, 0, 1, 1);
            this._EntitySprite = new Sprite(this.LoadedTexture, this._TextureRectangle); //przypisanie textury
            this.SetPosition(x, y);
        }
        public Vector2f Get32Position()
        {
            return new Vector2f((float)this.X/32, (float)this.Y/32);
        }
        public void Set32Position(float x, float y)
        {
            this.SetPosition(32 * x, 32 * y);
        }
        public Vector2f GetCenterPosition()
        {
            return new Vector2f(this.X + (float)0.5 * this.Width, this.Y + (float)0.5 * this.Height);
        }
        public void SetColor(Color color)
        {
            this._EntitySprite.Color = color;
        }
    }
}
