using SFML.Audio;
using SFML.Graphics;
using SFML.System;

namespace Game
{
    public abstract class Creature : Entity
    {
        protected static Sound sJump;
        public static Sound sKill = new Sound(new SoundBuffer(@"sfx/kill.wav"));
        protected Animation _animLeft;
        protected Animation _animRight;
        public int Points { get; set; }
        public Creature(float x, float y, Texture texture) : base(x, y, texture)
        {
            Points = 0;
            IsDead = false;

            SpeedX = 0f;
            SpeedY = 0f;

            MaxSpeedX = 5f;
            MaxSpeedY = 13f;

            dX = 0.53f;

            GravityForce = 0.81f;

            IsStandingOnBlocks = false;

            MovementDirection = Movement.None;
        }
        public float SpeedX { get; set; } //szybkość po X
        public float SpeedY { get; set; } //szybkość po Y
        public float MaxSpeedX { get; }
        public float MaxSpeedY { get; }
        public float dX { get; }
        public float GravityForce { get; set; } //szybkosc grawitacji
        public bool IsStandingOnBlocks { get; set; }
        public bool IsDead { get; set; }
        public Movement MovementDirection { get; set; }

        public Vector2f Get32NextPosition()
        {
            return new Vector2f((X + SpeedX) / 32, (Y + SpeedY) / 32);
        }

        // movement
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

        ////
        public virtual void CollisionDependence(Level level)
        {
            var NewX = Get32NextPosition().X;
            var NewY = Get32NextPosition().Y;
            Block obstacle;

            // HORIZONTAL
            if (SpeedX < 0) // moving left
            {
                if (level.UnpassableContains(
                        (obstacle = level.GetObstacle(Get32NextPosition().X + 0.01f, Get32Position().Y + 0.15f))
                        .Type) ||
                    level.UnpassableContains((obstacle =
                        level.GetObstacle(Get32NextPosition().X + 0.01f, Get32Position().Y + 0.85f)).Type))
                {
                    NewX = (int) Get32NextPosition().X + 1;
                    SpeedX = 0;
                    //Console.WriteLine("from RIGHT side");
                }
            }
            else if (SpeedX > 0) //moving right
            {
                if (level.UnpassableContains(
                        (obstacle = level.GetObstacle(Get32NextPosition().X + 0.99f, Get32Position().Y + 0.15f))
                        .Type) ||
                    level.UnpassableContains((obstacle =
                        level.GetObstacle(Get32NextPosition().X + 0.99f, Get32Position().Y + 0.85f)).Type))
                {
                    NewX = (int) Get32NextPosition().X;
                    SpeedX = 0;
                    //Console.WriteLine("from LEFT side");
                }
            }

            // VERTICAL
            IsStandingOnBlocks = false;

            if (SpeedY < 0) // moving up
            {
                if (level.UnpassableContains((obstacle =
                        level.GetObstacle(Get32NextPosition().X + 0.15f, Get32NextPosition().Y + 0.1f)).Type) ||
                    level.UnpassableContains((obstacle =
                        level.GetObstacle(Get32NextPosition().X + 0.85f, Get32NextPosition().Y + 0.1f)).Type))
                {
                    NewY = (int) Get32NextPosition().Y + 1;
                    SpeedY = 0;
                    sJump.Stop();
                    //Console.WriteLine("from BOTTOM side");
                }
            }
            else //moving down
            {
                if (level.UnpassableContains((obstacle =
                        level.GetObstacle(Get32NextPosition().X + 0.15f, Get32NextPosition().Y + 0.99f)).Type) ||
                    level.UnpassableContains((obstacle =
                        level.GetObstacle(Get32NextPosition().X + 0.85f, Get32NextPosition().Y + 0.99f)).Type))
                {
                    NewY = (int) Get32NextPosition().Y;
                    IsStandingOnBlocks = true;
                    if (obstacle.Type == BlockType.Stone) obstacle.Stomp();
                    SpeedY = 0;
                }
            }

            Set32Position(NewX, NewY);
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
    }
}