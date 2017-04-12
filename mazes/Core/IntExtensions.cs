namespace mazes.Core {
    public static class IntExtensions {
        public static string ToBase36(this int i) {
            const string chars = "0123456789abcdefghijklmnopqrstuvwxyz";
            return chars[i % 36].ToString();
        }

        public static string ToBase64(this int i) {
            const string chars = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ%=";
            return chars[i % 64].ToString();
        }
    }
}