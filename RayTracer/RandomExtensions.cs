using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace RayTracer
{
    /// <summary>
    /// A Random class to be used with multi-thread appliction
    /// </summary>
    public static class StaticRandom
    {
        static int seed = Environment.TickCount;

        static readonly ThreadLocal<Random> random = new ThreadLocal<Random>(() => new Random(Interlocked.Increment(ref seed)));

        /// <summary>
        /// Get a random number
        /// </summary>
        /// <returns></returns>
        public static int Rand()
        {
            return random.Value.Next();
        }

        /// <summary>
        ///   Generates normally distributed numbers. 
        ///   Each operation makes two Gaussians for the price of one.
        /// </summary>
        /// <param name="r"></param>
        /// <param name = "mu">Mean of the distribution</param>
        /// <param name = "sigma">Standard deviation</param>
        /// <returns></returns>
        public static double NextGaussian(double mu = 0, double sigma = 1)
        {
            var u1 = random.Value.NextDouble();
            var u2 = random.Value.NextDouble();

            var rand_std_normal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);

            var rand_normal = mu + sigma * rand_std_normal;

            return rand_normal;
        }

        /// <summary>
        /// Random vector in unit sphere
        /// </summary>
        public static Vector3D RandomVectorInUnitSphere()
        {
            var x = NextGaussian();
            var y = NextGaussian();
            var z = NextGaussian();

            var randomLocation = new Vector3D(x, y, z);
            randomLocation.Normalize();

            return randomLocation;
        }

        /// <summary>
        /// Random vector in a determined sphere
        /// </summary>
        /// <param name="location">Sphere center location</param>
        /// <param name="radius">Sphere radius</param>
        /// <returns>Random vector in the given sphere</returns>
        public static Vector3D RandomVectorInSphere(Vector3D location, double radius)
        {
            return radius * RandomVectorInUnitSphere() + location;
        }
    }
}
