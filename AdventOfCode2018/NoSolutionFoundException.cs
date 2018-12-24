using System;

namespace AdventOfCode2018
{
    public class NoSolutionFoundException : Exception
    {
        public NoSolutionFoundException()
            : base()
        { }

        public NoSolutionFoundException(string message)
            : base(message)
        { }
    }
}
