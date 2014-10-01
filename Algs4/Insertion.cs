//-----------------------------------------------------------------------
// <copyright file="Insertion.cs" company="Eusebio Rufian-Zilbermann">
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
   /// The <tt>Insertion</tt> class provides methods for sorting an
   /// array using insertion sort.
   /// <para/>
   /// For additional documentation, see <a href="http://algs4.cs.princeton.edu/21elementary">Section 2.1</a> of
   /// <i>Algorithms, fourth Edition</i> by Robert Sedgewick and Kevin Wayne.
   /// </summary>
   public class Insertion : ISortingAlgorithm
   {
      #region Singleton
      /// <summary>
      /// The single Instance of the Insertion Sort Algorithm.
      /// </summary>
      private static readonly Lazy<Insertion> Singleton = new Lazy<Insertion>(() => new Insertion());

      /// <summary>
      /// Prevents a default instance of the <see cref="Insertion"/> class from being created.
      /// </summary>
      private Insertion()
      {
      }

      /// <summary>
      /// Gets the instance of the Sorting Algorithm, for polymorphism purposes.
      /// </summary>
      internal static Insertion Instance
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
         for (int i = 0; i < itemCount; i++)
         {
            for (int j = i; j > 0 && SortingCommon.Less(sortableItems[j], sortableItems[j - 1]); j--)
            {
               SortingCommon.Exch(sortableItems, j, j - 1);
            }

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
         ArgumentValidator.CheckNotNull(sortableItems, "sortableItems");
         int itemCount = sortableItems.Length;
         for (int i = 0; i < itemCount; i++)
         {
            for (int j = i; j > 0 && SortingCommon.Less(comparerMethod, sortableItems[j], sortableItems[j - 1]); j--)
            {
               SortingCommon.Exch(sortableItems, j, j - 1);
            }

            Debug.Assert(SortingCommon.IsSorted(sortableItems, comparerMethod, 0, i), "The array is not sorted");
         }

         Debug.Assert(SortingCommon.IsSorted(sortableItems, comparerMethod), "The array is not sorted");
      }

      /// <summary>
      /// Calculate a permutation that gives the elements in the array in ascending order.
      /// </summary>
      /// <param name="sortableItems">The array to be sorted.</param>
      /// <returns>
      /// A permutation <tt>p[]</tt> such that <tt>a[p[0]]</tt>, <tt>a[p[1]]</tt>,
      /// ..., <tt>a[p[N-1]]</tt> are in ascending order
      /// </returns>
      public static int[] IndexSort(System.IComparable[] sortableItems)
      {
         ArgumentValidator.CheckNotNull(sortableItems, "sortableItems");
         int itemCount = sortableItems.Length;
         int[] index = new int[itemCount];
         for (int i = 0; i < itemCount; i++)
         {
            index[i] = i;
         }

         for (int i = 0; i < itemCount; i++)
         {
            for (int j = i; j > 0 && SortingCommon.Less(sortableItems[index[j]], sortableItems[index[j - 1]]); j--)
            {
               SortingCommon.Exch(index, j, j - 1);
            }
         }

         return index;
      }

      /// <summary>
      /// Rearranges an array of items in ascending order, using the natural order.
      /// </summary>
      /// <param name="sortableItems">The array to be sorted.</param>
      /// <remarks>Unlike the static version, the instance version allows polymorphism.</remarks>
      void ISortingAlgorithm.Sort(IComparable[] sortableItems)
      {
         Insertion.Sort(sortableItems);
      }

      /// <summary>
      /// Rearranges an array of items in ascending order, using the natural order.
      /// </summary>
      /// <typeparam name="T">The type of items in the array.</typeparam>
      /// <param name="sortableItems">The array to be sorted.</param>
      /// <param name="comparerMethod">The comparer to be used for sorting.</param>
      /// <remarks>Unlike the static version, the instance version allows polymorphism.</remarks>
      void ISortingAlgorithm.Sort<T>(T[] sortableItems, IComparer<T> comparerMethod)
      {
         Insertion.Sort(sortableItems, comparerMethod);
      }
   }
}
