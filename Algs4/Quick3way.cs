//-----------------------------------------------------------------------
// <copyright file="Quick3Way.cs" company="Eusebio Rufian-Zilbermann">
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
   /// The <tt>Quick3Way</tt> class provides methods for sorting an array using 3-way quick sort.
   /// <para/>
   /// For additional documentation, see <a href="http://algs4.cs.princeton.edu/21elementary">Section 2.1</a> of
   /// <i>Algorithms, fourth Edition</i> by Robert Sedgewick and Kevin Wayne.
   /// </summary>
   public class Quick3Way : ISortingAlgorithm
   {
      #region Singleton
      /// <summary>
      /// The single Instance of the Quick3Way Sort Algorithm.
      /// </summary>
      private static readonly Lazy<Quick3Way> Singleton = new Lazy<Quick3Way>(() => new Quick3Way());

      /// <summary>
      /// Prevents a default instance of the <see cref="Quick3Way"/> class from being created.
      /// </summary>
      private Quick3Way()
      {
      }

      /// <summary>
      /// Gets the instance of the Sorting Algorithm, for polymorphism purposes.
      /// </summary>
      internal static Quick3Way Instance
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
         Stdlib.StdRandom.Shuffle(sortableItems);
         Sort(sortableItems, 0, sortableItems.Length - 1);
         Debug.Assert(SortingCommon.IsSorted(sortableItems), "The array is not sorted");
      }

      /// <summary>
      /// Rearranges the array in ascending order, using a specified comparer.
      /// </summary>
      /// <typeparam name="T">The type of items in the array.</typeparam>
      /// <param name="sortableItems">The array to be sorted</param>
      /// <param name="comparerMethod">The comparer to be used for sorting.</param>
      public static void Sort<T>(T[] sortableItems, IComparer<T> comparerMethod)
      {
         ArgumentValidator.CheckNotNull(sortableItems, "sortableItems");
         Stdlib.StdRandom.Shuffle(sortableItems);
         Sort(sortableItems, comparerMethod, 0, sortableItems.Length - 1);
         Debug.Assert(SortingCommon.IsSorted(sortableItems, comparerMethod), "The array is not sorted");
      }

      /// <summary>
      /// Rearranges an array of items in ascending order, using the natural order.
      /// </summary>
      /// <param name="sortableItems">The array to be sorted.</param>
      /// <remarks>Unlike the static version, the instance version allows polymorphism.</remarks>
      void ISortingAlgorithm.Sort(IComparable[] sortableItems)
      {
         Quick3Way.Sort(sortableItems);
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
         Quick3Way.Sort(sortableItems, comparerMethod);
      }

      /// <summary>
      /// Sort sourceItems[lowIndex..highIndex].
      /// </summary>
      /// <param name="sortableItems">The array to be sorted.</param>
      /// <param name="lowIndex">Starting index of the sub-array being processed.</param>
      /// <param name="highIndex">Ending index of the sub-array being processed.</param>
      private static void Sort(IComparable[] sortableItems, int lowIndex, int highIndex)
      {
         if (highIndex <= lowIndex)
         {
            return;
         }

         int lt = lowIndex;
         int gt = highIndex;
         IComparable v = sortableItems[lowIndex];
         int i = lowIndex;
         while (i <= gt)
         {
            int cmp = sortableItems[i].CompareTo(v);
            if (cmp < 0)
            {
               SortingCommon.Exch(sortableItems, lt++, i++);
            }
            else if (cmp > 0)
            {
               SortingCommon.Exch(sortableItems, i, gt--);
            }
            else
            {
               i++;
            }
         }

         // sortableItems[lowIndex..lt-1] < v = sortableItems[lt..gt] < sortableItems[gt+1..highIndex]. 
         Sort(sortableItems, lowIndex, lt - 1);
         Sort(sortableItems, gt + 1, highIndex);
         Debug.Assert(SortingCommon.IsSorted(sortableItems, lowIndex, highIndex), "The array is not sorted");
      }

      /// <summary>
      /// Sort sourceItems[lowIndex..highIndex].
      /// </summary>
      /// <typeparam name="T">The type of items in the array.</typeparam>
      /// <param name="sortableItems">The array to be sorted.</param>
      /// <param name="comparerMethod">The comparer to be used for sorting.</param>
      /// <param name="lowIndex">Starting index of the sub-array being processed.</param>
      /// <param name="highIndex">Ending index of the sub-array being processed.</param>
      private static void Sort<T>(T[] sortableItems, IComparer<T> comparerMethod, int lowIndex, int highIndex)
      {
         if (highIndex <= lowIndex)
         {
            return;
         }

         int lt = lowIndex;
         int gt = highIndex;
         T v = sortableItems[lowIndex];
         int i = lowIndex;
         while (i <= gt)
         {
            int cmp = comparerMethod.Compare(sortableItems[i], v);
            if (cmp < 0)
            {
               SortingCommon.Exch(sortableItems, lt++, i++);
            }
            else if (cmp > 0)
            {
               SortingCommon.Exch(sortableItems, i, gt--);
            }
            else
            {
               i++;
            }
         }

         // sortableItems[lowIndex..lt-1] < v = sortableItems[lt..gt] < sortableItems[gt+1..highIndex]. 
         Sort(sortableItems, comparerMethod, lowIndex, lt - 1);
         Sort(sortableItems, comparerMethod, gt + 1, highIndex);
         Debug.Assert(SortingCommon.IsSorted(sortableItems, comparerMethod, lowIndex, highIndex), "The array is not sorted");
      }
   }
}