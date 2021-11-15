using System;

namespace Hosts.Domain.Exceptions
{
    //Comment: this should be a custom Exception Type and handled in a middleware to return a custom response 
    public class PokemonNotFoundException : Exception
    {
        public PokemonNotFoundException(string name) : base($"\"{name}\" is not a Pokemon")
        {

        }
    }
}