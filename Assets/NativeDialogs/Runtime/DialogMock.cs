#if !UNITY_EDITOR && !ENABLE_MONO && !UNITY_STANDALONE_OSX && !UNITY_ANDROID && !UNITY_IOS
using System.Collections;
using UnityEngine;

namespace NativeDialogs.Runtime
{
    internal sealed class DialogMock : MonoBehaviour, IDialog
    {
        [SerializeField]
        private DialogResult mockResult = DialogResult.Confirm;

        private int m_ID = 0;
        private IDialogReceiver m_DialogReceiver;


        public void Initialize(IDialogReceiver receiver,DialogResult result)
        {
            this.m_DialogReceiver = receiver;
            this.mockResult = result;
        }
        public int ShowDialog(string title = null, string message = null, string cancel = null, string confirm = null,
            string other = null)
        {
            int newID = ++m_ID;
            ExecuteMockCallback(newID);
            return newID;
        }
        private void ExecuteMockCallback(int id)
        {
            CoroutineManager.Instance.DelayFrame(() =>
            {
                m_DialogReceiver.OnClick(id, mockResult);
            });
        }
    }
}
#endif