//-----------------------------------------------------------------------
// <copyright file="IndexPQDictionary.cs" company="Eusebio Rufian-Zilbermann">
//   Copyright (c) Eusebio Rufian-Zilbermann for the C# implementation
//   based on algorithms published by Robert Sedgewick and Kevin Wayne
// </copyright>
//-----------------------------------------------------------------------
namespace Algs4
{
   using System;
   using System.Collections;
   using System.Collections.Generic;
   using System.Linq;

   /// <summary>
   /// The <tt>IndexPQCollection</tt> class represents a Priority Queue
   /// where a zero-based set of indices are associated with a priority object each.
   /// Queue behaviors like en-queue, de-queue and peek are supported but 
   /// the difference with a standard queue is that removal from queue and peeking 
   /// is performed by priority (and not related to when the item was en-queued).
   /// Dictionary behaviors like iteration through keys and values, and retrieving
   /// the priority for a given item are also supported.
   /// <para/>
   /// This implementation uses a binary heap plus an array that associates keys 
   /// with values.
   /// The <em>insert</em>, <em>de-queue</em>, <em>delete</em>, <em>change-key</em>,
   /// <em>decrease-key</em>, and <em>increase-key</em> operations take logarithmic time.
   /// The <em>peek-index</em>, <em>peek-key</em>, <em>key-of</em>, <em>size</em>, and 
   /// <em>is-empty</em> operations take constant time. <em>Indexing by key</em> 
   /// is proportional to the number of items in the queue.
   /// Construction takes time proportional to the maximum number of indices.
   /// <para/>
   /// For additional documentation, see <a href="http://algs4.cs.princeton.edu/24pq">Section 2.4</a> of
   /// <i>Algorithms, fourth Edition</i> by Robert Sedgewick and Kevin Wayne.
   /// </summary>
   /// <typeparam name="T">The type of the priorities.</typeparam>
   /// <remarks>
   /// The naming of the original Java implementation has a very unfortunate 
   /// cognitive dissonance with the Dictionary behavior, that is why the original API
   /// has a <code>keyOf(int i)</code> method and not a <code>valueAt(Key key)</code> method.
   /// In this implementation we will follow a naming convention that matches the
   /// expectations for a dictionary, and the original API will be tagged as Obsolete.
   /// </remarks>
   public abstract class IndexPQDictionary<T> : IDictionary<int, T>
   {
      /// <summary>
      /// Maximum number of items in the Indexed Priority Queue.
      /// </summary>
      private int maxCount;

      /// <summary>
      /// Actual number of items in the Indexed Priority Queue.
      /// </summary>
      private int count;

      /// <summary>
      /// Internal array representation of the binary heap, 1-based.
      /// </summary>
      private int[] pq;

      /// <summary>
      /// Inverse lookup for array representation of the heap.
      /// </summary>
      private int[] qp; // qp[pq[i]] = pq[qp[i]] = i

      /// <summary>
      /// The actual priorities.
      /// </summary>
      private T[] itemPriorities;

      /// <summary>
      /// Comparator used for determining order and equality.
      /// </summary>
      private IComparer<T> comparator;

      /// <summary>
      /// Initializes a new instance of the <see cref="IndexPQDictionary{T}"/> class.
      /// </summary>
      /// <param name="maxCount">The maximum capacity of the Indexed Priority Queue.</param>
      protected IndexPQDictionary(int maxCount)
         : this(maxCount, Comparer<T>.Default)
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="IndexPQDictionary{T}"/> class.
      /// </summary>
      /// <param name="maxCount">The maximum capacity of the Indexed Priority Queue.</param>
      /// <param name="comparator">Comparator used for determining order and equality.</param>
      protected IndexPQDictionary(int maxCount, IComparer<T> comparator)
      {
         ArgumentValidator.CheckNotNull(comparator, "comparer");
         if (0 > maxCount || (int.MaxValue / 2) - 1 <= maxCount)
         {
            throw new ArgumentOutOfRangeException("maxCount");
         }

         this.comparator = comparator;
         this.maxCount = maxCount;
         this.Clear();
      }

      #region Properties
      /// <summary>
      /// Gets the actual number of items in the Indexed Priority Queue.
      /// </summary>
      public int Count
      {
         get { return this.count; }
      }

