//-----------------------------------------------------------------------
// <copyright file="IndexMaxPQ.cs" company="Eusebio Rufian-Zilbermann">
//   Copyright (c) Eusebio Rufian-Zilbermann for the C# implementation
//   based on algorithms published by Robert Sedgewick and Kevin Wayne
// </copyright>
//-----------------------------------------------------------------------
namespace Algs4
{
   using System;
   using System.Collections.Generic;

   /// <summary>
   /// The <tt>IndexMaxPQ</tt> class represents a priority queue 
   /// where a zero-based set of indices are associated with a priority object each.
   /// Queue behaviors like en-queue, de-queue and peek are supported but 
   /// the difference with a standard queue is that removal from queue and peeking 
   /// is performed by priority (and not related to when the item was en-queued).
   /// Dictionary behaviors like iteration through keys and values, and retrieving
   /// the priority for a given item are also supported.
   /// <para/>
   /// This implementation uses a binary heap plus an array that associates keys 
   /// with values.
   /// The <em>insert</em>, <em>de-queue</em>, <em>delete</em>, <em>change-key</em>,
   /// <em>decrease-key</em>, and <em>increase-key</em> operations take logarithmic time.
   /// The <em>peek-index</em>, <em>peek-key</em>, <em>key-of</em>, <em>size</em>, and 
   /// <em>is-empty</em> operations take constant time. <em>Indexing by key</em> 
   /// is proportional to the number of items in the queue.
   /// Construction takes time proportional to the maximum number of indices.
   /// <para/>
   /// For additional documentation, see <a href="http://algs4.cs.princeton.edu/24pq">Section 2.4</a> of
   /// <i>Algorithms, fourth Edition</i> by Robert Sedgewick and Kevin Wayne.
   /// </summary>
   /// <typeparam name="T">The type of the priorities.</typeparam>
   public class IndexMaxPQ<T> : IndexPQDictionary<T>
   {
      #region Constructors
      /// <summary>
      /// Initializes a new instance of the <see cref="IndexMaxPQ{T}"/> class.
      /// </summary>
      /// <param name="maxCount">The maximum capacity of the Indexed Priority Queue.</param>
      public IndexMaxPQ(int maxCount)
         : base(maxCount, Comparer<T>.Default)
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="IndexMaxPQ{T}"/> class.
      /// </summary>
      /// <param name="maxCount">The maximum capacity of the Indexed Priority Queue.</param>
      /// <param name="comparator">Comparator used for determining order and equality.</param>
      public IndexMaxPQ(int maxCount, IComparer<T> comparator)
         : base(maxCount, comparator)
      {
      }
      #endregion

      #region IEnumerable<T> implementation
      /// <summary>
      /// Get an enumerator for ordered traversal of the heap items.
      /// </summary>
      /// <returns>An enumerator for traversal of the heap items.</returns>
      /// <remarks>
      /// The enumeration is non-destructive, it will not cause any item to be removed.
      /// The enumeration is ordered, the queue items will be sorted.
      /// </remarks>
      public override IEnumerator<KeyValuePair<int, T>> GetEnumerator()
      {
         IndexMaxPQ<T> copy = new IndexMaxPQ<T>(this.Count);
         for (int i = 1; i <= this.Count; i++)
         {
            int pq = this.GetPQItem(i);
            copy.Enqueue(pq, this[pq]);
         }

         while (0 != copy.Count)
         {
            KeyValuePair<int, T> item = copy.Peek();
            copy.Dequeue();
            yield return item;
         }
      }
      #endregion

      #region Original API (obsolete, there are preferred replacement methods in the IQueue<T> interface)
      /// <summary>
      /// Get the index of the smallest priority item on the priority queue.
      /// </summary>
      /// <returns>The index of the smallest priority item on the priority queue.</returns>
      [Obsolete("Use the PeekIndex() method instead.")]
      public int MaxIndex()
      {
         return this.PeekIndex();
      }

      /// <summary>
      /// Get a smallest priority on the priority queue.
      /// </summary>
      /// <returns>A smallest priority on the priority queue.</returns>
      [Obsolete("Use the PeekPriority() method instead.")]
      public T MaxKey()
      {
         return this.PeekPriority();
      }

      /// <summary>
      /// Removes and returns a smallest item on the priority queue.
      /// </summary>
      /// <returns>The index for the smallest item in the queue.</returns>
      [Obsolete("Use the Dequeue() method instead.")]
      public int DelMax()
      {
         return this.Dequeue();
      }
      #endregion

      /// <summary>
      /// Compare elements at the two specified indices.
      /// </summary>
      /// <param name="firstIndex">Index of the first item to compare.</param>
      /// <param name="secondIndex">Index of the second item to compare.</param>
      /// <returns>True if the first item is smaller than the second one, false otherwise.</returns>
      protected override bool PerformCompare(int firstIndex, int secondIndex)
      {
         return 0 > this.Comparator.Compare(
            this[this.GetPQItem(firstIndex)],
            this[this.GetPQItem(secondIndex)]);
      }
   }
}