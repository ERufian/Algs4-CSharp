//-----------------------------------------------------------------------
// <copyright file="CommonUFUnitTests.cs" company="Eusebio Rufian-Zilbermann">
//   Copyright (c) Eusebio Rufian-Zilbermann for the C# implementation
//   based on materials published by Robert Sedgewick and Kevin Wayne
// </copyright>
//-----------------------------------------------------------------------
namespace Algs4UnitTests
{
   using Algs4;
   using Microsoft.VisualStudio.TestTools.UnitTesting;
   using Stdlib;

   /// <summary>
   /// Common methods for testing Union Find classes.
   /// </summary>
   public static class CommonUFUnitTests
   {
      /// <summary>
      /// Perform a Quick Find on a small text file.
      /// </summary>
      /// <param name="unionFind">The union find to be tested.</param>
      internal static void CommonUnionFindTiny(IUnionFind unionFind)
      {
         using (In input = new In("TinyUF.txt"))
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
         }

         Assert.AreEqual(2, unionFind.Count);
      }
   }
}
