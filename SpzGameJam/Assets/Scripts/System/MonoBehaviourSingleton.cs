using UnityEngine;

/// <summary>
/// Singleton class
/// </summary>
/// <typeparam name="T">Type of the singleton</typeparam>
public abstract class MonoBehaviourSingleton<T> : MonoBehaviour where T : MonoBehaviourSingleton<T>
{
    private static T _instance;

    /// <summary>
    /// The static reference to the instance
    /// </summary>
    public static T Instance
    {
        get
        {
            return _instance;
        }
        protected set
        {
            _instance = value;
        }
    }

    // Instance の省略系
    public static T I{get{return _instance;}}

    /// <summary>
    /// Gets whether an instance of this singleton exists
    /// </summary>
    public static bool InstanceExists { get { return _instance != null; } }

    /// <summary>
    /// Awake method to associate singleton with instance
    /// </summary>
    protected virtual void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = (T)this;
        }
    }

    /// <summary>
    /// OnDestroy method to clear singleton association
    /// </summary>
    protected virtual void OnDestroy()
    {
        if (_instance == this)
        {
            _instance = null;
        }
    }
}