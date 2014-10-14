//-----------------------------------------------------------------------
// <copyright file="CommonIndexPQUnitTests.cs" company="Eusebio Rufian-Zilbermann">
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
   /// Common methods for testing Index Priority Queue classes.
   /// </summary>
   internal static class CommonIndexPQUnitTests
   {
      /// <summary>
      /// Test that de-queue behavior on an index PQ is the same as a regular queue.
      /// </summary>
      /// <param name="testItems">The stream from where the input strings will be read.</param>
      /// <param name="priorityQueue">The queue to operate on.</param>
      /// <param name="expectedItems">The expected items.</param>
      public static void DequeuePQTest(string[] testItems, IndexPQDictionary<string> priorityQueue, Queue<string> expectedItems)
      {         
         for (int i = 0; i < testItems.Length; i++)
         {
            priorityQueue.Enqueue(i, testItems[i]);
         }

         // Compare dequeueing
         while (0 != priorityQueue.Count)
         {
            Assert.AreEqual(priorityQueue.Count, expectedItems.Count);
            string actualItem = testItems[priorityQueue.Dequeue()];
            string expectedItem = expectedItems.Dequeue();
            Assert.AreEqual(expectedItem, actualItem);
         }
      }

      /// <summary>
      /// Test that enumerator behavior on an index PQ is the same as a regular queue.
      /// </summary>
      /// <param name="testItems">The stream from where the input strings will be read.</param>
      /// <param name="priorityQueue">The queue to operate on.</param>
      /// <param name="expectedItems">The expected items.</param>
      public static void EnumerationPQTest(string[] testItems, IndexPQDictionary<string> priorityQueue, Queue<string> expectedItems)
      {
         for (int i = 0; i < testItems.Length; i++)
         {
            priorityQueue.Enqueue(i, testItems[i]);
         }

         Assert.AreEqual(priorityQueue.Count, expectedItems.Count);
         foreach (KeyValuePair<int, string> kvp in priorityQueue)
         {
            string actualItem = kvp.Value;
            string expectedItem = expectedItems.Dequeue();
            Assert.AreEqual(expectedItem, actualItem);
         }

         // Enumeration should be non-destructive (as opposed to de-queueing)
         Assert.AreEqual(testItems.Length, priorityQueue.Count);
      }

      /// <summary>
      /// Read multiple sorted streams and merge their contents in sorted order.
      /// </summary>
      /// <param name="streamNames">The stream names to read from.</param>
      /// <param name="priorityQueue">The Index Priority Queue to use for merging.</param>
      /// <param name="expectedItems">The expected results.</param>
      /// <remarks>
      /// This method expects each input stream to be sorted but does not verify it.
      /// If the inputs are unsorted the results will be incorrect.
      /// </remarks>
      public static void MergeMultiWay(string[] streamNames, IndexPQDictionary<string> priorityQueue, Queue<string> expectedItems)
      {
         In[] inputStream = new In[streamNames.Length];

         try
         {
            for (int i = 0; streamNames.Length > i; i++)
            {
               inputStream[i] = new In(streamNames[i]);
               priorityQueue.Enqueue(i, inputStream[i].ReadString());
            }

            // Extract and print min and read next from its stream. 
            while (0 != priorityQueue.Count)
            {
               string actualItem = priorityQueue.PeekPriority();
               string expectedItem = expectedItems.Dequeue();
               Assert.AreEqual(expectedItem, actualItem);
               int i = priorityQueue.Dequeue();
               if (!inputStream[i].IsEmpty())
               {
                  priorityQueue.Enqueue(i, inputStream[i].ReadString());
               }
            }
         }
         finally
         {
            for (int i = 0; streamNames.Length > i; i++)
            {
               inputStream[i].Close();
            }
         }
      } 
   }
}
