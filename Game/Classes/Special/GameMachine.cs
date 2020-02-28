using SFML.System;
using SFML.Graphics;
using System;

namespace Game.Classes.Special
{
    public class GameMachine : Entity 
    {
        public static Texture MachineTexture = new Texture(@"img/machine.png");
        private Random _rnd;
        public Reward LootedReward { get; private set; }


        public GameMachine(float x, float y, Texture texture) : base(x,y,texture)
        {
            this.SetTextureRectanlge(0, 0, 64, 128);
            this._rnd = new Random();
            this.LootedReward = Reward.Nothing;
        }

        public void Roll()
        {
            this.LootedReward = Reward.Nothing;
            int loss = this._rnd.Next(257);

            if (loss >= 0 && loss <= 49) { this.LootedReward = Reward.Coins10; }
            else if (loss >= 50 && loss <= 99) { this.LootedReward = Reward.Mana3; }
            else if (loss >= 100 && loss <= 149) { this.LootedReward = Reward.Arrow3; }
            else if (loss >= 150 && loss <= 169) { this.LootedReward = Reward.Coins100; }
            else if (loss >= 170 && loss <= 189) { this.LootedReward = Reward.Mana10; }
            else if (loss >= 190 && loss <= 209) { this.LootedReward = Reward.Arrow10; }
            else if (loss >= 210 && loss <= 219) { this.LootedReward = Reward.Coins1000; }
            else if (loss >= 220 && loss <= 229) { this.LootedReward = Reward.Mana25; }
            else if (loss >= 230 && loss <= 239) { this.LootedReward = Reward.Arrow25; }
            else if (loss >= 240 && loss <= 242) { this.LootedReward = Reward.Life; }
            else if (loss >= 243 && loss <= 245) { this.LootedReward = Reward.Score10000; }
            else if (loss == 246) { this.LootedReward = Reward.Jackpot; }
            else { this.LootedReward = Reward.Nothing; }
        }

        public void GrantReward(MainCahracter character)
        {
            switch (this.LootedReward)
            {
                case Reward.Coins10: { character.Coins += 10; break; }
                case Reward.Coins100: { character.Coins += 100; break; }
                case Reward.Coins1000: { character.Coins += 1000; break; }
                case Reward.Jackpot: { character.Coins += 10000; break; }

                case Reward.Arrow3: { character.ArrowAmount += 3; break; }
                case Reward.Arrow10: { character.ArrowAmount += 10; break; }
                case Reward.Arrow25: { character.ArrowAmount += 25; break; }

                case Reward.Mana3: { character.Mana += 3; break; }
                case Reward.Mana10: { character.Mana += 10; break; }
                case Reward.Mana25: { character.Mana += 25; break; }

                case Reward.Score10000: { character.Score += 10000; break; }
                case Reward.Life: { character.Lives++; break; }

                default: { break; }
            }
        }
    }
}
