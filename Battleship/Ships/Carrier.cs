namespace Battleship
{
    class Carrier : Ship
    {
        public Carrier()
        {
            _type = 4;
        }

        public Carrier(byte beg, bool vertical)
        {
            _type = 4;
            _is_vertical = vertical;
            _beg_pos = beg;
        }
    }
}
