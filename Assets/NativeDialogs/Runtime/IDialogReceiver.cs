namespace NativeDialogs.Runtime
{
    public interface IDialogReceiver
    {
        void OnConfirmClick(string args);
        void OnCancelClick(string args);
        void OnOtherClick(string args);

        void OnClick(int id, DialogResult dialogResult);
    }
}