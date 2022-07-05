using System;
using System.Collections;
using UnityEngine;

namespace NativeDialogs.Runtime
{
    public class CoroutineManager : MonoBehaviour,IDelayManager
    {
        private readonly WaitForEndOfFrame m_Frame = new WaitForEndOfFrame();
        
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