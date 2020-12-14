using System;
using System.Collections.Generic;
using System.Text;

namespace RaytrAkkar.Raytracer
{
    public class Vec3
    {
        private readonly double e0, e1, e2;

        public Vec3(double e0, double e1, double e2)
        {
            this.e0 = e0;
            this.e1 = e1;
            this.e2 = e2;
        }

        public double X => e0;
        public double Y => e1;
        public double Z => e2;

        public static Vec3 Reflect(Vec3 v, Vec3 normal)
        {
            return v - 2 * v.Dot(normal) * normal;
        }

        public static bool Refract(Vec3 v, Vec3 n, double niOverNt, out Vec3 refracted)
        {
            var uv = v.MakeUnitVector();
            var dt = uv.Dot(n);
            var discriminant = 1.0 - niOverNt * niOverNt * (1 - dt * dt);
            if(discriminant > 0)
            {
                refracted = niOverNt * (uv - n * dt) - n * Math.Sqrt(discriminant);
                return true;
            }
            else
            {
                refracted = null;
                return false;
            }
        }

        public double R => e0;
        public double G => e1;

        public static Vec3 RandomInUnitSphere()
        {
            var rnd = new Random();
            var x = rnd.NextDouble() - 0.5;
            var y = rnd.NextDouble() - 0.5;
            var z = rnd.NextDouble() - 0.5;
            return new Vec3(x, y, z).MakeUnitVector();
        }

        public double B => e2;


        public static Vec3 operator +(Vec3 a) => new Vec3(a.e0, a.e1, a.e2);
        public static Vec3 operator -(Vec3 a) => new Vec3(-a.e0, -a.e1, -a.e2);
        public static Vec3 operator +(Vec3 a, Vec3 b) => new Vec3(a.e0 + b.e0, a.e1 + b.e1, a.e2 + b.e2);
        public static Vec3 operator -(Vec3 a, Vec3 b) => new Vec3(a.e0 - b.e0, a.e1 - b.e1, a.e2 - b.e2);
        public static Vec3 operator *(Vec3 a, Vec3 b) => new Vec3(a.e0 * b.e0, a.e1 * b.e1, a.e2 * b.e2);
        public static Vec3 operator /(Vec3 a, Vec3 b) => new Vec3(a.e0 / b.e0, a.e1 / b.e1, a.e2 / b.e2);

        public static Vec3 operator +(Vec3 a, double b) => new Vec3(a.e0 + b, a.e1 + b, a.e2 + b);
        public static Vec3 operator -(Vec3 a, double b) => new Vec3(a.e0 - b, a.e1 - b, a.e2 - b);
        public static Vec3 operator *(Vec3 a, double b) => new Vec3(a.e0 * b, a.e1 * b, a.e2 * b);
        public static Vec3 operator /(Vec3 a, double b) => new Vec3(a.e0 / b, a.e1 / b, a.e2 / b);

        public static Vec3 operator +(double a, Vec3 b) => new Vec3(a + b.e0, a + b.e1, a + b.e2);
        public static Vec3 operator -(double a, Vec3 b) => new Vec3(a - b.e0, a - b.e1, a - b.e2);
        public static Vec3 operator *(double a, Vec3 b) => new Vec3(a * b.e0, a * b.e1, a * b.e2);
        public static Vec3 operator /(double a, Vec3 b) => new Vec3(a / b.e0, a / b.e1, a / b.e2);

        public double Length() => Math.Sqrt(e0 * e0 + e1 * e1 + e2 * e2);
        public double SquaredLength() => e0 * e0 + e1 * e1 + e2 * e2;

        public Vec3 MakeUnitVector()
        {
            double k = Length();
            return this / k;
        }

        public Vec3 Sqrt()
        {
            return new Vec3(Math.Sqrt(e0), Math.Sqrt(e1), Math.Sqrt(e2));
        }

        public double Dot(Vec3 v)
        {
            return e0 * v.e0 + e1 * v.e1 + e2 * v.e2;
        }

        public Vec3 Cross(Vec3 v)
        {
            return new Vec3(e1*v.e2 - e2*v.e1, -(e0*v.e2 - e2*v.e0), e0*v.e1 - e1*v.e0);
        }

    }
}
