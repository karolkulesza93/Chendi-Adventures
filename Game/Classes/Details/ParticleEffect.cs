using System;
using System.Collections.Generic;
using SFML.Graphics;
using SFML.System;

namespace Game
{
    public class ParticleEffect : Drawable
    {
        public static int ParticleCount = 15;
        public readonly List<Particle> _particles;
        private readonly Random _rnd;
        public Clock Timer;

        public ParticleEffect(float x, float y, Color color)
        {
            _rnd = new Random();
            Timer = new Clock();
            _particles = new List<Particle>();
            for (var i = 0; i < ParticleCount; i++)
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