      /// <summary>
      /// Gets a value indicating whether the collection is read-only.
      /// </summary>
      /// <remarks>This is always false.</remarks>
      public bool IsReadOnly
      {
         get
         {
            return false;
         }
      }

      /// <summary>
      /// Gets a copy of all the indices as an ICollection.
      /// </summary>
      /// <returns>A copy of all the indices.</returns>
      /// <remarks>
      /// The results may be unsorted but the collections of 
      /// Keys and Values are guaranteed to have a correspondence
      /// </remarks>
      public ICollection<int> Keys
      {
         get
         {
            List<int> keys = new List<int>(this.count - 1);
            for (int i = 1; i <= this.Count; i++)
            {
               keys.Add(this.pq[i]);
            }

            return keys;
         }
      }

      /// <summary>
      /// Gets a shallow copy of all the priorities as an ICollection.
      /// </summary>
      /// <returns>A copy of all the priorities.</returns>
      /// <remarks>
      /// The results may be unsorted but the collections of 
      /// Keys and Values are guaranteed to have a correspondence
      /// </remarks>
      public ICollection<T> Values
      {
         get
         {
            List<T> values = new List<T>(this.count - 1);
            for (int i = 1; i <= this.Count; i++)
            {
               values.Add(this.itemPriorities[this.pq[i]]);
            }

            return values;
         }
      }

      /// <summary>
      /// Gets the comparator used to determine ordering
      /// </summary>
      protected IComparer<T> Comparator
      {
         get { return this.comparator; }
      }

      /// <summary>
      /// Gets or sets the priority associated with a given index.
      /// </summary>
      /// <param name="key">Index to be retrieved from the queue.</param>
      /// <returns>The item with the specified priority.</returns>
      /// <exception cref="KeyNotFoundException">
      /// Thrown if no items with the specified priority are found.
      /// </exception>
      public T this[int key]
      {
         get
         {
            if (!this.ContainsKey(key))
            {
               throw new KeyNotFoundException();
            }

            return this.itemPriorities[key];
         }

         set
         {
            if (!this.ContainsKey(key))
            {
               throw new NoSuchElementException("index is not in the priority queue");
            }

            this.itemPriorities[key] = value;
            this.Swim(this.qp[key]);
            this.Sink(this.qp[key]);
         }
      }
      #endregion

      #region IDictionary<T> implementation (methods)
      /// <summary>
      /// Add an item to the priority queue.
      /// </summary>
      /// <param name="item">New item to be inserted into the queue.</param>
      /// <remarks>
      /// This is required for the <see cref="IDictionary{T}"/> interface however, 
      /// using Enqueue is preferred.
      /// </remarks>
      [Obsolete("Preferred: Use the Enqueue method instead.")]
      public void Add(KeyValuePair<int, T> item)
      {
         this.Enqueue(item.Key, item.Value);
      }

      /// <summary>
      /// Add an item to the priority queue.
      /// </summary>
      /// <param name="key">Index to be inserted into the queue.</param>
      /// <param name="value">Priority for the new index to be inserted into the queue.</param>
      /// <remarks>
      /// This is required for the <see cref="IDictionary{T}"/> interface however, 
      /// using Enqueue is preferred.
      /// </remarks>
      [Obsolete("Use the Enqueue method instead.")]
      public void Add(int key, T value)
      {
         this.Enqueue(key, value);
      }

      /// <summary>
      /// Remove all items from the priority queue.
      /// </summary>
      public void Clear()
      {
         this.itemPriorities = new T[this.maxCount];
         this.pq = new int[this.maxCount + 1];
         this.qp = new int[this.maxCount];
         for (int i = 0; i < this.maxCount; i++)
         {
            this.qp[i] = -1;
         }
      }

      /// <summary>
      /// Find if an item is present in the priority queue.
      /// </summary>
      /// <param name="item">The item to search in the queue.</param>
      /// <returns>True if the item is found, false otherwise.</returns>
      public bool Contains(KeyValuePair<int, T> item)
      {
         return this.ContainsKey(item.Key)
            && 0 == this.comparator.Compare(this.itemPriorities[item.Key], item.Value);
      }

