using SFML.Audio;
using SFML.Graphics;
using SFML.System;

namespace ChendiAdventures
{
    public sealed class Trap : Entity
    {
        public bool IsUp;

        public Trap(float x, float y, Texture texture, TrapType type) : base(x, y, texture)
        {
            Type = type;
            DefaultTimer = new Clock();

            switch (Type)
            {
                case TrapType.BlowTorchLeft:
                {
                    SetTextureRectangle(32, 32);
                    Fire1 = new Block(X, Y, TrapsTexture);
                    Fire2 = new Block(X - 32, Y, TrapsTexture);

                    Fire1.SetTextureRectangle(8, 32);
                    Fire2.SetTextureRectangle(8, 32);

                    AnimFire1 = new Animation(Fire1, 0.03f,
                        new Vector2i(32, 96),
                        new Vector2i(96, 96),
                        new Vector2i(160, 96)
                    );
                    AnimFire2 = new Animation(Fire2, 0.03f,
                        new Vector2i(0, 96),
                        new Vector2i(64, 96),
                        new Vector2i(128, 96)
                    );

                    IsBlowing = false;
                    DefaultTimer.Restart();
                    sFire.Volume = 80;
                    break;
                }
                case TrapType.BlowTorchRight:
                {
                    SetTextureRectangle(0, 32);
                    Fire1 = new Block(X, Y, TrapsTexture);
                    Fire2 = new Block(X + 32, Y, TrapsTexture);

                    Fire1.SetTextureRectangle(8, 32);
                    Fire2.SetTextureRectangle(8, 32);

                    AnimFire1 = new Animation(Fire1, 0.03f,
                        new Vector2i(0, 64),
                        new Vector2i(64, 64),
                        new Vector2i(128, 64)
                    );

                    AnimFire2 = new Animation(Fire2, 0.03f,
                        new Vector2i(32, 64),
                        new Vector2i(96, 64),
                        new Vector2i(160, 64)
                    );

                    IsBlowing = false;
                    DefaultTimer.Restart();
                    sFire.Volume = 80;
                    break;
                }
                case TrapType.Crusher:
                {
                    SetTextureRectangle(0, 0);
                    Holder = new Block(X, Y, TrapsTexture);
                    Holder.SetTextureRectangle(32, 0);
                    Line1 = new Block(X, Y, TrapsTexture);
                    Line1.SetTextureRectangle(64, 0);
                    Line2 = new Block(X, Y, TrapsTexture);
                    Line2.SetTextureRectangle(64, 0);
                    SpeedY = 9f;
                    JustCrushed = false;
                    sGear.Volume = 50;
                    break;
                }
                case TrapType.Spikes:
                {
                    SetTextureRectangle(96, 0);
                    startY = Y;
                    Y += 32;
                    SpeedY = 1.5f;
                    IsUp = false;
                    sSpikes.Volume = 50;
                    break;
                }
                case TrapType.BlowerLeft:
                {
                    IsBlowing = false;
                    _blow = new Animation(this, 0.05f,
                        new Vector2i(128,0),
                        new Vector2i(160, 0),
                        new Vector2i(192, 0)
                        );
                    SetTextureRectangle(192,64);
                    DefaultTimer.Restart();
                    break;
                }
                case TrapType.BlowerRight:
                {
                    IsBlowing = false;
                    _blow = new Animation(this, 0.05f,
                        new Vector2i(128, 32),
                        new Vector2i(160, 32),
                        new Vector2i(192, 32)
                        );
                    SetTextureRectangle(192, 96);
                    DefaultTimer.Restart();
                    break;
                }
            }
            ApplyDifficulty();
        }

        public float SpeedY { get; }
        public TrapType Type { get; }
        public Clock DefaultTimer { get; }
        public Block Fire1 { get; }
        public Block Fire2 { get; }
        public bool IsBlowing { get; private set; }
        public Block Holder { get; }
        public Block Line1 { get; }
        public Block Line2 { get; }
        public bool JustCrushed { get; private set; }

