using System;
using System.Collections.Generic;
using System.Text;

namespace RaytrAkkar.Raytracer
{
    public class Camera
    {
        private Vec3 _lowerLeftCorner;
        private Vec3 _horizontal;
        private Vec3 _vertical;
        private Vec3 _origin;
        private readonly double _lensRadius;

        public Camera(Vec3 lookFrom, Vec3 lookAt, Vec3 vUp,double vfov, double aspect, double aperture, double focusDist)
        {
            _lensRadius = aperture / 2;
            var theta = vfov * Math.PI / 180.0;
            var halfHeight = Math.Tan(theta / 2);
            var halfWidth = aspect * halfHeight;
            _origin = lookFrom;
            var w = (lookFrom - lookAt).MakeUnitVector();
            var u = vUp.Cross(w).MakeUnitVector();
            var v = w.Cross(u);

            _lowerLeftCorner = _origin - halfWidth * focusDist * u - halfHeight * focusDist * v - focusDist * w;
            _horizontal = 2 * halfWidth * focusDist * u;
            _vertical = 2 * halfHeight * focusDist * v;
    }

        public Ray GetRay(double u, double v)
        {
            var rd = _lensRadius * Vec3.RandomInUnitSphere();
            var offset = u * rd.X + v * rd.Y;
            return new Ray(_origin+offset, _lowerLeftCorner + u * _horizontal + v * _vertical -_origin -offset);
        }
            
    }
}
