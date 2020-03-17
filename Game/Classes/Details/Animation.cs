using System.Collections.Generic;
using SFML.System;

namespace ChendiAdventures
{
    public class Animation
    {
        private readonly Clock _animationTimer;
        private readonly Entity _entity;
        private readonly float _frameTime;
        public readonly List<Vector2i> Frames;

        public Animation(Entity entity, float frameTime, params Vector2i[] frames)
        {
            _animationTimer = new Clock();
            Frame = 0;
            Frames = new List<Vector2i>();
            foreach (var frame in frames) Frames.Add(frame);
            _animationTimer.Restart();
            _entity = entity;
            _frameTime = frameTime;
        }

        public int Frame { get; set; }

        private void SetNextTexture(Vector2i framePos, int size = 32)
        {
            _entity.SetTextureRectangle(framePos.X, framePos.Y, size, size);
        }

        public void Animate(int size = 32, int frame = -2)
        {
            if (frame == -2)
            {
                if (_animationTimer.ElapsedTime.AsSeconds() > _frameTime)
                {
                    _animationTimer.Restart();
                    Frame++;
                    if (Frame > Frames.Count - 1) Frame = 0;
                }
            }
            else
            {
                //if (frame < 0 || frame > Frames.Count) Frame = 0;
                Frame = frame;
            }

            if (Frame < 0) Frame = 0; 
            SetNextTexture(Frames[Frame], size);
        }

        public void ResetAnimation()
        {
            //Frame = Frames.Count - 1;
            Frame = -1;
        }
    }
}