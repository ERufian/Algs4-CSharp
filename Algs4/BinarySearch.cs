//-----------------------------------------------------------------------
// <copyright file="BinarySearch.cs" company="Eusebio Rufian-Zilbermann">
//   Copyright (c) Eusebio Rufian-Zilbermann for the C# implementation
//   based on algorithms published by Robert Sedgewick and Kevin Wayne
// </copyright>
//-----------------------------------------------------------------------
namespace Algs4
{
   using System;

   /// <summary>
   /// The BinarySearch class provides a static method for binary
   /// searching for an integer in a sorted array of integers.
   /// <para></para>
   /// The rank operations takes logarithmic time in the worst case.
   /// <para></para>
   /// For additional documentation, see Section 1.1 of
   /// Algorithms, fourth Edition by Robert Sedgewick and Kevin Wayne.
   /// </summary>
   public static class BinarySearch
   {
      /// <summary>
      /// Searches for an integer key in a sorted array.
      /// </summary>
      /// <param name="key">The search key.</param>
      /// <param name="arrayToSearch">The array of integers, must be sorted in ascending order.</param>
      /// <returns>Index of key in array a[] if present, or -1 if not present.</returns>
      public static int Rank(int key, int[] arrayToSearch)
      {
         ArgumentValidator.CheckNotNull(arrayToSearch, "arrayToSearch");
         int lowIndex = 0;
         int highIndex = arrayToSearch.Length - 1;
         while (lowIndex <= highIndex)
         {
            // Key is in a[lowIndex..highIndex] or not present.
            int midIndex = lowIndex + ((highIndex - lowIndex) / 2);
            if (key < arrayToSearch[midIndex])
            {
               highIndex = midIndex - 1;
            }
            else if (key > arrayToSearch[midIndex])
            {
               lowIndex = midIndex + 1;
            }
            else
            {
               return midIndex;
            }
         }

         return -1;
      }
   }
}
