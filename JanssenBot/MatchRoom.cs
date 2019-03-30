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
        RoomState roomState;
        enum botState {playersNotInRoom, waitingWarmUpPick, playing, rolling, banning, waitingPlayerPick, matchDone}
        botState currentState = botState.playersNotInRoom;
        public int channel = 0;
        string teamOne = string.Empty;
        string teamTwo = string.Empty;
        string rollWinner = string.Empty;
        string phase = string.Empty;
        int bestOf;

        public MatchRoom(int matchNumber, string teamOne, string teamTwo, string matchName, string phase, int bestOf)
        {
            this.teamOne = teamOne;
            this.teamTwo = teamTwo;
            this.bestOf = bestOf;
            this.phase = phase;
            ircLib.CreateMatch(matchName);
            Main();
        }

        public void Main()
        {
            if(channel != 0)
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
        }

        private void invitePlayers()
        {
            roomState = APIThing.GetRoomInfo(channel);
        }
    }
}
 