//-----------------------------------------------------------------------
// <copyright file="GlobalSuppressions.cs" company="Eusebio Rufian-Zilbermann">
//   Copyright (c) Eusebio Rufian-Zilbermann for the C# implementation
// </copyright>
//-----------------------------------------------------------------------

// This file is used by Code Analysis to maintain SuppressMessage 
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given 
// a specific target and scoped to a namespace, type, member, etc.
//
// To add a suppression to this file, right-click the message in the 
// Code Analysis results, point to "Suppress Message", and click 
// "In Suppression File".
// You do not need to add suppressions to this file manually.

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Ints", Scope = "member", Target = "Stdlib.In.#ReadAllInts()", Justification = "The return type must be specified in the method name")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Ints", Scope = "member", Target = "Stdlib.In.#ReadInts()", Justification = "The return type must be specified in the method name")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Ints", Scope = "member", Target = "Stdlib.In.#ReadInts(System.String)", Justification = "The return type must be specified in the method name")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "In", Scope = "type", Target = "Stdlib.In", Justification = "Necessary for preserving API naming")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Stdlib", Scope = "namespace", Target = "Stdlib", Justification = "Necessary for preserving API naming")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Stdlib", Justification = "Necessary for preserving API naming")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "short", Scope = "member", Target = "Stdlib.In.#ReadShort()", Justification = "The return type must be specified in the method name")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "long", Scope = "member", Target = "Stdlib.In.#ReadLong()", Justification = "The return type must be specified in the method name")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "float", Scope = "member", Target = "Stdlib.In.#ReadFloat()", Justification = "The return type must be specified in the method name")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "int", Scope = "member", Target = "Stdlib.In.#ReadInt()", Justification = "The return type must be specified in the method name")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Std", Scope = "type", Target = "Stdlib.StdRandom", Justification = "Necessary for preserving API naming")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline", Scope = "member", Target = "Stdlib.StdRandom.#.cctor()", Justification = "The Pseudo-random generator needs to be re-initialized so that the seed is known, it cannot be done inline.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope = "member", Target = "Stdlib.StdRandom.#GetSeed()", Justification = "Necessary for preserving API naming")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Exp", Scope = "member", Target = "Stdlib.StdRandom.#Exp(System.Double)", Justification = "Necessary for preserving API naming")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1057:StringUriOverloadsCallSystemUriOverloads", Scope = "member", Target = "Stdlib.In.#.ctor(System.String)", Justification = "The suggested refactoring is not possible in the context of a constructor")]
