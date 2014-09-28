//-----------------------------------------------------------------------
// <copyright file="InsertionX.cs" company="Eusebio Rufian-Zilbermann">
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
   /// The <tt>InsertionX</tt> class provides static methods for sorting an
   /// array using an optimized insertion sort with half-exchanges.
   /// <para/>
   /// For additional documentation, see <a href="http://algs4.cs.princeton.edu/21elementary">Section 2.1</a> of
   /// <i>Algorithms, fourth Edition</i> by Robert Sedgewick and Kevin Wayne.
   /// </summary>
   public class InsertionX : ISortingAlgorithm
   {
      /// <summary>
      /// The single Instance of the InsertionX Sort Algorithm.
      /// </summary>
      private static readonly Lazy<InsertionX> Singleton = new Lazy<InsertionX>(() => new InsertionX());

      /// <summary>
      /// Prevents a default instance of the <see cref="InsertionX"/> class from being created.
      /// </summary>
      private InsertionX()
      {
      }

      /// <summary>
      /// Gets the instance of the Sorting Algorithm, for polymorphism purposes.
      /// </summary>
      internal static InsertionX Instance
      {
         get
         { 
            return Singleton.Value; 
         }
      }

      /// <summary>
      /// Rearranges an array of items in ascending order, using the natural order.
      /// </summary>
      /// <param name="sortableItems">The array to be sorted.</param>
      public static void Sort(System.IComparable[] sortableItems)
      {
         if (null == sortableItems)
         {
            throw new ArgumentNullException("sortableItems");
         }

         int itemCount = sortableItems.Length;

         // put smallest element in position to serve as sentinel
         for (int i = itemCount - 1; 0 < i; i--)
         {
            if (SortingCommon.Less(sortableItems[i], sortableItems[i - 1]))
            {
               SortingCommon.Exch(sortableItems, i, i - 1);
            }
         }

         // insertion sort with half-exchanges
         for (int i = 2; itemCount > i; i++)
         {
            IComparable itemV = sortableItems[i];
            int j = i;
            while (SortingCommon.Less(itemV, sortableItems[j - 1]))
            {
               sortableItems[j] = sortableItems[j - 1];
               j--;
            }

            sortableItems[j] = itemV;
         }

         Debug.Assert(SortingCommon.IsSorted(sortableItems), "The array is not sorted");
      }

      /// <summary>
      /// Rearranges an array in ascending order, using a comparator.
      /// </summary>
      /// <typeparam name="T">The type of items in the array.</typeparam>
      /// <param name="sortableItems">The array to be sorted.</param>
      /// <param name="comparerMethod">The comparer to be used for sorting.</param>
      public static void Sort<T>(T[] sortableItems, IComparer<T> comparerMethod)
      {
         if (null == sortableItems)
         {
            throw new ArgumentNullException("sortableItems");
         }

         int itemCount = sortableItems.Length;

         // put smallest element in position to serve as sentinel
         for (int i = itemCount - 1; 0 < i; i--)
         {
            if (SortingCommon.Less(comparerMethod, sortableItems[i], sortableItems[i - 1]))
            {
               SortingCommon.Exch(sortableItems, i, i - 1);
            }
         }

         // insertion sort with half-exchanges
         for (int i = 2; i < itemCount; i++)
         {
            T itemV = sortableItems[i];
            int j = i;
            while (SortingCommon.Less(comparerMethod, itemV, sortableItems[j - 1]))
            {
               sortableItems[j] = sortableItems[j - 1];
               j--;
            }

            sortableItems[j] = itemV;
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
         InsertionX.Sort(sortableItems);
      }
   }
}
