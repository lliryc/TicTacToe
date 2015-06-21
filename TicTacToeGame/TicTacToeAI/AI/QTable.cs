using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TicTacToeAI.AI
{
    public class QTable
    {
        private Dictionary<string, Dictionary<string, double>> _table = new Dictionary<string, Dictionary<string, double>>();
 
        public double this[string state, string action]
        {
            get
            {
                if(!_table.ContainsKey(state))
                {
                    _table.Add(state, new Dictionary<string,double>());                    
                }
                
                var subtable = _table[state];
                if(!subtable.ContainsKey(action))
                {
                    subtable.Add(action,0);
                }

                return _table[state][action];
            }
            set
            {
                if(!_table.ContainsKey(state))
                {
                    _table.Add(state, new Dictionary<string,double>());
                }
                
                var subtable = _table[state];
                if(!subtable.ContainsKey(action))
                {
                    subtable.Add(action, value);
                }
                else
                {
                    subtable[action] = value;
                }
            }
        }

        public double this[string state]
        {
            get
            {
                if (!_table.ContainsKey(state))
                {
                    return 0;
                }
                           
                var subtable = _table[state];
                return subtable.Values.Max();                
            }
        }

        public string Save()
        {
            return JsonConvert.SerializeObject(_table);
        }

        public void Load(string dictionaryStr) 
        {
            _table = (Dictionary<string, Dictionary<string, double>>)JsonConvert.DeserializeObject(dictionaryStr, typeof(Dictionary<string, Dictionary<string, double>>));
        }
    }
}
