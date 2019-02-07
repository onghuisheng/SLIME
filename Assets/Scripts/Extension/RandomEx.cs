using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class RandomEx
{

    public static string RandomString(params string[] strings)
    {
        return strings[Random.Range(0, strings.Length)];
    }

}