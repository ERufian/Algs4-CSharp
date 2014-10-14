//-----------------------------------------------------------------------
// <copyright file="NoSuchElementException.cs" company="Eusebio Rufian-Zilbermann">
//   Copyright (c) Eusebio Rufian-Zilbermann
// </copyright>
//-----------------------------------------------------------------------
namespace Algs4
{
   using System;
   using System.Runtime.Serialization;
   using System.Security.Permissions;

   /// <summary>
   /// Specialized exception class that indicates a failure when a specified element does not exist.
   /// </summary>
   [Serializable]
   public class NoSuchElementException : BaseException
   {
      /// <summary>
      /// Initializes a new instance of the NoSuchElementException class.
      /// </summary>
      public NoSuchElementException()
         : base()
      {
      }

      /// <summary>
      /// Initializes a new instance of the NoSuchElementException class.
      /// </summary>
      /// <param name="message">A string with some context about why the exception was thrown.</param>
      public NoSuchElementException(string message)
         : base("No Such Element Exception: " + message)
      {
      }

      /// <summary>
      /// Initializes a new instance of the NoSuchElementException class.
      /// </summary>
      /// <param name="message">A string with some context about why the exception was thrown.</param>
      /// <param name="inner">Inner exception to embed in this one.</param>
      public NoSuchElementException(string message, Exception inner)
         : base("No Such Element Exception: " + message, inner)
      {
      }

      /// <summary>
      /// Initializes a new instance of the NoSuchElementException class.
      /// </summary>
      /// <param name="info">The SerializationInfo that holds the serialized object data about the exception being thrown</param>
      /// <param name="context">The StreamingContext that contains contextual information about the source or destination. </param>
      protected NoSuchElementException(SerializationInfo info, StreamingContext context)
         : base(info, context)
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="NoSuchElementException" /> class with serialized data.
      /// </summary>
      /// <param name="info">The object that holds the serialized object data.</param>
      /// <param name="context">The contextual information about the source or destination.</param>
      [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
      public override void GetObjectData(SerializationInfo info, StreamingContext context)
      {
         base.GetObjectData(info, context);
      }
   }
}
