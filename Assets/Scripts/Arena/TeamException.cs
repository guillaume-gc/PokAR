using System;


public class TeamException : Exception
{
    public TeamException() : base()
    {

    }

    public TeamException(string message) : base(message)
    {

    }

    public TeamException(string message, Exception inner) : base(message, inner)
    {

    }
}

public class TooManyPokemonsException : TeamException
{
    public TooManyPokemonsException() : base()
    {

    }

    public TooManyPokemonsException(string message) : base (message)
    {

    }

    public TooManyPokemonsException(string message, Exception inner) : base(message, inner)
    {

    }
}