#if !UNITY_EDITOR && UNITY_ANDROID
using UnityEngine;

namespace NativeDialogs.Runtime
{
    public class DialogAndroid : IDialog
    {
        private readonly AndroidJavaObject m_AndroidJavaObject;

        public DialogAndroid()
        {
            m_AndroidJavaObject = new AndroidJavaClass("unity.plugins.dialog.DialogManager").CallStatic<AndroidJavaObject>("getInstance");
        }

        public int ShowDialog(string title = null, string message = null, string cancel = null, string confirm = null,
            string other = null)
        {
            return m_AndroidJavaObject.Call<int>("ShowDialog",title, message, cancel, confirm, other);
        }
    }
}
#endif