//-----------------------------------------------------------------------
// <copyright file="QuickUnionUFUnitTests.cs" company="Eusebio Rufian-Zilbermann">
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
   /// Unit Tests for the Quick Union Union Find class.
   /// </summary>
   [TestClass]
   public class QuickUnionUFUnitTests
   {
      /// <summary>
      /// Perform a Quick Find on a small text file.
      /// </summary>
      [TestCategory("Unit")]
      [TestMethod]
      public void QuickUnionUnionFindTiny()
      {
         IUnionFind unionFind = new QuickUnionUF();
         CommonUFUnitTests.CommonUnionFindTiny(unionFind);
      }
   }
}