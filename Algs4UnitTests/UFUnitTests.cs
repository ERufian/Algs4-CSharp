//-----------------------------------------------------------------------
// <copyright file="UFUnitTests.cs" company="Eusebio Rufian-Zilbermann">
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
   /// Functional Tests for the Quick Union Union Find class.
   /// </summary>
   [TestClass]
   public class UFUnitTests
   {
      /// <summary>
      /// Perform a Quick Find on a small text file.
      /// </summary>
      [TestCategory("Unit")]
      [TestMethod]
      public void UnionFindTiny()
      {
         IUnionFind unionFind = new UF();
         CommonUFUnitTests.CommonUnionFindTiny(unionFind);
      }
   }
}
