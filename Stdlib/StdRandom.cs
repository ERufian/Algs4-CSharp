//-----------------------------------------------------------------------
// <copyright file="StdRandom.cs" company="Eusebio Rufian-Zilbermann">
//   Copyright (c) Eusebio Rufian-Zilbermann for the C# implementation
//   partially based on materials published by Robert Sedgewick and Kevin Wayne
// </copyright>
//-----------------------------------------------------------------------
namespace Stdlib
{
   using System;
   using System.Collections.Generic;
   using System.Linq;
   using System.Text;
   using System.Threading.Tasks;

   /// <summary>
   /// <i>Standard random</i>. This class provides methods for generating
   /// random number from various distributions.
   /// <para/>
   /// For additional documentation, see <a href="http://introcs.cs.princeton.edu/22library">Section 2.2</a> of
   /// <i>Introduction to Programming in Java: An Interdisciplinary Approach</i> by Robert Sedgewick and Kevin Wayne.
   /// </summary>
   public static class StdRandom
   {
      /// <summary>
      /// Seed value.
      /// </summary>
      private static long seed;

      /// <summary>
      /// Pseudo random number generator.
      /// </summary>
      private static Random pseudoRandom = new Random();

      /// <summary>
      /// Cryptographically strong random number generator.
      /// </summary>
      private static System.Security.Cryptography.RNGCryptoServiceProvider rngCsp =
         new System.Security.Cryptography.RNGCryptoServiceProvider();

      /// <summary>
      /// Control usage of cryptographically strong or pseudo random number generation.
      /// </summary>
      private static bool disableCryptoStrength; // = false

      /// <summary>
      /// Initializes static members of the <see cref="StdRandom"/> class.
      /// </summary>
      static StdRandom()
      {
         // If we just use the default Random() constructor we won't know the seed
         // that is why we need to reinstantiate it
         seed = pseudoRandom.Next(int.MaxValue);
         pseudoRandom = new Random((int)seed);
      }

      /// <summary>
      /// Gets or sets a value indicating whether to use 
      /// a pseudo random number generator instead of a cryptographically strong one.
      /// </summary>
      /// <remarks>
      /// Pseudo random is faster (around 100 times on current hardware) 
      /// and allows repeatability when a seed is specified,
      /// but it should never be used when processing untrusted input
      /// (input produced by an external source that is potentially malicious).
      /// By default cryptographic strength randomness is enabled.
      /// </remarks>
      public static bool DisableCryptoStrength
      {
         get
         {
            return StdRandom.disableCryptoStrength;
         }

         set
         {
            StdRandom.disableCryptoStrength = value;
         }
      }

      /// <summary>
      /// Re-initialize the pseudo random generator with a specific seed.
      /// Note that the seed will be truncated into an 32 bit integer.
      /// </summary>
      /// <param name="seed">Seed value.</param>
      public static void SetSeed(long seed)
      {
         StdRandom.seed = seed;
         pseudoRandom = new Random(unchecked((int)seed));
      }

      /// <summary>
      /// Get the current seed.
      /// </summary>
      /// <returns>Seed value.</returns>
      public static long GetSeed()
      {
         return seed;
      }

      /// <summary>
      /// Produce random numbers that follow a uniform distribution in the [0, 1) range.
      /// </summary>
      /// <returns>A random integer uniformly in [0, 1).</returns>
      public static double Uniform()
      {
         if (DisableCryptoStrength)
         {
            return pseudoRandom.NextDouble();
         }

         byte[] result = new byte[8];
         ulong uniformUnsignedLong;
         do
         {
            rngCsp.GetBytes(result);
            uniformUnsignedLong = BitConverter.ToUInt64(result, 0);
         }
         while (uniformUnsignedLong == ulong.MaxValue); // loop if we would return a 1

         return (double)uniformUnsignedLong / (double)ulong.MaxValue;
      }

