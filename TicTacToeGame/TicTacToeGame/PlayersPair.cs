using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading;

namespace TicTacToeGame
{
    public class PlayersPair
    {
        private PlayersPair(){}

        public static PlayersPair Instance()
        {
            if(_instance!=null)
            {
                return _instance;
            }
            lock(_syncObj)
            {
                if(_instance==null)
                {
                    _instance = new PlayersPair();
                }
                return _instance;
            }

        }
        private static volatile PlayersPair _instance = null;
        private static object _syncObj = new object();

        public void Register(string connectionId)
        {
            _rwLock.EnterWriteLock();

            if(_curPair == null)
            {
                var newSession = new string[] { null, null };
                newSession[0] = connectionId;
                _pairs.Add(newSession);
                _curPair = newSession;
            }
            else
            {
                _curPair[1] = connectionId;
                _curPair = null;
            }

            _rwLock.ExitWriteLock();
            
        }
        private string[] _curPair = null;
        private List<string[]> _pairs = new List<string[]>();
        private ReaderWriterLockSlim _rwLock = new ReaderWriterLockSlim();

        public string[] GetPair(string connectionId)
        {
            _rwLock.EnterReadLock();
            var session = _pairs.FirstOrDefault(item => item[0] == connectionId || item[1] == connectionId);
            string[] outs = new string[] { session[0], session[1] };
            _rwLock.ExitReadLock();
            return outs;
        }
    }
}