        public void TrapUpdate()
        {
            switch (Type)
            {
                case TrapType.BlowTorchLeft:
                {
                    if (!IsBlowing && DefaultTimer.ElapsedTime.AsSeconds() > _blowtorchInterval - 2)
                        SetTextureRectangle(96, 32);
                    if (!IsBlowing && DefaultTimer.ElapsedTime.AsSeconds() > _blowtorchInterval) IsBlowing = true;
                    if (IsBlowing && DefaultTimer.ElapsedTime.AsSeconds() < _blowtorchInterval + 2)
                    {
                        AnimFire1.Animate();
                        AnimFire2.Animate();
                        if (sFire.Status != SoundStatus.Playing) sFire.Play();
                    }

                    if (IsBlowing && DefaultTimer.ElapsedTime.AsSeconds() >= _blowtorchInterval + 2)
                    {
                        IsBlowing = false;
                        DefaultTimer.Restart();
                        SetTextureRectangle(32, 32);
                        Fire1.SetTextureRectangle(8, 32);
                        Fire2.SetTextureRectangle(8, 32);
                    }

                    break;
                }
                case TrapType.BlowTorchRight:
                {
                    if (!IsBlowing && DefaultTimer.ElapsedTime.AsSeconds() > _blowtorchInterval - 2)
                        SetTextureRectangle(64, 32);
                    if (!IsBlowing && DefaultTimer.ElapsedTime.AsSeconds() > _blowtorchInterval) IsBlowing = true;
                    if (IsBlowing && DefaultTimer.ElapsedTime.AsSeconds() < _blowtorchInterval + 2)
                    {
                        AnimFire1.Animate();
                        AnimFire2.Animate();
                        if (sFire.Status != SoundStatus.Playing) sFire.Play();
                    }

                    if (IsBlowing && DefaultTimer.ElapsedTime.AsSeconds() >= _blowtorchInterval + 2)
                    {
                        IsBlowing = false;
                        DefaultTimer.Restart();
                        SetTextureRectangle(0, 32);
                        Fire1.SetTextureRectangle(8, 32);
                        Fire2.SetTextureRectangle(8, 32);
                    }

                    break;
                }
                case TrapType.Crusher:
                {
                    //moving down / crushing
                    if (!JustCrushed && DefaultTimer.ElapsedTime.AsSeconds() > _crusherInterval)
                    {
                        if (Y < Holder.Y + 96)
                        {
                            Y += SpeedY;
                        }
                        else
                        {
                            sCrush.Play();
                            JustCrushed = true;
                            Y = Holder.Y + 96;
                        }

                        if (Line2.Y < Holder.Y + 64)
                            Line2.Y += SpeedY;
                        else
                            Line2.Y = Holder.Y + 64;

                        if (Line1.Y < Holder.Y + 32)
                            Line1.Y += SpeedY;
                        else
                            Line1.Y = Holder.Y + 32;
                    }

                    //moving up / reloading
                    if (JustCrushed && DefaultTimer.ElapsedTime.AsSeconds() > _crusherInterval + 2)
                    {
                        if (Y > Holder.Y)
                        {
                            Y -= SpeedY / 3;
                            if (sGear.Status != SoundStatus.Playing) sGear.Play();
                            if (Line1.Y > Holder.Y) Line1.Y -= SpeedY / 3;
                            if (Line2.Y > Holder.Y) Line2.Y -= SpeedY / 3;
                        }
                        else
                        {
                            JustCrushed = false;
                            DefaultTimer.Restart();
                            Line1.Y = Holder.Y;
                            Line2.Y = Holder.Y;
                            Y = Holder.Y;
                        }
                    }


                    break;
                }
                case TrapType.Spikes:
                {
                    if (!IsUp) //upwards
                    {
                        Y -= SpeedY;
                        if (sSpikes.Status != SoundStatus.Playing) sSpikes.Play();
                        if (Y < startY)
                        {
                            IsUp = true;
                            Y = startY;
                        }
                    }
                    else //downwards
                    {
                        Y += SpeedY;
                        if (Y > startY + 32)
                        {
                            if (DefaultTimer.ElapsedTime.AsSeconds() > _spikeInterval)
                            {
                                IsUp = false;
                                DefaultTimer.Restart();
                            }

                            Y = startY + 32;
                        }
                    }


                    break;
                }
                case TrapType.BlowerLeft:
                {
                    if (DefaultTimer.ElapsedTime.AsSeconds() > _blowerInterval + 0.15f && IsBlowing)
                    {
                        IsBlowing = false;
                        _blow.ResetAnimation();
                        DefaultTimer.Restart();
                        SetTextureRectangle(192, 64);
                    }
                    else if (DefaultTimer.ElapsedTime.AsSeconds() > _blowerInterval && IsBlowing)
                    {
                        _blow.Animate();
                    }
                    else if (DefaultTimer.ElapsedTime.AsSeconds() > _blowerInterval && !IsBlowing)
                    {
                        IsBlowing = true;
                        sBlow.Play();
                    }
                    break;
                }
                case TrapType.BlowerRight:
                {
                    if (DefaultTimer.ElapsedTime.AsSeconds() > _blowerInterval + 0.15f && IsBlowing)
                    {
                        IsBlowing = false;
                        _blow.ResetAnimation();
                        DefaultTimer.Restart();
                        SetTextureRectangle(192, 96);
                    }
                    else if (DefaultTimer.ElapsedTime.AsSeconds() > _blowerInterval && IsBlowing)
                    {
                        _blow.Animate();
                    }
                    else if (DefaultTimer.ElapsedTime.AsSeconds() > _blowerInterval && !IsBlowing)
                    {
                        IsBlowing = true;
                        sBlow.Play();
                    }
                    break;
                }
            }
        }

