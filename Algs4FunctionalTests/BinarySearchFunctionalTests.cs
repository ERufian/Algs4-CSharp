//-----------------------------------------------------------------------
// <copyright file="BinarySearchFunctionalTests.cs" company="Eusebio Rufian-Zilbermann">
//   Copyright (c) Eusebio Rufian-Zilbermann for the C# implementation
//   based on materials published by Robert Sedgewick and Kevin Wayne
// </copyright>
//-----------------------------------------------------------------------
namespace Algs4FunctionalTests
{
   using System;
   using Algs4;
   using Microsoft.VisualStudio.TestTools.UnitTesting;
   using Stdlib;

   /// <summary>
   /// Functional tests for the Binary Search class
   /// </summary>
   [TestClass]
   public class BinarySearchFunctionalTests
   {
      /// <summary>
      /// Test with a large file.
      /// </summary>
      /// <remarks>
      /// This test takes a long time (a full minute on a fast machine)
      /// that is why it shouldn't be a unit test.
      /// </remarks>
      [TestCategory("Functional")]
      [TestMethod]
      [Ignore]
      public void BinarySearchLarge()
      {
         int[] expectedNotFound = { 499569, 984875, 295754, 207807, 140925, 161828 };
         int expectedIndex = 0;
         int[] whiteList;

         // read the integers from a file and sort them
         using (In inWhiteList = new In("largeW.txt"))
         {
            whiteList = inWhiteList.ReadAllInts();
         }

         Array.Sort(whiteList);

         // read integer keys from a file; search in whitelist
         using (In inKeys = new In("largeT.txt"))
         {
            while (!inKeys.IsEmpty())
            {
               int key = inKeys.ReadInt();
               int position = BinarySearch.Rank(key, whiteList);
               if (-1 == position)
               {
                  if (expectedNotFound.Length > expectedIndex)
                  {
                     Assert.AreEqual(expectedNotFound[expectedIndex], key);
                  }

                  expectedIndex++;
               }
            }
         }

         Assert.AreEqual(expectedIndex, 367966);
      }
   }
}
