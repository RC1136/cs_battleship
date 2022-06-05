using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Battleship
{
    class EnemyGrid : Grid
    {
        public EnemyGrid()
        {

        }

        public EnemyGrid(Stream stream)
        {
            _enemy = stream;
        }

        //Постріл по координатам (по ворожій базі)
        public byte Shoot(string position)
        {
            //Захист від дурака. Щоб соперника зайвий раз не дергати
            if (GetSquare(position).Occupied())
            {
                Console.WriteLine("Incorrect move!");
                return 0;
            }

            _enemy.WriteByte(Square.PositionToByte(position));  //Надсилаю сопернику координати пострілу
            byte result = (byte)_enemy.ReadByte();              //Отримую відповідь
            GetSquare(position).Shoot(result);
            if (result == 3)
            {
                if(!PutShip(Square.PositionToByte(position)))
                    Console.WriteLine("ERROR. EnemyGrid-> Shoot(string) {0}", position);
                //OccupySquare(FindShip(Square.PositionToByte(position)));
            }
            

            return result;
        }

        protected bool PutShip(byte last_hit_position)
        {
            byte horizontal = Square.ByteHorizontal(last_hit_position);
            byte vertical = Square.ByteVertical(last_hit_position);
            byte type = 1;
            //тернарний оператор потрібний лиш для того, щоб не вийти за межі (і не отримати виключенням по єбалу)
            for (int i = 1; horizontal - i >= 0 ? _squares[horizontal - i][vertical].Hit() : false; i++)
                type++;
            byte tmp = (byte)((horizontal - (type - 1)) * 10 + vertical);
            for (int i = 1; horizontal + i <= 9 ? _squares[horizontal + i][vertical].Hit() : false; i++)
                type++;
            //цей код виконається, якщо корабель вертикальний
            if (type > 1)
            {
                return PutShip(type, tmp, true);
            }
            else
            {
                type = 1;
                //цей код виконається, якщо корабель горизонтальний
                for (int i = 1; vertical - i >= 0 ? _squares[horizontal][vertical - i].Hit() : false; i++)
                    type++;
                tmp = (byte)(horizontal * 10 + vertical - (type - 1));
                for (int i = 1; vertical + i <= 9 ? _squares[horizontal][vertical + i].Hit() : false; i++)
                    type++;
                return PutShip(type, tmp, false);
            }

        }
    }
}