      /// <summary>
      /// Produce random numbers that follow a uniform distribution in a [0, maxValue) range.
      /// </summary>
      /// <param name="maxValue">The upper bound for the distribution.</param>
      /// <returns>An integer uniformly in the [0, maxValue) range.</returns>
      /// <exception cref="ArgumentException">Thrown if maxValue is 0 or negative.</exception>
      public static int Uniform(int maxValue)
      {
         if (maxValue <= 0)
         {
            throw new ArgumentException("Parameter N must be positive");
         }

         if (DisableCryptoStrength)
         {
            return pseudoRandom.Next(maxValue);
         }

         // Our desired range is the highest multiple of maxValue
         // (to preserve uniformity)
         int targetRange = int.MaxValue - (int.MaxValue % maxValue);
         int rawResult;
         byte[] result = new byte[4];
         if (0 == maxValue)
         {
            return 0;
         }

         do
         {
            rngCsp.GetBytes(result);

            // convert int.MinValue...int.MaxValue to 0...int.MaxValue
            // by setting the most significant bit to zero 
            result[BitConverter.IsLittleEndian ? 3 : 0] &= 0x7F;
            rawResult = BitConverter.ToInt32(result, 0);
         }
         while (targetRange <= rawResult); // keep looping while outside of the target range

         return (int)(rawResult % maxValue);
      }

      /// <summary>
      /// Produce random numbers that follow a uniform distribution in interval [0,1).
      /// </summary>
      /// <returns>A real number in interval [0, 1).</returns>
      [Obsolete("Deprecated, clearer to use Uniform()")]
      public static double Random()
      {
         return Uniform();
      }

      /// <summary>
      /// Produce random numbers that follow a uniform distribution in a specified interval.
      /// </summary>
      /// <param name="lowerBound">The lower bound of the distribution.</param>
      /// <param name="upperBound">The upper bound of the distribution.</param>
      /// <returns>An integer uniformly in [lowerBound, upperBound).</returns>
      /// <exception cref="ArgumentException">
      /// Thrown if the interval bounds are reversed or the interval range is larger than <code>int.MaxValue</code>.
      /// </exception>
      /// <remarks>The largest range is only half of the range possible for integer numbers.</remarks>
      public static int Uniform(int lowerBound, int upperBound)
      {
         if (upperBound <= lowerBound)
         {
            throw new ArgumentException("Invalid range");
         }

         if ((long)upperBound - lowerBound >= int.MaxValue)
         {
            throw new ArgumentException("Invalid range");
         }

         return lowerBound + Uniform(upperBound - lowerBound);
      }

      /// <summary>
      /// Produce random numbers that follow a uniform distribution in a specified interval.
      /// </summary>
      /// <param name="lowerBound">The lower bound of the distribution.</param>
      /// <param name="upperBound">The upper bound of the distribution.</param>
      /// <returns>A real number uniformly in  [lowerBound, upperBound).</returns>
      /// <exception cref="ArgumentException">
      /// Thrown if the interval bounds are reversed
      /// </exception>
      public static double Uniform(double lowerBound, double upperBound)
      {
         if (!(lowerBound < upperBound))
         {
            throw new ArgumentException("Invalid range");
         }

         return lowerBound + (Uniform() * (upperBound - lowerBound));
      }

      /// <summary>
      /// Produce random results that are true with a specified probability and false otherwise.
      /// </summary>
      /// <param name="probabilityTrue">The probability that the results will be true.</param>
      /// <returns>A boolean that is true with a probabilityTrue probability.</returns>
      /// <exception cref="ArgumentException">
      /// Thrown if the specified probability is outside the [0, 1] range.
      /// </exception>
      public static bool Bernoulli(double probabilityTrue)
      {
         if (!(probabilityTrue >= 0.0 && probabilityTrue <= 1.0))
         {
            throw new ArgumentException("Probability must be between 0.0 and 1.0");
         }

         return Uniform() < probabilityTrue;
      }

