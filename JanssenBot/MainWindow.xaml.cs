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
using System.Windows.Threading;
using System.IO;
using System.Net.Sockets;

namespace JanssenBot
{
    public partial class MainWindow : Window
    {
        public static TcpClient client;
        public static StreamReader reader;
        public static StreamWriter writer;
        private string username;
        private string password;
        DispatcherTimer timer;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Login win = new Login();
            win.ShowDialog();
            if(win.loggedIn == true)
            {
                username = win.UserBox.Text;
                password = win.PassBox.Text;
            }
            ircLib.Reconnect(username, password);
            Timer();
        }

        private void Timer()
        {
            timer = new DispatcherTimer();
            timer.Tick += new EventHandler(timer_Tick);
            timer.Interval = new TimeSpan(0, 0, 0, 0, 100);
            timer.IsEnabled = true;
            timer.Start();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            timer.Stop();

            if (!client.Connected)
            {
                ircLib.Reconnect(username, password);
            }

            if (client.Connected)
            {
                string message = ircLib.GetMessage();
                ircLib.PrintMessage(message, ircBox);
            }

            timer.Start();
        }

        private void NewMatch_Click(object sender, RoutedEventArgs e)
        {
            MatchRoom newMatch = new MatchRoom();
        }

        private void JoinOsu_Click(object sender, RoutedEventArgs e)
        {
            writer.WriteLine("JOIN #spanish");
            writer.Flush();
        }
    }
}
