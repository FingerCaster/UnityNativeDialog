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
        private IDelayManager m_DelayManager;

        public int ShowDialog(string title = null, string message = null, string cancel = null, string confirm = null,
            string other = null)
        {
            int newID = ++m_ID;
            ExecuteMockCallback(newID);
            return newID;
        }

        public void Initialize(IDialogReceiver dialogReceiver, IDelayManager delayManager = null)
        {
            m_DialogReceiver = dialogReceiver;
            m_DelayManager = delayManager;
        }

        private void ExecuteMockCallback(int id)
        {
            m_DelayManager.DelayFrame(() =>
            {
                m_DialogReceiver.OnClick(id, mockResult);
            });
        }
    }
}
#endif