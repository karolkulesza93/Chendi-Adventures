using SFML.System;
using SFML.Audio;
using SFML.Graphics;

namespace Game
{
    public abstract class Creature : Entity
    {
        public float SpeedX { get; set; } //szybkość po X
        public float SpeedY { get; set; } //szybkość po Y
        public float MaxSpeedX { get; private set; }
        public float MaxSpeedY { get; private set; }
        public float dX { get; private set; }
        public float GravityForce { get; set; } //szybkosc grawitacji
        public bool IsStandingOnBlocks { get; set; }
        public bool IsDead { get; set; }
        public Movement MovementDirection { get; set; }
        protected Animation _animLeft;
        protected Animation _animRight;
        protected static Sound sJump;
        public static Sound sKill = new Sound(new SoundBuffer(@"sfx/kill.wav"));
        public Vector2f Get32NextPosition()
        {
            return new Vector2f((float)(this.X + this.SpeedX) /32, (float)(this.Y + this.SpeedY) /32);
        }
        public Creature(float x, float y, Texture texture) : base(x, y, texture)
        {
            this.IsDead = false;

            this.SpeedX = 0f;
            this.SpeedY = 0f;

            this.MaxSpeedX = 5f;
            this.MaxSpeedY = 13f;

            this.dX = 0.53f;

            this.GravityForce = 0.81f;

            this.IsStandingOnBlocks = false;

            this.MovementDirection = Movement.None;

        }
        // movement
        public void MoveLeft()
        {
            if (this.SpeedX > -1 * this.MaxSpeedX)
                this.SpeedX -= this.dX;
            else this.SpeedX = -1 * this.MaxSpeedX;
        }
        public void MoveRight()
        {
            if (this.SpeedX < this.MaxSpeedX)
                this.SpeedX += this.dX;
            else this.SpeedX = this.MaxSpeedX;
        }
        public void Jump()
        {
            if (this.IsStandingOnBlocks)
            {
                sJump.Play();
                this.SpeedY = -1 * this.MaxSpeedY;
                this.IsStandingOnBlocks = false;
            }
        }
        ////
        public virtual void CollisionDependence(Level level)
        {
            float NewX = this.Get32NextPosition().X;
            float NewY = this.Get32NextPosition().Y;
            Block obstacle;
            
            // HORIZONTAL
            if (this.SpeedX < 0) // moving left
            {
                if (level.UnpassableContains((obstacle = level.GetObstacle(this.Get32NextPosition().X + 0.01f, this.Get32Position().Y + 0.15f)).Type) ||
                    level.UnpassableContains((obstacle = level.GetObstacle(this.Get32NextPosition().X + 0.01f, this.Get32Position().Y + 0.85f)).Type))
                {
                    NewX = (int)this.Get32NextPosition().X + 1;
                    this.SpeedX = 0;
                    //Console.WriteLine("from RIGHT side");
                }
            }
            else if (this.SpeedX > 0)//moving right
            {
                if (level.UnpassableContains((obstacle = level.GetObstacle(this.Get32NextPosition().X + 0.99f, this.Get32Position().Y + 0.15f)).Type) ||
                    level.UnpassableContains((obstacle = level.GetObstacle(this.Get32NextPosition().X + 0.99f, this.Get32Position().Y + 0.85f)).Type))
                {
                    NewX = (int)this.Get32NextPosition().X;
                    this.SpeedX = 0;
                    //Console.WriteLine("from LEFT side");
                }
            }

            // VERTICAL
            this.IsStandingOnBlocks = false;

            if (this.SpeedY < 0) // moving up
            {
                if (level.UnpassableContains((obstacle = level.GetObstacle(this.Get32NextPosition().X + 0.15f, this.Get32NextPosition().Y + 0.1f)).Type) ||
                    level.UnpassableContains((obstacle = level.GetObstacle(this.Get32NextPosition().X + 0.85f, this.Get32NextPosition().Y + 0.1f)).Type))
                {
                    NewY = (int)this.Get32NextPosition().Y + 1;
                    this.SpeedY = 0;
                    sJump.Stop();
                    //Console.WriteLine("from BOTTOM side");
                }
            }
            else //moving down
            {
                if (level.UnpassableContains((obstacle = level.GetObstacle(this.Get32NextPosition().X + 0.15f, this.Get32NextPosition().Y + 0.99f)).Type) ||
                    level.UnpassableContains((obstacle = level.GetObstacle(this.Get32NextPosition().X + 0.85f, this.Get32NextPosition().Y + 0.99f)).Type))
                {
                    NewY = (int)this.Get32NextPosition().Y;
                    this.IsStandingOnBlocks = true;
                    if (obstacle.Type == BlockType.Stone) obstacle.Stomp();
                    this.SpeedY = 0;
                }
            }
            this.Set32Position(NewX, NewY);
        }
        public virtual void UpdateTextures() { }
        public virtual void UpdateCreature() { }
    }
}
