using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Projeto_DIO___Catálogo_de_Jogos_com_DotNet.Entities;

namespace Projeto_DIO___Catálogo_de_Jogos_com_DotNet.Repository
{
    public interface IGameRepository : IDisposable 
    {
         Task<List<Game>> Get(int page, int quantity);
         Task<Game> Get(Guid id);
         Task<List<Game>> Get (string name, string producer);
         Task Insert(Game game);
         Task Update(Game game);
         Task Delete(Guid id);
    }
}
