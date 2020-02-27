using SFML.System;
using System.Collections.Generic;

namespace Game
{
    public class Animation
    {
        private readonly Clock _animationTimer;
        private Entity _entity;
        private float _frameTime;
        private int _frame;
        public readonly List<Vector2i> Frames;

        public Animation(Entity entity, float frameTime, params Vector2i[] frames)
        {
            this._animationTimer = new Clock();
            this._frame = 0;
            this.Frames = new List<Vector2i>();
            foreach (var frame in frames)
            {
                this.Frames.Add(frame);
            }
            this._animationTimer.Restart();
            this._entity = entity;
            this._frameTime = frameTime;
        }
        private void SetNextTexture(Vector2i framePos, int size = 32)
        {
            this._entity.SetTextureRectanlge(framePos.X, framePos.Y, size, size);
        }
        public void Animate(int size = 32)
        {
            if (this._animationTimer.ElapsedTime.AsSeconds() > this._frameTime )
            {
                this._animationTimer.Restart();
                this._frame++;
                if (this._frame > this.Frames.Count - 1)
                {
                    this._frame = 0;
                }
                this.SetNextTexture(this.Frames[this._frame], size);
            }
        }
        public void ResetAnimation()
        {
            this._frame = this.Frames.Count - 1;
        }
    }
}
