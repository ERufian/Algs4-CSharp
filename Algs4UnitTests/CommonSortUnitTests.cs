//-----------------------------------------------------------------------
// <copyright file="CommonSortUnitTests.cs" company="Eusebio Rufian-Zilbermann">
//   Copyright (c) Eusebio Rufian-Zilbermann for the C# implementation
//   based on materials published by Robert Sedgewick and Kevin Wayne
// </copyright>
//-----------------------------------------------------------------------
namespace Algs4UnitTests
{
   using System;
   using System.Diagnostics.Contracts;
   using Algs4;
   using Microsoft.VisualStudio.TestTools.UnitTesting;
   using Stdlib;

   /// <summary>
   /// Common methods for testing Sort classes.
   /// </summary>
   public static class CommonSortUnitTests
   {
      /// <summary>
      /// Common initialization.
      /// </summary>
      /// <remarks>
      /// When using code contracts inside automated tests it is necessary to modify
      /// the code contract failures so that they also trigger a test failure.
      /// </remarks>
      [AssemblyInitialize]
      public static void AssemblyInitialize()
      {
         Contract.ContractFailed +=
            (sender, e) =>
            {
               e.SetUnwind(); // cause code to abort after event
               Assert.Fail(e.ToString());
            };
      }

      /// <summary>
      /// Sort a small text file.
      /// </summary>
      /// <param name="streamName">The name of the file to sort.</param>
      /// <param name="sortingAlgorithm">The algorithm to use for sorting.</param>
      /// <returns>The sorted items.</returns>
      public static string[] SortCommon(string streamName, ISortingAlgorithm sortingAlgorithm)
      {
         Contract.Ensures(null != Contract.Result<string[]>());
         if (null == sortingAlgorithm)
         {
            throw new ArgumentNullException("sortingAlgorithm");
         }

         string[] testItems;
         using (In inStream = new In(streamName))
         {
            testItems = inStream.ReadAllStrings();
         }

         if (null == testItems)
         {
            throw new InternalTestFailureException("No items to test");
         }

         sortingAlgorithm.Sort(testItems);
         return testItems;
      }
   }
}
