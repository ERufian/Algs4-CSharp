//-----------------------------------------------------------------------
// <copyright file="Quick.cs" company="Eusebio Rufian-Zilbermann">
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
   /// The <tt>Quick</tt> class provides methods for sorting an array using quick sort .
   /// <para/>
   /// For additional documentation, see <a href="http://algs4.cs.princeton.edu/21elementary">Section 2.1</a> of
   /// <i>Algorithms, fourth Edition</i> by Robert Sedgewick and Kevin Wayne.
   /// </summary>
   public class Quick : ISortingAlgorithm
   {
      #region Singleton
      /// <summary>
      /// The single Instance of the Quick Sort Algorithm.
      /// </summary>
      private static readonly Lazy<Quick> Singleton = new Lazy<Quick>(() => new Quick());

      /// <summary>
      /// Prevents a default instance of the <see cref="Quick"/> class from being created.
      /// </summary>
      private Quick()
      {
      }

      /// <summary>
      /// Gets the instance of the Sorting Algorithm, for polymorphism purposes.
      /// </summary>
      internal static Quick Instance
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
         Quick.Sort(sortableItems);
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
         Quick.Sort(sortableItems, comparerMethod);
      }

      /// <summary>
      /// Partition and sort sourceItems[lowIndex..highIndex]
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

         int j = Partition(sortableItems, lowIndex, highIndex);
         Sort(sortableItems, lowIndex, j - 1);
         Sort(sortableItems, j + 1, highIndex);
         Debug.Assert(SortingCommon.IsSorted(sortableItems, lowIndex, highIndex), "The array is not sorted");
      }

      /// <summary>
      /// Partition and sort sourceItems[lowIndex..highIndex]
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

         int j = Partition(sortableItems, comparerMethod, lowIndex, highIndex);
         Sort(sortableItems, comparerMethod, lowIndex, j - 1);
         Sort(sortableItems, comparerMethod, j + 1, highIndex);
         Debug.Assert(SortingCommon.IsSorted(sortableItems, comparerMethod, lowIndex, highIndex), "The array is not sorted");
      }

      /// <summary>
      /// Partition the sub-array sortableItems[lowIndex..highIndex] so that 
      /// sortableItems[lowIndex..j-1] less or equal to sortableItems[j] less or equal to 
      /// sortableItems[j+1..highIndex].
      /// </summary>
      /// <param name="sortableItems">The array to be sorted.</param>
      /// <param name="lowIndex">Starting index of the sub-array being processed.</param>
      /// <param name="highIndex">Ending index of the sub-array being processed.</param>
      /// <returns>Index for the partitioning element.</returns>
      private static int Partition(IComparable[] sortableItems, int lowIndex, int highIndex)
      {
         int i = lowIndex;
         int j = highIndex + 1;
         IComparable v = sortableItems[lowIndex];
         while (true)
         {
            // find item on lowIndex to swap
            while (SortingCommon.Less(sortableItems[++i], v))
            {
               if (i == highIndex)
               {
                  break;
               }
            }

            // find item on highIndex to swap
            while (SortingCommon.Less(v, sortableItems[--j]))
            {
               if (j == lowIndex)
               {
                  break;      // redundant since sortableItems[lowIndex] acts as sentinel
               }
            }

            // check if pointers cross
            if (i >= j)
            {
               break;
            }

            SortingCommon.Exch(sortableItems, i, j);
         }

         // put partitioning item v at sortableItems[j]
         SortingCommon.Exch(sortableItems, lowIndex, j);

         // now, sortableItems[lowIndex .. j-1] <= sortableItems[j] <= sortableItems[j+1 .. highIndex]
         return j;
      }

      /// <summary>
      /// Partition the sub-array sortableItems[lowIndex..highIndex] so that 
      /// sortableItems[lowIndex..j-1] less or equal to sortableItems[j] less or equal to 
      /// sortableItems[j+1..highIndex].
      /// </summary>
      /// <typeparam name="T">The type of items in the array.</typeparam>
      /// <param name="sortableItems">The array to be sorted.</param>
      /// <param name="comparerMethod">The comparer to be used for sorting.</param>
      /// <param name="lowIndex">Starting index of the sub-array being processed.</param>
      /// <param name="highIndex">Ending index of the sub-array being processed.</param>
      /// <returns>Index for the partitioning element.</returns>
      private static int Partition<T>(T[] sortableItems, IComparer<T> comparerMethod, int lowIndex, int highIndex)
      {
         int i = lowIndex;
         int j = highIndex + 1;
         T v = sortableItems[lowIndex];
         while (true)
         {
            // find item on lowIndex to swap
            while (SortingCommon.Less(comparerMethod, sortableItems[++i], v))
            {
               if (i == highIndex)
               {
                  break;
               }
            }

            // find item on highIndex to swap
            while (SortingCommon.Less(comparerMethod, v, sortableItems[--j]))
            {
               if (j == lowIndex)
               {
                  break;      // redundant since sortableItems[lowIndex] acts as sentinel
               }
            }

            // check if pointers cross
            if (i >= j)
            {
               break;
            }

            SortingCommon.Exch(sortableItems, i, j);
         }

         // put partitioning item v at sortableItems[j]
         SortingCommon.Exch(sortableItems, lowIndex, j);

         // now, sortableItems[lowIndex .. j-1] <= sortableItems[j] <= sortableItems[j+1 .. highIndex]
         return j;
      }
   }
}