using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToeAI.Game
{
    public class GameController
    {
        private bool _player1Win = false;
        private bool _player2Win = false;

        private GameField _gameField = new GameField();

        public bool Player1Play(MarkAction action)
        {
            if(_player1Win || _player2Win)
            {
                throw new InvalidOperationException("Game ended");
            }
            _gameField.SetCross(action.I,action.J);
            return (_player1Win = Check(action.I, action.J));
        }

        public bool Player2Play(MarkAction action)
        {
            if(_player1Win || _player2Win)
            {
                throw new InvalidOperationException("Game ended");
            }
            _gameField.SetZero(action.I, action.J);
            return (_player2Win = Check(action.I,action.J));
        }

        public void Reset()
        {
            _player1Win = false;
            _player2Win = false;
            _gameField.Reset();
        }

        private bool Check(int i0, int j0)
        {
            char mark = _gameField.Get(i0, j0);
            int size = _gameField.GetSize();
            bool checkH = true;
            bool checkV = true;
            bool checkD = (i0==j0);
            bool checkID = (i0==(size - 1 - j0));
            for (int i = 0; i < size; i++ )
            {
                char h = _gameField.Get(i, j0);
                checkH = checkH ? (h==mark) : checkH;
                
                char v = _gameField.Get(i0, i);
                checkV = checkV ? (v == mark) : checkV;

                char d = _gameField.Get(i,i);
                checkD = checkD ? (d == mark) : checkD;

                char id = _gameField.Get(i,size-i-1);
                checkID = checkID ? (id == mark) : checkID;                
            }
            return checkH || checkV || checkD || checkID;
        }

        public List<MarkAction> AvailableActions()
        {
            var list = new List<MarkAction>();
            int size = _gameField.GetSize();
            for(int i = 0; i < size; i++)
            {
                for(int j = 0; j < size; j++)
                {
                    if(_gameField.Get(i,j)=='N')
                    {
                        list.Add(new MarkAction { I = i, J = j });
                    }
                }
            }
            return list;
        }

        public bool Finished
        {
            get { return _player1Win || _player2Win || AvailableActions().Count()==0; }
        }

        public string EnvState
        {
            get { return _gameField.GetState(); }
        }
    }
}
