using System.Windows;
using Coding4Fun.Toolkit.Controls;

namespace EightBitCamera.Data.Commands
{
    public class ShowSavedMessageCommand
    {
        public void Show()
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                var toast = new ToastPrompt { Message = "Saved!" };
                toast.Show();
            });
        }
    }
}