using System;
using SFML.System;
using SFML.Graphics;

namespace ChendiAdventures
{ 
    public class Golem : Creature
    {
        public float XMaxPos;
        public float XMinPos;
        public Clock DefaultClock;
        public int Health;
        public Boulder Boulder;
        public int HurlInterval;
        public float ProcsDistance { get; set; }
        public Golem(float x, float y, Texture texture) : base(x, y, texture)
        {
            XMinPos = x - 96;
            XMaxPos = x + 128;

            Health = 10;
            HurlInterval = 6;
            ProcsDistance = 200;

            SpeedX = 1f;
            SpeedY = 0f;

            Boulder = new Boulder(300,-50, GolemTexture, Movement.None);

            _animLeft = new Animation(this, 0.1f,
                new Vector2i(0, 0),
                new Vector2i(32, 0)
            );
            _animRight = new Animation(this, 0.1f,
                new Vector2i(0, 32),
                new Vector2i(32, 32)
            );

            DefaultClock = new Clock();

            SetTextureRectanlge(0, 0, 32, 32);
            IsDead = false;

            ApplyDifficulty();
        }

        public void UpdateCreature(MainCharacter character, Level level)
        {
            X += SpeedX;
            if (Left < XMinPos || Right > XMaxPos) SpeedX *= -1;

            if (Health <= 0 && !IsDead)
            {
                character.AddToScore(level, Points, X, Y);
                level.Particles.Add(new ParticleEffect(X, Y,
                    new Color(100, 100, 100)));
                Die(level);
            }

            Boulder.Direction = SpeedX > 0 ? Movement.Right : Movement.Left;

            if (DefaultClock.ElapsedTime.AsSeconds() > HurlInterval && (float)Math.Sqrt(Math.Pow(GetCenterPosition().X - character.GetCenterPosition().X, 2) +
                                                                                        Math.Pow(GetCenterPosition().Y - character.GetCenterPosition().Y, 2)) <
                ProcsDistance)
            {
                DefaultClock.Restart();
                Boulder.Attack(X,Y, character.X > X ? Movement.Right : Movement.Left);
            }

            if (Boulder.isAttacking) Boulder.BoulderUpdate(level);
            UpdateTextures();
        }
        public override void UpdateTextures()
        {
            if (SpeedX > 0) _animRight.Animate();
            else _animLeft.Animate();
        }

        public override void Die(Level level)
        {
            Block.sDestroy.Play();
            level.Particles.Add(new ParticleEffect(X, Y, new Color(57, 65, 81)));
            SetPosition(400, -100);
            IsDead = true;
        }

        public override void Draw(RenderTarget target, RenderStates states)
        {
            base.Draw(target, states);
            target.Draw(Boulder);
        }

        public override void ApplyDifficulty()
        {
            switch (MainGameWindow.GameDifficulty)
            {
                case Difficulty.Easy:
                {
                    SpeedX = 0.25f;
                    Health = 5;
                    HurlInterval = 8;
                    Points = 1500;
                    break;
                }
                case Difficulty.Medium:
                {
                    SpeedX = 0.5f;
                    Health = 10;
                    HurlInterval = 6;
                    Points = 2000;
                    break;
                }
                case Difficulty.Hard:
                {
                    SpeedX = 1f;
                    Health = 15;
                    HurlInterval = 4;
                    Points = 2500;
                    break;
                }
            }
        }

    }
}
