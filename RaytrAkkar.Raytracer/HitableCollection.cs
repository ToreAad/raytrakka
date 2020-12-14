using System;
using System.Collections.Generic;
using System.Text;

namespace RaytrAkkar.Raytracer
{
    public class HitableCollection : IHitable
    {
        public bool Hit(Ray r, double tMin, double tMax, ref HitRecord rec)
        {
            var tempRec = new HitRecord();
            var hitAnything = false;
            var closestSoFar = tMax;
            foreach (IHitable item in List)
            {
                if (item.Hit(r, tMin, closestSoFar, ref tempRec))
                {
                    hitAnything = true;
                    closestSoFar = tempRec.t;
                    rec = tempRec;
                }
            }

            return hitAnything;
        }

        public List<IHitable> List { get; } = new List<IHitable>();
    }
}
