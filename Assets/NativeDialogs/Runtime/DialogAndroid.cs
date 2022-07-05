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
            private IDialogReceiver m_DialogReceiver;

            public void SetDialogReceiver(IDialogReceiver dialogReceiver)
            {
                m_DialogReceiver = dialogReceiver;
            }

            public void CallBack(int id, int result)
            {
                m_DialogReceiver.OnClick(id, (DialogResult)result);
            }
        }
        private AndroidJavaObject m_AndroidJavaObject;
        
        public void Initialize(IDialogReceiver dialogReceiver, IDelayManager delayManager = null)
        {
            m_AndroidJavaObject = new AndroidJavaClass("unity.plugins.dialog.DialogManager").CallStatic<AndroidJavaObject>("getInstance");
            var callBackProxy = new CallBackProxy();
            callBackProxy.SetDialogReceiver(dialogReceiver);
            m_AndroidJavaObject.Call("SetCallBack", callBackProxy);
        }
        

        public int ShowDialog(string title = null, string message = null, string cancel = null, string confirm = null,
            string other = null)
        {
            return m_AndroidJavaObject.Call<int>("ShowDialog",title, message, cancel, confirm, other);
        }
    }
}
#endif