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
    public class MatchRoom
    {
        StreamReader reader;
        StreamWriter writer;

        public MatchRoom(StreamReader reader, StreamWriter writer)
        {
            reader = this.reader;
            writer = this.writer;
        }
    }
}
