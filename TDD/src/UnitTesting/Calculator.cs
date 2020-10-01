using System;

namespace UnitTesting
{
    class Calculator
    {
        public double Add( int x, double y )
        {
            if ( x < 0 )
            {
                throw new ArgumentException( "x" );
            }
            return x + y;
        }
    }
}