      /// <summary>
      /// Find if a Priority is present in the priority queue.
      /// </summary>
      /// <param name="itemPriority">The priority to search in the queue.</param>
      /// <returns>True if the item is found, false otherwise.</returns>
      /// <remarks>
      /// This operation takes time proportional to the number of items in the queue.
      /// </remarks>
      public bool Contains(T itemPriority)
      {
         if (null == (object)itemPriority)
         {
            for (int i = 1; this.Count >= i; i++)
            {
               if (null == (object)this.itemPriorities[this.pq[i]])
               {
                  return true;
               }
            }
         }
         else
         {
            for (int i = 1; this.Count >= i; i++)
            {
               if (0 == this.comparator.Compare(this.itemPriorities[this.pq[i]], itemPriority))
               {
                  return true;
               }
            }
         }

         return false;
      }

      /// <summary>
      /// Find if an Index is present in the priority queue.
      /// </summary>
      /// <param name="key">The index to search in the queue.</param>
      /// <returns>True if the item is found, false otherwise.</returns>
      public bool ContainsKey(int key)
      {
         IndexPQDictionary<T>.CheckIndexRange(key, 0, this.maxCount - 1);
         return -1 != this.qp[key];
      }

      /// <summary>
      /// Copy all the items in the Priority Queue to an array, starting at a specified position.
      /// </summary>
      /// <param name="array">The destination array.</param>
      /// <param name="arrayIndex">
      /// The index of the first element in the destination that will be overwritten.
      /// </param>
      /// <exception cref="ArgumentOutOfRangeException">Thrown if the index is negative.</exception>
      /// <exception cref="ArgumentException">Thrown if the destination has insufficient capacity.</exception>
      public void CopyTo(KeyValuePair<int, T>[] array, int arrayIndex)
      {
         ArgumentValidator.CheckNotNull(array, "array");
         if (0 > arrayIndex)
         {
            throw new ArgumentOutOfRangeException("arrayIndex");
         }

         if (array.Length - arrayIndex < this.Count)
         {
            throw new ArgumentException("The destination array has insufficient capacity.");
         }

         System.Threading.Tasks.Parallel.For(
            0,
            this.count,
            i =>
               array[arrayIndex + i] =
               new KeyValuePair<int, T>(this.pq[i + 1], this.itemPriorities[this.pq[i + 1]]));
      }

      /// <summary>
      /// Removes the item at the top of the heap if it matches the specified item.
      /// </summary>
      /// <param name="item">The item to match.</param>
      /// <returns>True if there was a match, false otherwise.</returns>
      /// <remarks>
      /// Removal of items that are not at the top of the heap is not supported.
      /// Even if the specified item is present in the queue, if it is not at the top 
      /// the removal will fail and this method will return false.
      /// </remarks>
      public bool Remove(KeyValuePair<int, T> item)
      {
         if (this.PeekIndex() == item.Key 
            && 0 == this.comparator.Compare(this.PeekPriority(), item.Value))
         {
            this.Dequeue();
            return true;
         }
         else
         {
            return false;
         }
      }

      /// <summary>
      /// Removes the priority associated with an index.
      /// </summary>
      /// <param name="key">The item index to match.</param>
      /// <returns>True if the item was removed, false if the queue is empty.</returns>
      /// <exception cref="NoSuchElementException">Thrown if the item was not found.</exception>
      public bool Remove(int key)
      {
         if (0 >= this.Count)
         {
            return false;
         }

         if (!this.ContainsKey(key))
         {
            throw new NoSuchElementException("Index is not in the priority queue.");
         }

         int reverseIndex = this.qp[key];
         this.Exch(reverseIndex, this.count--);
         this.Swim(reverseIndex);
         this.Sink(reverseIndex);
         this.itemPriorities[key] = default(T);
         this.qp[key] = -1;
         return true;
      }

      /// <summary>
      /// Gets the value associated with the specified key.
      /// </summary>
      /// <param name="key">The key whose value to get.</param>
      /// <param name="value">
      /// When this method returns, the value associated with the specified key, 
      /// if the key is found; otherwise, the default value for the type 
      /// of the value parameter. This parameter is passed uninitialized.
      /// </param>
      /// <returns>
      /// True if the specified index is found, false otherwise.
      /// </returns>
      public bool TryGetValue(int key, out T value)
      {
         if (0 < key || this.maxCount < key || -1 == this.qp[key])
         {
            value = default(T);
            return false;
         }

         value = this.itemPriorities[this.qp[key]];
         return true;
      }

