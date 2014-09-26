//-----------------------------------------------------------------------
// <copyright file="IUnionFind.cs" company="Eusebio Rufian-Zilbermann">
//   Copyright (c) Eusebio Rufian-Zilbermann for the C# implementation
//   based on materials published by Robert Sedgewick and Kevin Wayne
// </copyright>
//-----------------------------------------------------------------------
namespace Algs4
{
   using System;

   /// <summary>
   /// This interface allows the use of interchangeable union-find implementations
   /// It supports the <em>union</em> and <em>find</em> operations, along with
   /// methods for determining whether two objects are in the same component
   /// and the total number of components.
   /// <para></para>
   /// For additional documentation, see <a href="http://algs4.cs.princeton.edu/15uf">Section 1.5</a> of
   /// <i>Algorithms, fourth Edition</i> by Robert Sedgewick and Kevin Wayne.
   /// </summary>
   public interface IUnionFind
   {
      /// <summary>
      /// Gets the current count of components.
      /// </summary>
      int Count { get; }

      /// <summary>
      /// Determine if two sites are in the same component.
      /// </summary>
      /// <param name="siteP">The integer representing one site.</param>
      /// <param name="siteQ">The integer representing the other site.</param>
      /// <returns>True if both sites are in the same component, false otherwise.</returns>
      /// <exception cref="IndexOutOfRangeException">
      /// This exception is thrown if a site index is greater than the number of components
      /// </exception>
      /// <exception cref="ArgumentException">
      /// This exception is thrown if a site index is negative.
      /// </exception>
      bool Connected(int siteP, int siteQ);
      
      /// <summary>
      /// Returns the component identifier for the component containing a site. 
      /// </summary>
      /// <param name="site">The site to find.</param>
      /// <returns>The component containing the site to find.</returns>
      /// <exception cref="IndexOutOfRangeException">
      /// This exception is thrown if the site index is greater than the number of components
      /// </exception>
      /// <exception cref="ArgumentException">
      /// This exception is thrown if the site index is negative.
      /// </exception>
      int Find(int site);

      /// <summary>
      /// Merges the component containing siteP with the component containing siteQ
      /// </summary>
      /// <param name="siteP">The integer representing one site.</param>
      /// <param name="siteQ">The integer representing the other site.</param>
      /// <exception cref="IndexOutOfRangeException">
      /// This exception is thrown if a site index is greater than the number of components
      /// </exception>
      /// <exception cref="ArgumentException">
      /// This exception is thrown if a site index is negative.
      /// </exception>
      void Union(int siteP, int siteQ);

      /// <summary>
      /// Initialize components as isolated.
      /// This can be used to initialize the union-find or to reset (undo) all unions
      /// </summary>
      /// <param name="isolatedComponentCount">The initial number of components.</param>
      /// <exception cref="ArgumentException">
      /// This exception is thrown if the number of isolated components is negative.
      /// </exception>
      void IsolateComponents(int isolatedComponentCount);
   }
}
