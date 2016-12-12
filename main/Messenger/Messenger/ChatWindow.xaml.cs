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


namespace Messenger
{
    /// <summary>
    /// Interaction logic for ChatWindow.xaml
    /// </summary>
    public partial class ChatWindow : Window
    {
        public ChatWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            TestChatLabel.Content = ChatBox.Text;
            MainWindow.MessengerInterop.SendMessage("asd@dsa", "hi");
        }

        private void ChatBoxKeyDown(Object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                TestChatLabel.Content = ChatBox.Text;
            }
        }
    }
}