//-----------------------------------------------------------------------
// <copyright file="Merge.cs" company="Eusebio Rufian-Zilbermann">
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
   /// The <tt>Merge</tt> class provides methods for sorting an
   /// array using merge sort.
   /// <para/>
   /// For additional documentation, see <a href="http://algs4.cs.princeton.edu/22mergesort">Section 2.2</a> of
   /// <i>Algorithms, fourth Edition</i> by Robert Sedgewick and Kevin Wayne.
   /// For an optimized version, see MergeX.
   /// </summary>
   public class Merge : ISortingAlgorithm
   {
      #region Singleton
      /// <summary>
      /// The single Instance of the Merge Sort Algorithm.
      /// </summary>
      private static readonly Lazy<Merge> Singleton = new Lazy<Merge>(() => new Merge());

      /// <summary>
      /// Prevents a default instance of the <see cref="Merge"/> class from being created.
      /// </summary>
      private Merge()
      {
      }

      /// <summary>
      /// Gets the instance of the Sorting Algorithm, for polymorphism purposes.
      /// </summary>
      internal static Merge Instance
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
         IComparable[] auxiliaryItems = new IComparable[sortableItems.Length];
         Sort(sortableItems, auxiliaryItems, 0, sortableItems.Length - 1);
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
         T[] auxiliaryItems = new T[sortableItems.Length];
         Sort(sortableItems, auxiliaryItems, comparerMethod, 0, sortableItems.Length - 1);
         Debug.Assert(SortingCommon.IsSorted(sortableItems, comparerMethod), "The array is not sorted");
      }

      /// <summary>
      /// Returns a permutation that gives the elements in the array in ascending order. 
      /// </summary>
      /// <param name="sortableItems">The array to be sorted.</param>
      /// <returns>
      /// A permutation <tt>p[]</tt> such that <tt>sortableItems[p[0]]</tt>, <tt>sortableItems[p[1]]</tt>, ..., 
      /// <tt>sortableItems[p[itemCount-1]]</tt> are in ascending order
      /// </returns>
      public static int[] IndexSort(IComparable[] sortableItems)
      {
         ArgumentValidator.CheckNotNull(sortableItems, "sortableItems");
         int itemCount = sortableItems.Length;
         int[] index = new int[itemCount];
         for (int i = 0; itemCount > i; i++)
         {
            index[i] = i;
         }

         int[] auxiliaryItems = new int[itemCount];
         Sort(sortableItems, index, auxiliaryItems, 0, itemCount - 1);
         return index;
      }

      /// <summary>
      /// Rearranges an array of items in ascending order, using the natural order.
      /// </summary>
      /// <param name="sortableItems">The array to be sorted.</param>
      /// <remarks>Unlike the static version, the instance version allows polymorphism.</remarks>
      void ISortingAlgorithm.Sort(IComparable[] sortableItems)
      {
         Merge.Sort(sortableItems);
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
         Merge.Sort(sortableItems, comparerMethod);
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
      /// Merge sort sortableItems[lowIndex..highIndex] using auxiliary array auxiliaryItems[lowIndex..highIndex].
      /// </summary>
      /// <param name="sortableItems">The array to be sorted.</param>
      /// <param name="auxiliaryItems">Storage for a temporary copy of items being sorted.</param>
      /// <param name="lowIndex">Starting index of the sub-arrays being processed.</param>
      /// <param name="highIndex">Ending index of the sub-arrays being processed.</param>
      private static void Sort(IComparable[] sortableItems, IComparable[] auxiliaryItems, int lowIndex, int highIndex)
      {
         if (highIndex <= lowIndex)
         {
            return;
         }

         // Since lowIndex is strictly smaller than highIndex we will not have an integer overflow
         // and midIndex will also be strictly smaller than highIndex (we can safely add 1 to it)
         int midIndex = lowIndex + ((highIndex - lowIndex) / 2);
         Sort(sortableItems, auxiliaryItems, lowIndex, midIndex);
         Sort(sortableItems, auxiliaryItems, midIndex + 1, highIndex);
         MergeSubArrays(sortableItems, auxiliaryItems, lowIndex, midIndex, highIndex);
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

      /// <summary>
      /// Merge sort sortableItems[lowIndex..highIndex] using auxiliary array auxiliaryItems[lowIndex..highIndex].
      /// </summary>
      /// <typeparam name="T">The type of items in the array.</typeparam>
      /// <param name="sortableItems">The array to be sorted.</param>
      /// <param name="auxiliaryItems">Storage for a temporary copy of items being sorted.</param>
      /// <param name="comparerMethod">The comparer to be used for sorting.</param>
      /// <param name="lowIndex">Starting index of the sub-arrays being processed.</param>
      /// <param name="highIndex">Ending index of the sub-arrays being processed.</param>
      private static void Sort<T>(T[] sortableItems, T[] auxiliaryItems, IComparer<T> comparerMethod, int lowIndex, int highIndex)
      {
         if (highIndex <= lowIndex)
         {
            return;
         }

         // Since lowIndex is strictly smaller than highIndex we will not have an integer overflow
         // and midIndex will also be strictly smaller than highIndex (we can safely add 1 to it)
         int midIndex = lowIndex + ((highIndex - lowIndex) / 2);
         Sort(sortableItems, auxiliaryItems, comparerMethod, lowIndex, midIndex);
         Sort(sortableItems, auxiliaryItems, comparerMethod, midIndex + 1, highIndex);
         MergeSubArrays(sortableItems, auxiliaryItems, comparerMethod, lowIndex, midIndex, highIndex);
      }

      /// <summary>
      /// Stably merge sortableItems[lowIndex .. midIndex] with sortableItems[midIndex+1 .. highIndex] 
      /// using auxiliaryItems[lowIndex .. highIndex].
      /// </summary>
      /// <param name="sortableItems">The array to be sorted.</param>
      /// <param name="index">Index permutation that gives the elements in the array in ascending order.</param>
      /// <param name="auxiliaryItems">Storage for a temporary copy of items being sorted.</param>
      /// <param name="lowIndex">Starting index of the sub-arrays being processed.</param>
      /// <param name="midIndex">Split point (middle index) of the sub-arrays being processed.</param>
      /// <param name="highIndex">Ending index of the sub-arrays being processed.</param>
      private static void MergeSubArrays(IComparable[] sortableItems, int[] index, int[] auxiliaryItems, int lowIndex, int midIndex, int highIndex)
      {
         // copy to auxiliaryItems[]
         for (int k = lowIndex; highIndex >= k; k++)
         {
            auxiliaryItems[k] = index[k];
         }

         // merge back to sortableItems[]
         int i = lowIndex, j = midIndex + 1;
         for (int k = lowIndex; highIndex >= k; k++)
         {
            if (midIndex < i)
            {
               index[k] = auxiliaryItems[j++];
            }
            else if (highIndex < j)
            {
               index[k] = auxiliaryItems[i++];
            }
            else if (SortingCommon.Less(sortableItems[auxiliaryItems[j]], sortableItems[auxiliaryItems[i]]))
            {
               index[k] = auxiliaryItems[j++];
            }
            else
            {
               index[k] = auxiliaryItems[i++];
            }
         }
      }

      /// <summary>
      /// Merge sort sortableItems[lowIndex..highIndex] using auxiliary array auxiliaryItems[lowIndex..highIndex]
      /// </summary>
      /// <param name="sortableItems">The array to be sorted.</param>
      /// <param name="index">Index permutation that gives the elements in the array in ascending order.</param>
      /// <param name="auxiliaryItems">Storage for a temporary copy of items being sorted.</param>
      /// <param name="lowIndex">Starting index of the sub-arrays being processed.</param>
      /// <param name="highIndex">Ending index of the sub-arrays being processed.</param>
      private static void Sort(IComparable[] sortableItems, int[] index, int[] auxiliaryItems, int lowIndex, int highIndex)
      {
         if (highIndex <= lowIndex)
         {
            return;
         }

         // Since lowIndex is strictly smaller than highIndex we will not have an integer overflow
         // and midIndex will also be strictly smaller than highIndex (we can safely add 1 to it)
         int midIndex = lowIndex + ((highIndex - lowIndex) / 2);
         Sort(sortableItems, index, auxiliaryItems, lowIndex, midIndex);
         Sort(sortableItems, index, auxiliaryItems, midIndex + 1, highIndex);
         MergeSubArrays(sortableItems, index, auxiliaryItems, lowIndex, midIndex, highIndex);
      }
   }
}