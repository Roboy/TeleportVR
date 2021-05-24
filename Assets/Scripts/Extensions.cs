public static class Extensions
{
    public static T Next<T>(this T src) where T : System.Enum
    {
        if (!typeof(T).IsEnum) throw new System.ArgumentException(string.Format("Argument {0} is not an Enum", typeof(T).FullName));

        T[] Arr = (T[])System.Enum.GetValues(src.GetType());
        int j = System.Array.IndexOf<T>(Arr, src) + 1;
        return (Arr.Length == j) ? Arr[0] : Arr[j];
    }

    public static bool IsFirst<T>(this T src) where T : System.Enum
    {
        if (!typeof(T).IsEnum) throw new System.ArgumentException(string.Format("Argument {0} is not an Enum", typeof(T).FullName));
        T[] Arr = (T[])System.Enum.GetValues(src.GetType());
        int i = System.Array.IndexOf<T>(Arr, src);
        return i == 0;
    }
    public static bool IsLast<T>(this T src) where T : System.Enum
    {
        if (!typeof(T).IsEnum) throw new System.ArgumentException(string.Format("Argument {0} is not an Enum", typeof(T).FullName));
        T[] Arr = (T[])System.Enum.GetValues(src.GetType());
        int i = System.Array.IndexOf<T>(Arr, src) + 1;
        return Arr.Length == i;
    }
}
