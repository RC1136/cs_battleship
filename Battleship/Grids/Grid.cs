using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;


namespace Battleship
{
    class Grid : Fleet
    {
        protected Stream _enemy;

        protected List<List<Square>> _squares;                    //квадратікі

        public Grid()
        {
            //Поле - це клітинки...
            _squares = new List<List<Square>>();
            for (byte i = 0; i < 10; i++)
            {
                _squares.Add(new List<Square>());

                for (byte j = 0; j < 10; j++)
                {
                    _squares[i].Add(new Square((byte)(i*10+j)));
                    
                }
            }

        }


        //Повертає клітинку
        protected Square GetSquare(byte position)
        {
            return _squares[position / 10][position % 10];
        }

        protected Square GetSquare(string postion)
        {
            return GetSquare(Square.PositionToByte(postion));
        }

        protected Square GetSquare(char ver, byte hor)
        {
            //Горизонталь віднімаю 1 бо рахую від 0
            //а вертикаль віднімаю 65, бо зсув до літер - 65 (А==65, В==66...)
            return _squares[hor-1][ver-65];
        }

        protected Square GetSquare(byte hor, byte ver)
        {
            return _squares[hor][ver];
        }

        /// <summary>виводить поле на екран</summary>
        public void Show()//виводить поле на екран                  
        {
            Console.WriteLine("   A B C D E F G H I J");
            for(int i = 0; i < 10; i++)
            {
                Console.Write(i+1);
                Console.Write(i == 9 ? " " : "  ");
                for (int j = 0; j < 10; j++)
                {
                    
                    Console.Write(_squares[i][j].State());
                    Console.Write(' ');
                }
                Console.Write('\n');
            }
            Console.Write('\n');
        }


        //Замальовує всі клітинки навколо корабля
        protected void OccupySquare(Ship ship)
        {
            //тут я замальовую клітинки навколо корабля
            byte left_top, right_bottom;
            //Якщо на краю карти, то за край карти не заходжу
            left_top = (byte)(ship.Position() >= 10 ? ship.Position() - 10 : ship.Position());              //верхній край карти 
            left_top -= (byte)(ship.Position() % 10 > 0 ? 1 : 0);                                           //лівий край карти   
            right_bottom = (byte)(ship.EndPosition() < 90 ? ship.EndPosition() + 10 : ship.EndPosition());  //нижній край
            right_bottom += (byte)(ship.Position() % 10 < 9 ? 1 : 0);                                       //правий край
            OccupySquare(left_top, right_bottom);
        }

        //Замальовує всі клітинки, що знаходяться в прямокутнику
        protected void OccupySquare(byte left_top, byte right_bottom)
        {
            for(byte i = (byte)(left_top / 10); i <= right_bottom / 10; i++)
            {
                for(byte j = (byte)(left_top % 10); j <= right_bottom % 10; j++)
                {
                    _squares[i][j].Occupy();
                }
            }
        }


        public bool PutShip(byte type, string position, bool vertical)
        {
            return PutShip(type, Square.PositionToByte(position), vertical);
        }

        public bool PutShip(byte type, byte position, bool vertical)
        {
            byte numerator = 0;
            switch (type)
            {
                case 4:
                    if (_fleet[0].Position() == 255)
                    {
                        numerator = 0;
                    }
                    else
                        goto default;
                    break;
                case 3:
                    for (byte i = 1; numerator == 0; i++)
                    {
                        if (_fleet[i].Position() == 255)
                        {
                            numerator = i;
                        }
                        else if (i == 3)
                            goto default;
                    }
                    break;
                case 2:
                    for (byte i = 3; numerator == 0; i++)
                    {
                        if (_fleet[i].Position() == 255)
                        {
                            numerator = i;
                        }
                        else if (i == 6)
                            goto default;
                    }
                    break;
                case 1:
                    for (byte i = 6; numerator == 0; i++)
                    {
                        if (_fleet[i].Position() == 255)
                        {
                            numerator = i;
                        }
                        else if (i == 10)
                            goto default;
                    }
                    break;
                default:
                    return false;
            }
            //_fleet[numerator].Put(position, vertical);
            List<Square> ship = new List<Square>();
            if (vertical)
            {
                for (int i = 0; i < type; i++)
                {
                    ship.Add(_squares[position / 10 + i][position % 10]);
                }
            }
            else
            {
                for (int i = 0; i < type; i++)
                {
                    ship.Add(_squares[position / 10][position % 10 + i]);
                }
            }
            _fleet[numerator].Put(ref ship, vertical);
            return PutShip(_fleet[numerator]);
        }

        //спускаю човник на воду (true, якщо все збс)
        public bool PutShip(Ship ship)
        {

            if (ship.IsVertical())  //Якщо вертикальний, то працюю з рядками ship[pos/10][const]
            {
                //Тут перевіряю адекватність того, хто вводив дані (поверну false, якщо неадекват)
                //Чи не залазить за края*
                if (ship.Position() / 10 > 10 - ship.Type())
                {
                    return false;
                }

                for (int i = 0; i < ship.Type(); i++)//тут я перевіряю, чи місце не зайняте
                {
                    if (_squares[ship.Position() / 10 + i][ship.Position() % 10].Occupied())
                        return false;
                }
                for (int i = 0; i < ship.Type(); i++)//тут я замальовую клітинки де є кораблик
                {

                    _squares[ship.Position() / 10 + i][ship.Position() % 10].Order();
                }

            }
            else                  //Якщо горизонтальний, то працюю зі стовпцями shi[const][pos%10]                     
            {
                //все те саме, тільки для корабля, що дивиться в іншому напрямку

                if (ship.Position() % 10 > 10 - ship.Type())
                {
                    return false;
                }

                for (int i = 0; i < ship.Type(); i++)
                {
                    if (_squares[ship.Position() / 10][ship.Position() % 10 + i].Occupied())
                        return false;
                }

                for (int i = 0; i < ship.Type(); i++)
                {
                    _squares[ship.Position() / 10][ship.Position() % 10 + i].Order();
                }

                /*//тут пам'ятник моїй тупості, НЕ ЗНОСИТИ!!!
                bool hor_bor, ver_bor;
                hor_bor = ship.Position() / 10 == 0 || ship.Position() / 10 == 9;
                ver_bor = ship.Position() % 10 == 0 || ship.Position() % 10 == 9;
                int i = ship.Position() - (ship.Position() / 10 > 0 ? 1 : 0);
                while (i < 3 - (hor_bor ? 1 : 0))
                {
                    int j = ship.Position() - (ship.Position() % 10 > 0 ? 1 : 0);
                    while (j < 3 - (ver_bor ? 1 : 0))
                    {

                        j++;
                    }
                    i++;
                }
                */
            }

            OccupySquare(ship);
            //Console.WriteLine($"{ship.Type()} {ship.Position()}");
            return true;
        }


        //3 - "Вбив", 2 - "Поранив", 1 - "Мимо"
        protected byte Shoot(byte position)
        {
            if (position > 99)//Корректність введених даних
            {
                return 0;
            }

            if(_squares[position / 10][position % 10].Hit())
            {
                return 0;
            }
            
                
            if (_squares[position / 10][position % 10].Shoot())
            {
                return (byte)(Destroyed(position) ? 3 : 2);     
            }
            else { 
                return 1;                                       
            }         
        }

        static public string ShootResult(byte result)
        {
            switch (result)
            {
                case 3:
                    return "Destroyed";
                case 2:
                    return "Hit";
                case 1:
                    return "Miss";
                default:
                    return "ERROR";
            }
        }

        
    }
}
