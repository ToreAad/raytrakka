using System;
using System.Collections.Generic;
using System.Text;

namespace RaytrAkkar.Raytracer
{
    public class Lambertian : IMaterial
    {
        private readonly Vec3 _albedo;

        public Lambertian(Vec3 albedo)
        {
            _albedo = albedo;
        }

        public bool Scatter(Ray rayIn, ref HitRecord rec, out Vec3 attenuation, out Ray scattered)
        {
            var target = rec.p + rec.normal + Vec3.RandomInUnitSphere();
            scattered = new Ray(rec.p, target - rec.p);
            attenuation = _albedo;
            return true;            
        }
    }
}
