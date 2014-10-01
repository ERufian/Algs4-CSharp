//-----------------------------------------------------------------------
// <copyright file="MergeBU.cs" company="Eusebio Rufian-Zilbermann">
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
   /// The <tt>MergeBU</tt> class provides methods for sorting an
   /// array using bottom-up merge sort.
   /// <para/>
   /// For additional documentation, see <a href="http://algs4.cs.princeton.edu/22mergesort">Section 2.2</a> of
   /// <i>Algorithms, fourth Edition</i> by Robert Sedgewick and Kevin Wayne.
   /// </summary>
   public class MergeBU : ISortingAlgorithm
   {
      #region Singleton
      /// <summary>
      /// The single Instance of the MergeBU Sort Algorithm.
      /// </summary>
      private static readonly Lazy<MergeBU> Singleton = new Lazy<MergeBU>(() => new MergeBU());

      /// <summary>
      /// Prevents a default instance of the <see cref="MergeBU"/> class from being created.
      /// </summary>
      private MergeBU()
      {
      }

      /// <summary>
      /// Gets the instance of the Sorting Algorithm, for polymorphism purposes.
      /// </summary>
      internal static MergeBU Instance
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
         int itemCount = sortableItems.Length;
         IComparable[] auxiliaryItems = new IComparable[itemCount];
         for (int n = 1; itemCount > n; n = n + n)
         {
            for (int i = 0; itemCount - n > i; i += n + n)
            {
               int lowIndex = i;
               int midIndex = i + n - 1;
               int highIndex = Math.Min(i + n + n - 1, itemCount - 1);
               MergeSubArrays(sortableItems, auxiliaryItems, lowIndex, midIndex, highIndex);
            }
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
         int itemCount = sortableItems.Length;
         T[] auxiliaryItems = new T[itemCount];
         for (int n = 1; itemCount > n; n = n + n)
         {
            for (int i = 0; itemCount - n > i; i += n + n)
            {
               int lowIndex = i;
               int midIndex = i + n - 1;
               int highIndex = Math.Min(i + n + n - 1, itemCount - 1);
               MergeSubArrays(sortableItems, auxiliaryItems, comparerMethod, lowIndex, midIndex, highIndex);
            }
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
         MergeBU.Sort(sortableItems);
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
         MergeBU.Sort(sortableItems, comparerMethod);
      }

      /// <summary>
      /// Stably merge sortableItems[lowIndex .. midIndex] with sortableItems[midIndex+1 ..highIndex] 
      /// using auxiliaryItems[lowIndex .. highIndex] .
      /// </summary>
      /// <param name="sortableItems">The array to be sorted.</param>
      /// <param name="auxiliaryItems">Storage for a temporary copy of items being sorted.</param>
      /// <param name="lowIndex">Starting index of the sub-arrays being processed.</param>
      /// <param name="midIndex">Split point (middle index) of the sub-arrays being processed.</param>
      /// <param name="highIndex">Ending index of the sub-arrays being processed.</param>
      private static void MergeSubArrays(IComparable[] sortableItems, IComparable[] auxiliaryItems, int lowIndex, int midIndex, int highIndex)
      {
         // preconditions:
         // highIndex is strictly higher than midIndex and LowIndex
         // sortableItems[lowIndex .. midIndex] and sortableItems[midIndex+1 .. highIndex] are sorted sub-arrays
         Debug.Assert(highIndex > lowIndex, "lowIndex must be strictly less than HighIndex");
         Debug.Assert(highIndex > midIndex, "midIndex must be strictly less than HighIndex");
         Debug.Assert(SortingCommon.IsSorted(sortableItems, lowIndex, midIndex), "The array is not sorted");
         Debug.Assert(SortingCommon.IsSorted(sortableItems, midIndex + 1, highIndex), "The array is not sorted");

         // copy to auxiliaryItems[]
         for (int k = lowIndex; highIndex >= k; k++)
         {
            auxiliaryItems[k] = sortableItems[k];
         }

         // merge back to sortableItems[]
         int i = lowIndex, j = midIndex + 1;
         for (int k = lowIndex; highIndex >= k; k++)
         {
            if (midIndex < i)
            {
               //// sortableItems[k] = auxiliaryItems[j++];   // this copying is unnecessary
            }
            else if (highIndex < j)
            {
               sortableItems[k] = auxiliaryItems[i++];
            }
            else if (SortingCommon.Less(auxiliaryItems[j], auxiliaryItems[i]))
            {
               sortableItems[k] = auxiliaryItems[j++];
            }
            else
            {
               sortableItems[k] = auxiliaryItems[i++];
            }
         }

         // postcondition: sortableItems[lowIndex .. highIndex] is sorted
         Debug.Assert(SortingCommon.IsSorted(sortableItems, lowIndex, highIndex), "The array is not sorted");
      }

      /// <summary>
      /// Stably merge sortableItems[lowIndex .. midIndex] with sortableItems[midIndex+1 ..highIndex] 
      /// using auxiliaryItems[lowIndex .. highIndex] .
      /// </summary>
      /// <typeparam name="T">The type of items in the array.</typeparam>
      /// <param name="sortableItems">The array to be sorted.</param>
      /// <param name="auxiliaryItems">Storage for a temporary copy of items being sorted.</param>
      /// <param name="comparerMethod">The comparer to be used for sorting.</param>
      /// <param name="lowIndex">Starting index of the sub-arrays being processed.</param>
      /// <param name="midIndex">Split point (middle index) of the sub-arrays being processed.</param>
      /// <param name="highIndex">Ending index of the sub-arrays being processed.</param>
      private static void MergeSubArrays<T>(T[] sortableItems, T[] auxiliaryItems, IComparer<T> comparerMethod, int lowIndex, int midIndex, int highIndex)
      {
         // preconditions:
         // highIndex is strictly higher than midIndex and LowIndex
         // sortableItems[lowIndex .. midIndex] and sortableItems[midIndex+1 .. highIndex] are sorted sub-arrays
         Debug.Assert(highIndex > lowIndex, "lowIndex must be strictly less than HighIndex");
         Debug.Assert(highIndex > midIndex, "midIndex must be strictly less than HighIndex");
         Debug.Assert(SortingCommon.IsSorted(sortableItems, comparerMethod, lowIndex, midIndex), "The array is not sorted");
         Debug.Assert(SortingCommon.IsSorted(sortableItems, comparerMethod, midIndex + 1, highIndex), "The array is not sorted");

         // copy to auxiliaryItems[]
         for (int k = lowIndex; highIndex >= k; k++)
         {
            auxiliaryItems[k] = sortableItems[k];
         }

         // merge back to sortableItems[]
         int i = lowIndex, j = midIndex + 1;
         for (int k = lowIndex; highIndex >= k; k++)
         {
            if (midIndex < i)
            {
               //// sortableItems[k] = auxiliaryItems[j++];   // this copying is unnecessary
            }
            else if (highIndex < j)
            {
               sortableItems[k] = auxiliaryItems[i++];
            }
            else if (SortingCommon.Less(comparerMethod, auxiliaryItems[j], auxiliaryItems[i]))
            {
               sortableItems[k] = auxiliaryItems[j++];
            }
            else
            {
               sortableItems[k] = auxiliaryItems[i++];
            }
         }

         // postcondition: sortableItems[lowIndex .. highIndex] is sorted
         Debug.Assert(SortingCommon.IsSorted(sortableItems, comparerMethod, lowIndex, highIndex), "The array is not sorted");
      }
   }
}