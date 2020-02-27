using SFML.System;
using SFML.Graphics;

namespace Game
{
    public class TextLine : Drawable
    {
        public float X
        {
            get { return this._text.Position.X; }
            set { this._text.Position = new Vector2f(value, this._text.Position.Y); }
        }
        public float Y
        {
            get { return this._text.Position.Y; }
            set { this._text.Position = new Vector2f(this._text.Position.X, value); }
        }
        private Text _text;
        private Font _font;
        public byte Alpha
        {
            get { return this._text.Color.A; }
            set { this._text.Color = new Color(255, 255, 255, value); }
        }
        public TextLine(string line, int size, float x, float y, Color color)
        {
            this._font = new Font("font.ttf");
            this._text = new Text(line, this._font, (uint)size);
            this._text.Color = color;
            this._text.Position = new Vector2f(x, y);
            this._text.OutlineColor = Color.Black;
            this._text.OutlineThickness = 1;
        }
        public void EditText(string line)
        {
            this._text.DisplayedString = line;
        }
        public void ChangeColor(Color color)
        {
            this._text.Color = color;
        }
        public void MoveText(float x, float y)
        {
            this._text.Position = new Vector2f(x, y);
        }
        public void Draw(RenderTarget target, RenderStates states)
        {
            target.Draw(this._text, states);
        }
        public void SetOutlineThickness(float value)
        {
            this._text.OutlineThickness = value;
        }
    }
}
