using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading;
using System.IO;
using System.Text;
using System.Web;

namespace TicTacToeGame
{
    public class PlayersSingle
    {
        public PlayersSingle()
        {
            var enc = Encoding.GetEncoding("windows-1251");
            var path = HttpContext.Current.Server.MapPath(@"~/aiState.txt");
            _initState = File.ReadAllText(path,enc); 
        }
        public static PlayersSingle Instance()
        {
            if (_instance != null)
            {
                return _instance;
            }
            lock (_syncObj)
            {
                if (_instance == null)
                {
                    _instance = new PlayersSingle();
                }
                return _instance;
            }

        }

        public void Register(string connectionId)
        {
            SinglePlayerController c = null;
            _rwLock.EnterUpgradeableReadLock();
            if(!_controllers.TryGetValue(connectionId, out c))
            {
                _rwLock.EnterWriteLock();
                _controllers.Add(connectionId, new SinglePlayerController(_initState));
                _rwLock.ExitWriteLock();
            }
            _rwLock.ExitUpgradeableReadLock();
        }

        public SinglePlayerController GetController(string connectionId)
        {
            SinglePlayerController c = null;
            _rwLock.EnterReadLock();
            _controllers.TryGetValue(connectionId, out c);
            _rwLock.ExitReadLock();
            return c;
        }
        
        private static volatile PlayersSingle _instance = null;
        private static object _syncObj = new object();
        private string _initState = null;
        private Dictionary<string, SinglePlayerController> _controllers = new Dictionary<string, SinglePlayerController>();
        private ReaderWriterLockSlim _rwLock = new ReaderWriterLockSlim();
    }
}