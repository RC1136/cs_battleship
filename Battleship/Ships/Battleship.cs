namespace Battleship
{
    class Battleship : Ship
    {
        public Battleship()
        {
            _type = 3;
        }

        public Battleship(byte beg, bool vertical)
        {
            _type = 3;
            _is_vertical = vertical;
            _beg_pos = beg;
        }
    }
}
