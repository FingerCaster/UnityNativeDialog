 #if !UNITY_EDITOR && UNITY_IOS

using System.Runtime.InteropServices;
using AOT;

namespace NativeDialogs.Runtime
{
    public class DialogIos : IDialog
    {
        delegate void CallBack(int id,int result);
        [DllImport("__Internal")]
        private static extern int _ShowDialog(string title = null, string info = null,
            string cancel = null, string confirm = null, string other = null,CallBack callBack = null);
        
        public int ShowDialog(string title = null, string message = null, string cancel = null, string confirm = null,
            string other = null)
        {
            return _ShowDialog(title,message,cancel,confirm,other,Callback);
        }
       
        [MonoPInvokeCallback(typeof(CallBack))]
        private static void Callback(int id,int result)
        {
            DialogManager.Instance.OnClick(id,(DialogResult)result);
        }
    }
}
#endif