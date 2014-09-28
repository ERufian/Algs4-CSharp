//-----------------------------------------------------------------------
// <copyright file="ISortingAlgorithm.cs" company="Eusebio Rufian-Zilbermann">
//   Copyright (c) Eusebio Rufian-Zilbermann for the C# implementation
//   based on algorithms published by Robert Sedgewick and Kevin Wayne
// </copyright>
//-----------------------------------------------------------------------
namespace Algs4
{
   using System;

   /// <summary>
   /// The <tt>ISortingAlgorithm</tt> interface specifies methods for sorting an
   /// array.
   /// <para/>
   /// For additional documentation, see <a href="http://algs4.cs.princeton.edu/21elementary">Section 2.1</a> of
   /// <i>Algorithms, fourth Edition</i> by Robert Sedgewick and Kevin Wayne.
   /// </summary>
   public interface ISortingAlgorithm
   {
      /// <summary>
      /// Rearranges an array of items in ascending order, using the natural order.
      /// </summary>
      /// <param name="sortableItems">The array to be sorted.</param>
      /// <remarks>
      /// This method provides the benefit of polymorphism and allows using interchangeable algorithms.
      /// </remarks>
      void Sort(IComparable[] sortableItems);
   }
}
