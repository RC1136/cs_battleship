namespace Battleship
{
    class PatrolBoat : Ship
    {
        public PatrolBoat()
        {
            _type = 1;
        }

        public PatrolBoat(byte beg, bool vertical)
        {
            _type = 1;
            _is_vertical = vertical;
            _beg_pos = beg;
        }
    }
}
