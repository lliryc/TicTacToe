using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToeAI.Game
{
    public class MarkAction
    {
        public int I
        {
            get;
            set;
        }

        public int J
        {
            get;
            set;
        }

        public string GetState()
        {
            return string.Format("{0},{1}", I, J);
        }

        public void SetState(string state)
        {
            var parsedStr = state.Split(',');
            I = int.Parse(parsedStr[0]);
            J = int.Parse(parsedStr[1]);
        }
    }
}
