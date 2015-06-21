using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TicTacToeAI.Game;
using TicTacToeAI.AI;
namespace TicTacToeGame
{
    public class SinglePlayerController
    {
        public SinglePlayerController(string initStr)
        {
            _gameController = new GameController();
            _aiContoller = new AIController(_gameController, Players.Two); // init second ai player
            _aiContoller.LearningProb = 0.00;
            _aiContoller.LoadFromString(initStr);
            _aiContoller.OnAction+=HandleAction;
        }

        public event EventHandler<SinglePlayerCallback> Callback;       

        public void PlayerMove(int i, int j, Action<int,int> cbl)
        {
            if (!_gameController.Finished)
            {
                _gameController.Player1Play(new MarkAction { I = i, J = j });
                if (!_gameController.Finished)
                {
                    _callback = cbl;
                    _aiContoller.DoAction();
                    _callback = null;
                }
            }

        }

        private void HandleAction(object sender, AIController.ActionEventArgs args)
        {
            if(_callback!=null)
            {
                _callback(args.I, args.J);
            }
        }

        private GameController _gameController;
        private AIController _aiContoller;
        private Action<int, int> _callback;
    }
}