using System;

namespace Projeto_DIO___Catálogo_de_Jogos_com_DotNet.Exceptions
{
    public class GameNotInsertedException : Exception
    {
        public GameNotInsertedException() :
            base("This game was not inserted"){}        
    }

    public class GameAlreadyInsertedException : Exception
    {
        public GameAlreadyInsertedException() :
            base("This game was already inserted"){}
    }

    public class DatabaseConnectionException : Exception
    {
        public DatabaseConnectionException() :
            base("Could not connect to database. Please try again."){}
    }   
}
