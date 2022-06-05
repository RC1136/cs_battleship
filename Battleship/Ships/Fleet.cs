using System;
using System.Collections.Generic;
using System.Text;

namespace Battleship
{
    class Fleet
    {
        protected List<Ship> _fleet;

        protected Fleet()
        {

            // корабліки, що на цьому полі
            _fleet = new List<Ship>();

            _fleet.Add(new Carrier());          //0
            for (int i = 0; i < 2; i++)
                _fleet.Add(new Battleship());   //1,2
            for (int i = 0; i < 3; i++)
                _fleet.Add(new Destroyer());    //3,4,5
            for (int i = 0; i < 4; i++)
                _fleet.Add(new PatrolBoat());   //6,7,8,9

        }

        protected Ship FindShip(byte position)
        {
            //Проходжусь по всім кораблям
            for (int i = 0; i < 10; i++)
            {
                //Походжусь по всім клітинках, на яких стоїть корабель
                for (int j = 0; j < _fleet[i].Type(); j++)
                {
                    //Якщо корабель вертикальний, то требя зсуватись по десятках (перша цифра позиції)
                    if (position == (byte)(_fleet[i].Position() + (_fleet[i].IsVertical() ? j * 10 : j)))
                    {
                        return _fleet[i];
                    }
                }
            }

            return null;
        }

        public bool Destroyed(byte type, byte number)
        {
            number--;
            switch (type)
            {
                case 4:
                    return _fleet[0].Destroyed();
                case 3:
                    return _fleet[1 + number].Destroyed();
                case 2:
                    return _fleet[3 + number].Destroyed();
                case 1:
                    return _fleet[6 + number].Destroyed();
                default:
                    return false;
            }
        }

        //Чи знищений корабель, який займає клітинку "position"
        public bool Destroyed(byte position)
        {
            Ship tmp = FindShip(position);

            //Якщо корабель впринципі існує, то дізнаємось чи він знищений
            return tmp == null ? false : tmp.Destroyed();
        }


        public bool Destroyed()
        {
            for(int i = 0; i<10; i++)
            {
                if (!_fleet[i].Destroyed())
                {
                    return false;
                }
            }
            return true;
        }

    }
}