      /// <summary>
      /// Produce random results that are true with a 50% probability and false otherwise.
      /// </summary>
      /// <returns>A boolean that is true with 50% probability and false the remaining 50%.</returns>
      public static bool Bernoulli()
      {
         return Bernoulli(0.5);
      }

      /// <summary>
      /// Produce random numbers that follow a standard Gaussian distribution.
      /// </summary>
      /// <returns>A random real number that follows a standard Gaussian distribution.</returns>
      public static double Gaussian()
      {
         // use the polar form of the Box-Muller transform
         double r, x, y;
         do
         {
            x = Uniform(-1.0, 1.0);
            y = Uniform(-1.0, 1.0);
            r = (x * x) + (y * y);
         } 
         while (r >= 1 || r == 0);
         return x * Math.Sqrt(-2 * Math.Log(r) / r);

         // Remark:  y * Math.sqrt(-2 * Math.log(r) / r)
         // is an independent random gaussian
      }

      /// <summary>
      /// Produce random numbers that follow a Gaussian distribution with a specified mean and standard deviation.
      /// </summary>
      /// <param name="mean">The mean value of the distribution.</param>
      /// <param name="standardDeviation">The standard deviation of the distribution.</param>
      /// <returns>A random real number that follows a Gaussian distribution.</returns>
      public static double Gaussian(double mean, double standardDeviation)
      {
         return mean + (standardDeviation * Gaussian());
      }

      /// <summary>
      /// Produce random numbers that follow a Geometric distribution with a specified mean.
      /// </summary>
      /// <param name="probability">The mean value of the distribution.</param>
      /// <returns>A random integer that follows a Geometric distribution.</returns>
      /// <exception cref="ArgumentException">Thrown if the probability is not in the [0, 1] range.</exception>
      public static int Geometric(double probability)
      {
         if (!(probability >= 0.0 && probability <= 1.0))
         {
            throw new ArgumentException("Probability must be between 0.0 and 1.0");
         }
         
         // use algorithm given by Knuth
         return (int)Math.Ceiling(Math.Log(Uniform()) / Math.Log(1.0 - probability));
      }

      /// <summary>
      /// Produce random numbers that follow a Poisson distribution with mean lambda.
      /// </summary>
      /// <param name="lambda">The lambda value of the distribution.</param>
      /// <returns>A random integer that follows a Poisson distribution.</returns>
      /// <exception cref="ArgumentException">Thrown if the probability is not in the [0, infinity) range.</exception>
      public static int Poisson(double lambda)
      {
         if (!(lambda > 0.0))
         {
            throw new ArgumentException("Parameter lambda must be positive");
         }

         if (double.IsInfinity(lambda))
         {
            throw new ArgumentException("Parameter lambda must not be infinite");
         }
         
         // use algorithm given by Knuth
         // see http://en.wikipedia.org/wiki/Poisson_distribution
         int k = 0;
         double p = 1.0;
         double exponentialFactor = Math.Exp(-lambda);
         do
         {
            k++;
            p *= Uniform();
         } 
         while (p >= exponentialFactor);
         return k - 1;
      }

      /// <summary>
      /// Produce random numbers that follow a Pareto distribution with parameter alpha.
      /// </summary>
      /// <param name="alpha">The alpha shape parameter of the distribution.</param>
      /// <returns>A random real number that follows a Pareto distribution.</returns>
      /// <exception cref="ArgumentException">Thrown if alpha is not in the (0, infinity) range.</exception>
      public static double Pareto(double alpha)
      {
         if (!(alpha > 0.0))
         {
            throw new ArgumentException("Shape parameter alpha must be positive");
         }

         return Math.Pow(1 - Uniform(), -1.0 / alpha) - 1.0;
      }

      /// <summary>
      /// Produce random numbers that follow a Cauchy distribution.
      /// </summary>
      /// <returns>A random real number that follows a Cauchy distribution.</returns>
      public static double Cauchy()
      {
         return Math.Tan(Math.PI * (Uniform() - 0.5));
      }

