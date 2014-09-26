//-----------------------------------------------------------------------
// <copyright file="CommonUFFunctionalTests.cs" company="Eusebio Rufian-Zilbermann">
//   Copyright (c) Eusebio Rufian-Zilbermann for the C# implementation
//   based on materials published by Robert Sedgewick and Kevin Wayne
// </copyright>
//-----------------------------------------------------------------------
namespace Algs4FunctionalTests
{
   using System;
   using Algs4;
   using Stdlib;

   /// <summary>
   /// Common methods for Union Find tests
   /// </summary>
   public static class CommonUFFunctionalTests
   {
      /// <summary>
      /// Common portion of file input UnionFind tests.
      /// </summary>
      /// <param name="streamName">The name for the input stream.</param>
      /// <param name="unionFind">The union find to be tested.</param>
      /// <returns>The Union Find structure.</returns>
      public static IUnionFind QuickFindUnionFindCommon(string streamName, IUnionFind unionFind)
      {
         if (null == streamName)
         {
            throw new ArgumentNullException("streamName");
         }

         if (null == unionFind)
         {
            throw new ArgumentNullException("unionFind");
         }

         using (In input = new In(streamName))
         {
            int initialComponentCount = input.ReadInt();
            unionFind.IsolateComponents(initialComponentCount);
            while (!input.IsEmpty())
            {
               int siteP = input.ReadInt();
               int siteQ = input.ReadInt();
               if (unionFind.Connected(siteP, siteQ))
               {
                  continue;
               }

               unionFind.Union(siteP, siteQ);
            }

            return unionFind;
         }
      }
   }
}
