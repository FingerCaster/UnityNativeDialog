using System;
using System.Collections.Generic;
using UnityEngine;

namespace NativeDialogs.Runtime
{
    public enum DialogResult
    {
        Confirm = 0,
        Cancel = 1,
        Other = 2,
    }

    public class DialogManager :IDialogManager
    {
        private IDialog m_Dialog;
        private IDialogReceiver m_DialogReceiver;
        private IDelayManager m_DelayManager;

        public void Initialize(IDialogReceiver dialogReceiver,IDelayManager delayManager)
        {
            m_DialogReceiver = dialogReceiver;
            m_DelayManager = delayManager;
            CreateDialog();
        }

        private void CreateDialog()
        {
#if UNITY_EDITOR
            m_Dialog = new DialogEditor();
#elif UNITY_STANDALONE_OSX
            m_Dialog = new DialogMacOs();
#elif UNITY_STANDALONE_WIN
            m_Dialog = new DialogWindow();
#elif UNITY_ANDROID
            m_Dialog = new DialogAndroid();
#elif UNITY_IOS
            m_Dialog =  new DialogIos();
            DialogIos.CallBackEvent = dialogReceiver.OnClick;
#else
            Debug.LogWarning($"{Application.platform} is not supported.");
            GameObject dialog = new GameObject("DialogMock");
            m_Dialog = dialog.AddComponent<DialogMock>();
#endif
            m_Dialog.Initialize(m_DialogReceiver,m_DelayManager);
        }

        private string m_Confirm = "Confirm";

        public void SetNormalLabel(string confirm)
        {
            m_Confirm = confirm;
        }

        public void ShowDialog(string message, Action<DialogResult> callback)
        {
            int id = m_Dialog.ShowDialog(string.Empty, message, null, m_Confirm);
            m_DialogReceiver.Register(id, callback);
        }

        public void ShowDialog(string title, string message, Action<DialogResult> callback)
        {
            int id = m_Dialog.ShowDialog(title, message, null, m_Confirm);
            m_DialogReceiver.Register(id, callback);
        }

        public void ShowDialog(string title, string message, string confirm, Action<DialogResult> callback)
        {
            int id = m_Dialog.ShowDialog(title, message, null, confirm);
            m_DialogReceiver.Register(id, callback);
        }

        public void ShowDialog(string title, string message, string cancel, string confirm,
            Action<DialogResult> callback)
        {
            int id = m_Dialog.ShowDialog(title, message, cancel, confirm);
            m_DialogReceiver.Register(id, callback);
        }

        public void ShowDialog(string title, string message, string cancel, string confirm,
            string other, Action<DialogResult> callback)
        {
            int id = m_Dialog.ShowDialog(title, message, cancel, confirm, other);
            m_DialogReceiver.Register(id, callback);
        }
    }
}