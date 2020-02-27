using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;

namespace Game
{
    public class ParticleEffect : Drawable
    {
        public readonly List<Particle> _particles;
        public Clock Timer;
        private Random _rnd;
        public static int ParticleCount = 15;
        public ParticleEffect(float x, float y, Color color)
        {
            this._rnd = new Random();
            this.Timer = new Clock();
            this._particles = new List<Particle>();
            for (int i=0; i<ParticleCount; i++)
            {
                this._particles.Add(new Particle((float)this._rnd.Next(400) / 100 - 2,  -1 * (float)(this._rnd.Next(400) / 100 + 2),
                    this._rnd.Next(3200)/100 + x, this._rnd.Next(3200)/100 + y,
                    color));
            }
        }
        public void MakeParticles()
        {
            foreach (Particle particle in this._particles)
            {
                particle.Update();
            }
        }
        public void Draw(RenderTarget target, RenderStates states)
        {
            foreach (Particle particle in this._particles)
            {
                target.Draw(particle, states);
            }
        }
    }
}
