using System;

namespace Prosiak.Logic;

public record Pig(string Name, DateTime DateOfBirth)
{
    public TimeSpan Age => DateTime.Today - DateOfBirth;
}
