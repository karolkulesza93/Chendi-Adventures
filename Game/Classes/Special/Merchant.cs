using System;
using System.Collections.Generic;
using SFML.Audio;
using SFML.Graphics;

namespace ChendiAdventures
{
    public class Merchant : Entity
    {
        public static Sound sBuy = new Sound(new SoundBuffer(@"sfx/buy.wav"));
        public readonly Dictionary<Wares, int> ItemsCount;
        public readonly Dictionary<Wares, int> ItemsPrices;

        public TextLine Item1 = new TextLine("", 10, -100, -100, Color.White);
        public TextLine Item2 = new TextLine("", 10, -100, -100, Color.White);
        public TextLine Item3 = new TextLine("", 10, -100, -100, Color.White);
        public TextLine Item4 = new TextLine("", 10, -100, -100, Color.White);
        public TextLine MerchantTalk;

        public Merchant(float x, float y, Texture texture, MainCharacter character) : base(x, y, texture)
        {
            _character = character;
            _rnd = new Random();

            MerchantTalk = new TextLine("HELLO! WELCOME TO MY SHOP!", 10, -100, -100, Color.Cyan);
            MerchantTalk.SetOutlineThickness(3);

            ItemsPrices = new Dictionary<Wares, int>
            {
                {Wares.Arrow, 25},
                {Wares.Mana, 40},
                {Wares.Life, 100},
                {Wares.Score1000, 10}
            };

            ItemsCount = new Dictionary<Wares, int>
            {
                {Wares.Arrow, _rnd.Next(10) + 1},
                {Wares.Mana, _rnd.Next(5) + 1},
                {Wares.Life, _rnd.Next(3) + 1},
                {Wares.Score1000, 1000}
            };
        }

        public void ShopUpdate(int choice)
        {
            MerchantTalk.MoveText(X, Y + 130);

            Item1.MoveText(X, Y + 150);
            Item2.MoveText(X, Y + 162);
            Item3.MoveText(X, Y + 174);
            Item4.MoveText(X, Y + 186);

            Item1.EditText(string.Format("- ARROW '{0}' : {1} COIN:EA", ItemsCount[Wares.Arrow],
                ItemsPrices[Wares.Arrow]));
            Item2.EditText(string.Format("- MANA '{0}' : {1} COIN:EA", ItemsCount[Wares.Mana],
                ItemsPrices[Wares.Mana]));
            Item3.EditText(string.Format("- LIFE '{0}' : {1} COIN:EA", ItemsCount[Wares.Life],
                ItemsPrices[Wares.Life]));
            Item4.EditText(string.Format("- 1000 POINTS '{0}' : {1} COIN:EA", ItemsCount[Wares.Score1000],
                ItemsPrices[Wares.Score1000]));

            Item1.ChangeColor(Color.White);
            Item2.ChangeColor(Color.White);
            Item3.ChangeColor(Color.White);
            Item4.ChangeColor(Color.White);

            switch (choice)
            {
                case 1:
                {
                    Item1.ChangeColor(Color.Green);
                    break;
                }
                case 2:
                {
                    Item2.ChangeColor(Color.Green);
                    break;
                }
                case 3:
                {
                    Item3.ChangeColor(Color.Green);
                    break;
                }
                case 4:
                {
                    Item4.ChangeColor(Color.Green);
                    break;
                }
            }
        }

        public void SellWares(int choice)
        {
            var ware = Wares.Arrow;
            switch (choice)
            {
                case 1:
                {
                    ware = Wares.Arrow;
                    break;
                }
                case 2:
                {
                    ware = Wares.Mana;
                    break;
                }
                case 3:
                {
                    ware = Wares.Life;
                    break;
                }
                case 4:
                {
                    ware = Wares.Score1000;
                    break;
                }
            }


            if (_character.Coins < ItemsPrices[ware])
            {
                MerchantTalk.EditText("SORRY, YOU DO NOT HANE ENOUGH GOLD...");
                Creature.sKill.Play();
                return;
            }

            if (ItemsCount[ware] <= 0)
            {
                MerchantTalk.EditText("SORRY, THIS ITEM IS OUT OF STOCK...");
                Creature.sKill.Play();
                return;
            }

            switch (ware)
            {
                case Wares.Arrow:
                {
                    _character.ArrowAmount++;
                    _character.Coins -= ItemsPrices[ware];
                    ItemsCount[ware]--;
                    sBuy.Play();
                    break;
                }
                case Wares.Mana:
                {
                    _character.Mana++;
                    _character.Coins -= ItemsPrices[ware];
                    ItemsCount[ware]--;
                    sBuy.Play();
                    break;
                }
                case Wares.Life:
                {
                    _character.Lives++;
                    _character.Coins -= ItemsPrices[ware];
                    ItemsCount[ware]--;
                    sBuy.Play();
                    break;
                }
                case Wares.Score1000:
                {
                    _character.Score += 1000;
                    _character.Coins -= ItemsPrices[ware];
                    ItemsCount[ware]--;
                    sBuy.Play();
                    break;
                }
            }

            MerchantTalk.EditText("THANK YOU VERY MUCH!");
        }

        public override void Draw(RenderTarget target, RenderStates states)
        {
            base.Draw(target, states);
            target.Draw(MerchantTalk);

            target.Draw(Item1);
            target.Draw(Item2);
            target.Draw(Item3);
            target.Draw(Item4);
        }

        private readonly MainCharacter _character;
        private readonly Random _rnd;
    }
}