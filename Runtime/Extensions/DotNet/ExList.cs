using System;
using System.Collections.Generic;
using System.Linq;

namespace QuickUnity.Extensions.DotNet
{
    public static class ExList
    {
        /// <summary>
        /// Safely clear the list.
        /// </summary>
        /// <param name="list"></param>
        /// <typeparam name="T"></typeparam>
        public static void ClearNotThrow<T>(this IList<T> list)
        {
            if (list.IsNullOrEmpty())
            {
                return;
            }

            list.Clear();
        }

        /// <summary>
        /// Safely contains the list.
        /// </summary>
        /// <param name="list"></param>
        /// <param name="value"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static bool ContainsIfNullFalse<T>(this IList<T> list, T value)
        {
            if (list.IsNullOrEmpty())
            {
                return false;
            }

            return list.Contains(value);
        }

        /// <summary>
        /// Checks for the presence of the specified element in the list and removes the element from the list if it is present.
        /// </summary>
        public static void RemoveByNotThrow<T>(this List<T> list, Predicate<T> match)
        {
            if (list.IsNullOrEmpty())
            {
                return;
            }

            var index = list.FindIndex(match);
            if (index == -1)
            {
                return;
            }

            list.RemoveAt(index);
        }

        /// <summary>
        /// Remove the specified element from the list.
        /// If the element is not there, add it.
        /// </summary>
        /// <param name="list">List.</param>
        /// <param name="item">Item.</param>
        /// <param name="match">Match.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static void RemoveOrAdd<T>(this List<T> list, T item)
        {
            if (list.IsNullOrEmpty())
            {
                list.Add(item);
                return;
            }

            if (list.Contains(item))
            {
                list.Remove(item);
                return;
            }

            list.Add(item);
        }

        /// <summary>
        /// Get the element number safely.
        /// </summary>
        /// <param name="list"></param>
        /// <param name="index"></param>
        /// <param name="defaultValue"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T FindByIndexOrDefault<T>(this List<T> list, int index, T defaultValue)
        {
            if (list.IsNullOrEmpty() || index > list.Count)
            {
                return defaultValue;
            }

            return list[index];
        }

        /// <summary>
        /// Return a random one of the values in the list.
        /// If the list is empty or null, the defaultValue is returned.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="defaultValue"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetRandomOrDefault<T>(this List<T> self, T defaultValue)
        {
            return self.IsNullOrEmpty() ? defaultValue : self[UnityEngine.Random.Range(0, self.Count)];
        }

        /// <summary>
        /// Shuffle through the list.
        /// </summary>
        /// <param name="list">List.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static void Shuffle<T>(List<T> list)
        {
            if (list.IsNullOrEmpty()) return;

            for (var i = 0; i < list.Count; i++)
            {
                var temp = list[i];
                var randomIndex = UnityEngine.Random.Range(0, list.Count);
                list[i] = list[randomIndex];
                list[randomIndex] = temp;
            }
        }

        /// <summary>
        /// Compares the lists to each other and returns true if they are the same.
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="a2"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static bool SequenceMatch<T>(this List<T> a1, List<T> a2)
        {
            return a1.CountIfNullZero() == a2.CountIfNullZero() && a1.All(a2.Contains);
        }

        /// <summary>
        /// Find an element in a collection by binary searching. 
        /// This requires the collection to be sorted on the values returned by getSubElement
        /// This will compare some derived property of the elements in the collection, rather than the elements
        /// themselves.
        /// </summary>
        /// <typeparam name="TCollection"></typeparam>
        /// <typeparam name="TElement"></typeparam>
        /// <param name="source"></param>
        /// <param name="value"></param>
        /// <param name="getSubElement"></param>
        /// <returns></returns>
        public static int BinarySearch<TCollection, TElement>(
            this ICollection<TCollection> source, TElement value, Func<TCollection, TElement> getSubElement
        )
        {
            return BinarySearch(source, value, getSubElement, 0, source.Count, null);
        }

        /// <summary>
        /// Find an element in a collection by binary searching. 
        /// This requires the collection to be sorted on the values returned by getSubElement
        /// This will compare some derived property of the elements in the collection, rather than the elements
        /// themselves.
        /// </summary>
        /// <typeparam name="TCollection"></typeparam>
        /// <typeparam name="TElement"></typeparam>
        /// <param name="source"></param>
        /// <param name="value"></param>
        /// <param name="getSubElement"></param>
        /// <param name="comparer"></param>
        /// <returns></returns>
        public static int BinarySearch<TCollection, TElement>(
            this ICollection<TCollection> source, TElement value, Func<TCollection, TElement> getSubElement,
            IComparer<TElement> comparer
        )
        {
            return BinarySearch(source, value, getSubElement, 0, source.Count, comparer);
        }

        /// <summary>
        /// Find an element in a collection by binary searching. 
        /// This requires the collection to be sorted on the values returned by getSubElement
        /// This will compare some derived property of the elements in the collection, rather than the elements
        /// themselves.
        /// </summary>
        /// <typeparam name="TCollection"></typeparam>
        /// <typeparam name="TElement"></typeparam>
        /// <param name="source"></param>
        /// <param name="value"></param>
        /// <param name="getSubElement"></param>
        /// <param name="index"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static int BinarySearch<TCollection, TElement>(
            this ICollection<TCollection> source, TElement value, Func<TCollection, TElement> getSubElement, int index,
            int length
        )
        {
            return BinarySearch(source, value, getSubElement, index, length, null);
        }

        /// <summary>
        /// Find an element in a collection by binary searching. 
        /// This requires the collection to be sorted on the values returned by getSubElement
        /// This will compare some derived property of the elements in the collection, rather than the elements
        /// themselves.
        /// </summary>
        /// <typeparam name="TCollection"></typeparam>
        /// <typeparam name="TElement"></typeparam>
        /// <param name="source"></param>
        /// <param name="value"></param>
        /// <param name="getSubElement"></param>
        /// <param name="index"></param>
        /// <param name="length"></param>
        /// <param name="comparer"></param>
        /// <returns></returns>
        public static int BinarySearch<TCollection, TElement>(
            this ICollection<TCollection> source, TElement value, Func<TCollection, TElement> getSubElement, int index,
            int length, IComparer<TElement> comparer
        )
        {
            if (index < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(index), "index is less than the lower bound of array.");
            }

            if (length < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(length), "Value has to be >= 0.");
            }

            // re-ordered to avoid possible integer overflow
            if (index > source.Count - length)
            {
                throw new ArgumentException("index and length do not specify a valid range in array.");
            }

            if (comparer == null)
            {
                comparer = Comparer<TElement>.Default;
            }

            var min = index;
            var max = index + length - 1;

            while (min <= max)
            {
                var mid = (min + ((max - min) >> 1));

                var cmp = comparer.Compare(getSubElement(source.ElementAt(mid)), value);

                if (cmp == 0) return mid;

                if (cmp > 0)
                {
                    max = mid - 1;
                }
                else
                {
                    min = mid + 1;
                }
            }

            return ~min;
        }
    }
}
