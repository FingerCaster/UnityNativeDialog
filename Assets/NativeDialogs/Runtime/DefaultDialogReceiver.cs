using System;
using System.Collections.Generic;
using UnityEngine;

namespace NativeDialogs.Runtime
{
    public class DefaultDialogReceiver : IDialogReceiver
    {
        private Dictionary<int, Action<DialogResult>> m_Callbacks;
        
        public void Initialize()
        {
            m_Callbacks = new Dictionary<int, Action<DialogResult>>();
        }

        public void Register(int id, Action<DialogResult> callBack)
        {
            m_Callbacks.Add(id, callBack);
        }

        public void OnClick(int id, DialogResult dialogResult)
        {
            if (m_Callbacks.ContainsKey(id))
            {
                m_Callbacks[id](dialogResult);
                m_Callbacks.Remove(id);
            }
            else
            {
                Debug.LogWarning("Undefined id:" + id);
            }
        }
    }
}