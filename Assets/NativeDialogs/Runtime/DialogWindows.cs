#if !UNITY_EDITOR && UNITY_STANDALONE_WIN
using System;
using System.Runtime.InteropServices;
using AOT;
using HookINCS;
using UnityEngine;

namespace NativeDialogs.Runtime
{
    public delegate int HookProc(int nCode, int wParam, IntPtr lParam);

    public class DialogWindow : IDialog
    {
        #region Hook

         /// <summary>
        /// 定义一个委托类型的事件 
        /// </summary>
        public static event Action<IntPtr> OnMessageBoxShow;
        /// <summary>
        /// 弹窗钩子
        /// </summary>
        private static int msboxHook = 0;

        /// <summary>
        /// 弹窗钩子回调函数
        /// </summary>
        private HookProc mboxHook;
        
        public void InstallMessageBoxHook()
        {
            if (msboxHook == 0)
            {
                mboxHook = new HookProc(DefaultMessageBoxHookProc);
                msboxHook = Win32Api_Hook.SetWindowsHookEx(
                    5,//HookType.WH_CBT = 5
                    mboxHook,
                    Win32Api_Hook.GetModuleHandle(
                        Application.productName
                    ),
                    Win32Api_Hook.GetCurrentThreadId() //自身线程，如果是0则表示全局
                );
                if (msboxHook == 0)
                    UninstallMessageBoxHook();
            }
        }
        /// <summary>
        /// 卸载钩子
        /// </summary>
        /// <param name="idhook"></param>
        private void UninstallHook(ref int idhook)
        {
            if (idhook != 0)
            {
                Win32Api_Hook.UnhookWindowsHookEx(idhook);
                idhook = 0;
            }
        }
        /// <summary>
        /// 卸载弹窗钩子
        /// </summary>
        public void UninstallMessageBoxHook()
        {
            UninstallHook(ref msboxHook);
        }
        
        [MonoPInvokeCallback(typeof(HookProc))]
        private static int DefaultMessageBoxHookProc(int nCode, int wParam, IntPtr lParam)
        {
            IntPtr hChildWnd; // msgbox is "child"
            // notification that a window is about to be activated
            // window handle is wParam
            if (nCode == 5) //HCBT_ACTIVATE = 5  
            {
                // set window handles of messagebox
                hChildWnd = (IntPtr) wParam;
                //to get the text of yes button
                //自定义事件
                OnMessageBoxShow?.Invoke(hChildWnd);
            }

            //return (IntPtr)1; //直接返回了，该消息就处理结束了
            return
                Win32Api_Hook.CallNextHookEx(msboxHook, nCode, wParam,
                    lParam); // otherwise, continue with any possible chained hooks; //返回，让后面的程序处理该消息
        }

        #endregion
        private int m_Id = 0;
        private IDialogReceiver m_DialogReceiver;

        public void Initialize(IDialogReceiver receiver)
        {
            m_DialogReceiver = receiver;
        }

        public int ShowDialog(string title = null, string message = null, string cancel = null, string confirm = null,
            string other = null)
        {
            int newId = m_Id++;
            int buttonCount = 0;
            if (!string.IsNullOrEmpty(confirm))
            {
                buttonCount ++;
            }
            if (!string.IsNullOrEmpty(cancel))
            {
                buttonCount ++;
            }
            if (!string.IsNullOrEmpty(other))
            {
                buttonCount ++;
            }

            int messageBoxButtons = buttonCount switch
            {
                0 => 0,
                1 => 0,
                2 => 4,
                3 => 3,
                _ => throw new ArgumentOutOfRangeException(nameof(buttonCount), buttonCount, null)
            };
            HookMessageBoxShow(message?? string.Empty, title?? string.Empty, messageBoxButtons, confirm, cancel, other,
                result => ExecuteCallback(newId, result));
            return newId;
        }
        
        private void HookMessageBoxShow(string text, string caption, int buttons,
            string yesText = "", string noText = "", string cancelText = "",
            Action<int> handleResult = null)
        {
            void OnHookOnOnMessageBoxShow( IntPtr hChildWnd)
            {
                if (!string.IsNullOrEmpty(yesText) && Win32Api_Hook.GetDlgItem(hChildWnd, 1) != 0) //IDOK = 6
                {
                    Win32Api_Hook.SetDlgItemTextA(hChildWnd, 1, $"{yesText}");
                }
            
                if (!string.IsNullOrEmpty(yesText) && Win32Api_Hook.GetDlgItem(hChildWnd, 6) != 0) //IDYES = 6
                {
                    Win32Api_Hook.SetDlgItemTextA(hChildWnd, 6, $"{yesText}");
                }
            
                if (!string.IsNullOrEmpty(noText) && Win32Api_Hook.GetDlgItem(hChildWnd, 7) != 0) //IDNO = 7
                {
                    Win32Api_Hook.SetDlgItemTextA(hChildWnd, 7, $"{noText}");
                }
            
                if (!string.IsNullOrEmpty(cancelText) && Win32Api_Hook.GetDlgItem(hChildWnd, 2) != 0) //IDCANCEL = 2
                {
                    Win32Api_Hook.SetDlgItemTextA(hChildWnd, 2, $"{cancelText}");
                }
            }
            
            OnMessageBoxShow += OnHookOnOnMessageBoxShow;
            InstallMessageBoxHook();
            var dialogResult = Win32Api_Hook.MessageBoxW(IntPtr.Zero, text, caption, buttons);
            handleResult?.Invoke(dialogResult);
            UninstallMessageBoxHook();
            OnMessageBoxShow -= OnHookOnOnMessageBoxShow;
        }

        private void ExecuteCallback(int id, int result)
        {
            CoroutineManager.Instance.DelayFrame(() =>
            {
                DialogResult dialogResult = result switch
                {
                    2 => DialogResult.Other,
                    6 => DialogResult.Confirm,
                    7 => DialogResult.Cancel,
                    1 => DialogResult.Confirm,
                    _ => throw new ArgumentOutOfRangeException(nameof(result), result, null)
                };
                m_DialogReceiver.OnClick(id, dialogResult);
            });
        }
    }
}
#endif