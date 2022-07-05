using System;
using UnityEngine;

namespace NativeDialogs.Runtime
{
    public class NativeDialogComponent : MonoBehaviour
    {
        private IDelayManager m_DelayManager;
        private IDialogReceiver m_DialogReceiver;
        private IDialogManager m_DialogManager;
        private void Awake()
        {
            m_DelayManager = gameObject.AddComponent<CoroutineManager>();
            m_DialogReceiver = new DefaultDialogReceiver();
            m_DialogReceiver.Initialize();
            m_DialogManager = new DialogManager();
            m_DialogManager.Initialize(m_DialogReceiver,m_DelayManager);
        }
        public void ShowDialog(string message, Action<DialogResult> callback)
        {
            m_DialogManager.ShowDialog(message, callback);
        }

        public void ShowDialog(string title, string message, Action<DialogResult> callback)
        {
            m_DialogManager.ShowDialog(title, message,callback);
        }

        public void ShowDialog(string title, string message, string confirm, Action<DialogResult> callback)
        {
            m_DialogManager.ShowDialog(title,message,confirm, callback);
        }

        public void ShowDialog(string title, string message, string cancel, string confirm,
            Action<DialogResult> callback)
        {
            m_DialogManager.ShowDialog(title,message,cancel,confirm, callback);
        }

        public void ShowDialog(string title, string message, string cancel, string confirm,
            string other, Action<DialogResult> callback)
        {
            m_DialogManager.ShowDialog(title,message,cancel,confirm,other,callback);
        }
    }
}