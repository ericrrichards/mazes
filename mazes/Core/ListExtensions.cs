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
    }
}