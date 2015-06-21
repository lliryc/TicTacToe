using System;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Collections.Generic;

namespace TicTacToeGame
{
    public class GameHub : Hub
    {
        private string SinglePlayerMode = "single";
        
        private string MultiPlayerMode = "multi";
        
        public void Send(int i, int j, string mode)
        {
            var connId = Context.ConnectionId;

            if(mode == SinglePlayerMode)
            {
                var c = PlayersSingle.Instance().GetController(connId);
                Clients.Client(connId).broadcastMessage(connId, i, j);
                var action = new Action<int,int>(this.Callback);
                c.PlayerMove(i, j, action);                
            }
            else if(mode == MultiPlayerMode)
            {
                var session = PlayersPair.Instance().GetPair(connId);
                // Call the broadcastMessage method to update clients.
                if (session == null || session[1] == null)
                {
                    return;
                }
                Clients.Clients(new List<string>(session)).broadcastMessage(connId, i, j);

            }
            
        }
      
        void Callback(int i, int j)
        {
            Clients.Client(Context.ConnectionId).broadcastMessage("0000-0000-0000-0000", i, j);
        }

        public string Register(string mode)
        {
            var connectionId = Context.ConnectionId;
            if (mode == SinglePlayerMode)
            {
                PlayersSingle.Instance().Register(connectionId);

            }
            else if (mode == MultiPlayerMode)
            {
                PlayersPair.Instance().Register(connectionId);
            }
            return connectionId;
        }
    }    
}
