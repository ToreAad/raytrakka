using System;
using System.Collections.Generic;
using System.Text;

namespace RaytrAkkar.Raytracer
{
    public interface IMaterial
    {
        bool Scatter(Ray rayIn, ref HitRecord rec, out Vec3 attenuation, out Ray scattered);
    }
}
