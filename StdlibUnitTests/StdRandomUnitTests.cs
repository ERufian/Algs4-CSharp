//-----------------------------------------------------------------------
// <copyright file="StdRandomUnitTests.cs" company="Eusebio Rufian-Zilbermann">
//   Copyright (c) Eusebio Rufian-Zilbermann for the C# implementation.
// </copyright>
//-----------------------------------------------------------------------
namespace StdlibUnitTests
{
   using System;
   using Microsoft.VisualStudio.TestTools.UnitTesting;
   using Stdlib;

   /// <summary>
   /// Unit Tests for the <code>StdRandom</code> class.
   /// </summary>
   [TestClass]
   public class StdRandomUnitTests
   {
      /// <summary>
      /// Check that we get the expected sequence with a certain seed.
      /// </summary>
      [TestMethod]
      public void RandomEqual()
      {
         StdRandom.DisableCryptoStrength = true; // We need repeatable pseudo random generation

         int sequenceLength = 5;
         long seed = 1316600602069;
         int[] expectedUniform1 = 
         { 
            99, 2, 68, 46, 56 
         };
         
         double[] expectedUniform2 = 
         { 
            67.8439312706906, 74.69504380072236, 54.353673830327423, 63.355658009813943, 79.770092458357141 
         };
         
         bool[] expectedBernouilli = 
         { 
            true, false, false, false, false 
         };
         
         double[] expectedGaussian = 
         { 
            8.8603941333819165, 8.8060159917070191, 9.1960408855354352, 8.941908468343053, 9.1652641351561481 
         };
         
         int[] expectedDiscrete = 
         { 
            0, 0, 1, 0, 0 
         };

         StdRandomUnitTests.TestCommonEqual(
            sequenceLength, 
            seed, 
            expectedUniform1, 
            expectedUniform2, 
            expectedBernouilli, 
            expectedGaussian, 
            expectedDiscrete);
      }

      /// <summary>
      /// Check that we get a different sequence when changing the seed.
      /// </summary>
      [TestMethod]
      public void RandomDifferentPseudo()
      {
         StdRandom.DisableCryptoStrength = true; // We need repeatable pseudo random generation

         int sequenceLength = 5;
         long seed = 1316600602060;
         int[] expectedUniform1 = 
         { 
            99, 2, 68, 46, 56 
         };

         double[] expectedUniform2 = 
         { 
            67.8439312706906, 74.69504380072236, 54.353673830327423, 63.355658009813943, 79.770092458357141 
         };

         bool[] expectedBernouilli = 
         { 
            true, false, false, false, false 
         };
         
         double[] expectedGaussian = 
         { 
            8.8603941333819165, 8.8060159917070191, 9.1960408855354352, 8.941908468343053, 9.1652641351561481 
         };
         
         int[] expectedDiscrete = 
         { 
            0, 0, 1, 0, 0 
         };

         StdRandomUnitTests.TestCommonDifferent(
            sequenceLength, 
            seed, 
            expectedUniform1, 
            expectedUniform2, 
            expectedBernouilli, 
            expectedGaussian, 
            expectedDiscrete);
      }

      /// <summary>
      /// Check that we get a different sequence with the same seed (ignored) and non-pseudo random generation.
      /// </summary>
      [TestMethod]
      public void RandomDifferentCrypto()
      {
         StdRandom.DisableCryptoStrength = false;

         int sequenceLength = 5;
         long seed = 1316600602069;
         int[] expectedUniform1 = 
         { 
            99, 2, 68, 46, 56 
         };

         double[] expectedUniform2 = 
         { 
            67.8439312706906, 74.69504380072236, 54.353673830327423, 63.355658009813943, 79.770092458357141 
         };

         bool[] expectedBernouilli = 
         { 
            true, false, false, false, false 
         };
         
         double[] expectedGaussian = 
         { 
            8.8603941333819165, 8.8060159917070191, 9.1960408855354352, 8.941908468343053, 9.1652641351561481 
         };
         
         int[] expectedDiscrete = 
         { 
            0, 0, 1, 0, 0 
         };

         StdRandomUnitTests.TestCommonDifferent(
            sequenceLength, 
            seed, 
            expectedUniform1, 
            expectedUniform2, 
            expectedBernouilli, 
            expectedGaussian, 
            expectedDiscrete);
      }

      /// <summary>
      /// Calculate a sequence of pseudo-random numbers in different distributions.
      /// </summary>
      /// <remarks>
      /// This is meant to be run in the debugger for calculating 
      /// the expected results for a given seed, it is not really a Unit Test.
      /// </remarks>
      [TestMethod]
      public void CalculatePseudorandom()
      {
         StdRandom.DisableCryptoStrength = false;

         int sequenceLength = 5;
         long seed = 1316600602069;
         int[] expectedUniform1;
         double[] expectedUniform2;
         bool[] expectedBernouilli;
         double[] expectedGaussian;
         int[] expectedDiscrete;
         StdRandomUnitTests.TestCommonCalculate(
            sequenceLength, 
            seed, 
            out expectedUniform1, 
            out expectedUniform2, 
            out expectedBernouilli, 
            out expectedGaussian, 
            out expectedDiscrete);
      }

