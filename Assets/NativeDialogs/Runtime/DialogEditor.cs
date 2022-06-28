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
            bool result = EditorUtility.DisplayDialog(title, message, confirm, cancel);
            ExecuteCallback(newId,result);
            return newId;
        }

        private void ExecuteCallback(int id, bool result)
        {
            CoroutineManager.Instance.DelayFrame(() =>
            {
                DialogResult dialogResult = result switch
                {
                    true => DialogResult.Confirm,
                    false => DialogResult.Cancel
                };

                m_DialogReceiver.OnClick(id, dialogResult);
            });
        }
    }
}
#endif
