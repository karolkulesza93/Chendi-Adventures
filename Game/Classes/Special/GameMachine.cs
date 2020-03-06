using System;
using SFML.Audio;
using SFML.Graphics;

namespace Game
{
    public class GameMachine : Entity
    {
        private readonly Random _rnd;
        public Block Item { get; set; }
        private int _loss;
        private Sound _click;
        private TextLine _reward;
        private readonly View _view;

        public GameMachine(float x, float y, Texture texture, View view) : base(x, y, texture)
        {
            SetTextureRectanlge(0, 0, 64, 128);
            Item = new Block(-100, y+16, Entity.RewardsTexture);
            Item.SetTextureRectanlge(0,0);
            _reward = new TextLine("",10,1000, 0, Color.Green);
            _reward.SetOutlineThickness(1);
            _rnd = new Random(432);
            _loss = 0;
            _click = new Sound(new SoundBuffer(@"sfx/click.wav"));
            _click.Volume = 30;
            LootedReward = Reward.Nothing;
            this._view = view;
        }

        public Reward LootedReward { get; private set; }

        public void Roll()
        {
            var tmp = LootedReward;

            Item.X = X + 16;
            Item.Y = Y + 16;

            _loss = _rnd.Next(300);
            _click.Play();

            if (_loss >= 0 && _loss <= 49)
                LootedReward = Reward.Coins10;
            else if (_loss >= 50 && _loss <= 99)
                LootedReward = Reward.Mana3;
            else if (_loss >= 100 && _loss <= 149)
                LootedReward = Reward.Arrow3;
            else if (_loss >= 150 && _loss <= 169)
                LootedReward = Reward.Coins100;
            else if (_loss >= 170 && _loss <= 189)
                LootedReward = Reward.Mana10;
            else if (_loss >= 190 && _loss <= 209)
                LootedReward = Reward.Arrow10;
            else if (_loss >= 210 && _loss <= 219)
                LootedReward = Reward.Coins1000;
            else if (_loss >= 220 && _loss <= 229)
                LootedReward = Reward.Mana25;
            else if (_loss >= 230 && _loss <= 239)
                LootedReward = Reward.Arrow25;
            else if (_loss >= 240 && _loss <= 242)
                LootedReward = Reward.Life;
            else if (_loss >= 243 && _loss <= 245)
                LootedReward = Reward.Score10000;
            else if (_loss == 246)
                LootedReward = Reward.Jackpot;
            else if (_loss == 247)
                LootedReward = Reward.TripleLife;
            else
                LootedReward = Reward.Nothing;

            if (tmp == LootedReward) Roll();
        }

        public void GrantReward(MainCharacter character)
        {
            _reward.MoveText( _view.Center.X - _view.Size.X/2 -300, _view.Center.Y + 180);
            _reward.EditText("");

            switch (LootedReward)
            {
                case Reward.Nothing:
                    {
                        _reward.EditText("YOU HAVE NOT WON ANYTHING...");
                        break;
                    }
                case Reward.Coins10:
                {
                    character.Coins += 10;
                    _reward.EditText("YOU HAVE WON 10 COINS");
                        break;
                }
                case Reward.Coins100:
                {
                    character.Coins += 100;
                    _reward.EditText("YOU HAVE WON 100 COINS");
                        break;
                }
                case Reward.Coins1000:
                {
                    character.Coins += 1000;
                    _reward.EditText("YOU HAVE WON 1000 COINS!");
                        break;
                }
                case Reward.Jackpot:
                {
                    character.Coins += 10000;
                    _reward.EditText("JACKPOT! YOU HAVE WON 10000 COINS");
                        break;
                }

                case Reward.Arrow3:
                {
                    character.ArrowAmount += 1;
                    _reward.EditText("YOU HAVE WON 1 ARROW");
                        break;
                }
                case Reward.Arrow10:
                {
                    character.ArrowAmount += 3;
                    _reward.EditText("YOU HAVE WON 3 ARROWS");
                        break;
                }
                case Reward.Arrow25:
                {
                    character.ArrowAmount += 5;
                    _reward.EditText("YOU HAVE WON 5 ARROWS");
                        break;
                }

                case Reward.Mana3:
                {
                    character.Mana += 1;
                    _reward.EditText("YOU HAVE WON 1 MANA POTION");
                        break;
                }
                case Reward.Mana10:
                {
                    character.Mana += 2;
                    _reward.EditText("YOU HAVE WON 2 MANA POTIONS");
                        break;
                }
                case Reward.Mana25:
                {
                    character.Mana += 3;
                    _reward.EditText("YOU HAVE WON 3 MANA POTIONS");
                        break;
                }

                case Reward.Score10000:
                {
                    character.Score += 10000;
                    _reward.EditText("YOU HAVE WON 10000 POINTS");
                    break;
                }
                case Reward.Life:
                {
                    character.Lives++; 
                    _reward.EditText("YOU HAVE WON 1 LIFE");
                        break;
                }
                case Reward.TripleLife:
                {
                    character.Lives += 3;
                    _reward.EditText("YOU HAVE WON 3 LIVES");
                    break;
                }
            }
        }

        public void TextureUpdate()
        {
            if (_loss >= 0 && _loss <= 49)
            {
                Item.SetTextureRectanlge(0, 32);
            }
            else if (_loss >= 50 && _loss <= 99)
            {
                Item.SetTextureRectanlge(0, 96);
            }
            else if (_loss >= 100 && _loss <= 149)
            {
                Item.SetTextureRectanlge(0, 64);
            }
            else if (_loss >= 150 && _loss <= 169)
            {
                Item.SetTextureRectanlge(32, 32);
            }
            else if (_loss >= 170 && _loss <= 189)
            {
                Item.SetTextureRectanlge(32, 96);
            }
            else if (_loss >= 190 && _loss <= 209)
            {
                Item.SetTextureRectanlge(32, 64);
            }
            else if (_loss >= 210 && _loss <= 219)
            {
                Item.SetTextureRectanlge(64, 32);
            }
            else if (_loss >= 220 && _loss <= 229)
            {
                Item.SetTextureRectanlge(64, 96);
            }
            else if (_loss >= 230 && _loss <= 239)
            {
                Item.SetTextureRectanlge(64, 64);
            }
            else if (_loss >= 240 && _loss <= 242)
            {
                Item.SetTextureRectanlge(0, 128);
            }
            else if (_loss >= 243 && _loss <= 245)
            {
                Item.SetTextureRectanlge(0, 160);
            }
            else if (_loss == 246)
            {
                Item.SetTextureRectanlge(0, 192);
            }
            else if (_loss == 247)
            {
                Item.SetTextureRectanlge(32, 128);
            }
            else
            {
                Item.SetTextureRectanlge(0, 0);
            }
        }

        public void GameMachineUpdate()
        {
            if (X < _view.Center.X - 32)
            {
                X += 10;
                Item.X = X + 16;
                Item.Y = Y + 16;
            }
            else
            {
                X = _view.Center.X - 32;
                Item.X = X + 16;
                Item.Y = Y + 16;
            }

            if (_reward.X < X - _reward.Width/2 )
            {
                _reward.MoveText(_reward.X + 15, _reward.Y);
            }
            TextureUpdate();
        }

        public override void Draw(RenderTarget target, RenderStates states)
        {
            base.Draw(target, states);
            //if (X == (int)_view.Center.X - 32)
            {
                target.Draw(Item);
                target.Draw(_reward);
            }
        }
    }
}