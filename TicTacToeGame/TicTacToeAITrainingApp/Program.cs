using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToeAI.AI;
using TicTacToeAI.Game;

namespace TicTacToeAITrainingApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var game = new GameController();
            var ai1 = new AIController(game, Players.One);
            var ai2 = new AIController(game, Players.Two);
            int p1wins = 0;
            int p2wins = 0;
            for (int i = 0; i < 1000000; i++)
            {
                //System.Threading.Thread.Sleep(2000);
                Console.WriteLine("-------------- New Game --------------");
                Console.WriteLine("--------------------------------------");
                while (!game.Finished)
                {
                    //System.Threading.Thread.Sleep(2000);
                    if (ai1.DoAction()) p1wins++;
                    Console.WriteLine("-------------- Player1 --------------");
                    string s = game.EnvState;
                    Console.WriteLine(s.Substring(0, 3));
                    Console.WriteLine(s.Substring(3, 3));
                    Console.WriteLine(s.Substring(6));
                    if (game.Finished)
                        break;
                    if (ai2.DoAction()) p2wins++;
                    Console.WriteLine("-------------- Player2 --------------");
                    s = game.EnvState;
                    Console.WriteLine(s.Substring(0, 3));
                    Console.WriteLine(s.Substring(3, 3));
                    Console.WriteLine(s.Substring(6));
                }
                ai1.Learn();
                ai1.Reset();
                ai2.Learn();
                ai2.Reset();
                game.Reset();
            }
            System.Console.WriteLine("player 1 has {0} wins, player 2 has {1} wins", p1wins, p2wins);
            System.Console.ReadLine();
            Console.WriteLine("-------------- New Game --------------");
            Console.WriteLine("--------------------------------------");
            ai1.LearningProb = 0.0;
            ai2.LearningProb = 0.0;
            while (!game.Finished)
            {
                System.Threading.Thread.Sleep(2000);
                ai1.DoAction();
                Console.WriteLine("-------------- Player1 --------------");
                string s = game.EnvState;
                Console.WriteLine(s.Substring(0, 3));
                Console.WriteLine(s.Substring(3, 3));
                Console.WriteLine(s.Substring(6));
                if (game.Finished)
                    break;
                ai2.DoAction();
                Console.WriteLine("-------------- Player2 --------------");
                s = game.EnvState;
                Console.WriteLine(s.Substring(0, 3));
                Console.WriteLine(s.Substring(3, 3));
                Console.WriteLine(s.Substring(6));
            }
            ai2.Save("qtable.txt");
            System.Console.ReadLine();
        }
    }
}
