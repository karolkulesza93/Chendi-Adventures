using System;
using SFML.Audio;
using SFML.Graphics;

namespace ChendiAdventures
{
    public class GameMachine : Entity
    {
        public GameMachine(float x, float y, Texture texture, View view) : base(x, y, texture)
        {
            SetTextureRectangle(0, 0, 64, 128);
            Item = new Block(-100, y + 16, RewardsTexture);
            Item.SetTextureRectangle(0, 0);
            _reward = new TextLine("", 10, 1000, 0, Color.Green);
            _reward.SetOutlineThickness(1);
            _rnd = new Random(432);
            _loss = 0;
            Click = new Sound(new SoundBuffer(@"sfx/click.wav"));
            Click.Volume = 30;
            LootedReward = Reward.Nothing;
            _view = view;
        }

        public Block Item { get; set; }

        public Reward LootedReward { get; private set; }

        public void Roll()
        {
            var tmp = LootedReward;

            Item.X = X + 16;
            Item.Y = Y + 16;

            _loss = _rnd.Next(300);
            Click.Play();

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
            TextureUpdate();
        }

        public void GrantReward(MainCharacter character)
        {
            _reward.MoveText(_view.Center.X - _view.Size.X / 2 - 300, _view.Center.Y + 180);
            _reward.EditText("");

            switch (LootedReward)
            {
                case Reward.Nothing:
                {
                    Creature.sKill.Play();
                    _reward.EditText("YOU HAVE NOT WON ANYTHING...");
                    break;
                }
                case Reward.Coins10:
                {
                    character.sCoin.Play();
                    character.Coins += 10;
                    _reward.EditText("YOU HAVE WON 10 COINS");
                    break;
                }
                case Reward.Coins100:
                {
                    character.sCoin.Play();
                    character.Coins += 100;
                    _reward.EditText("YOU HAVE WON 100 COINS");
                    break;
                }
                case Reward.Coins1000:
                {
                    character.sCoin.Play();
                    character.Coins += 1000;
                    _reward.EditText("YOU HAVE WON 1000 COINS!");
                    break;
                }
                case Reward.Jackpot:
                {
                    MainGameWindow.Victory.Play();
                    character.Coins += 10000;
                    _reward.EditText("JACKPOT! YOU HAVE WON 10000 COINS");
                    break;
                }

                case Reward.Arrow3:
                {
                    character.sPickup.Play();
                    character.ArrowAmount += 1;
                    _reward.EditText("YOU HAVE WON 1 ARROW");
                    break;
                }
                case Reward.Arrow10:
                {
                    character.sPickup.Play();
                    character.ArrowAmount += 3;
                    _reward.EditText("YOU HAVE WON 3 ARROWS");
                    break;
                }
                case Reward.Arrow25:
                {
                    character.sPickup.Play();
                    character.ArrowAmount += 5;
                    _reward.EditText("YOU HAVE WON 5 ARROWS");
                    break;
                }

                case Reward.Mana3:
                {
                    character.sPickup.Play();
                    character.Mana += 1;
                    _reward.EditText("YOU HAVE WON 1 MANA POTION");
                    break;
                }
                case Reward.Mana10:
                {
                    character.sPickup.Play();
                    character.Mana += 2;
                    _reward.EditText("YOU HAVE WON 2 MANA POTIONS");
                    break;
                }
                case Reward.Mana25:
                {
                    character.sPickup.Play();
                    character.Mana += 3;
                    _reward.EditText("YOU HAVE WON 3 MANA POTIONS");
                    break;
                }

                case Reward.Score10000:
                {
                    character.sPickup.Play();
                    character.Score += 50000;
                    _reward.EditText("WOW! YOU HAVE WON 50000 POINTS!");
                    break;
                }
                case Reward.Life:
                {
                    character.sLife.Play();
                    character.Lives++;
                    _reward.EditText("YOU HAVE WON 1 LIFE");
                    break;
                }
                case Reward.TripleLife:
                {
                    character.sLife.Play();
                    character.Lives += 3;
                    _reward.EditText("YOU HAVE WON 3 LIVES");
                    break;
                }
            }
        }

        public void TextureUpdate() // tu zmienic na switch bo bez sensu to to
        {
            if (_loss >= 0 && _loss <= 49)
                Item.SetTextureRectangle(0, 32);
            else if (_loss >= 50 && _loss <= 99)
                Item.SetTextureRectangle(0, 96);
            else if (_loss >= 100 && _loss <= 149)
                Item.SetTextureRectangle(0, 64);
            else if (_loss >= 150 && _loss <= 169)
                Item.SetTextureRectangle(32, 32);
            else if (_loss >= 170 && _loss <= 189)
                Item.SetTextureRectangle(32, 96);
            else if (_loss >= 190 && _loss <= 209)
                Item.SetTextureRectangle(32, 64);
            else if (_loss >= 210 && _loss <= 219)
                Item.SetTextureRectangle(64, 32);
            else if (_loss >= 220 && _loss <= 229)
                Item.SetTextureRectangle(64, 96);
            else if (_loss >= 230 && _loss <= 239)
                Item.SetTextureRectangle(64, 64);
            else if (_loss >= 240 && _loss <= 242)
                Item.SetTextureRectangle(0, 128);
            else if (_loss >= 243 && _loss <= 245)
                Item.SetTextureRectangle(0, 160);
            else if (_loss == 246)
                Item.SetTextureRectangle(0, 192);
            else if (_loss == 247)
                Item.SetTextureRectangle(32, 128);
            else
                Item.SetTextureRectangle(0, 0);
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

            if (_reward.X < X - _reward.Width / 2) _reward.MoveText(_reward.X + 15, _reward.Y);
        }

        public override void Draw(RenderTarget target, RenderStates states)
        {
            base.Draw(target, states);
            //if (X == (int)MainView.Center.X - 32)
            {
                target.Draw(Item);
                target.Draw(_reward);
            }
        }

        private readonly Random _rnd;
        private readonly View _view;
        public static Sound Click;
        private int _loss;
        private readonly TextLine _reward;
    }
}