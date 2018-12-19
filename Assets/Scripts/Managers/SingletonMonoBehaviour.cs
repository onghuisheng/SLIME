using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
    private static volatile T _instance;
    private static object _syncObj = new object();

    public static T Instance {
        get {
            if (_instance == null)
            {
                _instance = FindObjectOfType<T>() as T;

                if (FindObjectsOfType<T>().Length > 1)
                {
                    return _instance;
                }

                if (_instance == null)
                {
                    lock (_syncObj)
                    {
                        var singleton = new GameObject();

                        singleton.name = typeof(T).ToString() + " (Singleton)";

                        _instance = singleton.AddComponent<T>();

                        DontDestroyOnLoad(singleton);
                    }
                }
            }
            return _instance;
        }
        private set {
            _instance = value;
        }
    }

    public static bool IsInstance()
    {
        return _instance;
    }

    public static void Destroy()
    {
        Destroy(Instance);
    }
    void OnDestroy()
    {
        Instance = null;
    }

    protected SingletonMonoBehaviour() { }
}