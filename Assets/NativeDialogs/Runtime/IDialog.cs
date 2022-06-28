namespace NativeDialogs.Runtime
{
    public interface IDialog
    {
        int ShowDialog(string title = null, string message = null, string cancel = null, string confirm = null,
            string other = null);
    }
}