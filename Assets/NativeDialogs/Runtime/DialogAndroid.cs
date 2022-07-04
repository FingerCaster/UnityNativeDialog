#if !UNITY_EDITOR && UNITY_ANDROID
using UnityEngine;

namespace NativeDialogs.Runtime
{
    
    public class DialogAndroid : IDialog
    {
        class CallBackProxy : AndroidJavaProxy
        {
            public CallBackProxy() : base("unity.plugins.dialog.ICallback")
            {
            
            }

            public void CallBack(int id, int result)
            {
                DialogManager.Instance.OnClick(id, (DialogResult)result);
            }
        }
        private readonly AndroidJavaObject m_AndroidJavaObject;
        
        public DialogAndroid()
        {
            m_AndroidJavaObject = new AndroidJavaClass("unity.plugins.dialog.DialogManager").CallStatic<AndroidJavaObject>("getInstance");
            m_AndroidJavaObject.Call("SetCallBack", new CallBackProxy());
        }

        public int ShowDialog(string title = null, string message = null, string cancel = null, string confirm = null,
            string other = null)
        {
            return m_AndroidJavaObject.Call<int>("ShowDialog",title, message, cancel, confirm, other);
        }
    }
}
#endif