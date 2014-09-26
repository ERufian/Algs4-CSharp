//-----------------------------------------------------------------------
// <copyright file="InUnitTests.cs" company="Eusebio Rufian-Zilbermann">
//   Copyright (c) Eusebio Rufian-Zilbermann for the C# implementation
//   based on materials published by Robert Sedgewick and Kevin Wayne
// </copyright>
//-----------------------------------------------------------------------
namespace StdlibUnitTests
{
   using System;
   using System.Globalization;
   using System.IO;
   using Microsoft.VisualStudio.TestTools.UnitTesting;
   using Stdlib;

   /// <summary>
   /// Unit Tests for the In class.
   /// </summary>
   [TestClass]
   public class InUnitTests
   {
      /// <summary>
      /// URL location for the test file.
      /// </summary>
      private const string UrlName = "http://introcs.cs.princeton.edu/stdlib/InTest.txt";

      /// <summary>
      /// Expected text in the Test file (local or URL).
      /// </summary>
      private const string InTest = "This is a test file.\nHere    is line 2.\n";

      /// <summary>
      /// Expected text as individual lines (line separators excluded).
      /// </summary>
      private static readonly string[] InTestLines = InTest.Split("\n".ToCharArray());

      /// <summary>
      /// Expected text as individual words (whitespace excluded).
      /// </summary>
      private static readonly string[] InTestWords = InTest.Split("\n ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

      /// <summary>
      /// Expected text as individual characters (whitespace and line separators included).
      /// </summary>
      private static readonly char[] InTestChars = InTest.ToCharArray();

      /// <summary>
      /// Read one line at a time from URL.
      /// </summary>
      [TestMethod]
      public void ReadAllFromUrl()
      {
         using (In inObject = new In(UrlName))
         {
            string s = inObject.ReadAll();
            Assert.AreEqual(InUnitTests.InTest, s);
         }
      }

      /// <summary>
      /// Read one line at a time from URL.
      /// </summary>
      [TestMethod]
      public void ReadLineFromUrl()
      {
         using (In inObject = new In(UrlName))
         {
            int expectedIndex = 0;
            while (!inObject.IsEmpty())
            {
               string s = inObject.ReadLine();
               Assert.IsTrue(expectedIndex < InUnitTests.InTestLines.Length);
               Assert.AreEqual(InUnitTests.InTestLines[expectedIndex++], s);
            }
         }
      }

      /// <summary>
      /// Read one string at a time from URL.
      /// </summary>
      [TestMethod]
      public void ReadStringFromUrl()
      {
         using (In inObject = new In(UrlName))
         {
            int expectedIndex = 0;
            while (!inObject.IsEmpty())
            {
               string s = inObject.ReadString();
               Assert.IsTrue(expectedIndex < InUnitTests.InTestWords.Length);
               Assert.AreEqual(InUnitTests.InTestWords[expectedIndex++], s);
            }
         }
      }

      /// <summary>
      /// Read one line at a time from file in current directory.
      /// </summary>
      [TestMethod]
      public void ReadLineFromCurrentDirectory()
      {
         using (In inObject = new In("./InTest.txt"))
         {
            while (!inObject.IsEmpty())
            {
               int expectedIndex = 0;
               while (!inObject.IsEmpty())
               {
                  string s = inObject.ReadLine();
                  Assert.IsTrue(expectedIndex < InUnitTests.InTestLines.Length);
                  Assert.AreEqual(InUnitTests.InTestLines[expectedIndex++], s);
               }
            }
         }
      }

      /// <summary>
      /// Read one line at a time from file using relative path.
      /// </summary>
      [TestMethod]
      public void ReadLineFromRelativePath()
      {
         string relativeFile = string.Format(
            CultureInfo.InvariantCulture,
            "..\\{0}\\InTest.txt",
            Path.GetFileName(Directory.GetCurrentDirectory()));

         using (In inObject = new In(relativeFile))
         {
            int expectedIndex = 0;
            while (!inObject.IsEmpty())
            {
               string s = inObject.ReadLine();
               Assert.IsTrue(expectedIndex < InUnitTests.InTestLines.Length);
               Assert.AreEqual(InUnitTests.InTestLines[expectedIndex++], s);
            }
         }
      }

      /// <summary>
      /// Read one char at a time.
      /// </summary>
      [TestMethod]
      public void ReadCharFromFile()
      {
         using (In inObject = new In("InTest.txt"))
         {
            int expectedIndex = 0;
            while (!inObject.IsEmpty())
            {
               char c = inObject.ReadChar();
               Assert.IsTrue(expectedIndex < InUnitTests.InTestChars.Length);
               Assert.AreEqual(InUnitTests.InTestChars[expectedIndex++], c);
            }
         }
      }

      /// <summary>
      /// Read one line at a time from absolute Windows path.
      /// </summary>
      [TestMethod]
      public void ReadLineAbsoluteWindowsPath()
      {
         string currentDirectory = Directory.GetCurrentDirectory();
         string fullPath = Path.Combine(currentDirectory, "InTest.txt");
         using (In inObject = new In(fullPath))
         {
            int expectedIndex = 0;
            while (!inObject.IsEmpty())
            {
               string s = inObject.ReadLine();
               Assert.IsTrue(expectedIndex < InUnitTests.InTestLines.Length);
               Assert.AreEqual(InUnitTests.InTestLines[expectedIndex++], s);
            }
         }
      }
   }
}