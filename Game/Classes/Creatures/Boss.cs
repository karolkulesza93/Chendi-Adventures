using SFML.Graphics;
using SFML.System;

namespace ChendiAdventures
{
    public sealed class Boss : Creature
    {
        public Clock DefaultTime { get; }
        public int Health { get; set; }
        public Boss(float x, float y, Texture texture) : base(x, y, texture)
        {
            DefaultTime = new Clock();
        }

        public override void UpdateCreature()
        {

        }

        public void Attack()
        {

        }
    }
}