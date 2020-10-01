using System;

namespace Prosiak.Logic
{
    public class Pig
    {
        public DateTime DateOfBirth { get; }

        public string Name { get; }

        public TimeSpan Age => DateTime.Today - DateOfBirth;

        public Pig(string name, DateTime dateOfBirth)
        {
            Name = name;
            DateOfBirth = dateOfBirth;
        }
    }
}