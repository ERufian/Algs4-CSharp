//-----------------------------------------------------------------------
// <copyright file="MergeX.cs" company="Eusebio Rufian-Zilbermann">
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
   /// The <tt>MergeX</tt> class provides methods for sorting an array 
   /// using an optimized merge sort (switching to insertion sort for small arrays).
   /// <para/>
   /// For additional documentation, see <a href="http://algs4.cs.princeton.edu/22mergesort">Section 2.2</a> of
   /// <i>Algorithms, fourth Edition</i> by Robert Sedgewick and Kevin Wayne.
   /// </summary>
   public class MergeX : ISortingAlgorithm
   {
      /// <summary>
      /// cutoff to insertion sort
      /// </summary>
      private const int CUTOFF = 7;

      #region Singleton
      /// <summary>
      /// The single Instance of the MergeX Sort Algorithm.
      /// </summary>
      private static readonly Lazy<MergeX> Singleton = new Lazy<MergeX>(() => new MergeX());

      /// <summary>
      /// Prevents a default instance of the <see cref="MergeX"/> class from being created.
      /// </summary>
      private MergeX()
      {
      }

      /// <summary>
      /// Gets the instance of the Sorting Algorithm, for polymorphism purposes.
      /// </summary>
      internal static MergeX Instance
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
         IComparable[] auxiliaryItems = (IComparable[])sortableItems.Clone();
         Sort(auxiliaryItems, sortableItems, 0, sortableItems.Length - 1);
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
         T[] auxiliaryItems = (T[])sortableItems.Clone();
         Sort(auxiliaryItems, sortableItems, comparerMethod, 0, sortableItems.Length - 1);
         Debug.Assert(SortingCommon.IsSorted(sortableItems, comparerMethod), "The array is not sorted");
      }

      /// <summary>
      /// Rearranges an array of items in ascending order, using the natural order.
      /// </summary>
      /// <param name="sortableItems">The array to be sorted.</param>
      /// <remarks>Unlike the static version, the instance version allows polymorphism.</remarks>
      void ISortingAlgorithm.Sort(IComparable[] sortableItems)
      {
         MergeX.Sort(sortableItems);
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
         MergeX.Sort(sortableItems, comparerMethod);
      }

      /// <summary>
      /// Stably merge sourceItems[lowIndex .. midIndex] with sourceItems[midIndex+1 ..highIndex] 
      /// into destinationItems[lowIndex .. highIndex] .
      /// </summary>
      /// <param name="sourceItems">The array to be sorted.</param>
      /// <param name="destinationItems">Destination array for sorted items.</param>
      /// <param name="lowIndex">Starting index of the sub-arrays being processed.</param>
      /// <param name="midIndex">Split point (middle index) of the sub-arrays being processed.</param>
      /// <param name="highIndex">Ending index of the sub-arrays being processed.</param>
      private static void MergeSubArrays(IComparable[] sourceItems, IComparable[] destinationItems, int lowIndex, int midIndex, int highIndex)
      {
         // precondition: sourceItems[lo .. mid] and sourceItems[mid+1 .. hi] are sorted subarrays
         Debug.Assert(SortingCommon.IsSorted(sourceItems, lowIndex, midIndex), "The array is not sorted");
         Debug.Assert(SortingCommon.IsSorted(sourceItems, midIndex + 1, highIndex), "The array is not sorted");

         int i = lowIndex, j = midIndex + 1;

         for (int k = lowIndex; highIndex >= k; k++)
         {
            if (midIndex < i)
            {
               destinationItems[k] = sourceItems[j++];
            }
            else if (highIndex < j)
            {
               destinationItems[k] = sourceItems[i++];
            }
            else if (SortingCommon.Less(sourceItems[j], sourceItems[i]))
            {
               destinationItems[k] = sourceItems[j++];   // to ensure stability
            }
            else
            {
               destinationItems[k] = sourceItems[i++];
            }
         }

         // postcondition: destinationItems[lo .. hi] is a sorted subarray
         Debug.Assert(SortingCommon.IsSorted(destinationItems, lowIndex, highIndex), "The array is not sorted");
      }

      /// <summary>
      /// Stably merge sourceItems[lowIndex .. midIndex] with sourceItems[midIndex+1 ..highIndex] 
      /// into destinationItems[lowIndex .. highIndex] .
      /// </summary>
      /// <typeparam name="T">The type of items in the array.</typeparam>
      /// <param name="sourceItems">The array to be sorted.</param>
      /// <param name="destinationItems">Destination array for sorted items.</param>
      /// <param name="comparerMethod">The comparer to be used for sorting.</param>
      /// <param name="lowIndex">Starting index of the sub-arrays being processed.</param>
      /// <param name="midIndex">Split point (middle index) of the sub-arrays being processed.</param>
      /// <param name="highIndex">Ending index of the sub-arrays being processed.</param>
      private static void MergeSubArrays<T>(T[] sourceItems, T[] destinationItems, IComparer<T> comparerMethod, int lowIndex, int midIndex, int highIndex)
      {
         // precondition: sourceItems[lo .. mid] and sourceItems[mid+1 .. hi] are sorted subarrays
         Debug.Assert(SortingCommon.IsSorted(sourceItems, comparerMethod, lowIndex, midIndex), "The array is not sorted");
         Debug.Assert(SortingCommon.IsSorted(sourceItems, comparerMethod, midIndex + 1, highIndex), "The array is not sorted");

         int i = lowIndex, j = midIndex + 1;

         for (int k = lowIndex; highIndex >= k; k++)
         {
            if (midIndex < i)
            {
               destinationItems[k] = sourceItems[j++];
            }
            else if (highIndex < j)
            {
               destinationItems[k] = sourceItems[i++];
            }
            else if (SortingCommon.Less(comparerMethod, sourceItems[j], sourceItems[i]))
            {
               destinationItems[k] = sourceItems[j++];   // to ensure stability
            }
            else
            {
               destinationItems[k] = sourceItems[i++];
            }
         }

         // postcondition: destinationItems[lo .. hi] is a sorted subarray
         Debug.Assert(SortingCommon.IsSorted(destinationItems, comparerMethod, lowIndex, highIndex), "The array is not sorted");
      }

      /// <summary>
      /// Merge sort sourceItems[lowIndex..highIndex] into destinationItems[lowIndex..highIndex].
      /// </summary>
      /// <param name="sourceItems">The array to be sorted.</param>
      /// <param name="destinationItems">Destination array for sorted items.</param>
      /// <param name="lowIndex">Starting index of the sub-array being processed.</param>
      /// <param name="highIndex">Ending index of the sub-array being processed.</param>
      private static void Sort(IComparable[] sourceItems, IComparable[] destinationItems, int lowIndex, int highIndex)
      {
         if (highIndex < CUTOFF || highIndex - CUTOFF <= lowIndex)
         {
            InsertionSort(destinationItems, lowIndex, highIndex);
            return;
         }

         // Since lowIndex is strictly smaller than highIndex we will not have an integer overflow
         // and midIndex will also be strictly smaller than highIndex (we can safely add 1 to it)
         int midIndex = lowIndex + ((highIndex - lowIndex) / 2);
         Sort(destinationItems, sourceItems, lowIndex, midIndex);
         Sort(destinationItems, sourceItems, midIndex + 1, highIndex);
         if (!SortingCommon.Less(sourceItems[midIndex + 1], sourceItems[midIndex]))
         {
            Array.Copy(sourceItems, lowIndex, destinationItems, lowIndex, highIndex - lowIndex + 1);
            return;
         }

         MergeSubArrays(sourceItems, destinationItems, lowIndex, midIndex, highIndex);
      }

      /// <summary>
      /// Merge sort sourceItems[lowIndex..highIndex] into destinationItems[lowIndex..highIndex].
      /// </summary>
      /// <typeparam name="T">The type of items in the array.</typeparam>
      /// <param name="sourceItems">The array to be sorted.</param>
      /// <param name="destinationItems">Destination array for sorted items.</param>
      /// <param name="comparerMethod">The comparer to be used for sorting.</param>
      /// <param name="lowIndex">Starting index of the sub-array being processed.</param>
      /// <param name="highIndex">Ending index of the sub-array being processed.</param>
      private static void Sort<T>(T[] sourceItems, T[] destinationItems, IComparer<T> comparerMethod, int lowIndex, int highIndex)
      {
         if (highIndex < CUTOFF || highIndex - CUTOFF <= lowIndex)
         {
            InsertionSort(destinationItems, comparerMethod, lowIndex, highIndex);
            return;
         }

         // Since lowIndex is strictly smaller than highIndex we will not have an integer overflow
         // and midIndex will also be strictly smaller than highIndex (we can safely add 1 to it)
         int midIndex = lowIndex + ((highIndex - lowIndex) / 2);
         Sort(destinationItems, sourceItems, comparerMethod, lowIndex, midIndex);
         Sort(destinationItems, sourceItems, comparerMethod, midIndex + 1, highIndex);
         if (!SortingCommon.Less(comparerMethod, sourceItems[midIndex + 1], sourceItems[midIndex]))
         {
            Array.Copy(sourceItems, lowIndex, destinationItems, lowIndex, highIndex - lowIndex + 1);
            return;
         }

         MergeSubArrays(sourceItems, destinationItems, comparerMethod, lowIndex, midIndex, highIndex);
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
   }
}