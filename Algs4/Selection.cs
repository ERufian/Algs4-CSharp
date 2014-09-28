//-----------------------------------------------------------------------
// <copyright file="Selection.cs" company="Eusebio Rufian-Zilbermann">
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
   /// The <tt>Selection</tt> class provides methods for sorting an
   /// array using selection sort.
   /// <para/>
   /// For additional documentation, see <a href="http://algs4.cs.princeton.edu/21elementary">Section 2.1</a> of
   /// <i>Algorithms, fourth Edition</i> by Robert Sedgewick and Kevin Wayne.
   /// </summary>
   public class Selection : ISortingAlgorithm
   {
      /// <summary>
      /// The single Instance of the Selection Sort Algorithm.
      /// </summary>
      private static readonly Lazy<Selection> Singleton = new Lazy<Selection>(() => new Selection());

      /// <summary>
      /// Prevents a default instance of the <see cref="Selection"/> class from being created.
      /// </summary>
      private Selection()
      {
      }

      /// <summary>
      /// Gets the instance of the Sorting Algorithm, for polymorphism purposes.
      /// </summary>
      internal static Selection Instance
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
      public static void Sort(IComparable[] sortableItems)
      {
         if (null == sortableItems)
         {
            throw new ArgumentNullException("sortableItems");
         }

         int itemCount = sortableItems.Length;
         for (int i = 0; itemCount > i; i++)
         {
            int min = i;
            for (int j = i + 1; itemCount > j; j++)
            {
               if (SortingCommon.Less(sortableItems[j], sortableItems[min]))
               {
                  min = j;
               }
            }

            SortingCommon.Exch(sortableItems, i, min);
            Debug.Assert(SortingCommon.IsSorted(sortableItems, 0, i), "The array is not sorted");
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
         for (int i = 0; i < itemCount; i++)
         {
            int min = i;
            for (int j = i + 1; j < itemCount; j++)
            {
               if (SortingCommon.Less(comparerMethod, sortableItems[j], sortableItems[min]))
               {
                  min = j;
               }
            }

            SortingCommon.Exch(sortableItems, i, min);
            Debug.Assert(SortingCommon.IsSorted(sortableItems, comparerMethod, 0, i), "The array is not sorted");
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
         Selection.Sort(sortableItems);
      }
   }
}
