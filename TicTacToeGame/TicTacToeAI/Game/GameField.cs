using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToeAI.Game
{
    public class GameField
    {
        public GameField()
        {
            _field = new char[SIZE, SIZE];
            for(int i=0; i<SIZE; i++)
            {
                for (int j = 0; j < SIZE; j++)
                {
                    _field[i, j] = 'N';
                }
            }
        }
        public const int SIZE = 3;
        private char[,] _field;

        private void SetMark(int i, int j, char c) 
        {
           
           if(j>=SIZE || i>=SIZE)
           {
               throw new InvalidOperationException("Incorrect range");
           }
           if (_field[i,j] != 'N')
           {
               throw new InvalidOperationException("The cell was already filled");
           }
           _field[i, j] = c;
           
        }

        public void SetCross(int i, int j)
        {
            SetMark(i,j,'X');
        }

        public void SetZero(int i, int j)
        {
            SetMark(i, j, 'O');
        }

        public char Get(int i,int j)
        {
            return _field[i,j];
        }

        public void Reset()
        {
            _field = new char[SIZE, SIZE];
            for (int i = 0; i < SIZE; i++)
            {
                for (int j = 0; j < SIZE; j++)
                {
                    _field[i, j] = 'N';
                }
            }
        }

        public string GetState()
        {
            char[]field1d = _field.Cast<char>().ToArray();            
            return new string(field1d);
        }

        public int GetSize()
        {
            return SIZE;
        }
    }
}
