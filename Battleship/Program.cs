using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Threading;

/*
   A B C D E F G H I J
1
2
3
4
5
6
7
8
9
10
*/

namespace Battleship
{ 

    class Program
    {
        static void Main(string[] args)
        {
            string enemyIP = args.Length > 0 ? args[0] : null;

            if (enemyIP == null)
            {
                NamedPipeServerStream servpipe = new NamedPipeServerStream("Pipe1", PipeDirection.InOut, 1);
                servpipe.WaitForConnection();
                Game Game = new Game(servpipe);
                Game.Play(true);
            }
            else
            {
                NamedPipeClientStream clipipe = new NamedPipeClientStream(enemyIP, "Pipe1", PipeDirection.InOut);
                clipipe.Connect();
                Game Game = new Game(clipipe);
                Game.Play(false);
            }


            Console.ReadKey();

            /*
            Grid grid = new Grid();
            grid.Show();
            grid.PutShip(new Carrier());
            grid.Show();
            grid.PutShip(new Battleship(20, false));
            grid.PutShip(new Battleship(5, true));
            grid.Show();
            */

            /*
            Console.WriteLine($"{Square.PositionToByte("B2")} {Square.PositionToString(11)}");
            */

            /*
            PlayerGrid pg = new PlayerGrid();
            pg.Show();
            pg.PutShip(4, "A1", true);
            pg.PutShip(3, "C1", false);
            pg.PutShip(3, "C4", false);
            pg.PutShip(2, "B2", true);
            pg.Show();
            */


            /*
            PlayerGrid pg = new PlayerGrid(File.OpenRead(@"F:\projects\Battleship\map.txt"));
            pg.Show();
            pg.Shoot("B1");
            pg.Shoot("C1");
            pg.Shoot("D1");
            pg.Shoot("E1");
            if (pg.Destroyed(4, 0))
                Console.WriteLine("EST PROBITIE");
            pg.Shoot("A3");
            pg.Shoot("A1");
            pg.Show();
            */

            /*
            PlayerGrid pg = new PlayerGrid(File.OpenRead(@"F:\projects\Battleship\map.txt"));
            pg.Show();
            Console.WriteLine(Grid.ShootResult(pg.Shoot("B1")));
            Console.WriteLine(Grid.ShootResult(pg.Shoot("C1")));
            Console.WriteLine(Grid.ShootResult(pg.Shoot("D1")));
            Console.WriteLine(Grid.ShootResult(pg.Shoot("E1")));
            pg.Show();
            */

            /*
            MemoryStream meme = new MemoryStream();
            EnemyGrid eg = new EnemyGrid(meme);
            PlayerGrid pg = new PlayerGrid(meme);
            pg.LoadFromStream(File.OpenRead(@"F:\projects\Battleship\emap.txt"));
            pg.Show();
            eg.Show();
            */



            /*
            Mutex mutex = null;
            if (!Mutex.TryOpenExisting("Game1", out mutex))
            {
                Console.WriteLine();
                mutex = new Mutex(true, "Game1");
                NamedPipeServerStream servpipe = new NamedPipeServerStream("Pipe1", PipeDirection.InOut, 1);
                servpipe.WaitForConnection();
                Game Game = new Game(servpipe);
                Game.Play(true);
            }
            else
            {
                NamedPipeClientStream clipipe = new NamedPipeClientStream(".", "Pipe1", PipeDirection.InOut);
                clipipe.Connect();
                Game Game = new Game(clipipe);
                Game.Play(false);
            }
            */




        }
    }
}
