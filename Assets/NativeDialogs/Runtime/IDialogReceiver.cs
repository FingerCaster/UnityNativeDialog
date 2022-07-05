using System;

namespace NativeDialogs.Runtime
{
    public interface IDialogReceiver
    {
        void Initialize();
        void Register(int id, Action<DialogResult> callBack);
        void OnClick(int id, DialogResult dialogResult);
    }
}