//-----------------------------------------------------------------------
// <copyright file="In.cs" company="Eusebio Rufian-Zilbermann">
//   Copyright (c) Eusebio Rufian-Zilbermann for the C# implementation
//   based on materials published by Robert Sedgewick and Kevin Wayne
// </copyright>
//-----------------------------------------------------------------------
namespace Stdlib
{
   using System;
   using System.Collections;
   using System.Globalization;
   using System.IO;
   using System.Net;
   using System.Net.Sockets;
   using System.Resources;
   using System.Text;
   using System.Text.RegularExpressions;

   /// <summary>
   /// Input. This class provides methods for reading strings
   /// and numbers from standard input, file input, URLs, and sockets. 
   /// <para></para>
   /// The Locale used is: InvariantCulture. This is consistent
   /// with the formatting conventions with Java floating-point literals,
   /// command-line arguments and standard output. 
   /// <para></para>
   /// For additional documentation, see 
   /// <a href="http://introcs.cs.princeton.edu/31datatype">Section 3.1</a> of
   /// <i>Introduction to Programming in Java: An Interdisciplinary Approach</i> 
   /// by Robert Sedgewick and Kevin Wayne.
   /// <para></para>
   /// Like Java's Scanner, reading a token also consumes whitespace, 
   /// reading a full line consumes the following end-of-line delimiter, 
   /// while reading a character consumes nothing extra. 
   /// </summary>
   public sealed class In : IDisposable
   {
      /// <summary>
      /// Assume Unicode UTF-8 encoding. 
      /// </summary>
      private readonly Encoding defaultEncoding = Encoding.UTF8;

      /// <summary>
      /// Use invariant culture for parsing.
      /// </summary>
      private readonly CultureInfo locale = CultureInfo.InvariantCulture;

      // Definition of whitespace:
      // ASCII space character (SP), the ASCII horizontal tab character (HT), the ASCII formfeed character (FF),
      // ASCII newline character (LF), the ASCII carriage return character (CR).
      
      /// <summary>
      /// White Space as a char array.
      /// </summary>
      private readonly char[] whiteSpace = { ' ', '\t', '\f', '\n', '\r' };
      
      /// <summary>
      /// White space as a Regex.
      /// </summary>
      private readonly Regex whiteSpaceExpression = new Regex("[ \t\f\n\r]", RegexOptions.Compiled);

      /// <summary>
      /// The input stream.
      /// </summary>
      private TextReader scanner;

