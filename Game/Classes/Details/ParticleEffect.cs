using System;
using System.Collections.Generic;
using SFML.Graphics;
using SFML.System;

namespace ChendiAdventures
{
    public class ParticleEffect : Drawable
    {
        public readonly List<Particle> _particles;
        private readonly Random _rnd;
        public Clock Timer;

        public ParticleEffect(float x, float y, Color color, int howMany = 30)
        {
            _rnd = new Random();
            Timer = new Clock();
            _particles = new List<Particle>();
            for (var i = 0; i < howMany; i++)
                _particles.Add(new Particle((float) _rnd.Next(400) / 100 - 2, -1 * (float) (_rnd.Next(400) / 100 + 2),
                    _rnd.Next(3200) / 100 + x, _rnd.Next(3200) / 100 + y,
                    color));
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            foreach (var particle in _particles) target.Draw(particle, states);
        }

        public void MakeParticles()
        {
            foreach (var particle in _particles) particle.Update();
        }
    }
}