      #region IEnumerable<T> implementation
      /// <summary>
      /// Get an enumerator for ordered traversal of the heap items.
      /// </summary>
      /// <returns>An enumerator for traversal of the heap items.</returns>
      /// <remarks>
      /// The enumeration is non-destructive, it will not cause any item to be removed.
      /// The enumeration is ordered, the queue items will be sorted.
      /// </remarks>
      public abstract IEnumerator<KeyValuePair<int, T>> GetEnumerator();
      #endregion

      #region IEnumerable implementation
      /// <summary>
      /// Get an enumerator for ordered traversal of the heap items.
      /// </summary>
      /// <returns>An enumerator for traversal of the heap items.</returns>
      /// <remarks>
      /// The enumeration is non-destructive, it will not cause any item to be removed.
      /// The enumeration is ordered, the queue items will be sorted.
      /// </remarks>
      IEnumerator IEnumerable.GetEnumerator()
      {
         return this.GetEnumerator();
      }
      #endregion
      #endregion

      /// <summary>
      /// Remove the topmost key on the priority queue heap.
      /// </summary>
      /// <returns>The topmost key on the priority queue.</returns>
      /// <exception cref="NoSuchElementException">Thrown if the priority queue is empty.</exception>
      public int Dequeue()
      {
         if (0 >= this.Count)
         {
            throw new NoSuchElementException("Priority queue underflow.");
         }

        int top = this.pq[1];
        this.Exch(1, this.count--); 
        this.Sink(1);
        this.qp[top] = -1;
        this.itemPriorities[this.pq[this.count + 1]] = default(T);
        return top; 
      }

      /// <summary>
      /// Add an item to the priority queue.
      /// </summary>
      /// <param name="item">The item that will be added to the priority queue.</param>
      public void Enqueue(KeyValuePair<int, T> item)
      {
         this.Enqueue(item.Key, item.Value);
      }

      /// <summary>
      /// Add an item to the priority queue.
      /// </summary>
      /// <param name="itemIndex">The index for the item to be added.</param>
      /// <param name="itemPriority">The priority for the item to be added.</param>
      public void Enqueue(int itemIndex, T itemPriority)
      {
         if (this.ContainsKey(itemIndex))
         {
            throw new ArgumentException("itemIndex is already in the priority queue", "itemIndex");
         }

         this.count++;
         this.qp[itemIndex] = this.count;
         this.pq[this.count] = itemIndex;
         this.itemPriorities[itemIndex] = itemPriority;
         this.Swim(this.count);
      }

      /// <summary>
      /// Get the topmost item on the priority queue heap.
      /// </summary>
      /// <returns>The topmost key on the priority queue.</returns>
      /// <exception cref="NoSuchElementException">Thrown if the priority queue is empty.</exception>
      public KeyValuePair<int, T> Peek()
      {
         return new KeyValuePair<int, T>(this.PeekIndex(), this.PeekPriority());
      }

      /// <summary>
      /// Get the topmost index on the priority queue heap.
      /// </summary>
      /// <returns>The topmost key on the priority queue.</returns>
      /// <exception cref="NoSuchElementException">Thrown if the priority queue is empty.</exception>
      public int PeekIndex()
      {
         if (0 == this.Count)
         {
            throw new NoSuchElementException("Priority queue underflow.");
         }

         return this.pq[1];
      }

      /// <summary>
      /// Get the topmost priority on the priority queue heap.
      /// </summary>
      /// <returns>The topmost key on the priority queue.</returns>
      /// <exception cref="NoSuchElementException">Thrown if the priority queue is empty.</exception>
      public T PeekPriority()
      {
         if (0 == this.Count)
         {
            throw new NoSuchElementException("Priority queue underflow.");
         }

         return this.itemPriorities[this.pq[1]];
      }

