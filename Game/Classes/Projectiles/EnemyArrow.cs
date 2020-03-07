using SFML.Graphics;

namespace ChendiAdventures
{
    public class EnemyArrow : Projectile
    {
        public EnemyArrow(float x, float y, Texture texture, Movement dir) : base(x, y, texture, dir)
        {
        }
    }
}