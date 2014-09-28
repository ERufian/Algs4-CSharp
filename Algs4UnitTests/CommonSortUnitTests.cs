//-----------------------------------------------------------------------
// <copyright file="CommonSortUnitTests.cs" company="Eusebio Rufian-Zilbermann">
//   Copyright (c) Eusebio Rufian-Zilbermann for the C# implementation
//   based on materials published by Robert Sedgewick and Kevin Wayne
// </copyright>
//-----------------------------------------------------------------------
namespace Algs4UnitTests
{
   using System;
   using Algs4;
   using Stdlib;

   /// <summary>
   /// Common methods for testing Sort classes.
   /// </summary>
   public static class CommonSortUnitTests
   {
      /// <summary>
      /// Sort a small text file.
      /// </summary>
      /// <param name="streamName">The name of the file to sort.</param>
      /// <param name="sortingAlgorithm">The algorithm to use for sorting.</param>
      /// <returns>The sorted items.</returns>
      public static string[] SortCommon(string streamName, ISortingAlgorithm sortingAlgorithm)
      {
         if (null == sortingAlgorithm)
         {
            throw new ArgumentNullException("sortingAlgorithm");
         }

         string[] testItems;
         using (In inStream = new In(streamName))
         {
            testItems = inStream.ReadAllStrings();
         }

         sortingAlgorithm.Sort(testItems);
         return testItems;
      }
   }
}
