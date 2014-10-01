//-----------------------------------------------------------------------
// <copyright file="ValidatedNotNullAttribute.cs" company="Eusebio Rufian-Zilbermann">
//   Copyright (c) Eusebio Rufian-Zilbermann for the C# implementation
//   based on algorithms published by Robert Sedgewick and Kevin Wayne
// </copyright>
//-----------------------------------------------------------------------
namespace Algs4
{
   using System;

   /// <summary>
   /// Attribute for notifying static analysis tools that an argument has been validated as not null.
   /// see <a href="http://esmithy.net/2011/03/15/suppressing-ca1062/">Eric Smith's article about this.</a>
   /// </summary>
   internal sealed class ValidatedNotNullAttribute : Attribute
   {
   }
}
