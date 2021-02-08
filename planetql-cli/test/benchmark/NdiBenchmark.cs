using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ndi_benchmark
{
    class NdiBenchmark
    {
        static void Main(string[] args)
        {
            const int randSeed = 10;
            DataProcessor processor = new DataProcessor(randSeed);

            // Ordered Collection
            List<string> list = new List<string>();
            Queue<string> queue = new Queue<string>();
            ArrayList arrayList = new ArrayList();
            Stack<string> stack = new Stack<string>();

            // Unordered Collection
            Dictionary<string, string> dict = new Dictionary<string, string>();
            Hashtable ht = new Hashtable();
            HashSet<string> hs = new HashSet<string>();
            EnumClass userClass = new EnumClass();

            Object[] collections = { list, queue, arrayList, stack, dict, ht, hs, userClass };

            foreach (Object collection in collections)
            {
                processor.InitializeData(collection);
                processor.DoIteration(collection);
            }
        }
    }

    class DataProcessor
    {

        private static Random random;
        private static List<string> data;
        private const int dataNum = 5;
        private const int dataLen = 5;

        private void prepareData()
        {
            data = new List<string>(dataNum);
            for (int i = 0; i < dataNum; i++)
            {
                data.Add(RandomString(dataLen));
            }
        }
        public DataProcessor()
        {
            random = new Random();
            prepareData();
        }
        public DataProcessor(int seed)
        {
            random = new Random(seed);
            prepareData();
        }

        private static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public void InitializeData(Object collection)
        {
            int i;
            switch (collection)
            {
                case List<string> list:
                    for (i = 0; i < dataNum; i++)
                    {
                        list.Add(data[i]);
                    }
                    break;
                case Queue<string> queue:
                    for (i = 0; i < dataNum; i++)
                    {
                        queue.Append(data[i]);
                    }
                    break;
                case ArrayList arrayList:
                    for (i = 0; i < dataNum; i++)
                    {
                        arrayList.Add(data[i]);
                    }
                    break;
                case Stack<string> stack:
                    for (i = 0; i < dataNum; i++)
                    {
                        stack.Push(data[i]);
                    }
                    break;
                case Dictionary<string, string> dict:
                    for (i = 0; i < dataNum; i++)
                    {
                        dict.Add(data[i], data[i]);
                    }
                    break;
                case Hashtable ht:
                    for (i = 0; i < dataNum; i++)
                    {
                        ht.Add(data[i], data[i]);
                    }
                    break;
                case HashSet<string> hs:
                    for (i = 0; i < dataNum; i++)
                    {
                        hs.Add(data[i]);
                    }
                    break;
                case EnumClass userClass:
                    for (i = 0; i < dataNum; i++)
                    {
                        userClass.Add(data[i]);
                    }
                    break;
            }
        }
        /// <summary>
        /// 검출해야 하는 Non-deterministic Iteration Case
        /// <list type="number">
        /// <item>
        /// <description>Iterable Expression의 type이 Unordered Collection인 경우</description>
        /// </item>
        /// <item>
        /// <description>Unordered Collection으로부터 유래한 element를 Ordering Method 없이 사용한 경우</description>
        /// </item>
        /// <item>
        /// <description>User defined IEnumerable Class에 대한 반복문이 Non-deterministic할 가능성이 있는 경우</description>
        /// </item>
        /// <item>
        /// <description>IEnumerable type의 변수가 Unordered Collection을 가르킬 가능성이 있는 경우</description>
        /// </item>
        /// </list>
        /// </summary>
        public void DoIteration(Object collection)
        {
            switch (collection)
            {
                case List<string> list:
                    foreach (var v in list) ; // negative
                    foreach (var v in list.ToHashSet()) ; // case 1
                    foreach (var v in list.ToLookup(v => v)) ; // case 1
                    foreach (var v in list.ToDictionary(v => v)) ; // case 1
                    foreach (var v in list.Select(p => p)) ; // negative
                    foreach (var v in list.Where(v => v.Length == dataLen)) ; // negative
                    foreach (var v in list.OrderBy(v => v)) ; // negative
                    foreach (var v in list.ToList()) ; // negative
                    foreach (var v in list.ToArray()) ; // negative
                    foreach (var v in list.AsEnumerable()) ; // negative
                    foreach (var v in list.Select(p => p).Where(p => p.Length == dataLen).OrderBy(p => p)) ; // negative
                    DoIterationIEnumerable(list);
                    break;
                case Queue<string> queue:
                    foreach (var v in queue) ; // negative
                    foreach (var v in queue.ToHashSet()) ; // case 1
                    foreach (var v in queue.ToLookup(v => v)) ; // case 1
                    foreach (var v in queue.ToDictionary(v => v)) ; // case 1
                    foreach (var v in queue.Select(p => p)) ; // negative
                    foreach (var v in queue.Where(v => v.Length == dataLen)) ; // negative
                    foreach (var v in queue.OrderBy(v => v)) ; // negative
                    foreach (var v in queue.ToList()) ; // negative
                    foreach (var v in queue.ToArray()) ; // negative
                    foreach (var v in queue.AsEnumerable()) ; // negative
                    foreach (var v in queue.Reverse()) ; // negative
                    foreach (var v in queue.Select(p => p).Where(p => p.Length == dataLen).OrderBy(p => p)) ; // negative
                    DoIterationIEnumerable(queue);
                    break;
                case ArrayList arrayList:
                    foreach (var v in arrayList) ; // negative
                    foreach (var v in arrayList.ToArray()) ; // negative
                    break;
                case Stack<string> stack:
                    foreach (var v in stack) ; // negative
                    foreach (var v in stack.ToHashSet()) ; // case 1
                    foreach (var v in stack.ToLookup(v => v)) ; // case 1
                    foreach (var v in stack.ToDictionary(v => v)) ; // case 1
                    foreach (var v in stack.Select(p => p)) ; // negative
                    foreach (var v in stack.Where(v => v.Length == dataLen)) ; // negative
                    foreach (var v in stack.OrderBy(v => v)) ; // negative
                    foreach (var v in stack.ToList()) ; // negative
                    foreach (var v in stack.ToArray()) ; // negative
                    foreach (var v in stack.AsEnumerable()) ; // negative
                    foreach (var v in stack.Reverse()) ; // negative
                    foreach (var v in stack.Select(p => p).Where(p => p.Length == dataLen).OrderBy(p => p)) ; // negative
                    DoIterationIEnumerable(stack);
                    break;
                case Dictionary<string, string> dict:
                    foreach (var v in dict) ; // case 1
                    foreach (var v in dict.Keys) ; // case 1
                    foreach (var v in dict.Values) ; // case 1
                    foreach (var v in dict.Keys.ToHashSet()) ; // case 1
                    foreach (var v in dict.Keys.ToLookup(v => v)) ; // case 1
                    foreach (var v in dict.Keys.ToDictionary(v => v)) ; // case 1
                    foreach (var v in dict.Keys.Select(p => p)) ; // case 2 
                    foreach (var v in dict.Keys.Where(v => v.Length == dataLen)) ; // case 2 
                    foreach (var v in dict.Keys.OrderBy(v => v)) ; // negative
                    foreach (var v in dict.Keys.ToList()) ; //case 2
                    foreach (var v in dict.Keys.ToArray()) ; // case 2
                    foreach (var v in dict.Keys.AsEnumerable()) ; //case 2
                    foreach (var v in dict.Keys.Reverse()) ; // case 2
                    foreach (var v in dict.Keys.Select(p => p).Where(p => p.Length == dataLen).OrderBy(p => p)) ; // negative
                    DoIterationIEnumerable(dict.Keys);
                    DoIterationIEnumerable(dict.Values);
                    break;
                case Hashtable ht:
                    foreach (var v in ht) ; // case 1
                    foreach (var v in ht.Keys) ; // case 2
                    foreach (var v in ht.Values) ; // case 2
                    break;
                case HashSet<string> hs:
                    foreach (var v in hs) ; // case 2
                    foreach (var v in hs.ToHashSet()) ; // case 1
                    foreach (var v in hs.ToLookup(v => v)) ; // case 1
                    foreach (var v in hs.ToDictionary(v => v)) ; // case 1
                    foreach (var v in hs.Select(p => p)) ; // case 2
                    foreach (var v in hs.Where(v => v.Length == dataLen)) ; // case 2
                    foreach (var v in hs.OrderBy(v => v)) ; // negative
                    foreach (var v in hs.ToList()) ; // case 2
                    foreach (var v in hs.ToArray()) ; //case 2
                    foreach (var v in hs.AsEnumerable()) ; // case 2
                    foreach (var v in hs.Reverse()) ; // case 2
                    foreach (var v in hs.Select(p => p).Where(p => p.Length == dataLen).OrderBy(p => p)) ; // negative
                    DoIterationIEnumerable(hs);
                    break;
                case EnumClass userClass:
                    foreach (var v in userClass) ; // case 3
                    foreach (var v in userClass.property) ; // case 1
                    foreach (var v in userClass.property.ToHashSet()) ; // case 1
                    foreach (var v in userClass.property.ToLookup(v => v)) ; // case 1
                    foreach (var v in userClass.property.ToDictionary(v => v)) ; // case 1
                    foreach (var v in userClass.property.Select(p => p)) ; // case 2
                    foreach (var v in userClass.property.Where(v => v.Length == dataLen)) ; // case 2
                    foreach (var v in userClass.property.OrderBy(v => v)) ; // negative
                    foreach (var v in userClass.property.ToList()) ; // case 2
                    foreach (var v in userClass.property.ToArray()) ; // case 2
                    foreach (var v in userClass.property.AsEnumerable()) ; // case 2
                    foreach (var v in userClass.property.Reverse()) ; // case 2
                    foreach (var v in userClass.property.Select(p => p).Where(p => p.Length == dataLen).OrderBy(p => p)) ; // negative
                    foreach (var v in userClass.field) ; // case 1
                    foreach (var v in userClass.field.ToHashSet()) ; // case 1
                    foreach (var v in userClass.field.ToLookup(v => v)) ; // case 1
                    foreach (var v in userClass.field.ToDictionary(v => v)) ; // case 1
                    foreach (var v in userClass.field.Select(p => p)) ; // case 2
                    foreach (var v in userClass.field.Where(v => v.Length == dataLen)) ; // case 2
                    foreach (var v in userClass.field.OrderBy(v => v)) ; // negative
                    foreach (var v in userClass.field.ToList()) ; // case 2
                    foreach (var v in userClass.field.ToArray()) ; // case 2
                    foreach (var v in userClass.field.AsEnumerable()) ; // case 2
                    foreach (var v in userClass.field.Reverse()) ; // case 2
                    foreach (var v in userClass.field.Select(p => p).Where(p => p.Length == dataLen).OrderBy(p => p)) ; // negative
                    foreach (var v in userClass.listField) ; // negative
                    foreach (var v in userClass.listField.ToHashSet()) ; // case 1
                    foreach (var v in userClass.listField.ToLookup(v => v)) ; // case 1
                    foreach (var v in userClass.listField.ToDictionary(v => v)) ; // case 1
                    foreach (var v in userClass.listField.Select(p => p)) ; // negative
                    foreach (var v in userClass.listField.Where(v => v.Length == dataLen)) ; // negative
                    foreach (var v in userClass.listField.OrderBy(v => v)) ; // negative
                    foreach (var v in userClass.listField.ToList()) ; // negative
                    foreach (var v in userClass.listField.ToArray()) ; // negative
                    foreach (var v in userClass.listField.AsEnumerable()) ; // negative
                    foreach (var v in userClass.listField.Select(p => p).Where(p => p.Length == dataLen).OrderBy(p => p)) ; // negative
                    DoIterationIEnumerable(userClass);
                    DoIterationIEnumerable(userClass.property);
                    DoIterationIEnumerable(userClass.field);
                    DoIterationIEnumerable(userClass.listField);
                    break;
            }
        }

        private void DoIterationIEnumerable(IEnumerable<string> enumerable)
        {
            foreach (var v in enumerable.ToHashSet()) ; // case 1
            foreach (var v in enumerable.ToLookup(v => v)) ; // case 1
            foreach (var v in enumerable.ToDictionary(v => v)) ; // case 1
            foreach (var v in enumerable.Where(v => v.Length == dataLen)) ; // case 4
            foreach (var v in enumerable.OrderBy(v => v)) ; // negative
            foreach (var v in enumerable.ToList()) ; // case 4
            foreach (var v in enumerable.ToArray()) ; // case 4
            foreach (var v in enumerable.AsEnumerable()) ; // case 4
            foreach (var v in enumerable.Reverse()) ; // case 4
            foreach (var v in enumerable.Select(p => p).Where(p => p.Length == dataLen).OrderBy(p => p)) ; // negative
        }

    }

    class EnumClass : IEnumerable<string>
    {

        public HashSet<string> property { get; set; }
        public HashSet<string> field;
        public List<string> listField;
        public EnumClass()
        {
            this.property = new HashSet<string>();
            this.field = new HashSet<string>();
            this.listField = new List<string>();
        }
        public void Add(string value)
        {
            this.property.Add(value);
            this.field.Add(value);
            this.listField.Add(value);
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
        public IEnumerator<string> GetEnumerator()
        {
            foreach (var v in property) // case 1
            {
                yield return v;
            }
        }
    }
}
