//-----------------------------------------------------------------------
// <copyright file="ArgumentValidator.cs" company="Eusebio Rufian-Zilbermann">
//   Copyright (c) Eusebio Rufian-Zilbermann for the C# implementation
//   based on algorithms published by Robert Sedgewick and Kevin Wayne
// </copyright>
//-----------------------------------------------------------------------
namespace Algs4
{
   using System;

   /// <summary>
   /// Utility methods for making argument checking more concise.
   /// </summary>
   internal static class ArgumentValidator
   {
      /// <summary>
      /// Validate that an argument is not null.
      /// </summary>
      /// <typeparam name="T">The type of argument to check.</typeparam>
      /// <param name="argumentValue">The value to check for null.</param>
      /// <param name="argumentName">The parameter name to be reported in the exception if a null is found.</param>
      /// <exception cref="ArgumentNullException">This exception is thrown if a null parameter is passed in.</exception>
      public static void CheckNotNull<T>([ValidatedNotNullAttribute] T argumentValue, string argumentName)
      {
         if (null == argumentValue)
         {
            throw new ArgumentNullException(argumentName);
         }
      }
   }
}
