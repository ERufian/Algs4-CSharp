//-----------------------------------------------------------------------
// <copyright file="QuickFindUFFunctionalTests.cs" company="Eusebio Rufian-Zilbermann">
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
   /// Functional Tests for the Quick Find Union Find class.
   /// </summary>
   [TestClass]
   public class QuickFindUFFunctionalTests
   {
      /// <summary>
      /// Perform a Quick Find on a medium text file.
      /// </summary>
      [TestCategory("Functional")]
      [TestMethod]
      public void QuickFindUnionFindMedium()
      {
         IUnionFind unionFind = new QuickFindUF();
         CommonUFFunctionalTests.UnionFindCommon("MediumUF.txt", unionFind);
         Assert.AreEqual(3, unionFind.Count);
      }
      
      /// <summary>
      /// Perform a Quick Find on a large text file.
      /// </summary>
      [TestCategory("Functional")]
      [TestMethod]
      [Ignore]
      public void QuickFindUnionFindLarge()
      {
         IUnionFind unionFind = new QuickFindUF();
         CommonUFFunctionalTests.UnionFindCommon("LargeUF.txt", unionFind);
         Assert.AreEqual(6, unionFind.Count);
      }
   }
}
