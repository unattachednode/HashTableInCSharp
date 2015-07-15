using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashTableInCSharp
{
    class Program
    {
        static void Main(string[] args)
        {

            Hashtable<string, int> h = new Hashtable<string, int>();

            h.Add("mohit", 1);
            h.Add("gupta", 2);
            h.Add("misys", 3);
            h.Add("Rahul", 5);
            h.Add("Sodani", 6);
            h.Add("1Redbus", 1);

            h.Add("2Redbus", 2);
            h.Add("3Redbus", 3);
            h.Add("4Redbus", 4);
            h.Add("5Redbus", 5);

            h.Add("6Redbus", 6);

            h.Add("7Redbus", 7);

            h.Add("21Redbus", 20);
            h.Add("31Redbus", 34);
            h.Add("41Redbus", 44);
            h.Add("51Redbus", 53);

            h.Add("61Redbus", 63);

            h.Add("71Redbus", 712);

            



        }
    }

    public class Hashtable<TKey, TValue>
    {
        private class Entry
        {
            public TKey Key;
            public TValue Value;
            public Entry Next;
            public int Hashcode;
        }

        private const int MIN_CAPACITY = 16;

        private Entry[] buckets;
        private int count;

        public Hashtable()
            : this(MIN_CAPACITY)
        { }

        public Hashtable(int capacity)
        {
            capacity = (capacity < MIN_CAPACITY) ? MIN_CAPACITY : capacity;
            buckets = new Entry[capacity];
        }

        public void Add(TKey key, TValue value)
        {
            int hashcode = key.GetHashCode();
            int targetBucket = (hashcode & int.MaxValue) % buckets.Length;
            Entry ent = null;

            // Search for existing key
            for (ent = buckets[targetBucket]; ent != null; ent = ent.Next)
            {
                if (ent.Hashcode == hashcode && ent.Key.Equals(key))
                {
                    // Key already exists
                    ent.Value = value;
                    return;
                }
            }

            // Rehash if necessary
            if (count + 1 > buckets.Length)
            {
                Expand();
                targetBucket = (hashcode & int.MaxValue) % buckets.Length;
            }

            // Create new entry to house key-value pair
            ent = new Entry()
            {
                Key = key,
                Value = value,
                Hashcode = hashcode
            };

            // And add to table
            ent.Next = buckets[targetBucket];
            buckets[targetBucket] = ent;
            count++;
        }

        public TValue Get(TKey key)
        {
            Entry ent = Find(key);
            if (ent != null)
                return ent.Value;
            return default(TValue);
        }

        public void Remove(TKey key)
        {
            int hashcode = key.GetHashCode();
            int targetBucket = (hashcode & int.MaxValue) % buckets.Length;
            Entry ent = buckets[targetBucket];
            Entry last = ent;

            if (ent == null)
                return;

            // Found entry at head of linked list
            if (ent.Hashcode == hashcode && ent.Key.Equals(key))
            {
                buckets[targetBucket] = ent.Next;
                count--;
            }
            else
            {
                while (ent != null)
                {
                    if (ent.Hashcode == hashcode && ent.Key.Equals(key))
                    {
                        last.Next = ent.Next;
                        count--;
                    }
                    last = ent;
                    ent = last.Next;
                }
            }
        }

        private Entry Find(TKey key)
        {
            int hashcode = key.GetHashCode();
            int targetBucket = (hashcode & int.MaxValue) % buckets.Length;
            // Search for entry
            for (Entry ent = buckets[targetBucket]; ent != null; ent = ent.Next)
            {
                if (ent.Hashcode == hashcode && ent.Key.Equals(key))
                    return ent;
            }
            return null;
        }

        private void Expand()
        {
            Rehash(buckets.Length * 2);
        }

        private void Rehash(int newCapacity)
        {
            // Resize bucket array and redistribute entries
            int oldCapacity = buckets.Length;
            int targetBucket;
            Entry ent, nextEntry;
            Entry[] newBuckets = new Entry[newCapacity];

            for (int i = 0; i < oldCapacity; i++)
            {
                if (buckets[i] != null)
                {
                    ent = buckets[i];
                    while (ent != null)
                    {
                        targetBucket = (ent.Hashcode & int.MaxValue) % newCapacity;
                        nextEntry = ent.Next;
                        ent.Next = newBuckets[targetBucket];
                        newBuckets[targetBucket] = ent;
                        ent = nextEntry;
                    }
                }
            }

            buckets = newBuckets;
        }
    }
}
