using System;
using System.Collections.Generic;
using System.Text;

namespace RaytrAkkar.Raytracer
{
    public class Metal : IMaterial
    {
        private readonly Vec3 _albedo;
        private readonly double _fuzz;

        public Metal(Vec3 albedo, double fuzz)
        {
            _albedo = albedo;
            if(fuzz< 1)
            {
                _fuzz = fuzz;
            }
            else
            {
                _fuzz = 1;
            }
        }

        public bool Scatter(Ray rayIn, ref HitRecord rec, out Vec3 attenuation, out Ray scattered)
        {
            var reflected = Vec3.Reflect(rayIn.Direction.MakeUnitVector(), rec.normal);
            scattered = new Ray(rec.p, reflected+_fuzz*Vec3.RandomInUnitSphere());
            attenuation = _albedo;
            return scattered.Direction.Dot(rec.normal) > 0;
        }
    }
}
