using UnityEngine;

public abstract class SingletonMonoBehaviorNoDestroy<T> : MonoBehaviour where T : SingletonMonoBehaviorNoDestroy<T>
{
    public static T instance => s_instance;

    private static T s_instance;

    protected virtual void Awake()
    {
        if (s_instance == null)
        {
            DontDestroyOnLoad(gameObject);
            s_instance = this as T;
            init();
        }
        else
        {
            Destroy(this);
        }
    }

    protected virtual void init() { }

    public static void ReleaseInstance()
    {
        if (s_instance != null)
        {
            Destroy(s_instance);
            s_instance = null;
        }
    }
}