      /// <summary>
      /// Initializes a new instance of the <see cref="In" /> class from standard input.
      /// </summary>
      public In() 
      {
        this.scanner = Console.In;
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="In" /> class from a socket.
      /// </summary>
      /// <param name="socket">The input socket to read from.</param>
      public In(Socket socket) 
      {
         try 
         {
            using (NetworkStream netStream = new NetworkStream(socket))
            {
               this.scanner = new StreamReader(netStream);
            }
         }
         catch (IOException ex)
         {
            Console.Error.WriteLine("Could not open socket. Exception: {1}", ex);
         }
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="In" /> class from a URL.
      /// </summary>
      /// <param name="url">The URL to read from.</param>
      public In(Uri url) 
      {
         this.InitializeStreamFromUri(url);
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="In" /> class from a file stream.
      /// </summary>
      /// <param name="file">The file stream to read from.</param>
      public In(FileStream file)
      {
         if (null == file)
         {
            throw new ArgumentNullException("file");
         }

         try
         {
            this.scanner = new StreamReader(file, this.defaultEncoding);
         }
         catch (IOException ex)
         {
            Console.Error.WriteLine("Could not open filestream for {0}. Exception: {1}", file.Name, ex);
         }
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="In" /> class from a filename or web page name.
      /// </summary>
      /// <param name="fileResource">The string representation of the file name or URL to read from.</param>
      public In(string fileResource) 
      {
         try 
         {
            // first try to read file from local file system
            if (File.Exists(fileResource)) 
            {
                this.scanner = new StreamReader(fileResource, this.defaultEncoding);
                return;
            }

            // next try for embedded resources
            ResourceManager resMan = new ResourceManager(this.GetType());
            try
            {
               string resourceString = resMan.GetString(fileResource);
               if (!string.IsNullOrWhiteSpace(resourceString))
               {
                  this.scanner = new StringReader(resourceString);
                  return;
               }
            }
            catch (MissingManifestResourceException)
            {
               // No resource found, move on to URL
            }

            // last try an URL from the web
            this.InitializeStreamFromUri(new Uri(fileResource));

            return;
         }
         catch (IOException ex) 
         {
            Console.Error.WriteLine("Could not open {0}. Exception: {1}", fileResource, ex);
         }
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="In" /> class from a given Scanner source;
      /// use with new StringReader(String) to read from a string.
      /// Note that this does not create a defensive copy, so the scanner will be mutated as you read on. 
      /// </summary>
      /// <param name="scanner">The stream to read from</param>
      public In(StreamReader scanner)
      {
         this.scanner = scanner;
      }

      /// <summary>
      /// Reads all integers from a file.
      /// </summary>
      /// <remarks>deprecated: Clearer to use new <see cref="In(filename).readAllInts()"/>.</remarks>
      /// <param name="fileName">The name of the file to read from.</param>
      /// <returns>The integer values in the file.</returns>
      public static int[] ReadInts(string fileName)
      {
         using (In inputStream = new In(fileName))
         {
            return inputStream.ReadAllInts();
         }
      }

      /// <summary>
      /// Reads all doubles from a file 
      /// </summary>
      /// <remarks>deprecated: Clearer to use new <see cref="In(filename).readAllDoubles()"/>.</remarks>
      /// <param name="fileName">The name of the file to read from.</param>
      /// <returns>The double values in the file.</returns>
      public static double[] ReadDoubles(string fileName)
      {
         using (In inputStream = new In(fileName))
         {
            return inputStream.ReadAllDoubles();
         }
      }

      /// <summary>
      /// Reads all strings from a file 
      /// </summary>
      /// <remarks>deprecated: Clearer to use <see cref="new In(filename).readAllStrings()"/>.</remarks>
      /// <param name="fileName">The name of the file to read from.</param>
      /// <returns>The strings in the file.</returns>
      public static string[] ReadStrings(string fileName)
      {
         using (In inputStream = new In(fileName))
         {
            return inputStream.ReadAllStrings();
         }
      }

      /// <summary>
      /// Reads all integers from standard input.
      /// </summary>
      /// <remarks>deprecated: Clearer to use <see cref="In().readAllInts()"/></remarks>
      /// <returns>The integer values read from standard input.</returns>
      public static int[] ReadInts()
      {
         using (In inputStream = new In())
         {
            return inputStream.ReadAllInts();
         }
      }

      /// <summary>
      /// Reads all doubles from standard input.
      /// </summary>
      /// <remarks>deprecated: Clearer to use new <see cref="In().readAllDoubles()"/>.</remarks>
      /// <returns>The double values read from standard input.</returns>
      public static double[] ReadDoubles()
      {
         using (In inputStream = new In())
         {
            return inputStream.ReadAllDoubles();
         }
      }

      /// <summary>
      /// Reads all strings from standard input.
      /// </summary>
      /// <remarks>deprecated: Clearer to use new <see cref="In().readAllStrings()"/>.</remarks>
      /// <returns>The strings read from standard input.</returns>
      public static string[] ReadStrings()
      {
         using (In inputStream = new In())
         {
            return inputStream.ReadAllStrings();
         }
      }

      /// <summary>
      /// Does the input stream exist?
      /// </summary>
      /// <returns>True if the input stream exists, false if it doesn't exist.</returns>
      public bool Exists()
      {
         return this.scanner != null;
      }

      /// <summary>
      /// Is the input empty? 
      /// Use this to know whether the next call to readString(), readDouble(), etc. will succeed.
      /// Note that a stream containing only whitespace will be identified as nonempty
      /// </summary>
      /// <returns>True if the input stream doesn't have any characters to read, false if it does.</returns>
      public bool IsEmpty()
      {
         return -1 == this.scanner.Peek();
      }

      /// <summary>
      /// Does the input have a next line? 
      /// Use this to know whether the next call to readLine() will succeed.
      /// Identical to hasNextChar().
      /// </summary>
      /// <returns>True if the input stream has more characters to read, false if it doesn't.</returns>
      public bool HasNextLine()
      {
         return -1 != this.scanner.Peek();
      }

      /// <summary>
      /// Is the input empty (including whitespace)? 
      /// Use this to know whether the next call to readChar() will succeed. 
      /// Identical to hasNextLine().
      /// </summary>
      /// <returns>True if the input stream has more characters to read, false if it doesn't</returns>
      public bool HasNextChar()
      {
         return -1 != this.scanner.Peek();
      }

      /// <summary>
      /// Read and return the next line.
      /// </summary>
      /// <returns>A line of characters read from the current input stream.</returns>
      public string ReadLine()
      {
         string line = null;
         try 
         { 
            line = this.scanner.ReadLine(); 
         }
         catch (IOException)
         { 
            line = null; 
         }

         return line;
      }

      /// <summary>
      /// Read and return the next character.
      /// </summary>
      /// <returns>The next character in the input stream.</returns>
      public char ReadChar() 
      {
        return (char)this.scanner.Read();
      }

      /// <summary>
      /// Read and return the remainder of the input as a string.
      /// </summary>
      /// <returns>A string containing the remainder of the input.</returns>
      public string ReadAll()
      {
         return this.scanner.ReadToEnd();
      }

      /// <summary>
      /// Read and return the next string.
      /// </summary>
      /// <returns>A string containing the next characters up to a whitespace.</returns>
      public string ReadString()
      {
         StringBuilder result = new StringBuilder();
         char[] inputBuffer = new char[1];
         int readCount;
         
         do
         {
            readCount = this.scanner.Read(inputBuffer, 0, 1);
            if (0 != readCount)
            {
               if (!this.whiteSpaceExpression.IsMatch(inputBuffer[0].ToString()))
               {
                  result.Append(inputBuffer[0].ToString());
               }
               else
               {
                  if (0 < result.Length)
                  {
                     readCount = this.DiscardEndingWhitespace();
                     break;
                  }
                  else
                  {
                     continue; // Discard starting whitespace
                  }
               }
            }
         } 
         while (0 != readCount);

         return result.ToString();
      }

      /// <summary>
      /// Read and return the next integer.
      /// </summary>
      /// <returns>The next input string parsed as an integer.</returns>
      /// <exception cref="ArgumentNullException">The next input string is null.</exception>
      /// <exception cref="FormatException">The next input string not an integer number.</exception>
      /// <exception cref="OverflowException">The next input string is a number larger than an int.</exception>
      [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", Justification = "The return type must be specified in the method name")]
      public int ReadInt()
      {
         return int.Parse(this.ReadString(), this.locale);
      }

      /// <summary>
      /// Read and return the next double.
      /// </summary>
      /// <returns>The next input string parsed as a double.</returns>
      /// <exception cref="ArgumentNullException">The next input string is null.</exception>
      /// <exception cref="FormatException">The next input string not a double number.</exception>
      /// <exception cref="OverflowException">The next input string is a number larger than a double.</exception>
      public double ReadDouble()
      {
         return double.Parse(this.ReadString(), this.locale); 
      }

      /// <summary>
      /// Read and return the next float.
      /// </summary>
      /// <returns>The next input string parsed as a float.</returns>
      /// <exception cref="ArgumentNullException">The next input string is null.</exception>
      /// <exception cref="FormatException">The next input string not a float number.</exception>
      /// <exception cref="OverflowException">The next input string is a number larger than a float.</exception>
      [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", Justification = "The return type must be specified in the method name")]
      public float ReadFloat()
      {
         return float.Parse(this.ReadString(), this.locale);
      }

      /// <summary>
      /// Read and return the next long.
      /// </summary>
      /// <returns>The next input string parsed as a long.</returns>
      /// <exception cref="ArgumentNullException">The next input string is null.</exception>
      /// <exception cref="FormatException">The next input string not a long number.</exception>
      /// <exception cref="OverflowException">The next input string is a number larger than a long.</exception>
      [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", Justification = "The return type must be specified in the method name")]
      public long ReadLong()
      {
         return long.Parse(this.ReadString(), this.locale);
      }

      /// <summary>
      /// Read and return the next short.
      /// </summary>
      /// <returns>The next input string parsed as a short.</returns>
      /// <exception cref="ArgumentNullException">The next input string is null.</exception>
      /// <exception cref="FormatException">The next input string not a short number.</exception>
      /// <exception cref="OverflowException">The next input string is a number larger than a short.</exception>
      [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", Justification = "The return type must be specified in the method name")]
      public short ReadShort()
      {
         return short.Parse(this.ReadString(), this.locale);
      }

      /// <summary>
      /// Read and return the next double.
      /// </summary>
      /// <returns>The next input string parsed as a byte.</returns>
      /// <exception cref="ArgumentNullException">The next input string is null.</exception>
      /// <exception cref="FormatException">The next input string not a byte number.</exception>
      /// <exception cref="OverflowException">The next input string is a number larger than a byte.</exception>
      public byte ReadByte()
      {
         return byte.Parse(this.ReadString(), this.locale);
      }

      /// <summary>
      /// Read and return the next boolean, allowing case-insensitive
      /// "true" or "1" for true, and "false" or "0" for false.
      /// </summary>
      /// <returns>The next input string parsed as a boolean.</returns>
      /// <exception cref="ArgumentNullException">The next input string is null.</exception>
      /// <exception cref="FormatException">The next input string not a boolean.</exception>
      public bool ReadBoolean()
      {
         string s = this.ReadString();
         if (null == s)
         {
            throw new ArgumentNullException();
         }

         if (0 == string.Compare(s, "true", StringComparison.OrdinalIgnoreCase))
         {
            return true;
         }

         if (0 == string.Compare(s, "false", StringComparison.OrdinalIgnoreCase))
         {
            return false;
         }

         if (0 == string.Compare(s, "1", StringComparison.Ordinal))
         {
            return true;
         }

         if (0 == string.Compare(s, "0", StringComparison.Ordinal))
         {
            return false;
         }

         throw new FormatException();
      }

      /// <summary>
      /// Read all strings until the end of input is reached, and return them.
      /// </summary>
      /// <returns>All strings until the end of input.</returns>
      public string[] ReadAllStrings()
      {
         string allText = this.ReadAll();
         return allText.Split(this.whiteSpace, StringSplitOptions.RemoveEmptyEntries);
      }

      /// <summary>
      /// Reads all remaining lines from input stream and returns them as an array of strings.
      /// </summary>
      /// <returns>All remaining lines on input stream, as an array of strings.</returns>
      public string[] ReadAllLines()
      {
         ArrayList lines = new ArrayList();
         while (this.HasNextLine())
         {
            lines.Add(this.ReadLine());
         }

         return (string[])lines.ToArray();
      }

      /// <summary>
      /// Reads all remaining integers from input stream and returns them as an array of integers.
      /// </summary>
      /// <returns>All remaining integers on input stream, as an array of integers.</returns>
      public int[] ReadAllInts()
      {
         string[] fields = this.ReadAllStrings();
         int[] vals = new int[fields.Length];
         for (int i = 0; i < fields.Length; i++)
         {
            vals[i] = int.Parse(fields[i], this.locale);
         }

         return vals;
      }

      /// <summary>
      /// Reads all remaining doubles from input stream and returns them as an array of doubles.
      /// </summary>
      /// <returns>All remaining doubles on input stream, as an array of doubles.</returns>
      public double[] ReadAllDoubles()
      {
         string[] fields = this.ReadAllStrings();
         double[] vals = new double[fields.Length];
         for (int i = 0; i < fields.Length; i++)
         {
            vals[i] = double.Parse(fields[i], this.locale);
         }

         return vals;
      }

      /// <summary>
      /// Close the input stream.
      /// </summary>
      public void Close()
      {
         this.scanner.Close();
      }

      /// <summary>
      /// Dispose resources.
      /// </summary>
      public void Dispose()
      {
         this.Dispose(true);
         GC.SuppressFinalize(this);
      }

      /// <summary>
      /// Dispose resources.
      /// </summary>
      /// <param name="disposing">This is the guard.</param>
      private void Dispose(bool disposing)
      {
         if (disposing)
         {
            if (this.scanner != null)
            {
               this.scanner.Dispose();
               this.scanner = null;
            }
         }
      }

      /// <summary>
      /// Common initialization based on an Uri.
      /// </summary>
      /// <param name="url">The Url for initializing the scanner.</param>
      private void InitializeStreamFromUri(Uri url)
      {
         try
         {
            using (WebClient client = new WebClient())
            {
               Stream stream = client.OpenRead(url);
               this.scanner = new StreamReader(stream);
            }

            return;
         }
         catch (IOException ex)
         {
            Console.Error.WriteLine("Could not open URL {0}. Exception: {1}", url, ex);
         }
      }

      /// <summary>
      /// Advance the reading position in a stream to discard whitespace (after reading a word).
      /// </summary>
      /// <returns>1 if there are additional characters in the file, 0 if EOF is reached.</returns>
      private int DiscardEndingWhitespace()
      {
         char[] inputBuffer = new char[1];
         int readCount = 1;
         do
         {
            int next = this.scanner.Peek();
            if (0xFFFF > next)
            {
               if (this.whiteSpaceExpression.IsMatch(((char)next).ToString()))
               {
                  readCount = this.scanner.Read(inputBuffer, 0, 1);
               }
               else
               {
                  break;
               }
            }
            else
            {
               throw new FormatException(
                  string.Format(CultureInfo.CurrentCulture, "Non-char found in input: {0}", next));
            }
         } 
         while (0 != readCount);

         return readCount;
      }
   }
}