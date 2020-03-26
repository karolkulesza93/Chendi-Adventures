using System.Collections.Generic;
using SFML.Graphics;
using SFML.System;

namespace ChendiAdventures
{
    public sealed class Boss : Creature
    {
        public Clock DefaultTime { get; }
        public int Health { get; set; }
        public List<Vector2i> TeleportPositions;
        public Boss(float x, float y, Texture texture) : base(x, y, texture)
        {
            DefaultTime = new Clock();
            TeleportPositions = new List<Vector2i>();
        }

        public override void UpdateCreature()
        {

        }

        public void Attack()
        {

        }
    }
}