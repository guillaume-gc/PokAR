using System;


public class PokemonAnimationException : Exception
{
    public PokemonAnimationException() : base()
    {

    }

    public PokemonAnimationException(string message) : base(message)
    {

    }

    public PokemonAnimationException(string message, Exception inner) : base(message, inner)
    {

    }
}