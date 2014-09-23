//-----------------------------------------------------------------------
// <copyright file="BinarySearchUnitTests.cs" company="Eusebio Rufian-Zilbermann">
//   Copyright (c) Eusebio Rufian-Zilbermann for the C# implementation
//   based on materials published by Robert Sedgewick and Kevin Wayne
// </copyright>
//-----------------------------------------------------------------------
namespace Algs4UnitTests
{
   using System;
   using Algs4;
   using Microsoft.VisualStudio.TestTools.UnitTesting;
   using Stdlib;
   
   /// <summary>
   /// Unit Tests for the Binary Search class
   /// </summary>
   [TestClass]
   public class BinarySearchUnitTests
   {
      /// <summary>
      /// Perform a binary search on a small text file.
      /// </summary>
      [TestMethod]
      public void BinarySearchTiny()
      {
         int[] expected = { 5, -1, 0, -1, 4, 5, 15, 14, 1, 0, 9, 13, -1, 10, 15, 13, 13, 12 };
         int[] whiteList;

         // read the integers from a file and sort them
         using (In inWhiteList = new In("TinyW.txt"))
         {
            whiteList = inWhiteList.ReadAllInts();
         }
         
         Array.Sort(whiteList);

         // read integer keys from a file; search in whitelist
         using (In inKeys = new In("TinyT.txt"))
         {
            int expectedIndex = 0;
            while (!inKeys.IsEmpty())
            {
               int key = inKeys.ReadInt();
               int position = BinarySearch.Rank(key, whiteList);
               Assert.AreEqual(expected[expectedIndex++], position);
            }
         }
      }
   }
}