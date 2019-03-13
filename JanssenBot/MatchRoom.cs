using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Windows;
using System.Windows.Controls;
using System.IO;
using System.Net.Sockets;

namespace JanssenBot
{
    public class MatchRoom
    {
        string channel = string.Empty;

        public MatchRoom()
        {
            CreateMatch();
        }

        private void CreateMatch()
        {
            if (MainWindow.client.Connected)
            {
                MainWindow.writer.WriteLine(ircLib.PrivMessageString("!mp make lol", "BanchoBot", false));
                MainWindow.writer.Flush();
            }
        }
    }
}
