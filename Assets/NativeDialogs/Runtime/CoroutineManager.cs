using System;
using System.Collections;
using UnityEngine;

namespace NativeDialogs.Runtime
{
    public class CoroutineManager : MonoBehaviour
    {
        private static CoroutineManager m_Instance;
        public static CoroutineManager Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    // Find if there is already DialogManager in the scene
                    m_Instance = FindObjectOfType<CoroutineManager>();
                    if (m_Instance == null)
                    {
                        m_Instance = new GameObject("CoroutineManager").AddComponent<CoroutineManager>();
                    }
                    DontDestroyOnLoad(m_Instance.gameObject);
                }
                return m_Instance;
            }
        }
        public void Awake()
        {
            if (m_Instance == null)
            {
                m_Instance = this;
                DontDestroyOnLoad(this);
            }
            else
            {
                if (this != m_Instance)
                {
                    Destroy(gameObject);
                }
            }
        }

        private WaitForEndOfFrame m_Frame = new WaitForEndOfFrame();
        
        public void DelayFrame(Action callback)
        {
            StartCoroutine(InternalDelayFrame(callback));
        }

        private IEnumerator InternalDelayFrame(Action callback)
        {
            yield return m_Frame;
            callback?.Invoke();
        }
    }
}