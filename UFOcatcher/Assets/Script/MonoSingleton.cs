using UnityEngine;

namespace Utility
{
    public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T m_instance;
        private static bool m_isShuttingDown = false;

        public static T Instance
        {
            get
            {
                if(m_isShuttingDown)
                {
                    Debug.LogWarning("[MonoSingleton] Instance '" + typeof(T) +
                                     "' already destroyed. Returning null.");
                    return null;
                }

                if(m_instance == null)
                {
                    // Try to find an existing instance
                    m_instance = (T)FindObjectOfType(typeof(T));

                    // Create a new instance if one doesn't already exist
                    if(m_instance == null)
                    {
                        string resourcePath = $"{typeof(T).FullName?.Replace('.', '/')}";
                        Debug.Log(
                            $"[MonoSingleton] Instance '{typeof(T)}' not found. Creating a new instance from '{resourcePath}'");

                        // Load the prefab from the Resources folder
                        GameObject prefab = Resources.Load<GameObject>(resourcePath);
                        if(prefab != null)
                        {
                            Debug.Log("Resource loaded successfully.");

                            // Create a new GameObject and attach the singleton component
                            m_instance = Instantiate(prefab).GetComponent<T>();
                        }
                        else
                        {
                            Debug.LogError("Resource not found at path: " + resourcePath);
                        }

                        // Make the instance persistent across scenes)
                        DontDestroyOnLoad(m_instance);
                    }
                }

                return m_instance;
            }
        }

        protected virtual void Awake()
        {
            if(m_instance == null)
            {
                m_instance = this as T;
                Init();
                DontDestroyOnLoad(gameObject);
            }
            else if(m_instance != this)
            {
                Destroy(gameObject);
            }
        }

        protected abstract void Init();

        private void OnApplicationQuit()
        {
            m_isShuttingDown = true;
        }

        private void OnDestroy()
        {
            m_isShuttingDown = true;
        }
    }
}