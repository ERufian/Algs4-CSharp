//-----------------------------------------------------------------------
// <copyright file="UFFunctionalTests.cs" company="Eusebio Rufian-Zilbermann">
//   Copyright (c) Eusebio Rufian-Zilbermann for the C# implementation
//   based on materials published by Robert Sedgewick and Kevin Wayne
// </copyright>
//-----------------------------------------------------------------------
namespace Algs4FunctionalTests
{
   using Algs4;
   using Microsoft.VisualStudio.TestTools.UnitTesting;
   using Stdlib;

   /// <summary>
   /// Functional Tests for the Union Find class.
   /// </summary>
   [TestClass]
   public class UFFunctionalTests
   {
      /// <summary>
      /// Perform a Quick Find on a medium text file.
      /// </summary>
      [TestCategory("Functional")]
      [TestMethod]
      public void QuickUnionUnionFindMedium()
      {
         IUnionFind unionFind = new UF();
         CommonUFFunctionalTests.UnionFindCommon("MediumUF.txt", unionFind);
         Assert.AreEqual(3, unionFind.Count);
      }

      /// <summary>
      /// Perform a Quick Find on a large text file.
      /// </summary>
      [TestCategory("Functional")]
      [TestMethod]
      [Ignore]
      public void QuickUnionUnionFindLarge()
      {
         IUnionFind unionFind = new UF();
         CommonUFFunctionalTests.UnionFindCommon("LargeUF.txt", unionFind);
         Assert.AreEqual(6, unionFind.Count);
      }
   }
}
