//-----------------------------------------------------------------------
// <copyright file="Heap.cs" company="Eusebio Rufian-Zilbermann">
//   Copyright (c) Eusebio Rufian-Zilbermann for the C# implementation
//   based on algorithms published by Robert Sedgewick and Kevin Wayne
// </copyright>
//-----------------------------------------------------------------------
namespace Algs4
{
   using System;
   using System.Collections.Generic;
   using System.Diagnostics;

   /// <summary>
   /// The <tt>Heap</tt> class provides methods for sorting an
   /// array using heap sort.
   /// <para/>
   /// For additional documentation, see <a href="http://algs4.cs.princeton.edu/24pq">Section 2.4</a> of
   /// <i>Algorithms, fourth Edition</i> by Robert Sedgewick and Kevin Wayne.
   /// </summary>
   public class Heap : ISortingAlgorithm
   {
      #region Singleton
      /// <summary>
      /// The single Instance of the Heap Sort Algorithm.
      /// </summary>
      private static readonly Lazy<Heap> Singleton = new Lazy<Heap>(() => new Heap());

      /// <summary>
      /// Prevents a default instance of the <see cref="Heap"/> class from being created.
      /// </summary>
      private Heap()
      {
      }

      /// <summary>
      /// Gets the instance of the Sorting Algorithm, for polymorphism purposes.
      /// </summary>
      internal static Heap Instance
      {
         get
         {
            return Singleton.Value;
         }
      }
      #endregion

      /// <summary>
      /// Rearranges the array in ascending order, using the natural order.
      /// </summary>
      /// <param name="sortableItems">The array to be sorted</param>
      public static void Sort(IComparable[] sortableItems)
      {
         ArgumentValidator.CheckNotNull(sortableItems, "sortableItems");
         int upperBound = sortableItems.Length;
         for (int k = upperBound / 2; k >= 1; k--)
         {
            Heap.Sink(sortableItems, k, upperBound);
         }

         while (upperBound > 1)
         {
            SortingCommon.Exch(sortableItems, 0, upperBound - 1);
            upperBound--;
            Heap.Sink(sortableItems, 1, upperBound);
         }

         Debug.Assert(SortingCommon.IsSorted(sortableItems), "The array is not sorted");
      }

      /// <summary>
      /// Rearranges the array in ascending order, using a specified comparer.
      /// </summary>
      /// <typeparam name="T">The type of items in the array.</typeparam>
      /// <param name="sortableItems">The array to be sorted.</param>
      /// <param name="comparerMethod">The comparer to be used for sorting.</param>
      public static void Sort<T>(T[] sortableItems, IComparer<T> comparerMethod)
      {
         ArgumentValidator.CheckNotNull(sortableItems, "sortableItems");
         int upperBound = sortableItems.Length;
         for (int k = upperBound / 2; k >= 1; k--)
         {
            Heap.Sink(sortableItems, comparerMethod, k, upperBound);
         }

         while (upperBound > 1)
         {
            SortingCommon.Exch(sortableItems, 0, upperBound - 1);
            upperBound--;
            Heap.Sink(sortableItems, comparerMethod, 1, upperBound);
         }

         Debug.Assert(SortingCommon.IsSorted(sortableItems, comparerMethod), "The array is not sorted");
      }

      /// <summary>
      /// Rearranges an array of items in ascending order, using the natural order.
      /// </summary>
      /// <param name="sortableItems">The array to be sorted.</param>
      /// <remarks>Unlike the static version, the instance version allows polymorphism.</remarks>
      void ISortingAlgorithm.Sort(IComparable[] sortableItems)
      {
         Heap.Sort(sortableItems);
      }

      /// <summary>
      /// Rearranges an array of items in ascending order, using a specified comparer.
      /// </summary>
      /// <typeparam name="T">The type of items in the array.</typeparam>
      /// <param name="sortableItems">The array to be sorted.</param>
      /// <param name="comparerMethod">The comparer to be used for sorting.</param>
      /// <remarks>Unlike the static version, the instance version allows polymorphism.</remarks>
      void ISortingAlgorithm.Sort<T>(T[] sortableItems, IComparer<T> comparerMethod)
      {
         Heap.Sort(sortableItems, comparerMethod);
      }

      /// <summary>
      /// Top-down restoration of the heap invariant.
      /// </summary>
      /// <param name="sortableItems">The array of items where to sink nodes.</param>
      /// <param name="nodeIndex">Index of the node that may have broken the heap invariant.</param>
      /// <param name="upperBound">The upper bound of the sub-array where to sink nodes.</param>
      private static void Sink(IComparable[] sortableItems, int nodeIndex, int upperBound)
      {
         if (int.MaxValue / 2 <= nodeIndex)
         {
            throw new ArgumentException("Node index overflow", "nodeIndex");
         }

         while (2 * nodeIndex <= upperBound)
         {
            int j = 2 * nodeIndex;
            if (j < upperBound && SortingCommon.Less(sortableItems[j - 1], sortableItems[j]))
            {
               j++;
            }

            if (!SortingCommon.Less(sortableItems[nodeIndex - 1], sortableItems[j - 1]))
            {
               break;
            }

            SortingCommon.Exch(sortableItems, nodeIndex - 1, j - 1);
            nodeIndex = j;
         }
      }

      /// <summary>
      /// Top-down restoration of the heap invariant.
      /// </summary>
      /// <typeparam name="T">The type of items in the array.</typeparam>
      /// <param name="sortableItems">The array of items where to sink nodes.</param>
      /// <param name="comparerMethod">The comparer to be used for sorting.</param>
      /// <param name="nodeIndex">Index of the node that may have broken the heap invariant.</param>
      /// <param name="upperBound">The upper bound of the sub-array where to sink nodes.</param>
      private static void Sink<T>(T[] sortableItems, IComparer<T> comparerMethod, int nodeIndex, int upperBound)
      {
         if (int.MaxValue / 2 <= nodeIndex)
         {
            throw new ArgumentException("Node index overflow", "nodeIndex");
         }

         while (2 * nodeIndex <= upperBound)
         {
            int j = 2 * nodeIndex;
            if (j < upperBound && SortingCommon.Less(comparerMethod, sortableItems[j - 1], sortableItems[j]))
            {
               j++;
            }

            if (!SortingCommon.Less(comparerMethod, sortableItems[nodeIndex - 1], sortableItems[j - 1]))
            {
               break;
            }

            SortingCommon.Exch(sortableItems, nodeIndex - 1, j - 1);
            nodeIndex = j;
         }
      }
   }
}