        public new FloatRect GetBoundingBox()
        {
            switch (Type)
            {
                case TrapType.BlowTorchLeft:
                {
                    return new FloatRect(X - 32, Y + 4, 60, 24);
                }
                case TrapType.BlowTorchRight:
                {
                    return new FloatRect(X, Y + 4, 60, 24);
                }
                case TrapType.Crusher:
                {
                    return new FloatRect(X+2, Top + 17, 28, 15);
                }
                case TrapType.Spikes:
                {
                    return new FloatRect(X + 4, Top + 17, 24, 1);
                }
                case TrapType.BlowerRight:
                {
                    return new FloatRect(X, Y - 10, 52,52);
                }
                case TrapType.BlowerLeft:
                {
                    return new FloatRect(X - 10, Y - 10, 52, 52);
                    }
                default: 
                {
                    return GetBoundingBox();
                }
            }
        }

        public override void Draw(RenderTarget target, RenderStates states)
        {
            switch (Type)
            {
                case TrapType.BlowTorchLeft:
                {
                    base.Draw(target, states);
                    target.Draw(Fire1, states);
                    target.Draw(Fire2, states);
                    break;
                }
                case TrapType.BlowTorchRight:
                {
                    base.Draw(target, states);
                    target.Draw(Fire1, states);
                    target.Draw(Fire2, states);
                    break;
                }
                case TrapType.Crusher:
                {
                    target.Draw(Line1, states);
                    target.Draw(Line2, states);
                    target.Draw(Holder, states);
                    base.Draw(target, states);
                    break;
                }
                case TrapType.Spikes:
                {
                    base.Draw(target, states);
                    break;
                }
                case TrapType.BlowerLeft:
                {
                    base.Draw(target, states);
                    break;
                }
                case TrapType.BlowerRight:
                {
                    base.Draw(target, states);
                    break;
                }
            }
        }

        public new Vector2f Get32Position()
        {
            if (Type == TrapType.Spikes) return new Vector2f(X / 32, (Y - 32) / 32);
            return base.Get32Position();
        }

        public void ApplyDifficulty()
        {
            switch (MainGameWindow.GameDifficulty)
            {
                case Difficulty.Easy:
                {
                    _spikeInterval = 5;
                    _crusherInterval = 5;
                    _blowtorchInterval = 7;
                    _blowerInterval = 9;
                    break;
                }
                case Difficulty.Medium:
                {
                    _spikeInterval = 3;
                    _crusherInterval = 3;
                    _blowtorchInterval = 5;
                    _blowerInterval = 6;
                    break;
                }
                case Difficulty.Hard:
                {
                    _spikeInterval = 2;
                    _crusherInterval = 2;
                    _blowtorchInterval = 3;
                    _blowerInterval = 3;
                    break;
                }
            }
        }

        private static readonly Sound sFire = new Sound(new SoundBuffer(@"sfx/fire.wav"));
        private static readonly Sound sCrush = new Sound(new SoundBuffer(@"sfx/crusher.wav"));
        private static readonly Sound sGear = new Sound(new SoundBuffer(@"sfx/gear.wav"));
        private static readonly Sound sSpikes = new Sound(new SoundBuffer(@"sfx/spike.wav"));
        private static readonly Sound sBlow = new Sound(new SoundBuffer(@"sfx/blow.wav")) {Volume = 230};
        private readonly Animation AnimFire1;
        private readonly Animation AnimFire2;
        private readonly Animation _blow;
        private readonly float startY;
        private int _blowtorchInterval;
        private int _crusherInterval;
        private int _spikeInterval;
        private int _blowerInterval;
    }
}