using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;


namespace Battleship
{
    abstract class Ship
    {
       
        protected byte _type;
        protected bool _is_vertical;
        protected byte _beg_pos; //впринципі, тут мав би бути ітератор на клітинку, але най буде
        List<Square> _squares;

        protected Ship()
        {
            _beg_pos = 255;
            _type = 0;
        }

        //Видаю кораблику координати
        public void Put(ref List<Square> squares, bool vertical)
        {
            _beg_pos = squares[0].BytePosition();
            _squares = squares;
            _is_vertical = vertical;
        }

        public void Put(byte beg, bool vertical)
        {
            _is_vertical = vertical;
            _beg_pos = beg;
        }

        public bool Destroyed()
        {
            if (_beg_pos == 255)
                return false;
            for(int i = 0; i < _type; i++)
            {
                if (!_squares[i].Hit())
                    return false;
            }
            return true;
        }


        //Повертає орієнтацію (true якщо зверху вниз)
        public bool IsVertical()
        {
            return _is_vertical;
        }

        //Повертає тип корабля (скільки клітинок займає)
        public byte Type()
        {
            return _type;
        }

        //Повертає початок корабля (зліва/зверху) у форматі числа
        public byte Position()
        {
            return _beg_pos;
        }

        //Повертає кінець корабля (справа/знизу) у форматі числа
        public byte EndPosition()
        {
            return (byte)( _beg_pos + (_is_vertical ? (_type-1) * 10 : (_type-1))); //кінець, це як початок, тільки зсунутий вправо/вниз
        }
    }
}
