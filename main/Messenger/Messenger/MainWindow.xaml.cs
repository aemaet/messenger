using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Runtime.InteropServices;

namespace Messenger
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public enum OperationResult: int {
        Ok,
        AuthError,
        NetworkError,
        InternalError
    }
    
    
    public partial class MainWindow : Window
    {

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void LoginCallback(OperationResult result);

        public static class MessengerInterop 
        {
            [DllImport("interop.dll", CallingConvention = CallingConvention.Cdecl)]
            public static extern void Init();

            [DllImport("interop.dll", CallingConvention = CallingConvention.Cdecl)]
            public static extern void Login(IntPtr loginCallback);

            [DllImport("interop.dll", CallingConvention = CallingConvention.Cdecl)]
            public static extern void Disconnect();

            [DllImport("interop.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
            public static extern void SendMessage([MarshalAs(UnmanagedType.LPStr)] string user, string message);
        }

        public sealed class MessengerClient
        {
            public Task<OperationResult> Login()
            {
                var task = new TaskCompletionSource<OperationResult>();
                var loginCallback = Marshal.GetFunctionPointerForDelegate(new LoginCallback(task.SetResult));
                MessengerInterop.Login(loginCallback);
                return task.Task;

            }

        }
        MessengerClient msgClient = new MessengerClient();
        public MainWindow()
        {
            MessengerInterop.Init();
        }

        private async void Connect_Click(object sender, RoutedEventArgs e)
        {
            var loginInfo = msgClient.Login();
            TestLabel.Content = await loginInfo;

           // TestLabel.Content = res;
         
        }

        private void Disconnect_Click(object sender, RoutedEventArgs e)
        {
            MessengerInterop.Disconnect();
        }

        private void Users_Click(object sender, RoutedEventArgs e)
        {
            ChatWindow chatWindow = new ChatWindow();
            chatWindow.Show();
        }
    }
}

