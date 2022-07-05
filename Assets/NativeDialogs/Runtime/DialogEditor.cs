#if UNITY_EDITOR

using System;
using UnityEditor;

namespace NativeDialogs.Runtime
{
    public class DialogEditor : IDialog
    {
        private IDialogReceiver m_DialogReceiver;
        private IDelayManager m_DelayManager;
        private int m_Id = 0;

        public int ShowDialog(string title = null, string message = null, string cancel = null, string confirm = null,
            string other = null)
        {
            int newId = m_Id++;
            int result;
            if (string.IsNullOrEmpty(other))
            {
                result = EditorUtility.DisplayDialog(title, message, confirm, cancel)?0:1;
            }
            else
            {
                result = EditorUtility.DisplayDialogComplex(title, message, confirm, cancel,other);
            }

            ExecuteCallback(newId,result);
            return newId;
        }

        public void Initialize(IDialogReceiver dialogReceiver, IDelayManager delayManager = null)
        {
            m_DialogReceiver = dialogReceiver;
            m_DelayManager = delayManager;
        }

        private void ExecuteCallback(int id, int result)
        {
            m_DelayManager.DelayFrame(() =>
            {
                m_DialogReceiver.OnClick(id,(DialogResult) result);
            });
        }
    }
}
#endif