      /// <summary>
      /// Lower the priority associated with an item.
      /// </summary>
      /// <param name="itemIndex">The index for the item being modified.</param>
      /// <param name="newPriority">The new priority.</param>
      public void DecreasePriority(int itemIndex, T newPriority)
      {
         if (!this.ContainsKey(itemIndex))
         {
            throw new NoSuchElementException("index is not in the priority queue");
         }

         if (0 >= this.comparator.Compare(this.itemPriorities[itemIndex], newPriority))
         {
            throw new ArgumentException("New priority is not strictly lower than previous.", "newPriority");
         }

         this.itemPriorities[itemIndex] = newPriority;
         this.Sink(itemIndex);
      }

      /// <summary>
      /// Raise the priority associated with an item.
      /// </summary>
      /// <param name="itemIndex">The index for the item being modified.</param>
      /// <param name="newPriority">The new priority.</param>
      public void IncreasePriority(int itemIndex, T newPriority)
      {
         if (!this.ContainsKey(itemIndex))
         {
            throw new NoSuchElementException("index is not in the priority queue");
         }

         if (0 <= this.comparator.Compare(this.itemPriorities[itemIndex], newPriority))
         {
            throw new ArgumentException("New priority is not strictly higher than previous.", "newPriority");
         }

         this.itemPriorities[itemIndex] = newPriority;
         this.Swim(itemIndex);
      }

      #region Original API (Obsolete)
      /// <summary>
      /// Is the priority queue empty?
      /// </summary>
      /// <returns>True if the priority queue is empty, false otherwise.</returns>
      [Obsolete("Compare the Count property to zero instead.")]
      public bool IsEmpty()
      {
         return this.Count <= 0;
      }

      /// <summary>
      /// Is i an index on the priority queue?
      /// </summary>
      /// <param name="itemIndex">The index to search.</param>
      /// <returns>True if the item is found in the priority queue, false otherwise.</returns>
      [Obsolete("Use the ContainsIndex method instead.")]
      public bool Contains(int itemIndex)
      {
         return this.ContainsKey(itemIndex);
      }

      /// <summary>
      /// Get the number of items in the priority queue
      /// </summary>
      /// <returns>The number of items in the priority queue.</returns>
      [Obsolete("Use the Count property instead.")]
      public int Size()
      {
         return this.Count;
      }

      /// <summary>
      /// Associates a priority with an index.
      /// </summary>
      /// <param name="itemIndex">The index for the new element.</param>
      /// <param name="itemPriority">The priority for the new element.</param>
      [Obsolete("Use the Enqueue method instead.")]
      public void Insert(int itemIndex, T itemPriority)
      {
         this.Enqueue(itemIndex, itemPriority);
      }

      /// <summary>
      /// Returns the priority associated with an index.
      /// </summary>
      /// <param name="itemIndex">The index for the element to search.</param>
      /// <returns>The priority for the item.</returns>
      /// <remarks>
      /// In the obsolete API, key refers to the priority,
      /// not to the index (Dictionary key)
      /// </remarks>
      [Obsolete("Use indexing instead.")]
      public T KeyOf(int itemIndex)
      {
         return this[itemIndex];
      }

      /// <summary>
      /// Change the priority associated with an index.
      /// </summary>
      /// <param name="itemIndex">The index for the item to change.</param>
      /// <param name="itemPriority">The new priority for the item.</param>
      [Obsolete("Use indexing instead.")]
      public void Change(int itemIndex, T itemPriority)
      {
         this[itemIndex] = itemPriority;
      }

      /// <summary>
      /// Change the priority associated with an index.
      /// </summary>
      /// <param name="itemIndex">The index for the item to change.</param>
      /// <param name="itemPriority">The new priority for the item.</param>
      /// <remarks>
      /// In the obsolete API, key refers to the priority,
      /// not to the index (Dictionary key)
      /// </remarks>
      [Obsolete("Use indexing instead.")]
      public void ChangeKey(int itemIndex, T itemPriority)
      {
         this[itemIndex] = itemPriority;
      }

      /// <summary>
      /// Lower the priority associated with an index.
      /// </summary>
      /// <param name="itemIndex">The index for the item to change.</param>
      /// <param name="itemPriority">The new priority for the item.</param>
      /// <remarks>
      /// In the obsolete API, key refers to the priority,
      /// not to the index (Dictionary key)
      /// </remarks>
      [Obsolete("Use the DecreasePriority method instead.")]
      public void DecreaseKey(int itemIndex, T itemPriority)
      {
         this.DecreasePriority(itemIndex, itemPriority);
      }

