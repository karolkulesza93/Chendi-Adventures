using System;
using SFML.Graphics;

namespace Game.Classes.Special
{
    public class GameMachine : Entity
    {
        public static Texture MachineTexture = new Texture(@"img/machine.png");
        private readonly Random _rnd;


        public GameMachine(float x, float y, Texture texture) : base(x, y, texture)
        {
            SetTextureRectanlge(0, 0, 64, 128);
            _rnd = new Random();
            LootedReward = Reward.Nothing;
        }

        public Reward LootedReward { get; private set; }

        public void Roll()
        {
            LootedReward = Reward.Nothing;
            var loss = _rnd.Next(257);

            if (loss >= 0 && loss <= 49)
                LootedReward = Reward.Coins10;
            else if (loss >= 50 && loss <= 99)
                LootedReward = Reward.Mana3;
            else if (loss >= 100 && loss <= 149)
                LootedReward = Reward.Arrow3;
            else if (loss >= 150 && loss <= 169)
                LootedReward = Reward.Coins100;
            else if (loss >= 170 && loss <= 189)
                LootedReward = Reward.Mana10;
            else if (loss >= 190 && loss <= 209)
                LootedReward = Reward.Arrow10;
            else if (loss >= 210 && loss <= 219)
                LootedReward = Reward.Coins1000;
            else if (loss >= 220 && loss <= 229)
                LootedReward = Reward.Mana25;
            else if (loss >= 230 && loss <= 239)
                LootedReward = Reward.Arrow25;
            else if (loss >= 240 && loss <= 242)
                LootedReward = Reward.Life;
            else if (loss >= 243 && loss <= 245)
                LootedReward = Reward.Score10000;
            else if (loss == 246)
                LootedReward = Reward.Jackpot;
            else
                LootedReward = Reward.Nothing;
        }

        public void GrantReward(MainCharacter character)
        {
            switch (LootedReward)
            {
                case Reward.Coins10:
                {
                    character.Coins += 10;
                    break;
                }
                case Reward.Coins100:
                {
                    character.Coins += 100;
                    break;
                }
                case Reward.Coins1000:
                {
                    character.Coins += 1000;
                    break;
                }
                case Reward.Jackpot:
                {
                    character.Coins += 10000;
                    break;
                }

                case Reward.Arrow3:
                {
                    character.ArrowAmount += 3;
                    break;
                }
                case Reward.Arrow10:
                {
                    character.ArrowAmount += 10;
                    break;
                }
                case Reward.Arrow25:
                {
                    character.ArrowAmount += 25;
                    break;
                }

                case Reward.Mana3:
                {
                    character.Mana += 3;
                    break;
                }
                case Reward.Mana10:
                {
                    character.Mana += 10;
                    break;
                }
                case Reward.Mana25:
                {
                    character.Mana += 25;
                    break;
                }

                case Reward.Score10000:
                {
                    character.Score += 10000;
                    break;
                }
                case Reward.Life:
                {
                    character.Lives++;
                    break;
                }
            }
        }
    }
}