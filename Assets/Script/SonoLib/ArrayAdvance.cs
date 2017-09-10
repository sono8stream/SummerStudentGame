using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrayAdvance
{ 
    public static Type[] MergeArray<Type>(Type[] a,Type[] b)
    {
        List<Type> list = new List<Type>();
        list.AddRange(a);
        list.AddRange(b);
        return list.ToArray();
    }

    public static Type[] InsertArray<Type>(Type[] a, Type[] b, int index)
    {
        List<Type> list = new List<Type>();
        list.AddRange(a);
        list.InsertRange(index,b);
        return list.ToArray();
    }
}
