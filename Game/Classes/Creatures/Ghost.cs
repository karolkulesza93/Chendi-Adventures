using System;
using SFML.Audio;
using SFML.Graphics;
using SFML.System;

namespace Game
{
    public class Ghost : Creature
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
            sGhost = new Sound(new SoundBuffer(@"sfx/ghost.wav"));
            sGhost.Volume = 40;
        }

        public float Speed { get; }
        public float ProcsDistance { get; }
        public Sound sGhost { get; }
        public Clock DefaultClock { get; }

        public void UpdateGhostTexture()
        {
            if (SpeedX < 0) _animLeft.Animate();
            else _animRight.Animate();
        }

        public void UpdateGhost(Level level, MainCharacter character)
        {
            UpdateGhostTexture();

            if (!IsDead && (float) Math.Sqrt(Math.Pow(GetCenterPosition().X - character.GetCenterPosition().X, 2) +
                                             Math.Pow(GetCenterPosition().Y - character.GetCenterPosition().Y, 2)) <
                ProcsDistance)
            {
                if (GetCenterPosition().X > character.GetCenterPosition().X) SpeedX = -1 * Speed;
                else SpeedX = Speed;

                if (GetCenterPosition().Y > character.GetCenterPosition().Y) SpeedY = -1 * Speed;
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

        public void Die(Level level)
        {
            sKill.Play();
            level.Particles.Add(new ParticleEffect(X, Y, Color.Cyan));
            SetPosition(400, -100);
            IsDead = true;
            DefaultClock.Dispose();
        }
    }
}