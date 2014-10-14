//-----------------------------------------------------------------------
// <copyright file="MaxPQUnitTests.cs" company="Eusebio Rufian-Zilbermann">
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
   /// Unit Tests for the MaxPQ class.
   /// </summary>
   [TestClass]
   public class MaxPQUnitTests
   {
      /// <summary>
      /// Test MaxPQ using TinyPQ.txt
      /// </summary>
      [TestMethod]
      public void MaxPQTiny()
      {
         MaxPQ<string> pq = new MaxPQ<string>();
         string streamName = "Algs4-Data\\TinyPQ.txt";
         string[] expectedResults = { "Q", "X", "P" };
         int expectedRemainder = 6;
         CommonPriorityQueueUnitTests.StringPQTest(streamName, pq, expectedResults, expectedRemainder);
      }
   }
}
