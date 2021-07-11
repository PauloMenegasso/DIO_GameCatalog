using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Projeto_DIO___Catálogo_de_Jogos_com_DotNet.imputModel;
using Projeto_DIO___Catálogo_de_Jogos_com_DotNet.viewModel;

namespace Projeto_DIO___Catálogo_de_Jogos_com_DotNet.services
{
    public interface IGameService : IDisposable
    {
        Task<List<GameViewModel>> Get(int page, int quantity);
        Task<GameViewModel> Get(Guid id);
        Task<GameViewModel> Insert(GameImputModel game);
        Task Update(Guid id, GameImputModel game);
        Task Update(Guid id, double price);
        Task Delete(Guid id);
    }
}
