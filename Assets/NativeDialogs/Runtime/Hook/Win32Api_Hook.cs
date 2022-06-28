#if !UNITY_EDITOR && UNITY_STANDALONE_WIN
using System;
using System.Runtime.InteropServices;
namespace NativeDialogs.Runtime
{
    class Win32Api_Hook
    {
        #region DllImport

        /// <summary>
        /// 设置钩子
        /// </summary>
        /// <param name="idHook">钩子类型，此处用整形的枚举表示</param>
        /// <param name="lpfn">钩子发挥作用时的回调函数</param>
        /// <param name="hInstance">应用程序实例的模块句柄(一般来说是你钩子回调函数所在的应用程序实例模块句柄)</param>
        /// <param name="threadId">与安装的钩子子程相关联的线程的标识符</param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern int SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hInstance, int threadId);

        /// <summary>
        /// 抽掉钩子
        /// </summary>
        /// <param name="idHook">要取消的钩子的句柄</param>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern bool UnhookWindowsHookEx(int idHook);

        /// <summary>
        /// 调用下一个钩子
        /// </summary>
        /// <param name="idHook">当前钩子的句柄</param>
        /// <param name="nCode">钩子链传回的参数，非0表示要丢弃这条消息，0表示继续调用钩子</param>
        /// <param name="wParam">传递的参数</param>
        /// <param name="lParam">传递的参数</param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern int CallNextHookEx(int idHook, int nCode, int wParam, IntPtr lParam);

        /// <summary>
        /// 获取一个应用程序或动态链接库的模块句柄
        /// </summary>
        /// <param name="name">指定模块名，这通常是与模块的文件名相同的一个名字</param>
        /// <returns></returns>
        [DllImport("kernel32.dll")]
        public static extern IntPtr GetModuleHandle(string name);

        #endregion

        /// <summary>
        /// 得到当前的线程ID
        /// </summary>
        /// <returns></returns>
        [DllImport("kernel32.dll")]
        public static extern int GetCurrentThreadId();

        /// <summary>
        /// 得到Dialog窗口的子项
        /// </summary>
        /// <param name="hDlg"></param>
        /// <param name="nIDDlgItem"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern int GetDlgItem(IntPtr hDlg, int nIDDlgItem);

        /// <summary>
        /// 设置Dialog窗口子项的文本
        /// </summary>
        /// <param name="hDlg"></param>
        /// <param name="nIDDlgItem"></param>
        /// <param name="lpString"></param>
        /// <returns></returns>
        [DllImport("user32", EntryPoint = "SetDlgItemText", CharSet = CharSet.Unicode)]
        public static extern int SetDlgItemTextA(IntPtr hDlg, int nIDDlgItem, string lpString);
        
        /// <summary>
        /// 消息框
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="text"></param>
        /// <param name="caption"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        [DllImport("User32.dll",CharSet = CharSet.Unicode)]
        public static extern int MessageBoxW(IntPtr hWnd, string text, string caption, int type);
    }
}

#endif