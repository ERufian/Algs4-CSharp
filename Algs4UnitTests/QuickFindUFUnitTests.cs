//-----------------------------------------------------------------------
// <copyright file="QuickFindUFUnitTests.cs" company="Eusebio Rufian-Zilbermann">
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
   /// Unit Tests for the Quick Find Union Find class.
   /// </summary>
   [TestClass]
   public class QuickFindUFUnitTests
   {
      /// <summary>
      /// Perform a Quick Find on a small text file.
      /// </summary>
      [TestCategory("Unit")]
      [TestMethod]
      public void QuickFindUnionFindTiny()
      {
         IUnionFind unionFind = new QuickFindUF();
         CommonUFUnitTests.CommonUnionFindTiny(unionFind);
      }
   }
}
