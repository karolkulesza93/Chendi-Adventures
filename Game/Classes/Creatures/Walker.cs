using System;
using SFML.Audio;
using SFML.Graphics;
using SFML.System;

namespace ChendiAdventures
{
    public class Walker : Creature
    {
        public float YMaxPos;
        public float YMinPos;
        public Clock DefaultClock { get; }
        public Laser Laser { get; }
        public float ProcsDistance { get; set; }
        public Movement Direction { get; set; }
        public int Health { get; set; }
        public static Sound sShot = new Sound(EnergyBall.sEnergyShoot) {Volume = 50, Pitch = 2};

        public Walker(float x, float y, Texture texture, Movement dir) : base(x, y, texture)
        {
            DefaultClock = new Clock();

            Direction = dir;

            Laser = new Laser(-500, 200, texture, dir);

            YMinPos = y - 64;
            YMaxPos = y + 96;

            SpeedX = 0f;
            SpeedY = 1f;

            ProcsDistance = 400;

            _animLeft = new Animation(this, 0.1f,
                new Vector2i(0, 0),
                new Vector2i(32, 0)
            );
            _animRight = new Animation(this, 0.1f,
                new Vector2i(0, 32),
                new Vector2i(32, 32)
            );
            switch (dir)
            {
                case Movement.Left:
                    {
                        SetTextureRectangle(0, 0);
                        break;
                    }
                case Movement.Right:
                    {
                        SetTextureRectangle(0, 32);
                        break;
                    }
            }
            IsDead = false;

            ApplyDifficulty();
        }

        public override void Die(Level level)
        {
            Block.sDestroy.Play();
            level.Particles.Add(new ParticleEffect(X, Y, Color.Yellow));
            SetPosition(400, -100);
            IsDead = true;
            DefaultClock.Dispose();
        }

        public void UpdateCreature(MainCharacter character, Level level)
        {
            Y += SpeedY;
            if (Top < YMinPos || Bottom > YMaxPos) SpeedY *= -1;

            if (!IsDead && !character.IsDead && DefaultClock.ElapsedTime.AsSeconds() > _shotInterval &&
                (float)Math.Sqrt(
                    Math.Pow(GetCenterPosition().X - character.GetCenterPosition().X, 2) +
                    Math.Pow(GetCenterPosition().Y - character.GetCenterPosition().Y, 2)) <
                ProcsDistance)
            {
                sShot.Play();
                Laser.SetPosition(X + 13, Y + 13);
                Laser.Attack(character);
                DefaultClock.Restart();
            }

            if (Health < 1 && !IsDead) Die(level);

            Laser.LaserUpdate(level, character);
            UpdateTextures();
        }

        public override void Draw(RenderTarget target, RenderStates states)
        {
            base.Draw(target, states);
            target.Draw(Laser);
        }

        public override void UpdateTextures()
        {
            switch (Direction)
            {
                case Movement.Right:
                    {
                        _animRight.Animate();
                        break;
                    }
                case Movement.Left:
                    {
                        _animLeft.Animate();
                        break;
                    }
            }
        }

        public override void ApplyDifficulty()
        {
            switch (MainGameWindow.GameDifficulty)
            {
                case Difficulty.Easy:
                    {
                        ProcsDistance = 300f;
                        _shotInterval = 3f;
                        Health = 1;
                        Points = 2000;
                        break;
                    }
                case Difficulty.Medium:
                    {
                        ProcsDistance = 400f;
                        _shotInterval = 2f;
                        Health = 2;
                        Points = 2500;
                        break;
                    }
                case Difficulty.Hard:
                    {
                        ProcsDistance = 600f;
                        _shotInterval = 1.2f;
                        Health = 3;
                        Points = 3000;
                        break;
                    }
            }
        }

        private float _shotInterval;

    }
}
