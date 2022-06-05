using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Battleship
{
    class PlayerGrid : Grid
    {
        public PlayerGrid()
        {

        }


        public PlayerGrid(Stream stream)
        {
            _enemy = stream;
        }


        //Завантажує розставлення з потоку (файл, консоль - пох)
        public bool LoadFromStream(Stream stream)
        {
            //тут я заповнюю карту клітинками, де стоять кораблі
            for(int i = 0; i < 10; i++)             //рядки
            {
                for(int j = 0; j < 10; j++)         //стовпці
                {
                    int tmp = stream.ReadByte();
                    if (tmp == 49)                  //'1' == 49
                    {
                        _squares[i][j].Order();     //якщо тут 1, то там є кусок корабля
                    }
                    else if (tmp != 48)             //'0' == 48
                    {
                        //Console.WriteLine(1);
                        goto error;                 //якщо стикаюсь з неадекватним введенням (має бути '0' або '1')
                    }
                }
                stream.ReadByte();                  //скіпаю '\n'
                stream.ReadByte();
            }

            //Тут я заповнюю список корабліків
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (!_squares[i][j].Occupied() && _squares[i][j].Ordered()) //Якщо там стоїть кораблік, але місце не зарезервовано (occupied)
                    {
                        if (i < 9 ? _squares[i + 1][j].Ordered() : false)       //Це я дивлюсь, чи кораблік вертикальний
                        {
                            byte type = 0;
                            while (_squares[i + type][j].Ordered())             //дивлюся, наскільки кораблік довгий
                            {
                                //Console.WriteLine($"{type} {i} {j}");
                                if (i + ++type > 9)                             //перевірка на край карти
                                {
                                    break;
                                }
                                //Console.WriteLine($"{type} {i} {j}");
                            }
                            if (!PutShip(type, (byte)(i * 10 + j), true))       //ставлю кораблік (з усіма витікаючими)
                            {
                                //Console.WriteLine($"{type} {i} {j}");
                                goto error;                                     //якщо шота йде по пізді, то звершую читання
                            }

                        }
                        else                                                    //все те саме, але для горизонтального корабліка
                        {
                            byte type = 0;
                            while (_squares[i][j + type].Ordered())
                            {
                                if (j + ++type > 9)
                                {
                                    break;
                                }
                                //Console.WriteLine($"{type} {i} {j}");
                            }
                            if (!PutShip(type, (byte)(i * 10 + j), false))
                            {
                                goto error;
                            }
                        }
                    }
                }
            }

            //Перевіряю, чи всі корабліки були створені
            for (int i = 0; i < 10; i++)
            {
                if (_fleet[i].Position() == 255)
                {
                    goto error;
                }
            }

            return true;//всі перевірки успішно пройдені)

        error:
            //тут має бути адекватний відкат ініціалізації
            Console.WriteLine("ERROR");
            return false;
        }

        //Ворог стріляє по мені
        public byte Shoot()
        {
            byte position = (byte)_enemy.ReadByte();    //Отримую координати пострілу
            byte result = Shoot(position);              //стреляю

            _enemy.WriteByte(result);
            return result;
        }

        //3 - "Вбив", 2 - "Поранив", 1 - "Мимо"
        public byte Shoot(string position)
        {
            return Shoot(Square.PositionToByte(position));
        }

    }
}
