#if !UNITY_EDITOR && UNITY_STANDALONE_OSX

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace NativeDialogs.Runtime
{
    public class DialogMacOs : IDialog
    {
        enum MacDialogStyle
        {
            Informational = 0,
            Warring = 1,
            Critical = 2,
        }
        
        enum MacDialogResult
        {
            Confirm = 1000,
            Cancel = 1001,
            Other = 1002,
        }

        [DllImport("MessageDialog")]
        private static extern int _ShowDialog(int style, string title = null, string info = null,
            string cancel = null, string confirm = null, string other = null);
        
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
            MacDialogResult result = (MacDialogResult)_ShowDialog((int) MacDialogStyle.Informational,title,message,cancel,confirm,other);
            ExecuteCallback(newId, result);
            return newId;
        }

        private void ExecuteCallback(int id, MacDialogResult result)
        {
            CoroutineManager.Instance.DelayFrame(() =>
            {
                DialogResult dialogResult;
                switch (result)
                {
                    case MacDialogResult.Confirm:
                        dialogResult = DialogResult.Confirm;
                        break;
                    case MacDialogResult.Cancel:
                        dialogResult = DialogResult.Cancel;
                        break;
                    case MacDialogResult.Other:
                        dialogResult = DialogResult.Other;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(result), result, null);
                }

                m_DialogReceiver.OnClick(id,dialogResult);
            });
        }
    }
}
#endif