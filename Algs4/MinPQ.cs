//-----------------------------------------------------------------------
// <copyright file="MinPQ.cs" company="Eusebio Rufian-Zilbermann">
//   Copyright (c) Eusebio Rufian-Zilbermann for the C# implementation
//   based on algorithms published by Robert Sedgewick and Kevin Wayne
// </copyright>
//-----------------------------------------------------------------------
namespace Algs4
{
   using System;
   using System.Collections.Generic;

   /// <summary>
   /// The <tt>MinPQ</tt> class represents a priority queue of generic keys.
   /// It supports the usual <em>insert</em> and <em>delete-the-minimum</em>
   /// operations, along with methods for peeking at the minimum key,
   /// testing if the priority queue is empty, and iterating through
   /// the keys.
   /// <para/>
   /// This implementation uses a binary heap.
   /// The <em>insert</em> and <em>delete-the-minimum</em> operations take
   /// logarithmic amortized time.
   /// The <em>min</em>, <em>size</em>, and <em>is-empty</em> operations take constant time.
   /// Construction takes time proportional to the number of items used to initialize the data structure.
   /// <para/>
   /// For additional documentation, see <a href="http://algs4.cs.princeton.edu/24pq">Section 2.4</a> of
   /// <i>Algorithms, fourth Edition</i> by Robert Sedgewick and Kevin Wayne.
   /// </summary>
   /// <typeparam name="T">The type of objects to be inserted in the queue</typeparam>
   /// <remarks>Implements ICollection so that <code>Linq</code> extension methods and aggregates can be used.</remarks>
   public class MinPQ<T> : PQCollection<T>
   {
      #region Constructors
      /// <summary>
      /// Initializes a new instance of the <see cref="MinPQ{T}"/> class with the given initial capacity.
      /// </summary>
      /// <param name="initialCapacity">The initial capacity of the priority queue.</param>
      /// <exception cref="ArgumentOutOfRangeException">Thrown when the specified initial capacity is <code>int.MaxValue</code></exception>
      public MinPQ(int initialCapacity) 
         : base(initialCapacity)
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="MinPQ{T}"/> class with an initial capacity of 1 item.
      /// </summary>
      public MinPQ()
         : base(1)
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="MinPQ{T}"/> class with the given initial capacity
      /// and a given comparator.
      /// </summary>
      /// <param name="initialCapacity">The initial capacity of the priority queue.</param>
      /// <param name="comparator">The order in which to compare the keys.</param>
      /// <exception cref="ArgumentOutOfRangeException">Thrown when the specified initial capacity is <code>int.MaxValue</code></exception>
      public MinPQ(int initialCapacity, IComparer<T> comparator)
         : base(initialCapacity, comparator)
      {
      }

      /// <summary>
      ///  Initializes a new instance of the <see cref="MinPQ{T}"/> class with the given comparator and capacity for 1 element.
      /// </summary>
      /// <param name="comparator">The order in which to compare the keys.</param>
      public MinPQ(IComparer<T> comparator)
         : base(1, comparator)
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="MinPQ{T}"/> class from an array of keys.
      /// Takes time proportional to the number of keys, using sink-based heap construction.
      /// </summary>
      /// <param name="keys">The array of keys.</param>
      public MinPQ(T[] keys)
         : base(keys)
      {
         for (int k = this.Count / 2; k >= 1; k--)
         {
            this.Sink(k);
         }
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
      public override IEnumerator<T> GetEnumerator()
      {
         MinPQ<T> copy = new MinPQ<T>(this.Count);
         for (int i = 1; i <= this.Count; i++)
         {
            copy.Enqueue(this.GetPQItem(i));
         }

         while (0 != copy.Count)
         {
            yield return copy.Dequeue();
         }
      }
      #endregion

      #region Original API (obsolete, there are preferred replacement methods in the IQueue<T> interface)
      /// <summary>
      /// Removes and returns a smallest key on the priority queue.
      /// </summary>
      /// <returns>A smallest key on the priority queue.</returns>
      /// <exception cref="NoSuchElementException">Thrown if the priority queue is empty.</exception>
      [Obsolete("Use the Dequeue() method instead.")]
      public T DelMin()
      {
         return this.Dequeue();
      }

      /// <summary>
      /// Get a smallest key on the priority queue.
      /// </summary>
      /// <returns>A smallest key on the priority queue.</returns>
      /// <exception cref="NoSuchElementException">Thrown if the priority queue is empty.</exception>
      [Obsolete("Use the Peek() method instead.")]
      public T Min()
      {
         return this.Peek();
      }
      #endregion

      #region Methods for compares and swaps.
      /// <summary>
      /// Compare elements at the two specified indices.
      /// </summary>
      /// <param name="firstIndex">Index of the first item to compare.</param>
      /// <param name="secondIndex">Index of the second item to compare.</param>
      /// <returns>True if the first item is smaller than the second one, false otherwise.</returns>
      protected override bool PerformCompare(int firstIndex, int secondIndex)
      {
         return 0 < this.Comparator.Compare(this.GetPQItem(firstIndex), this.GetPQItem(secondIndex));
      }
      #endregion
   }
}