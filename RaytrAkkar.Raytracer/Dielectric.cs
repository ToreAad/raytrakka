using System;
using System.Collections.Generic;
using System.Text;

namespace RaytrAkkar.Raytracer
{
    public class Dielectric : IMaterial
    {
        private readonly double _refIdx;

        public Dielectric(double refIdx)
        {
            _refIdx = refIdx;
        }

        private double Schlick(double cosine, double refIdx)
        {
            var r0 = (1 - refIdx) / (1 + refIdx);
            r0 *= r0;
            return r0 + (1 - r0) * Math.Pow(1 - cosine, 5);
        }

        public bool Scatter(Ray rayIn, ref HitRecord rec, out Vec3 attenuation, out Ray scattered)
        {
            Vec3 outwardNormal;
            var reflected = Vec3.Reflect(rayIn.Direction, rec.normal);
            double niOverNt;
            attenuation = new Vec3(1.0, 1.0, 1.0);
            double cosine;
            if(rayIn.Direction.Dot(rec.normal) > 0)
            {
                outwardNormal = -rec.normal;
                niOverNt = _refIdx;
                cosine = _refIdx * rayIn.Direction.Dot(rec.normal) / rayIn.Direction.Length();
            }
            else
            {
                outwardNormal = rec.normal;
                niOverNt = 1.0 / _refIdx;
                cosine = -rayIn.Direction.Dot(rec.normal) / rayIn.Direction.Length();
            }
            double reflectProb;
            if (Vec3.Refract(rayIn.Direction, outwardNormal, niOverNt, out var refracted))
            {
                reflectProb = Schlick(cosine, _refIdx);   
            }
            else
            {
                reflectProb = 1.0;
            }
            var rnd = new System.Random();
            if(rnd.NextDouble() < reflectProb)
            {
                scattered = new Ray(rec.p, reflected);
            }
            else
            {
                scattered = new Ray(rec.p, refracted);
            }
            return true;

        }
    }
}
