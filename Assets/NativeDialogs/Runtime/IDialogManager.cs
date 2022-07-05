using System;

namespace NativeDialogs.Runtime
{
    public interface IDialogManager
    {
        void Initialize(IDialogReceiver dialogReceiver,IDelayManager delayManager);
        void SetNormalLabel(string confirm);
        void ShowDialog(string message, Action<DialogResult> callback);
        void ShowDialog(string title, string message, Action<DialogResult> callback);
        void ShowDialog(string title, string message, string confirm, Action<DialogResult> callback);

        void ShowDialog(string title, string message, string cancel, string confirm,
            Action<DialogResult> callback);

        void ShowDialog(string title, string message, string cancel, string confirm,
            string other, Action<DialogResult> callback);
    }
}