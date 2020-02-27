using SFML.Audio;
using SFML.System;
using SFML.Graphics;

namespace Game
{
    public class EnergyBall : Projectile
    {
        private Animation _anim;
        private Sound sAtk;
        public Clock DefaultTimer;
        public float Speed { get; private set; }
        public float SpeedX { get; private set; }
        public float SpeedY { get; private set; }
        public bool isAttacking { get; private set; }
        public EnergyBall(float x, float y, Texture texture, Movement dir = Movement.None) : base(x,y,texture,dir)
        {
            this.Speed = 2f;
            this.SpeedX = 0f;
            this.SpeedY = 0f;
            this.isAttacking = false;

            this.DefaultTimer = new Clock();

            this._anim = new Animation(this, 0.1f,
                new Vector2i(0, 64),
                new Vector2i(16, 64),
                new Vector2i(32, 64),
                new Vector2i(48, 64),
                new Vector2i(32, 64),
                new Vector2i(16, 64)
                );

            this.sAtk = new Sound(new SoundBuffer("sfx/spell.wav"));
        }

        public void UpdateEnergyBall(MainCahracter character)
        {
            if (this.isAttacking)
            {
                if (this.GetCenterPosition().X > character.GetCenterPosition().X) this.SpeedX = -1 * this.Speed;
                else this.SpeedX = this.Speed;

                if (this.GetCenterPosition().Y > character.GetCenterPosition().Y) this.SpeedY = -1 * this.Speed;
                else this.SpeedY = this.Speed;

                this.X += this.SpeedX;
                this.Y += this.SpeedY;

                if (this.DefaultTimer.ElapsedTime.AsSeconds() > 8)
                {
                    this.isAttacking = false;
                    this.SetPosition(-100, -100);
                }

                this._anim.Animate(16); 
            }
        }
        public void Attack(float x, float y)
        {
            this.sAtk.Play();
            this.isAttacking = true;
            this.DefaultTimer.Restart();
            this.SetPosition(x, y);
        }
    }
}
