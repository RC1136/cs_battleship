namespace Battleship
{
    class Destroyer : Ship
    {
        public Destroyer()
        {
            _type = 2;
        }

        public Destroyer(byte beg, bool vertical)
        {
            _type = 2;
            _is_vertical = vertical;
            _beg_pos = beg;
        }
    }
}
