using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ArrayExtensions 
{
    public static List<T> ToList<T>(this T[] array)
    {
        List<T> components = new List<T>(array);
        return components;
    }
}