      /// <summary>
      /// Common tests for verifying that pseudo random results equal the expected values.
      /// </summary>
      /// <param name="sequenceLength">Length of random sequence.</param>
      /// <param name="seed">Pseudo random seed.</param>
      /// <param name="expectedUniform1">Expected uniform values (integer).</param>
      /// <param name="expectedUniform2">Expected uniform values (real).</param>
      /// <param name="expectedBernoulli">Expected Bernoulli values.</param>
      /// <param name="expectedGaussian">Expected Gaussian values.</param>
      /// <param name="expectedDiscrete">Expected Discrete values.</param>
      private static void TestCommonEqual(
         int sequenceLength, 
         long seed, 
         int[] expectedUniform1, 
         double[] expectedUniform2, 
         bool[] expectedBernoulli, 
         double[] expectedGaussian, 
         int[] expectedDiscrete)
      {
         double[] t = { .5, .3, .1, .1 };

         StdRandom.SetSeed(seed);
         for (int i = 0; i < sequenceLength; i++)
         {
            Assert.AreEqual(expectedUniform1[i], StdRandom.Uniform(100));
            Assert.AreEqual(expectedUniform2[i], StdRandom.Uniform(10.0, 99.0));
            Assert.AreEqual(expectedBernoulli[i], StdRandom.Bernoulli(.5));
            Assert.AreEqual(expectedGaussian[i], StdRandom.Gaussian(9.0, .2));
            Assert.AreEqual(expectedDiscrete[i], StdRandom.Discrete(t));
         }
      }

      /// <summary>
      /// Common tests for verifying that random results are different from a set of values.
      /// </summary>
      /// <param name="sequenceLength">Length of random sequence.</param>
      /// <param name="seed">Pseudo random seed.</param>
      /// <param name="expectedUniform1">Expected uniform values (integer).</param>
      /// <param name="expectedUniform2">Expected uniform values (real).</param>
      /// <param name="expectedBernoulli">Expected Bernoulli values.</param>
      /// <param name="expectedGaussian">Expected Gaussian values.</param>
      /// <param name="expectedDiscrete">Expected Discrete values.</param>
      private static void TestCommonDifferent(
         int sequenceLength, 
         long seed, 
         int[] expectedUniform1, 
         double[] expectedUniform2, 
         bool[] expectedBernoulli, 
         double[] expectedGaussian, 
         int[] expectedDiscrete)
      {
         double[] t = { .5, .3, .1, .1 };

         bool differentBernoulliFound = false;
         bool differentDiscreteFound = false;

         StdRandom.SetSeed(seed);
         for (int i = 0; i < sequenceLength; i++)
         {
            Assert.AreNotEqual(expectedUniform1[i], StdRandom.Uniform(100));
            Assert.AreNotEqual(expectedUniform2[i], StdRandom.Uniform(10.0, 99.0));
            if (expectedBernoulli[i] != StdRandom.Bernoulli(.5))
            {
               differentBernoulliFound = true;
            }

            Assert.AreNotEqual(expectedGaussian[i], StdRandom.Gaussian(9.0, .2));
            if (expectedDiscrete[i] != StdRandom.Discrete(t))
            {
               differentDiscreteFound = true;
            }
         }

         // Bernouilli and Discrete have a high probability of coincidence
         // by checking the entire sequence at least we reduce the number of random failures
         Assert.IsTrue(differentBernoulliFound);
         Assert.IsTrue(differentDiscreteFound);
      }

      /// <summary>
      /// Utility method tests for calculating pseudo random results for a given seed.
      /// </summary>
      /// <param name="sequenceLength">Length of random sequence.</param>
      /// <param name="seed">Pseudo random seed.</param>
      /// <param name="calculatedUniform1">Expected uniform values (integer).</param>
      /// <param name="calculatedUniform2">Expected uniform values (real).</param>
      /// <param name="calculatedBernouilli">Expected Bernoulli values.</param>
      /// <param name="calculatedGaussian">Expected Gaussian values.</param>
      /// <param name="calculatedDiscrete">Expected Discrete values.</param>
      private static void TestCommonCalculate(
         int sequenceLength, 
         long seed,
         out int[] calculatedUniform1,
         out double[] calculatedUniform2,
         out bool[] calculatedBernouilli,
         out double[] calculatedGaussian,
         out int[] calculatedDiscrete)
      {
         StdRandom.SetSeed(seed);

         double[] t = { .5, .3, .1, .1 };

         calculatedUniform1 = new int[sequenceLength];
         calculatedUniform2 = new double[sequenceLength];
         calculatedBernouilli = new bool[sequenceLength];
         calculatedGaussian = new double[sequenceLength];
         calculatedDiscrete = new int[sequenceLength];

         for (int i = 0; i < sequenceLength; i++)
         {
            calculatedUniform1[i] = StdRandom.Uniform(100);
            calculatedUniform2[i] = StdRandom.Uniform(10.0, 99.0);
            calculatedBernouilli[i] = StdRandom.Bernoulli(.5);
            calculatedGaussian[i] = StdRandom.Gaussian(9.0, .2);
            calculatedDiscrete[i] = StdRandom.Discrete(t);
         }
      }
   }
}