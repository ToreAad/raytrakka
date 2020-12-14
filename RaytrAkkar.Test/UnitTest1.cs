using NUnit.Framework;
using RaytrAkkar.Common;

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
    }
}