      /// <summary>
      /// Produce random numbers that follow a specified discrete distribution.
      /// </summary>
      /// <param name="discreteProbabilities">The discrete probabilities for each result.</param>
      /// <returns>A random integer that follows a discrete distribution.</returns>
      /// <exception cref="ArgumentException">
      /// Thrown if one of the discrete probabilities is negative, 
      /// or if the sum of probabilities differs from 1.0 by more than 1E-14.
      /// </exception>
      public static int Discrete(double[] discreteProbabilities)
      {
         if (null == discreteProbabilities)
         {
            throw new ArgumentNullException("discreteProbabilities");
         }

         const double EPSILON = 1E-14;
         double sum = 0.0;
         for (int i = 0; i < discreteProbabilities.Length; i++)
         {
            if (!(discreteProbabilities[i] >= 0.0))
            {
               throw new ArgumentException(string.Format(
                  System.Globalization.CultureInfo.InvariantCulture, 
                  "Array entry {0} must be nonnegative: {1}", 
                  i, 
                  discreteProbabilities[i]));
            }

            sum = sum + discreteProbabilities[i];
         }

         if (sum > 1.0 + EPSILON || sum < 1.0 - EPSILON)
         {
            throw new ArgumentException(string.Format(
               System.Globalization.CultureInfo.InvariantCulture,
               "Sum of array entries does not approximately equal 1.0: {0}", 
               sum));
         }

         // the for loop may not return a value when both r is (nearly) 1.0 and when the
         // cumulative sum is less than 1.0 (as a result of floating-point roundoff error)
         while (true)
         {
            double r = Uniform();
            sum = 0.0;
            for (int i = 0; i < discreteProbabilities.Length; i++)
            {
               sum = sum + discreteProbabilities[i];
               if (sum > r)
               {
                  return i;
               }
            }
         }
      }

      /// <summary>
      /// Produce random numbers that follow an Exponential distribution with rate lambda.
      /// </summary>
      /// <param name="lambda">The lambda rate of the distribution.</param>
      /// <returns>A random integer that follows an exponential distribution.</returns>
      /// <exception cref="ArgumentException">Thrown if lambda is not greater than zero.</exception>
      public static double Exp(double lambda)
      {
         if (!(lambda > 0.0))
         {
            throw new ArgumentException("Rate lambda must be positive");
         }

         return -Math.Log(1 - Uniform()) / lambda;
      }

      /// <summary>
      /// Rearrange the elements of an array in random order.
      /// </summary>
      /// <typeparam name="T">The type of items in the array.</typeparam>
      /// <param name="items">The items to shuffle.</param>
      public static void Shuffle<T>(T[] items)
      {
         if (null == items)
         {
            throw new ArgumentNullException("items");
         }

         int itemCount = items.Length;
         for (int i = 0; i < itemCount; i++)
         {
            int r = i + Uniform(itemCount - i);     // between i and N-1
            T temp = items[i];
            items[i] = items[r];
            items[r] = temp;
         }
      }

      /// <summary>
      /// Rearrange the elements of a sub-array in random order.
      /// </summary>
      /// <typeparam name="T">The type of items in the array.</typeparam>
      /// <param name="items">The items to shuffle.</param>
      /// <param name="lowIndex">Lower bound of the sub-array.</param>
      /// <param name="highIndex">Higher bound of the sub-array.</param>
      /// <exception cref="ArgumentException">
      /// Thrown if the sub-array bounds are reversed or outside the array.
      /// </exception>
      public static void Shuffle<T>(T[] items, int lowIndex, int highIndex)
      {
         if (null == items)
         {
            throw new ArgumentNullException("items");
         }

         if (lowIndex < 0 || lowIndex > highIndex || highIndex >= items.Length)
         {
            throw new ArgumentException("Illegal sub-array range");
         }

         for (int i = lowIndex; i <= highIndex; i++)
         {
            int r = i + Uniform(highIndex - i + 1);     // between i and hi
            T temp = items[i];
            items[i] = items[r];
            items[r] = temp;
         }
      }
   }
}