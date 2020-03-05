using System.Collections.Generic;
using SFML.Graphics;
using SFML.Audio;
using SFML.System;

namespace Game
{
    public class Merchant : Entity
    {
        public readonly Dictionary<Wares, int> ItemsForSale;
        public Merchant(float x, float y, Texture texture) : base(x,y,texture)
        {
            ItemsForSale = new Dictionary<Wares, int>()
            {
                {Wares.Arrow, 100}, 
                {Wares.Mana, 150}, 
                {Wares.Life, 500}, 
                {Wares.Score1000, 250}
            };
        }
    }
}
