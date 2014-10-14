//-----------------------------------------------------------------------
// <copyright file="BaseException.cs" company="Eusebio Rufian-Zilbermann">
//   Copyright (c) Eusebio Rufian-Zilbermann
// </copyright>
//-----------------------------------------------------------------------
namespace Algs4
{
   using System;
   using System.Runtime.Serialization;
   using System.Security.Permissions;

   /// <summary>
   /// Specialized exception class that indicates a failure within a class in this namespace.
   /// </summary>
   [Serializable]
   public abstract class BaseException : Exception
   {
      /// <summary>
      /// Initializes a new instance of the BaseException class.
      /// </summary>
      protected BaseException()
         : base()
      {
      }

      /// <summary>
      /// Initializes a new instance of the BaseException class.
      /// </summary>
      /// <param name="message">A string with some context about why the exception was thrown.</param>
      protected BaseException(string message)
         : base(message)
      {
      }

      /// <summary>
      /// Initializes a new instance of the BaseException class.
      /// </summary>
      /// <param name="message">A string with some context about why the exception was thrown.</param>
      /// <param name="inner">Inner exception to embed in this one.</param>
      protected BaseException(string message, Exception inner)
         : base(message, inner)
      {
      }

      /// <summary>
      /// Initializes a new instance of the BaseException class.
      /// </summary>
      /// <param name="info">The SerializationInfo that holds the serialized object data about the exception being thrown</param>
      /// <param name="context">The StreamingContext that contains contextual information about the source or destination. </param>
      protected BaseException(SerializationInfo info, StreamingContext context)
         : base(info, context)
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="BaseException" /> class with serialized data.
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
