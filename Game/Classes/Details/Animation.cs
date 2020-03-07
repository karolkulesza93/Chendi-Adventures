using System.Collections.Generic;
using SFML.System;

namespace ChendiAdventures
{
    public class Animation
    {
        private readonly Clock _animationTimer;
        public readonly List<Vector2i> Frames;
        private readonly Entity _entity;
        private int _frame;
        private readonly float _frameTime;

        public Animation(Entity entity, float frameTime, params Vector2i[] frames)
        {
            _animationTimer = new Clock();
            _frame = 0;
            Frames = new List<Vector2i>();
            foreach (var frame in frames) Frames.Add(frame);
            _animationTimer.Restart();
            _entity = entity;
            _frameTime = frameTime;
        }

        private void SetNextTexture(Vector2i framePos, int size = 32)
        {
            _entity.SetTextureRectanlge(framePos.X, framePos.Y, size, size);
        }

        public void Animate(int size = 32)
        {
            if (_animationTimer.ElapsedTime.AsSeconds() > _frameTime)
            {
                _animationTimer.Restart();
                _frame++;
                if (_frame > Frames.Count - 1) _frame = 0;
                SetNextTexture(Frames[_frame], size);
            }
        }

        public void ResetAnimation()
        {
            _frame = Frames.Count - 1;
        }
    }
}