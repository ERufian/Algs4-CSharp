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

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Algs", Scope = "namespace", Target = "Algs4UnitTests", Justification = "Necessary for preserving API naming")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Algs", Justification = "Necessary for preserving API naming")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "1", Scope = "member", Target = "Algs4UnitTests.CommonPriorityQueueUnitTests.#StringPQTest(System.String,Algs4.PQCollection`1<System.String>,System.String[],System.Int32)", Justification = "This check needs to be suppressed when Code Contracts are used (the Code Contract static checker must be used as a replacement for this validator)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "2", Scope = "member", Target = "Algs4UnitTests.CommonPriorityQueueUnitTests.#StringPQTest(System.String,Algs4.PQCollection`1<System.String>,System.String[],System.Int32)", Justification = "This check needs to be suppressed when Code Contracts are used (the Code Contract static checker must be used as a replacement for this validator)")]
