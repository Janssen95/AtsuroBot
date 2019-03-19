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
        enum botState {playersNotInRoom, waitingWarmUpPick, playing, rolling, banning, waitingPlayerPick, matchDone}
        botState currentState = botState.playersNotInRoom;
        string channel = string.Empty;
        string teamOne = string.Empty;
        string teamTwo = string.Empty;
        string rollWinner = string.Empty;
        int bestOf;

        public MatchRoom(string teamOne, string teamTwo, string matchName, int bestOf)
        {
            this.teamOne = teamOne;
            this.teamTwo = teamTwo;
            this.bestOf = bestOf;
            ircLib.CreateMatch(matchName);
            Main();
        }

        private void Main()
        {
            switch (currentState)
            {
                case botState.playersNotInRoom:
                    invitePlayers();
                    break;
                case botState.waitingWarmUpPick:
                    break;
                case botState.rolling:
                    break;
                case botState.banning:
                    break;
                case botState.waitingPlayerPick:
                    break;
                case botState.playing:
                    break;
                case botState.matchDone:
                    break;
            }
        }

        private void invitePlayers()
        {
            APIThing.GetInfo(channel);
            Main();
        }
    }
}
 