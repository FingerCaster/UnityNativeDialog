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

    public class DialogManager : MonoBehaviour, IDialogReceiver
    {
        private static DialogManager m_Instance;

        public static DialogManager Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    // Find if there is already DialogManager in the scene
                    m_Instance = FindObjectOfType<DialogManager>();
                    if (m_Instance == null)
                    {
                        m_Instance = new GameObject("DialogManager").AddComponent<DialogManager>();
                    }

                    DontDestroyOnLoad(m_Instance.gameObject);
                }

                return m_Instance;
            }
        }

        private Dictionary<int, Action<DialogResult>> m_Callbacks;
        private IDialog dialog;

        public void Awake()
        {
            if (m_Instance == null)
            {
                // If I am the first instance, make me the Singleton
                m_Instance = this;
                DontDestroyOnLoad(this);

                m_Callbacks = new Dictionary<int, Action<DialogResult>>();
                dialog = CreateDialog();
            }
            else
            {
                // If s singleton already exists and you find
                // another reference in scene, destroy it!
                if (this != m_Instance)
                {
                    Destroy(gameObject);
                }
            }
        }

        private IDialog CreateDialog()
        {
#if UNITY_EDITOR
            DialogEditor dialogEditor = new DialogEditor();
            dialogEditor.Initialize(this);
            return dialogEditor;
#elif UNITY_STANDALONE_OSX
            DialogMacOs dialogMac = new DialogMacOs();
            dialogMac.Initialize(this);
            return dialogMac;
#elif UNITY_STANDALONE_WIN
            DialogWindow dialogWindow = new DialogWindow();
            dialogWindow.Initialize(this);
            return dialogWindow;
#elif UNITY_ANDROID
            return new DialogAndroid();
#elif UNITY_IOS
            return new DialogIos();
#else
            Debug.LogWarning($"{Application.platform} is not supported.");
            var mock = gameObject.AddComponent<DialogMock>();
            mock.Initialize(this, DialogResult.Confirm);
            return mock;
#endif
        }

        private string m_Confirm = "Confirm";
        private string m_Cancel = "Cancel";
        private string m_Other = "Other";

        public void SetNormalLabel(string confirm, string cancel, string other)
        {
            m_Confirm = confirm;
            m_Cancel = cancel;
            m_Other = other;
        }

        public void ShowDialog(string message, Action<DialogResult> callback)
        {
            int id = dialog.ShowDialog(string.Empty, message, null, m_Confirm);
            m_Callbacks.Add(id, callback);
        }

        public void ShowDialog(string title, string message, Action<DialogResult> callback)
        {
            int id = dialog.ShowDialog(title, message, null, m_Confirm);
            m_Callbacks.Add(id, callback);
        }

        public void ShowDialog(string title, string message, string confirm, Action<DialogResult> callback)
        {
            int id = dialog.ShowDialog(title, message, null, confirm);
            m_Callbacks.Add(id, callback);
        }

        public void ShowDialog(string title, string message, string cancel, string confirm,
            Action<DialogResult> callback)
        {
            int id = dialog.ShowDialog(title, message, cancel, confirm);
            m_Callbacks.Add(id, callback);
        }

        public void ShowDialog(string title, string message, string cancel, string confirm,
            string other, Action<DialogResult> callback)
        {
            int id = dialog.ShowDialog(title, message, cancel, confirm, other);
            m_Callbacks.Add(id, callback);
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