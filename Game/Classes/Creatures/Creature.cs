using SFML.Audio;
using SFML.Graphics;
using SFML.System;

namespace ChendiAdventures
{
    public abstract class Creature : Entity
    {
        public static Sound sJump;
        public static Sound sStep;
        public static Sound sLand;
        public static Sound sKill = new Sound(new SoundBuffer(@"sfx/kill.wav"));

        public Creature(float x, float y, Texture texture) : base(x, y, texture)
        {
            Points = 0;
            IsDead = false;

            SpeedX = 0f;
            SpeedY = 0f;


            IsStandingOnBlocks = false;

            MovementDirection = Movement.None;
        }

        public int Points { get; set; }
        public float SpeedX { get; set; } //szybkość po X
        public float SpeedY { get; set; } //szybkość po Y
        public float MaxSpeedX { get; protected set; }
        public float MaxSpeedY { get; protected set; }
        public float dX { get; set; }
        public float GravityForce { get; set; } //szybkosc grawitacji
        public bool IsStandingOnBlocks { get; set; }
        public bool IsDead { get; set; }
        public Movement MovementDirection { get; set; }

        public Vector2f Get32NextPosition()
        {
            return new Vector2f((X + SpeedX) / 32, (Y + SpeedY) / 32);
        }

        public void MoveLeft()
        {
            if (SpeedX > -1 * MaxSpeedX)
                SpeedX -= dX;
            else SpeedX = -1 * MaxSpeedX;
        }

        public void MoveRight()
        {
            if (SpeedX < MaxSpeedX)
                SpeedX += dX;
            else SpeedX = MaxSpeedX;
        }

        public void Jump()
        {
            if (IsStandingOnBlocks)
            {
                sJump.Play();
                SpeedY = -1 * MaxSpeedY;
                IsStandingOnBlocks = false;
            }
        }

        public virtual void Die(Level level)
        {
        }

        public virtual void UpdateTextures()
        {
        }

        public virtual void UpdateCreature()
        {
        }

        public virtual void ApplyDifficulty()
        {
            switch (MainGameWindow.GameDifficulty)
            {
                case Difficulty.Easy:
                {
                    break;
                }
                case Difficulty.Medium:
                {
                    break;
                }
                case Difficulty.Hard:
                {
                    break;
                }
            }
        }

        protected Animation _animLeft;
        protected Animation _animRight;
    }
}