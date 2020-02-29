using SFML.System;
using SFML.Graphics;

namespace Game
{
    public sealed class Monster : Creature
    {
        public float XMaxPos;
        public float XMinPos;
        
        public Monster(float x, float y, Texture texture) : base(x, y, texture)
        {
            this.XMinPos = x - 100;
            this.XMaxPos = x + 100;

            this.SpeedX = 2f;
            this.SpeedY = 0f;

            this._animLeft = new Animation(this, 0.05f,
                new Vector2i(0,0),
                new Vector2i(32,0)
                );
            this._animRight = new Animation(this, 0.05f,
                new Vector2i(0,32),
                new Vector2i(32,32)
                );

            this.SetTextureRectanlge(0, 0, 32, 32);
            this.IsDead = false;
        }
        public override void UpdateTextures()
        {
            if (this.SpeedX > 0) this._animRight.Animate();
            else this._animLeft.Animate();
        }
        public void MonsterUpdate()
        {
            this.X += this.SpeedX;
            if (this.Left < this.XMinPos || this.Right > this.XMaxPos) this.SpeedX *= -1;
            this.UpdateTextures();
        }
        public void Die(Level level)
        {
            sKill.Play();
            level.Particles.Add(new ParticleEffect(this.X, this.Y, Color.Red));

            this.SetPosition(400, -100);
            this.IsDead = true;
        }
    }
}
