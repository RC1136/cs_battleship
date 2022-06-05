using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Battleship
{
    class Square
    {
        
        bool _ordered;                                  //Чи тут є кораблік                               
        bool _hit;                                      //Чи сюди стріляли
        bool _occupied;                                 //Чи це поле незайняте (збоку нема корабля, сюди не стріляли), можна реалізувати у вигляді метода, але да

        //УВАГА, перша цифра координати відповідає за ГОРИЗОНТАЛЬ (рядок), а друга за ВЕРТИКАЛЬ (стовпець)
        byte _position;                                 //Позиція, в переводі на числа (А3 -> 30, F1 -> 05)

        public Square()
        {
            _ordered = _hit = _occupied = false;
        }

        public Square(byte position)
        {
            _position = position;
        }

        public Square(string position)
        {
            _position = PositionToByte(position);
        }

        public Square(char ver, byte hor)
        {
            _ordered = _hit = _occupied = false;
            _position = PositionToByte(ver.ToString() + (hor - 1).ToString());
        }

        static public byte PositionToByte(string pos)
        {
            return (byte)((int.Parse(pos.Substring(1)) - 1) * 10 + (pos[0] - 65));
        }

        static public string PositionToString(byte pos)
        {
            return ((byte)(pos % 10 + 65)).ToString() + ((byte)(pos / 10 + 1)).ToString();
        }

        public byte BytePosition()
        {
            return _position;
        }

        public string Position()
        {
            return PositionToString(_position);
        }

        public byte ByteHorizontal()
        {
            return ByteHorizontal(_position);
        }

        public static byte ByteHorizontal(byte position)
        {
            return (byte)(position / 10);
        }

        public byte ByteVertical()
        {
            return (byte)(_position % 10);
        }
        public static byte ByteVertical(byte position)
        {
            return (byte)(position % 10);
        }

        public bool Occupied()
        {
            return _occupied;
        }

        public bool Ordered()
        {
            return _ordered;
        }

        public bool Hit()
        {
            return _hit;
        }

        public char State() 
        {
            if (_ordered)
            {
                return _hit ? 'X' : '@';
            }
            else if (_occupied)
            {
                return '+';
            }
            else
            {
                return 'O';
            }
        }

        public bool Order()
        {
            if (!_occupied)
            {
                _ordered = true;
                return true;
            }
            else
                return false;
        }

        public bool Shoot()
        {
            _hit = true;
            _occupied = true;

            return _ordered;
        }

        public void Shoot(byte result)
        {
            if(result == 3 || result == 2)
                _ordered = true;

            _hit = true;
        }

        public void Occupy()
        {
            _occupied = true;
        }

        //най буде
        internal void SetState(bool ord, bool hit, bool occ) 
        {
            _ordered = ord;
            _hit = hit;
            _occupied = occ;
        }

    }
}
