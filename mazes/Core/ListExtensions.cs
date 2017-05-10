using System.Threading;

namespace mazes.Core {
    using System;
    using System.Collections.Generic;

    public static class ListExtensions {
        public static T Sample<T>(this List<T> list, Random rand = null) {
            if (rand == null) {
                rand = new Random();
            }
            return list[rand.Next(list.Count)];
        }

        public static List<T> Shuffle<T>(this List<T> list) {
            var n = list.Count;
            while (n > 1) {
                n--;
                var k = ThreadSafeRandom.ThisThreadsRandom.Next(n + 1);
                var value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
            return list;
        }
    }
    public static class ThreadSafeRandom {
        // http://stackoverflow.com/a/1262619/978460
        [ThreadStatic] private static Random _local;

        public static Random ThisThreadsRandom => _local ?? (_local = new Random(unchecked(Environment.TickCount * 31 + Thread.CurrentThread.ManagedThreadId)));
    }
}