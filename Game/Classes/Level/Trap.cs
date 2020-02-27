using SFML.System;
using SFML.Graphics;
using SFML.Audio;

namespace Game
{
    public class Trap : Entity
    {
        public float SpeedY { get; private set; }
        public TrapType Type { get; private set; }
        public Clock DefaultTimer { get; private set; }
        //blowtorch
        public Projectile FlameLeft { get; private set; }
        public Projectile FlameRight { get; private set; }
        public Block Fire1 { get; private set; }
        public Block Fire2 { get; private set; }
        private Animation AnimFire1;
        private Animation AnimFire2;
        public bool IsBlowing { get; private set; }
        private static Sound sFire = new Sound(new SoundBuffer(@"sfx/fire.wav"));
        //crusher
        public Block Holder { get; private set; }
        public Block Line { get; private set; }
        public bool JustCrushed { get; private set; }
        private static Sound sCrush = new Sound(new SoundBuffer(@"sfx/crusher.wav"));
        private static Sound sGear = new Sound(new SoundBuffer(@"sfx/gear.wav")); 
        //spikes
        private float startY;
        public bool IsUp;
        private static Sound sSpikes = new Sound(new SoundBuffer(@"sfx/spike.wav")); 
        
        public Trap(float x, float y, Texture texture, TrapType type) : base(x,y,texture)
        {
            this.Type = type;
            this.DefaultTimer = new Clock();

            switch (this.Type)
            {
                case TrapType.BlowTorchLeft:
                    {
                        this.SetTextureRectanlge(32, 32, 32, 32);
                        this.Fire1 = new Block(this.X, this.Y, TrapsTexture);
                        this.Fire2 = new Block(this.X - 32, this.Y, TrapsTexture);

                        this.Fire1.SetTextureRectanlge(128, 0, 32, 32);
                        this.Fire2.SetTextureRectanlge(128, 0, 32, 32);

                        this.AnimFire1 = new Animation(this.Fire1, 0.03f,
                            new Vector2i(32, 96),
                            new Vector2i(96,96),
                            new Vector2i(160,96)
                            );
                        this.AnimFire2 = new Animation(this.Fire2, 0.03f,
                            new Vector2i(0, 96),
                            new Vector2i(64, 96),
                            new Vector2i(128, 96)
                            ) ;

                        this.IsBlowing = false;
                        this.DefaultTimer.Restart();
                        sFire.Volume = 80;
                        break;
                    }
                case TrapType.BlowTorchRight:
                    {
                        this.SetTextureRectanlge(0, 32, 32, 32);
                        this.Fire1 = new Block(this.X, this.Y, TrapsTexture);
                        this.Fire2 = new Block(this.X+32, this.Y, TrapsTexture);

                        this.Fire1.SetTextureRectanlge(128, 0, 32, 32);
                        this.Fire2.SetTextureRectanlge(128, 0, 32, 32);

                        this.AnimFire1 = new Animation(this.Fire1, 0.03f,
                            new Vector2i(0,64),
                            new Vector2i(64,64),
                            new Vector2i(128,64)
                            );

                        this.AnimFire2 = new Animation(this.Fire2, 0.03f,
                            new Vector2i(32,64),
                            new Vector2i(96,64),
                            new Vector2i(160,64)
                            );

                        this.IsBlowing = false;
                        this.DefaultTimer.Restart();
                        sFire.Volume = 80;
                        break;
                    }
                case TrapType.Crusher:
                    {
                        this.SetTextureRectanlge(0, 0, 32, 32);
                        this.Holder = new Block(this.X, this.Y, TrapsTexture); this.Holder.SetTextureRectanlge(32, 0, 32, 32);
                        this.Line = new Block(this.X, this.Y, TrapsTexture); this.Line.SetTextureRectanlge(64, 0, 32, 32);
                        this.SpeedY = 6f;
                        this.JustCrushed = false;
                        sGear.Volume = 50;
                        break;
                    }
                case TrapType.Spikes:
                    {
                        this.SetTextureRectanlge(96, 0, 32, 32);
                        this.startY = this.Y;
                        this.Y += 32;
                        this.SpeedY = 1f;
                        this.IsUp = false;
                        sSpikes.Volume = 50;
                        break;
                    }
            }
        }
        public void TrapUpdate()
        {
            switch (this.Type)
            {
                case TrapType.BlowTorchLeft:
                    {
                        if (!this.IsBlowing && this.DefaultTimer.ElapsedTime.AsSeconds() > 3) this.SetTextureRectanlge(96, 32, 32, 32);
                        if (!this.IsBlowing && this.DefaultTimer.ElapsedTime.AsSeconds() > 5) this.IsBlowing = true;
                        if (this.IsBlowing && this.DefaultTimer.ElapsedTime.AsSeconds() < 7)
                        {
                            this.AnimFire1.Animate();
                            this.AnimFire2.Animate();
                            if (sFire.Status != SoundStatus.Playing) sFire.Play();
                        }
                        if (this.IsBlowing && this.DefaultTimer.ElapsedTime.AsSeconds() >= 7)
                        {
                            this.IsBlowing = false;
                            this.DefaultTimer.Restart();
                            this.SetTextureRectanlge(32, 32, 32, 32);
                            this.Fire1.SetTextureRectanlge(128, 0, 32, 32);
                            this.Fire2.SetTextureRectanlge(128, 0, 32, 32);
                        }
                        break;
                    }
                case TrapType.BlowTorchRight:
                    {
                        if (!this.IsBlowing && this.DefaultTimer.ElapsedTime.AsSeconds() > 3) this.SetTextureRectanlge(64, 32, 32, 32);
                        if (!this.IsBlowing && this.DefaultTimer.ElapsedTime.AsSeconds() > 5) this.IsBlowing = true;
                        if (this.IsBlowing && this.DefaultTimer.ElapsedTime.AsSeconds() < 7)
                        {
                            this.AnimFire1.Animate();
                            this.AnimFire2.Animate();
                            if (sFire.Status != SoundStatus.Playing) sFire.Play();
                        }
                        if (this.IsBlowing && this.DefaultTimer.ElapsedTime.AsSeconds() >= 7)
                        {
                            this.IsBlowing = false;
                            this.DefaultTimer.Restart();
                            this.SetTextureRectanlge(0, 32, 32, 32);
                            this.Fire1.SetTextureRectanlge(128, 0, 32, 32);
                            this.Fire2.SetTextureRectanlge(128, 0, 32, 32);
                        }
                        break;

                        break;
                    }
                case TrapType.Crusher:
                    {
                        //moving down / crushing
                        if (!this.JustCrushed && this.DefaultTimer.ElapsedTime.AsSeconds() > 3)
                        {
                            if (this.Y < this.Holder.Y + 64)
                            {
                                this.Y += this.SpeedY;
                            }
                            else
                            {
                                sCrush.Play();
                                this.JustCrushed = true;
                                this.Y = this.Holder.Y + 64;
                            }

                            if (this.Line.Y < this.Holder.Y + 32)
                            {
                                this.Line.Y += this.SpeedY;
                            }
                            else
                            {
                                this.Line.Y = this.Holder.Y + 32;
                            }
                        }
                        //moving up / reloading
                        if (this.JustCrushed && this.DefaultTimer.ElapsedTime.AsSeconds() > 5)
                        {
                            if (this.Y > this.Holder.Y)
                            {
                                this.Y -= this.SpeedY / 4;
                                if (sGear.Status != SoundStatus.Playing) sGear.Play();
                                if (this.Line.Y > this.Holder.Y) this.Line.Y -= this.SpeedY / 4;
                            }
                            else
                            {
                                this.JustCrushed = false;
                                this.DefaultTimer.Restart();
                                this.Line.Y = this.Holder.Y;
                                this.Y = this.Holder.Y;
                            }
                        }


                        break;
                    }
                case TrapType.Spikes:
                    {
                        if (!this.IsUp) //upwards
                        {
                            this.Y -= this.SpeedY;
                            if (sSpikes.Status != SoundStatus.Playing) sSpikes.Play();
                            if (this.Y < this.startY)
                            {
                                this.IsUp = true;
                                this.Y = this.startY;
                            }
                        }
                        else //downwards
                        {
                            this.Y += this.SpeedY;
                            if (this.Y > this.startY + 32)
                            {
                                if (this.DefaultTimer.ElapsedTime.AsSeconds() > 3)
                                { 
                                    this.IsUp = false;
                                    this.DefaultTimer.Restart();
                                }
                                this.Y = this.startY + 32;
                            }
                        }


                        break;
                    }
            }
        }
        public new FloatRect GetBoundingBox()
        {

            switch (this.Type)
            {
                case TrapType.BlowTorchLeft:
                    {
                        return new FloatRect(this.X - 32, this.Y, 64, 32);
                    }
                case TrapType.BlowTorchRight:
                    {
                        return new FloatRect(this.X + 32, this.Y, 64, 32);
                    }
                case TrapType.Crusher:
                    {
                        return new FloatRect(this.X, this.Top + 17, 32, 15);
                    }
                case TrapType.Spikes:
                    {
                        return new FloatRect(this.X + 2, this.Top + 17, 28, 1);
                    }
                default: return this.GetBoundingBox();
            }
        }
        public override void Draw(RenderTarget target, RenderStates states)
        {
            switch (this.Type)
            {
                case TrapType.BlowTorchLeft:
                    {
                        base.Draw(target, states);
                        target.Draw(this.Fire1, states);
                        target.Draw(this.Fire2, states);
                        break;
                    }
                case TrapType.BlowTorchRight:
                    {
                        base.Draw(target, states);
                        target.Draw(this.Fire1, states);
                        target.Draw(this.Fire2, states);
                        break;
                    }
                case TrapType.Crusher:
                    {
                        target.Draw(this.Holder, states);
                        target.Draw(this.Line, states);
                        base.Draw(target, states);
                        break;
                    }
                case TrapType.Spikes:
                    {
                        base.Draw(target, states);
                        break;
                    }
            }
        }
    }
}
