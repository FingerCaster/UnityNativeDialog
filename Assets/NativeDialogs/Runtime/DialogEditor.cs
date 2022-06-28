#if UNITY_EDITOR

using System;
using UnityEditor;

namespace NativeDialogs.Runtime
{
    public class DialogEditor : IDialog
    {
        private IDialogReceiver m_DialogReceiver;
        private int m_Id = 0;
        public void Initialize(IDialogReceiver receiver)
        {
            m_DialogReceiver = receiver;
        }

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

        private void ExecuteCallback(int id, int result)
        {
            CoroutineManager.Instance.DelayFrame(() =>
            {
                m_DialogReceiver.OnClick(id,(DialogResult) result);
            });
        }
    }
}
#endif
