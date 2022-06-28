#if !UNITY_EDITOR && UNITY_IOS
using System.Runtime.InteropServices;

namespace NativeDialogs.Runtime
{
    public class DialogIos : IDialog
    {
        [DllImport("__Internal")]
        private static extern int _ShowDialog(string title = null, string info = null,
            string cancel = null, string confirm = null, string other = null);

        public int ShowDialog(string title = null, string message = null, string cancel = null, string confirm = null,
            string other = null)
        {
            return _ShowDialog(title,message,cancel,confirm,other);
        }
    }
}
#endif