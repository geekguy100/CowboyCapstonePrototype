/*****************************************************************************
// File Name :         Singleton.cs
// Author :            Kyle Grenier
// Creation Date :     11/1/2020
//
// Brief Description : A generic singleton class.
*****************************************************************************/
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    private static T instance;
    public static T Instance
    {
        get { return instance; }
    }

    public static bool isInitialized()
    {
        return instance != null;
    }

    protected virtual void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning(gameObject.name + ": Trying to instantiate a second instance of singleton. Destroying clone.");
            Destroy(gameObject);
        }
        else
            instance = (T)this;
    }

    protected virtual void OnDestroy()
    {
        //If this object is destroyed, make instance null so another can be created.
        if (instance == this)
            instance = null;
    }
}