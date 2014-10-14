//-----------------------------------------------------------------------
// <copyright file="MinPQUnitTests.cs" company="Eusebio Rufian-Zilbermann">
//   Copyright (c) Eusebio Rufian-Zilbermann for the C# implementation
//   based on materials published by Robert Sedgewick and Kevin Wayne
// </copyright>
//-----------------------------------------------------------------------
namespace Algs4UnitTests
{
   using System;
   using System.Linq;
   using Algs4;
   using Microsoft.VisualStudio.TestTools.UnitTesting;

   /// <summary>
   /// Unit Tests for the MinPQ class.
   /// </summary>
   [TestClass]
   public class MinPQUnitTests
   {
      /// <summary>
      /// Test MinPQ using TinyPQ.txt
      /// </summary>
      [TestMethod]
      public void MinPQTiny()
      {
         MinPQ<string> pq = new MinPQ<string>();
         string streamName = "Algs4-Data\\TinyPQ.txt";
         string[] expectedResults = { "E", "A", "E" };
         int expectedRemainder = 6;
         CommonPriorityQueueUnitTests.StringPQTest(streamName, pq, expectedResults, expectedRemainder);
      }
   }
}
