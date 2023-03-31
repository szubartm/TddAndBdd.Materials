using System;

namespace UnitTesting
{
    class Calculator
    {
        public double Add(int x, double y) 
            => x < 0 ? throw new ArgumentOutOfRangeException(nameof(x)) : x + y;
    }
}