      /// <summary>
      /// Raise the priority associated with an index.
      /// </summary>
      /// <param name="itemIndex">The index for the item to change.</param>
      /// <param name="itemPriority">The new priority for the item.</param>
      /// <remarks>
      /// In the obsolete API, key refers to the priority,
      /// not to the index (Dictionary key)
      /// </remarks>
      [Obsolete("Use the IncreasePriority method instead.")]
      public void IncreaseKey(int itemIndex, T itemPriority)
      {
         this.IncreasePriority(itemIndex, itemPriority);
      }

      /// <summary>
      /// Remove the item associated with an index.
      /// </summary>
      /// <param name="itemIndex">The index of the item to remove.</param>
      [Obsolete("use the Remove method instead.")]
      public void Delete(int itemIndex)
      {
         this.Remove(itemIndex);
      }
      #endregion

      #region Methods for restoring the heap invariant after a temporary break.
      /// <summary>
      /// Bottom-up restoration of the heap invariant.
      /// </summary>
      /// <param name="nodeIndex">Index of the node that may have broken the heap invariant.</param>
      protected void Swim(int nodeIndex)
      {
         while (nodeIndex > 1 && this.PerformCompare(nodeIndex / 2, nodeIndex))
         {
            this.Exch(nodeIndex, nodeIndex / 2);
            nodeIndex = nodeIndex / 2;
         }
      }

      /// <summary>
      /// Top-down restoration of the heap invariant.
      /// </summary>
      /// <param name="nodeIndex">Index of the node that may have broken the heap invariant.</param>
      protected void Sink(int nodeIndex)
      {
         if (int.MaxValue / 2 <= nodeIndex)
         {
            throw new ArgumentException("Node index overflow", "nodeIndex");
         }

         while (2 * nodeIndex <= this.Count)
         {
            int j = 2 * nodeIndex;
            if (j < this.Count && this.PerformCompare(j, j + 1))
            {
               j++;
            }

            if (!this.PerformCompare(nodeIndex, j))
            {
               break;
            }

            this.Exch(nodeIndex, j);
            nodeIndex = j;
         }
      }
      #endregion

      /// <summary>
      /// Access an item from the internal queue.
      /// </summary>
      /// <param name="rawIndex">The index of the item to retrieve, from one to Count (included)</param>
      /// <returns>The item at the specified index.</returns>
      protected int GetPQItem(int rawIndex)
      {
         IndexPQDictionary<T>.CheckIndexRange(rawIndex, 1, this.maxCount);
         return this.pq[rawIndex];
      }

      /// <summary>
      /// Compare elements at the two specified indices.
      /// </summary>
      /// <param name="firstIndex">Index of the first item to compare.</param>
      /// <param name="secondIndex">Index of the second item to compare.</param>
      /// <returns>True if the first item is smaller than the second one, false otherwise.</returns>
      protected abstract bool PerformCompare(int firstIndex, int secondIndex);

      /// <summary>
      /// Validate that a requested index is within an allowed [lowRange, highRange] interval.
      /// </summary>
      /// <param name="itemIndex">The index to validate.</param>
      /// <param name="lowRange">The low range (included)</param>
      /// <param name="highRange">The high range (included)</param>
      /// <exception cref="ArgumentOutOfRangeException">
      /// Thrown if the item is out of range.
      /// </exception>
      private static void CheckIndexRange(int itemIndex, int lowRange, int highRange)
      {
         if (lowRange > itemIndex || highRange < itemIndex)
         {
            throw new ArgumentOutOfRangeException("itemIndex");
         }
      }

      /// <summary>
      /// Swap items at the two specified indices
      /// </summary>
      /// <param name="firstIndex">Index of the first item to compare.</param>
      /// <param name="secondIndex">Index of the second item to compare.</param>
      private void Exch(int firstIndex, int secondIndex)
      {
         int swap = this.pq[firstIndex];
         this.pq[firstIndex] = this.pq[secondIndex];
         this.pq[secondIndex] = swap;
         this.qp[this.pq[firstIndex]] = firstIndex;
         this.qp[this.pq[secondIndex]] = secondIndex;
      }
   }
}