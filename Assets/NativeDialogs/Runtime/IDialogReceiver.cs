namespace NativeDialogs.Runtime
{
    public interface IDialogReceiver
    {
        void OnClick(int id, DialogResult dialogResult);
    }
}