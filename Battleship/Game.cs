using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace Battleship
{
    public class Game
    {
        Stream enemy;
        PlayerGrid PlayerGrid;
        EnemyGrid EnemyGrid;


        Game()
        {
            PlayerGrid = new PlayerGrid();
            EnemyGrid = new EnemyGrid();
        }

        public Game(Stream stream)
        {
            enemy = stream;
            PlayerGrid = new PlayerGrid(stream);
            EnemyGrid = new EnemyGrid(stream);
        }

        public int Play(bool firstmove)
        {
            if (false)//тут треба буде прикрутити перевірку чи Stream робочий
            {
                return -1;
            }



            Console.WriteLine("Loading map...");
            int result;
            result = PlayerGrid.LoadFromStream(File.OpenRead("map.txt")) ? 1 : 0;
            Thread.Sleep(1000);
            Console.WriteLine("Map loaded");

            bool player_move = firstmove;
            while (!(PlayerGrid.Destroyed() || EnemyGrid.Destroyed()))
            {
                UpdateCMap();
                if (player_move)
                {
                    Console.WriteLine("Your move");

                    result = EnemyGrid.Shoot(Console.ReadLine());
                    Console.WriteLine(Grid.ShootResult((byte)result));
                    player_move = result != 1;
                }
                else
                {
                    Console.WriteLine("Enemy's move...");

                    result = PlayerGrid.Shoot();
                    Console.WriteLine(Grid.ShootResult((byte)result));

                    player_move = result == 1;
                }

            }
            Console.WriteLine("Game Over");
            if (PlayerGrid.Destroyed())
                Console.WriteLine("Our fleet has been destroyed");
            if (EnemyGrid.Destroyed())
                Console.WriteLine("Enemy fleet has been destroyed");

            return 0;
        }

        protected int Play()
        {
            return Play(false);
        }


        public int Play(bool firstmove, Stream toenemy)
        {
            enemy = toenemy;
            PlayerGrid = new PlayerGrid(toenemy);
            EnemyGrid = new EnemyGrid(toenemy);
            return Play(firstmove);
        }



        void UpdateCMap()
        {
            PlayerGrid.Show();
            EnemyGrid.Show();
        }

    }
}
