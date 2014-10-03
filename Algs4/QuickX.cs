//-----------------------------------------------------------------------
// <copyright file="QuickX.cs" company="Eusebio Rufian-Zilbermann">
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
   /// The <tt>QuickX</tt> class provides methods for sorting an array using 
   /// an optimized version of quicksort (using Bentley-McIlroy 3-way partitioning, 
   /// Tukey's ninther, and cutoff to insertion sort).
   /// <para/>
   /// For additional documentation, see <a href="http://algs4.cs.princeton.edu/21elementary">Section 2.1</a> of
   /// <i>Algorithms, fourth Edition</i> by Robert Sedgewick and Kevin Wayne.
   /// </summary>
   public class QuickX : ISortingAlgorithm
   {
      /// <summary>
      /// cutoff to insertion sort
      /// </summary>
      private const int CUTOFF = 8;

      #region Singleton
      /// <summary>
      /// The single Instance of the QuickX Sort Algorithm.
      /// </summary>
      private static readonly Lazy<QuickX> Singleton = new Lazy<QuickX>(() => new QuickX());

      /// <summary>
      /// Prevents a default instance of the <see cref="QuickX"/> class from being created.
      /// </summary>
      private QuickX()
      {
      }

      /// <summary>
      /// Gets the instance of the Sorting Algorithm, for polymorphism purposes.
      /// </summary>
      internal static QuickX Instance
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
         //// Stdlib.StdRandom.Shuffle(sortableItems);
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
         //// Stdlib.StdRandom.Shuffle(sortableItems);
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
         QuickX.Sort(sortableItems);
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
         QuickX.Sort(sortableItems, comparerMethod);
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

         int itemCount = highIndex - lowIndex + 1;

         // cutoff to insertion sort
         if (itemCount <= CUTOFF)
         {
            InsertionSort(sortableItems, lowIndex, highIndex);
            return;
         }
         else if (itemCount <= 40)
         {
            // use median-of-3 as partitioning element
            int m = Median3(sortableItems, lowIndex, lowIndex + (itemCount / 2), highIndex);
            SortingCommon.Exch(sortableItems, m, lowIndex);
         }
         else
         {
            // use Tukey ninther as partitioning element
            int eps = itemCount / 8;
            int mid = lowIndex + (itemCount / 2);
            int m1 = Median3(sortableItems, lowIndex, lowIndex + eps, lowIndex + eps + eps);
            int m2 = Median3(sortableItems, mid - eps, mid, mid + eps);
            int m3 = Median3(sortableItems, highIndex - eps - eps, highIndex - eps, highIndex);
            int ninther = Median3(sortableItems, m1, m2, m3);
            SortingCommon.Exch(sortableItems, ninther, lowIndex);
         }

         // Bentley-McIlroy 3-way partitioning
         int i = lowIndex;
         int j = highIndex + 1;
         int p = lowIndex;
         int q = highIndex + 1;
         IComparable v = sortableItems[lowIndex];
         while (true)
         {
            while (SortingCommon.Less(sortableItems[++i], v))
            {
               if (i == highIndex)
               {
                  break;
               }
            }

            while (SortingCommon.Less(v, sortableItems[--j]))
            {
               if (j == lowIndex)
               {
                  break;
               }
            }

            // pointers cross
            if (i == j && sortableItems[i].Equals(v))
            {
               SortingCommon.Exch(sortableItems, ++p, i);
            }

            if (i >= j)
            {
               break;
            }

            SortingCommon.Exch(sortableItems, i, j);
            if (sortableItems[i].Equals(v))
            {
               SortingCommon.Exch(sortableItems, ++p, i);
            }

            if (sortableItems[j].Equals(v))
            {
               SortingCommon.Exch(sortableItems, --q, j);
            }
         }

         i = j + 1;
         for (int k = lowIndex; k <= p; k++)
         {
            SortingCommon.Exch(sortableItems, k, j--);
         }

         for (int k = highIndex; k >= q; k--)
         {
            SortingCommon.Exch(sortableItems, k, i++);
         }

         Sort(sortableItems, lowIndex, j);
         Sort(sortableItems, i, highIndex);
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

         int itemCount = highIndex - lowIndex + 1;

         // cutoff to insertion sort
         if (itemCount <= CUTOFF)
         {
            InsertionSort(sortableItems, comparerMethod, lowIndex, highIndex);
            return;
         }
         else if (itemCount <= 40)
         {
            // use median-of-3 as partitioning element
            int m = Median3(sortableItems, comparerMethod, lowIndex, lowIndex + (itemCount / 2), highIndex);
            SortingCommon.Exch(sortableItems, m, lowIndex);
         }
         else
         {
            // use Tukey ninther as partitioning element
            int eps = itemCount / 8;
            int mid = lowIndex + (itemCount / 2);
            int m1 = Median3(sortableItems, comparerMethod, lowIndex, lowIndex + eps, lowIndex + eps + eps);
            int m2 = Median3(sortableItems, comparerMethod, mid - eps, mid, mid + eps);
            int m3 = Median3(sortableItems, comparerMethod, highIndex - eps - eps, highIndex - eps, highIndex);
            int ninther = Median3(sortableItems, comparerMethod, m1, m2, m3);
            SortingCommon.Exch(sortableItems, ninther, lowIndex);
         }

         // Bentley-McIlroy 3-way partitioning
         int i = lowIndex;
         int j = highIndex + 1;
         int p = lowIndex;
         int q = highIndex + 1;
         T v = sortableItems[lowIndex];
         while (true)
         {
            while (SortingCommon.Less(comparerMethod, sortableItems[++i], v))
            {
               if (i == highIndex)
               {
                  break;
               }
            }

            while (SortingCommon.Less(comparerMethod, v, sortableItems[--j]))
            {
               if (j == lowIndex)
               {
                  break;
               }
            }

            // pointers cross
            if (i == j && sortableItems[i].Equals(v))
            {
               SortingCommon.Exch(sortableItems, ++p, i);
            }

            if (i >= j)
            {
               break;
            }

            SortingCommon.Exch(sortableItems, i, j);
            if (sortableItems[i].Equals(v))
            {
               SortingCommon.Exch(sortableItems, ++p, i);
            }

            if (sortableItems[j].Equals(v))
            {
               SortingCommon.Exch(sortableItems, --q, j);
            }
         }

         i = j + 1;
         for (int k = lowIndex; k <= p; k++)
         {
            SortingCommon.Exch(sortableItems, k, j--);
         }

         for (int k = highIndex; k >= q; k--)
         {
            SortingCommon.Exch(sortableItems, k, i++);
         }

         Sort(sortableItems, comparerMethod, lowIndex, j);
         Sort(sortableItems, comparerMethod, i, highIndex);
         Debug.Assert(SortingCommon.IsSorted(sortableItems, comparerMethod, lowIndex, highIndex), "The array is not sorted");
      }

      /// <summary>
      /// Insertion sort a sub-array.
      /// </summary>
      /// <param name="sortableItems">The array to be sorted.</param>
      /// <param name="lowIndex">Starting index of the sub-array being processed.</param>
      /// <param name="highIndex">Ending index of the sub-array being processed.</param>
      private static void InsertionSort(IComparable[] sortableItems, int lowIndex, int highIndex)
      {
         for (int i = lowIndex; i <= highIndex; i++)
         {
            for (int j = i; j > lowIndex && SortingCommon.Less(sortableItems[j], sortableItems[j - 1]); j--)
            {
               SortingCommon.Exch(sortableItems, j, j - 1);
            }
         }
      }

      /// <summary>
      /// Insertion sort a sub-array.
      /// </summary>
      /// <typeparam name="T">The type of items in the array.</typeparam>
      /// <param name="sortableItems">The array to be sorted.</param>
      /// <param name="comparerMethod">The comparer to be used for sorting.</param>
      /// <param name="lowIndex">Starting index of the sub-array being processed.</param>
      /// <param name="highIndex">Ending index of the sub-array being processed.</param>
      private static void InsertionSort<T>(T[] sortableItems, IComparer<T> comparerMethod, int lowIndex, int highIndex)
      {
         for (int i = lowIndex; i <= highIndex; i++)
         {
            for (int j = i; j > lowIndex && SortingCommon.Less(comparerMethod, sortableItems[j], sortableItems[j - 1]); j--)
            {
               SortingCommon.Exch(sortableItems, j, j - 1);
            }
         }
      }

      /// <summary>
      /// Return the index of the median element among a[i], a[j], and a[k].
      /// </summary>
      /// <param name="sortableItems">The array to take elements from.</param>
      /// <param name="i">The index for the first item.</param>
      /// <param name="j">The index for the second item.</param>
      /// <param name="k">The index for the third item.</param>
      /// <returns>The index for the median element.</returns>
      private static int Median3(IComparable[] sortableItems, int i, int j, int k)
      {
         return SortingCommon.Less(sortableItems[i], sortableItems[j]) ?
                (SortingCommon.Less(sortableItems[j], sortableItems[k]) ? 
                j : SortingCommon.Less(sortableItems[i], sortableItems[k]) ? k : i) :
                (SortingCommon.Less(sortableItems[k], sortableItems[j]) ? 
                j : SortingCommon.Less(sortableItems[k], sortableItems[i]) ? k : i);
      }

      /// <summary>
      /// Return the index of the median element among a[i], a[j], and a[k].
      /// </summary>
      /// <typeparam name="T">The type of items in the array.</typeparam>
      /// <param name="sortableItems">The array to take elements from.</param>
      /// <param name="comparerMethod">The comparer to be used for sorting.</param>
      /// <param name="i">The index for the first item.</param>
      /// <param name="j">The index for the second item.</param>
      /// <param name="k">The index for the third item.</param>
      /// <returns>The index for the median element.</returns>
      private static int Median3<T>(T[] sortableItems, IComparer<T> comparerMethod, int i, int j, int k)
      {
         return SortingCommon.Less(comparerMethod, sortableItems[i], sortableItems[j]) ?
                (SortingCommon.Less(comparerMethod, sortableItems[j], sortableItems[k]) ? 
                j : SortingCommon.Less(comparerMethod, sortableItems[i], sortableItems[k]) ? k : i) :
                (SortingCommon.Less(comparerMethod, sortableItems[k], sortableItems[j]) ? 
                j : SortingCommon.Less(comparerMethod, sortableItems[k], sortableItems[i]) ? k : i);
      }
   }
}