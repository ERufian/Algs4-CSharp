//-----------------------------------------------------------------------
// <copyright file="SortingCommon.cs" company="Eusebio Rufian-Zilbermann">
//   Copyright (c) Eusebio Rufian-Zilbermann for the C# implementation
//   based on materials published by Robert Sedgewick and Kevin Wayne
// </copyright>
//-----------------------------------------------------------------------
namespace Algs4
{
   using System;
   using System.Collections.Generic;

   /// <summary>
   /// Common functions for sorting algorithms.
   /// </summary>
   public static class SortingCommon 
   {     
      /// <summary>
      /// Determine if an item V is smaller when compared to an item W using the natural comparison.
      /// </summary>
      /// <param name="itemV">The first item to compare.</param>
      /// <param name="itemW">The second item to compare.</param>
      /// <returns>True if v is less than w, false otherwise.</returns>
      public static bool Less(IComparable itemV, IComparable itemW)
      {
         ArgumentValidator.CheckNotNull(itemV, "itemV");
         ArgumentValidator.CheckNotNull(itemW, "itemW");
         return 0 > itemV.CompareTo(itemW);
      }

      /// <summary>
      /// Determine if an item V is smaller than item W when using a specified comparer.
      /// </summary>
      /// <typeparam name="T">The type of objects being compared.</typeparam>
      /// <param name="comparerMethod">The method for comparing the two items.</param>
      /// <param name="itemV">The first item to compare.</param>
      /// <param name="itemW">The second item to compare.</param>
      /// <returns>True if v is less than w, false otherwise.</returns>
      public static bool Less<T>(IComparer<T> comparerMethod, T itemV, T itemW)
      {
         ArgumentValidator.CheckNotNull(comparerMethod, "comparerMethod");
         return 0 > comparerMethod.Compare(itemV, itemW);
      }

      /// <summary>
      /// Exchange two items in an array.
      /// </summary>
      /// <typeparam name="T">The type of items being exchanged.</typeparam>
      /// <param name="items">The array of items where to perform the exchange.</param>
      /// <param name="indexI">The index of the first item to exchange.</param>
      /// <param name="indexJ">The index of the second item to exchange.</param>
      public static void Exch<T>(T[] items, int indexI, int indexJ)
      {
         ArgumentValidator.CheckNotNull(items, "items");

         T swap = items[indexI];
         items[indexI] = items[indexJ];
         items[indexJ] = swap;
      }

      /// <summary>
      /// Check if array is sorted using the natural order.
      /// </summary>
      /// <param name="sortableItems">An array of sortable items.</param>
      /// <returns>True if the array is sorted, false otherwise.</returns>
      /// <remarks>Useful for debugging.</remarks>
      public static bool IsSorted(IComparable[] sortableItems)
      {
         ArgumentValidator.CheckNotNull(sortableItems, "sortableItems");
         return IsSorted(sortableItems, 0, sortableItems.Length - 1);
      }

      /// <summary>
      /// Check if a subset of an array is sorted using the natural order.
      /// </summary>
      /// <param name="sortableItems">An array of sortable items.</param>
      /// <param name="lowIndex">The index where the subset starts.</param>
      /// <param name="highIndex">The index where the subset ends.</param>
      /// <returns>True if the subset is sorted, false otherwise.</returns>
      public static bool IsSorted(IComparable[] sortableItems, int lowIndex, int highIndex)
      {
         ArgumentValidator.CheckNotNull(sortableItems, "sortableItems");

         if (int.MaxValue == lowIndex)
         {
            throw new ArgumentOutOfRangeException("lowIndex");
         }

         for (int i = lowIndex + 1; i <= highIndex; i++)
         {
            if (Less(sortableItems[i], sortableItems[i - 1]))
            {
               return false;
            }
         }

         return true;
      }

      /// <summary>
      /// Check if array is sorted using a specified comparer.
      /// </summary>
      /// <typeparam name="T">The type of objects being sorted</typeparam>
      /// <param name="sortableItems">An array of sortable items.</param>
      /// <param name="comparerMethod">The comparer to use for sorting.</param>
      /// <returns>True if the array is sorted, false otherwise.</returns>
      public static bool IsSorted<T>(T[] sortableItems, IComparer<T> comparerMethod)
      {
         ArgumentValidator.CheckNotNull(sortableItems, "sortableItems");

         return IsSorted(sortableItems, comparerMethod, 0, sortableItems.Length - 1);
      }

      /// <summary>
      /// Check if a subset of an array is sorted using a specified comparer.
      /// </summary>
      /// <typeparam name="T">The type of objects being sorted</typeparam>
      /// <param name="sortableItems">An array of sortable items.</param>
      /// <param name="comparerMethod">The comparer to use for sorting.</param>
      /// <param name="lowIndex">The index where the subset starts.</param>
      /// <param name="highIndex">The index where the subset ends.</param>
      /// <returns>True if the subset is sorted, false otherwise.</returns>
      public static bool IsSorted<T>(T[] sortableItems, IComparer<T> comparerMethod, int lowIndex, int highIndex)
      {
         ArgumentValidator.CheckNotNull(sortableItems, "sortableItems");
         if (int.MaxValue == lowIndex)
         {
            throw new ArgumentOutOfRangeException("lowIndex");
         }

         for (int i = lowIndex + 1; i <= highIndex; i++)
         {
            if (Less(comparerMethod, sortableItems[i], sortableItems[i - 1]))
            {
               return false;
            }
         }

         return true;
      }

      /// <summary>
      /// Check if an array is h-sorted using the natural order.
      /// </summary>
      /// <param name="sortableItems">An array of sortable items.</param>
      /// <param name="h">Interleave index for extracting the items that should be sorted</param>
      /// <returns>True if the array is sorted, false otherwise.</returns>
      internal static bool IsHSorted(IComparable[] sortableItems, int h)
      {
         for (int i = h; sortableItems.Length > i; i++)
         {
            if (Less(sortableItems[i], sortableItems[i - h]))
            {
               return false;
            }
         }

         return true;
      }

      /// <summary>
      /// Check if an array is h-sorted using a specified comparer.
      /// </summary>
      /// <typeparam name="T">The type of objects being sorted</typeparam>
      /// <param name="sortableItems">An array of sortable items.</param>
      /// <param name="comparerMethod">The comparer to use for sorting.</param>
      /// <param name="h">Interleave index for extracting the items that should be sorted</param>
      /// <returns>True if the array is sorted, false otherwise.</returns>
      internal static bool IsHSorted<T>(T[] sortableItems, IComparer<T> comparerMethod, int h)
      {
         for (int i = h; sortableItems.Length > i; i++)
         {
            if (Less(comparerMethod, sortableItems[i], sortableItems[i - h]))
            {
               return false;
            }
         }

         return true;
      }
   }
}
