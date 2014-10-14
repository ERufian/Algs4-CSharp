//-----------------------------------------------------------------------
// <copyright file="IndexMaxPQUnitTests.cs" company="Eusebio Rufian-Zilbermann">
//   Copyright (c) Eusebio Rufian-Zilbermann for the C# implementation
//   based on materials published by Robert Sedgewick and Kevin Wayne
// </copyright>
//-----------------------------------------------------------------------
namespace Algs4UnitTests
{
   using System;
   using System.Collections.Generic;
   using System.Linq;
   using Algs4;
   using Microsoft.VisualStudio.TestTools.UnitTesting;

   /// <summary>
   /// Unit Tests for the IndexMaxPQ class.
   /// </summary>
   [TestClass]
   public class IndexMaxPQUnitTests
   {
      /// <summary>
      /// A set of test strings.
      /// </summary>
      private readonly string[] testStrings = { "it", "was", "the", "best", "of", "times", "it", "was", "the", "worst" };

      /// <summary>
      /// Queue and de-queue.
      /// </summary>
      [TestMethod]
      public void DequeueMaxPQTest()
      {
         // Expected: same as sorting the input and inserting it in a regular queue
         Queue<string> expectedItems = new Queue<string>(this.testStrings.OrderByDescending(s => s));
         IndexMaxPQ<string> pq = new IndexMaxPQ<string>(this.testStrings.Length);
         CommonIndexPQUnitTests.DequeuePQTest(this.testStrings, pq, expectedItems);
      }

      /// <summary>
      /// Queue and enumerate.
      /// </summary>
      [TestMethod]
      public void EnumeratorMaxPQTest()
      {
         // Expected: same as sorting the input and inserting it in a regular queue
         Queue<string> expectedItems = new Queue<string>(this.testStrings.OrderByDescending(s => s));
         IndexMaxPQ<string> pq = new IndexMaxPQ<string>(this.testStrings.Length);
         CommonIndexPQUnitTests.EnumerationPQTest(this.testStrings, pq, expectedItems);
      }

      /// <summary>
      /// Merge 3 files.
      /// </summary>
      [TestMethod]
      public void Merge3WayMaxPQTest()
      {
         string[] inputFiles = { "Algs4-Data\\m1r.txt", "Algs4-Data\\m2r.txt", "Algs4-Data\\m3r.txt" };
         Queue<string> expectedItems = new Queue<string>(
            "A B C F G I I Z B D H P Q Q A B E F J N".Split(" ".ToCharArray()).OrderByDescending(s => s));
         IndexMaxPQ<string> pq = new IndexMaxPQ<string>(expectedItems.Count);
         CommonIndexPQUnitTests.MergeMultiWay(inputFiles, pq, expectedItems);
      } 
   }
}