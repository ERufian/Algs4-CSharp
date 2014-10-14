//-----------------------------------------------------------------------
// <copyright file="QuickUnionUF.cs" company="Eusebio Rufian-Zilbermann">
//   Copyright (c) Eusebio Rufian-Zilbermann for the C# implementation
//   based on algorithms published by Robert Sedgewick and Kevin Wayne
// </copyright>
//-----------------------------------------------------------------------
namespace Algs4
{
   using System;

   /// <summary>
   /// The <tt>QuickUnionUF</tt> class represents a union-find data structure.
   /// It supports the <em>union</em> and <em>find</em> operations, along with
   /// methods for determining whether two objects are in the same component
   /// and the total number of components.
   /// <para></para>
   /// This implementation uses quick union.
   /// Initializing a data structure with <em>N</em> objects takes linear time.
   /// Afterwards, <em>union</em>, <em>find</em>, and <em>connected</em> take
   /// time linear time (in the worst case) and <em>count</em> takes constant
   /// time.
   /// <para></para>
   /// For additional documentation, see <a href="http://algs4.cs.princeton.edu/15uf">Section 1.5</a> of
   /// <i>Algorithms, fourth Edition</i> by Robert Sedgewick and Kevin Wayne.
   /// </summary>
   public class QuickUnionUF : IUnionFind
   {
      /// <summary>
      /// Component identifiers.
      /// </summary>
      private int[] componentIdentifier;

      /// <summary>
      /// Initializes a new instance of the <see cref="QuickUnionUF"/> class 
      /// where all the sites are disconnected (belong to different components).
      /// </summary>
      /// <param name="isolatedComponentCount">The initial number of components.</param>
      /// <exception cref="ArgumentException">
      /// This exception is thrown if the number of isolated components is negative.
      /// </exception>
      public QuickUnionUF(int isolatedComponentCount)
      {
         this.IsolateComponents(isolatedComponentCount);
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="QuickUnionUF"/> class
      /// where componentIdentifier is not initialized yet.
      /// IsolateComponents must be invoked before using any of the union find API methods
      /// </summary>
      /// <remarks>
      /// This constructor is meant to be used for internal testing only
      /// </remarks>
      public QuickUnionUF()
      {
      }

      /// <summary>
      /// Gets the current count of components.
      /// </summary>
      public int Count
      {
         get;
         private set;
      }

      /// <summary>
      /// Initialize components as isolated.
      /// This can be used to initialize the union-find or to reset (undo) all unions
      /// </summary>
      /// <param name="isolatedComponentCount">The initial number of components.</param>
      /// <exception cref="ArgumentException">
      /// This exception is thrown if the number of isolated components is negative.
      /// </exception>
      public void IsolateComponents(int isolatedComponentCount)
      {
         if (0 > isolatedComponentCount)
         {
            throw new ArgumentException("Component Count should not be negative", "isolatedComponentCount");
         }

         this.Count = isolatedComponentCount;
         this.componentIdentifier = new int[isolatedComponentCount];
         for (int i = 0; i < isolatedComponentCount; i++)
         {
            this.componentIdentifier[i] = i;
         }
      }

      /// <summary>
      /// Returns the component identifier for the component containing a site. 
      /// </summary>
      /// <param name="site">The site to find.</param>
      /// <returns>The component containing the site to find.</returns>
      /// <exception cref="ArgumentException">
      /// This exception is thrown if the site index is negative or greater than the number of components.
      /// </exception>
      public int Find(int site)
      {
         if (0 > site || this.componentIdentifier.Length < site)
         {
            throw new ArgumentException("Site out of range.", "site");
         }

         while (site != this.componentIdentifier[site])
         {
            site = this.componentIdentifier[site];            
            if (0 > site || this.componentIdentifier.Length < site)
            {
               throw new ArgumentException("Site out of range.", "site");
            }
         }

         return site;
      }

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
      public bool Connected(int siteP, int siteQ)
      {
         if (0 > siteP)
         {
            throw new ArgumentException("Site Index should not be negative", "siteP");
         }

         if (0 > siteQ)
         {
            throw new ArgumentException("Site Index should not be negative", "siteQ");
         }

         return this.Find(siteP) == this.Find(siteQ);
      }

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
      public void Union(int siteP, int siteQ)
      {
         if (0 > siteP)
         {
            throw new ArgumentException("Site Index should not be negative", "siteP");
         }

         if (0 > siteQ)
         {
            throw new ArgumentException("Site Index should not be negative", "siteQ");
         }

         int rootP = this.Find(siteP);
         int rootQ = this.Find(siteQ);
         if (rootP == rootQ)
         {
            return;
         }

         this.componentIdentifier[rootP] = rootQ;
         this.Count--;
      }
   }
}
