using SFML.Graphics;
using SFML.System;

namespace ChendiAdventures
{
    public class TextLine : Drawable
    {
        private static readonly Font _font = new Font("font.ttf");
        private readonly Text _text;

        public TextLine(string line, int size, float x, float y, Color color)
        {
            _text = new Text(line, _font, (uint) size);
            _text.Color = color;
            _text.Position = new Vector2f(x, y);
            _text.OutlineColor = Color.Black;
            _text.OutlineThickness = 1;
        }

        public float X
        {
            get => _text.Position.X;
            set => _text.Position = new Vector2f(value, _text.Position.Y);
        }

        public float Y
        {
            get => _text.Position.Y;
            set => _text.Position = new Vector2f(_text.Position.X, value);
        }

        public byte Alpha
        {
            get => _text.Color.A;
            set => _text.Color = new Color(255, 255, 255, value);
        }

        public float Width => (float) _text.DisplayedString.Length * _text.CharacterSize * 0.8f;

        public void Draw(RenderTarget target, RenderStates states)
        {
            target.Draw(_text, states);
        }

        public void EditText(string line)
        {
            _text.DisplayedString = line;
        }

        public void ChangeColor(Color color)
        {
            _text.Color = color;
        }

        public void MoveText(float x, float y)
        {
            _text.Position = new Vector2f(x, y);
        }

        public void SetOutlineThickness(float value)
        {
            _text.OutlineThickness = value;
        }

        public void SetOutlineColor(Color color)
        {
            _text.OutlineColor = color;
        }
    }
}