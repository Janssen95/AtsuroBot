using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.IO;
using System.Net.Sockets;

namespace JanssenBot
{
    class ircLib
    {
        public static string PrivMessageString(string message, string userReciever)
        {
            string result = "PRIVMSG " + userReciever + " :" + message;
            return result;
        }

        public static string GetChannel(string message)
        {
            string channel = string.Empty;

            int messCount = message.Length;
            char[] messChar = message.ToCharArray();
            for(int i = message.Length - 1; messChar[i] != '#'; i--)
            {
                channel = messChar[i] + channel;
            }

            messChar = channel.ToCharArray();
            channel = string.Empty;

            for(int i = 0; messChar[i] != ' '; i++)
            {
                channel = channel + messChar[i];
            }

            return channel;
        }
    }
}
