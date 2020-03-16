using SFML.Graphics;
using SFML.System;

namespace ChendiAdventures
{
    internal class ScreenChange : Drawable
    {
        private byte _alpha;
        private bool _flag;
        private readonly RectangleShape _shader;
        private readonly View _view;

        public ScreenChange(ref View view)
        {
            _view = view;
            _shader = new RectangleShape();
            _shader.Origin = new Vector2f(0, 0);
            _shader.FillColor = new Color(0, 0, 0, 0);
            _flag = false;
            Done = true;
        }

        public bool Done { get; private set; }

        public void Draw(RenderTarget target, RenderStates states)
        {
            target.Draw(_shader);
        }

        public void BlackOut()
        {
            if (_flag == false)
            {
                _alpha = 0;
                _shader.FillColor = new Color(0, 0, 0, _alpha);
                _flag = true;
                Done = false;
            }

            _shader.Size = 2 * _view.Size;
            _shader.Position = new Vector2f(_view.Center.X - _view.Size.X * 1.5f, _view.Center.Y - _view.Size.Y * 1.5f);
            if (_alpha < 255)
            {
                _alpha += 15;
                _shader.FillColor = new Color(0, 0, 0, _alpha);
            }
            else
            {
                Done = false;
                _alpha = 255;
                _shader.FillColor = new Color(0, 0, 0, _alpha);
            }
        }

        public void AppearIn()
        {
            if (_flag == false)
            {
                _alpha = 255;
                _shader.FillColor = new Color(0, 0, 0, _alpha);
                _flag = true;
                Done = false;
            }

            _shader.Size = 2 * _view.Size;
            _shader.Position = new Vector2f(_view.Center.X - _view.Size.X * 1.5f, _view.Center.Y - _view.Size.Y * 1.5f);
            if (_alpha > 0)
            {
                _alpha -= 15;
                _shader.FillColor = new Color(0, 0, 0, _alpha);
            }
            else
            {
                Done = true;
                _alpha = 0;
                _shader.FillColor = new Color(0, 0, 0, _alpha);
            }
        }

        public void Reset()
        {
            _shader.FillColor = new Color(0, 0, 0, 0);
            _flag = false;
            Done = false;
        }
    }
}