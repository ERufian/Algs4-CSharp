//-----------------------------------------------------------------------
// <copyright file="Shell.cs" company="Eusebio Rufian-Zilbermann">
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
   /// Sorts a sequence of strings from standard input using Shell's sort with
   /// Knuth's 3x+1 increment sequence (1, 4, 13, 40, ...)
   /// <para/>
   /// For additional documentation, see <a href="http://algs4.cs.princeton.edu/21elementary">Section 2.1</a> of
   /// <i>Algorithms, fourth Edition</i> by Robert Sedgewick and Kevin Wayne.
   /// </summary>
   /// <remarks>
   /// A possible alternative sequence, proposed by Sedgewick and Incerpi is:
   /// The nth element of the sequence is the smallest integer >= 2.5^n
   /// that is relatively prime to all previous terms in the sequence.
   /// For example, interleave[4] is 41 because 2.5^4 = 39.0625 and 41 is
   /// the next integer that is relatively prime to 3, 7, and 16.
   /// </remarks>
   public class Shell : ISortingAlgorithm
   {
      #region Singleton
      /// <summary>
      /// The single Instance of the Shell Sort Algorithm.
      /// </summary>
      private static readonly Lazy<Shell> Singleton = new Lazy<Shell>(() => new Shell());

      /// <summary>
      /// Prevents a default instance of the <see cref="Shell"/> class from being created.
      /// </summary>
      private Shell()
      {
      }

      /// <summary>
      /// Gets the instance of the Sorting Algorithm, for polymorphism purposes.
      /// </summary>
      internal static Shell Instance
      {
         get
         {
            return Singleton.Value;
         }
      }
      #endregion

      /// <summary>
      /// Rearranges an array of items in ascending order, using the natural order.
      /// </summary>
      /// <param name="sortableItems">The array to be sorted.</param>
      public static void Sort(System.IComparable[] sortableItems)
      {
         ArgumentValidator.CheckNotNull(sortableItems, "sortableItems");
         int itemCount = sortableItems.Length;

         // 3x+1 increment sequence:  1, 4, 13, 40, 121, 364, 1093, ... 
         int h = 1;
         while (h < itemCount / 3)
         {
            h = (3 * h) + 1;
         }

         while (h >= 1)
         {
            // h-sort the array
            for (int i = h; itemCount > i; i++)
            {
               for (int j = i; j >= h && SortingCommon.Less(sortableItems[j], sortableItems[j - h]); j -= h)
               {
                  SortingCommon.Exch(sortableItems, j, j - h);
               }
            }

            Debug.Assert(SortingCommon.IsHSorted(sortableItems, h), "The array is not h-sorted");
            h /= 3;
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
         ArgumentValidator.CheckNotNull(sortableItems, "sortableItems");

         int itemCount = sortableItems.Length;

         // 3x+1 increment sequence:  1, 4, 13, 40, 121, 364, 1093, ... 
         int h = 1;
         while (h < itemCount / 3)
         {
            h = (3 * h) + 1;
         }

         while (h >= 1)
         {
            // h-sort the array
            for (int i = h; itemCount > i; i++)
            {
               for (int j = i; j >= h && SortingCommon.Less(comparerMethod, sortableItems[j], sortableItems[j - h]); j -= h)
               {
                  SortingCommon.Exch(sortableItems, j, j - h);
               }
            }

            Debug.Assert(SortingCommon.IsHSorted(sortableItems, comparerMethod, h), "The array is not h-sorted");
            h /= 3;
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
         Shell.Sort(sortableItems);
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
         Shell.Sort(sortableItems, comparerMethod);
      }
   }
}
