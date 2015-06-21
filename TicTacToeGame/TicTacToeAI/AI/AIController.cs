using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TicTacToeAI.Game;
using System.IO;

namespace TicTacToeAI.AI
{
    public class AIController
    {
        private GameController _gameController;
        private Players _player;
        private Random _gen = new Random();

        private QTable _qTable = new QTable();

        private string _currentEnvState = null;
        private MarkAction _currentAction = null;
        private bool _currentRes = false;

        private Stack<string> _stateMemory = new Stack<string>();

        private Stack<string> _actionMemory = new Stack<string>();

        public AIController(GameController controller, Players player)
        {
            _gameController = controller;
            _player = player;
            LearningProb = 0.2;
        }

        public bool DoAction()
        {
            _currentEnvState = _gameController.EnvState;
            _currentAction = SelectAction(_currentEnvState);
            _stateMemory.Push(_currentEnvState);
            _actionMemory.Push(_currentAction.GetState());

            if (_player == Players.One)
            {
                _currentRes =  _gameController.Player1Play(_currentAction);
            }
            else
            {
                _currentRes = _gameController.Player2Play(_currentAction);
            }

            if(OnAction!=null)
            {
                OnAction(this, new ActionEventArgs() { I = _currentAction.I, J = _currentAction.J });
            }
            return _currentRes;
        }

        public class ActionEventArgs
        {
            public int I { get; set;}
            public int J { get; set;}            
        }

        public event EventHandler<ActionEventArgs> OnAction;

        public void Learn()
        {
            double learningRate = 0.02;

            string s2 = null;
            double r = _currentRes ? 1.00 : -1.00;
            double discountFactor = 0.9;
            double nmax = _stateMemory.Count;
            while (_stateMemory.Count > 0)
            {
                string s1 = _stateMemory.Pop();
                string a1 = _actionMemory.Pop();
                _qTable[s1, a1] = _qTable[s1, a1] + learningRate * (discountFactor * (s2 == null ? r : _qTable[s2]) - _qTable[s1, a1]);
                s2 = s1;
            }
        }

        private void Morph(string s, out List<string>states)
        {
            // use center symmetry to get another states and actions
            int size = (int)Math.Sqrt(s.Length);
            
            var transformsU = new List<Func<int, int>>();
            transformsU.Add(i => i);
            transformsU.Add(i => (size - 1 - i));           
            var transformsS = new List<Func<int, int, int>>();
            transformsS.Add((i,j) => i);
            transformsS.Add((i,j) => j);           

            states = new List<string>();             

            foreach (var tU1 in transformsU)
            {
                foreach (var tU2 in transformsU)
                {
                    foreach (var tS1 in transformsS)
                    {
                        foreach (var tS2 in transformsS)
                        {
                            var newS = new char[size, size];
                            for (int i = 0; i < size; i++)
                            {
                                for (int j = 0; j < size; j++)
                                {
                                    newS[tU1(tS1(i,j)), tU2(tS2(i,j))] = s[i*size+j];
                                }
                            }
                            states.Add(new string(newS.Cast<char>().ToArray()));                            
                        }
                    }
                }
            }
            
        }

        public void Reset()
        {
            _currentAction = null;
            _currentEnvState = null;
            _currentAction = null;
        }

        public double LearningProb
        {

            get;
            set;
        }

        public void Load(string file)
        {
            var encoding = Encoding.GetEncoding("windows-1251");
            string content = File.ReadAllText(file, encoding);
            _qTable.Load(content);
        }

        public void LoadFromString(string content)
        {
            _qTable.Load(content);
        }

        public void Save(string file)
        {
            var encoding = Encoding.GetEncoding("windows-1251");
            string contents = _qTable.Save();
            File.WriteAllText(file, contents,encoding);
        }
        private MarkAction SelectAction(string state)
        {
            var actions = _gameController.AvailableActions();
            var probVal = _gen.NextDouble();
            List<MarkAction> selectedActions = null;

            if (probVal <= LearningProb)
            {
                selectedActions = actions;
            }
            else
            {
                //greed 
                var maxVal = _qTable[state];
                selectedActions = actions.Where(a => _qTable[state, a.GetState()] >= maxVal).ToList();
                
            }
            // prob
            int alen = selectedActions.Count;
            int aind = _gen.Next(0, alen);
            return selectedActions[aind];
        }

        
    }
}
