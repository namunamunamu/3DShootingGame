using UnityEngine;

public class SingletonBehaviour<T> : MonoBehaviour where T: MonoBehaviour
{
    private static T _instance;
    private static bool _shuttingDown = false;
    private static object _lock = new object();
    
    public static T Instance
    {
        get
        {
            // if(_shuttingDown)
            // {
            //     Debug.LogWarning($"{typeof(T)} 는 이미 종료되었습니다.");
            //     return null;
            // }

            lock(_lock) // 스레드 안전
            {
                if(_instance == null)
                {
                    _instance = (T)FindFirstObjectByType(typeof(T));
                    DontDestroyOnLoad(_instance.gameObject);

                    if(_instance == null)
                    {
                        var singletonObject = new GameObject();
                        _instance = singletonObject.AddComponent<T>();
                        singletonObject.name = $"{typeof(T)}(Singleton)";
                        DontDestroyOnLoad(singletonObject);
                    }
                }
                return _instance;
            }


        }
    }

    // private void OnDestroy() 
    // {
    //     _shuttingDown = true;
    // }
}
