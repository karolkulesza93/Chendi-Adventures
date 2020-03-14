using System;
using SFML.Audio;
using SFML.Graphics;
using SFML.System;

namespace ChendiAdventures
{
    public sealed class Ghost : Creature
    {
        public Ghost(float x, float y, Texture texture) : base(x, y, texture)
        {
            Speed = 1f;
            ProcsDistance = 500f;

            _animLeft = new Animation(this, 0.2f,
                new Vector2i(0, 0),
                new Vector2i(32, 0)
            );

            _animRight = new Animation(this, 0.2f,
                new Vector2i(0, 32),
                new Vector2i(32, 32));

            DefaultClock = new Clock();

            IsDead = false;
            SetTextureRectanlge(0, 0, 32, 32);

            ApplyDifficulty();
        }

        public float Speed { get; set; }
        public float ProcsDistance { get; set; }
        public static Sound sGhost = new Sound(new SoundBuffer(@"sfx/ghost.wav")) { Volume = 40 };
        public Clock DefaultClock { get; }

        public override void UpdateTextures()
        {
            if (SpeedX < 0) _animLeft.Animate();
            else _animRight.Animate();
        }

        public new void UpdateCreature(Level level, MainCharacter character)
        {
            UpdateTextures();

            if (!IsDead &&  !character.IsDead && (float) Math.Sqrt(Math.Pow(GetCenterPosition().X - character.GetCenterPosition().X, 2) +
                                             Math.Pow(GetCenterPosition().Y - character.GetCenterPosition().Y, 2)) <
                ProcsDistance)
            {
                if (GetCenterPosition().X >= character.GetCenterPosition().X) SpeedX = -1 * Speed;
                else SpeedX = Speed;

                if (GetCenterPosition().Y >= character.GetCenterPosition().Y) SpeedY = -1 * Speed;
                else SpeedY = Speed;

                X += SpeedX;
                Y += SpeedY;
            }

            if (!IsDead && DefaultClock.ElapsedTime.AsSeconds() > 10)
            {
                sGhost.Play();
                DefaultClock.Restart();
            }
        }

        public override void Die(Level level)
        {
            level.Particles.Add(new ParticleEffect(X, Y, Color.Cyan));
            SetPosition(400, -100);
            Arrow.sEnergyHit.Play();
            IsDead = true;
            DefaultClock.Dispose();
        }

        public override void ApplyDifficulty()
        {
            switch (MainGameWindow.GameDifficulty)
            {
                case Difficulty.Easy:
                {
                    SpeedX = 0.5f;
                    ProcsDistance = 200f;
                    Points = 700;
                    break;
                }
                case Difficulty.Medium:
                {
                    Speed = 1f;
                    ProcsDistance = 300f;
                    Points = 1000;
                    break;
                }
                case Difficulty.Hard:
                {
                    Speed = 1.5f;
                    ProcsDistance = 400f;
                    Points = 1300;
                    break;
                }
            }
        }
    }
}