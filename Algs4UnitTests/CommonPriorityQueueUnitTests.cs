//-----------------------------------------------------------------------
// <copyright file="CommonPriorityQueueUnitTests.cs" company="Eusebio Rufian-Zilbermann">
//   Copyright (c) Eusebio Rufian-Zilbermann for the C# implementation
//   based on materials published by Robert Sedgewick and Kevin Wayne
// </copyright>
//-----------------------------------------------------------------------
namespace Algs4UnitTests
{
   using System;
   using System.Collections.Generic;
   using System.Diagnostics.Contracts;
   using System.Linq;
   using Algs4;
   using Microsoft.VisualStudio.TestTools.UnitTesting;
   using Stdlib;

   /// <summary>
   /// Common methods for testing Priority Queue classes.
   /// </summary>
   public static class CommonPriorityQueueUnitTests
   {
      /// <summary>
      /// Test a queue using a stream (file) with strings and taking the minus sign
      /// as an indication to de-queue an item.
      /// </summary>
      /// <param name="streamName">The stream from where the input strings will be read.</param>
      /// <param name="queue">The queue to operate on.</param>
      /// <param name="expectedResults">The expected sequence of items removed by the minus signs.</param>
      /// <param name="expectedRemainder">
      /// The expected amount of items in the queue after all the removals have been processed
      /// </param>
      public static void StringPQTest(string streamName, PQCollection<string> queue, string[] expectedResults, int expectedRemainder)
      {
         Contract.Requires<ArgumentNullException>(queue != null, "queue");
         Contract.Requires<ArgumentNullException>(expectedResults != null, "expectedResults");
         string[] testItems;
         int expectedIndex = 0;
         using (In inStream = new In(streamName))
         {
            testItems = inStream.ReadAllStrings();
         }

         if (null == testItems)
         {
            throw new InternalTestFailureException("No items to test.");
         }

         foreach (string testItem in testItems)
         {
            if (0 != string.CompareOrdinal(testItem, "-"))
            {
               queue.Enqueue(testItem);
            }
            else if (1 > queue.Count)
            {
               throw new InternalTestFailureException("Queue underflow.");
            }
            else if (expectedResults.Length <= expectedIndex)
            {
               throw new InternalTestFailureException("Expected Results underflow.");
            }
            else
            {
               Assert.AreEqual(expectedResults[expectedIndex++], queue.Dequeue());
            }
         }

         Assert.AreEqual(expectedRemainder, queue.Count());
      }
   }
}