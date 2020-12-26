using NUnit.Framework;
using RaytrAkkar.Common;
using RaytrAkkar.Lisp;
using System;

namespace RaytrAkkar.Test
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void FlattenThenUnflatten()
        {
            var original = new byte[5, 6, 2];
            byte counter = 0;
            for(int i = 0; i<5; i++)
            {
                for(int j = 0; j<6; j++)
                {
                    for(int k = 0; k<2; k++)
                    {
                        original[i, j, k] = counter++;
                    }
                }
            }
            var flattened = original.Flatten();
            var unflattened = flattened.Unflatten(5, 6, 2);
            Assert.AreEqual(original, unflattened);
        }

        [Test]
        public void GetSceneFail()
        {
            var exp = @"(SimpleScene (World
    (Sphere(Vec3 0.0 0.0 - 1.0) 0.5(Lambertian(Vec3 0.1 0.2 0.5)))
    (Sphere(Vec3 0.0 - 100.5 - 1.0) 100(Lambertian(Vec3 0.8 0.8 0.0)))
    (Sphere(Vec3 1.0 0.0 - 1.0) 0.5(Metal(Vec3 0.8 0.8 0.0) 0))
    (Sphere(Vec3 - 1.0 0.0 - 1.0) 0.5(Dielectric 1.5))
    ) (Vec3 0.0 0.0 - 1.0) (Vec3 3.0 3.0 2.0)
    ";
            string failureMsg = "";
            try
            {
                var simpleScene = GetScene.SimpleScene(exp);
            }
            catch (Exception ex)
            {
                failureMsg = ex.ToString();
            }

            Assert.AreNotEqual("", failureMsg);
        }
    }
}


