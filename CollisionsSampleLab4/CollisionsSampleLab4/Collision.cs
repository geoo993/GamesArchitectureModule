using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CollisionsSampleLab4.MacOS
{
    public class CollisionComparer : IEqualityComparer<Collision>
    {
        public bool Equals(Collision a, Collision b)
        {
            if ((a == null) || (b == null))
            {
                return false;
            }

            return a.Equals(b);
        }

        public int GetHashCode(Collision a)
        {
            return a.GetHashCode();
        }
    }

    public class Collision
    {
		public Collidable A;
        public Collidable B;

        public Collision(Collidable a, Collidable b)
        {
            A = a;
            B = b;
        }
    }
}
