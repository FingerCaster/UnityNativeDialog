 #if !UNITY_EDITOR && UNITY_IOS

 using System;
 using System.Runtime.InteropServices;
using AOT;

namespace NativeDialogs.Runtime
{
    public class DialogIos : IDialog
    {
        delegate void CallBack(int id,int result);

        public static Action<int, DialogResult> CallBack2Receiver; 
        [DllImport("__Internal")]
        private static extern int _ShowDialog(string title = null, string info = null,
            string cancel = null, string confirm = null, string other = null,CallBack callBack = null);
        
        public int ShowDialog(string title = null, string message = null, string cancel = null, string confirm = null,
            string other = null)
        {
            return _ShowDialog(title,message,cancel,confirm,other,Callback);
        }
        public void Initialize(IDialogReceiver dialogReceiver, IDelayManager delayManager = null)
        {
            
        }

        [MonoPInvokeCallback(typeof(CallBack))]
        private static void Callback(int id,int result)
        {
            CallBack2Receiver.Invoke(id, (DialogResult)result);
        }
    }
}
#endif