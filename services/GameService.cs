using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Projeto_DIO___Catálogo_de_Jogos_com_DotNet.Entities;
using Projeto_DIO___Catálogo_de_Jogos_com_DotNet.Exceptions;
using Projeto_DIO___Catálogo_de_Jogos_com_DotNet.imputModel;
using Projeto_DIO___Catálogo_de_Jogos_com_DotNet.Repository;
using Projeto_DIO___Catálogo_de_Jogos_com_DotNet.viewModel;

namespace Projeto_DIO___Catálogo_de_Jogos_com_DotNet.services
{
    public class GameService : IGameService
    {
        private readonly IGameRepository _gameRepository;

        public GameService(IGameRepository gameRepository)
        {
            _gameRepository = gameRepository;
        }

        public async Task<List<GameViewModel>> Get(int page, int quantity)
        {
            var games = await _gameRepository.Get(page, quantity);

            return games.Select(game => new GameViewModel
            {
                Id = game.Id,
                Name = game.Name,
                Producer = game.Producer,
                Price = game.Price
            }).ToList();
        }

        public async Task<GameViewModel> Get(Guid id)
        {
            var game = await _gameRepository.Get(id);

            return game == null 
                ? null 
                : new GameViewModel
                {
                    Id = game.Id,
                    Name = game.Name,
                    Producer = game.Producer,
                    Price = game.Price
                };
        }

        public async Task<GameViewModel> Insert(GameImputModel game)
        {
            var gameEntity = await _gameRepository.Get(game.Name, game.Producer);

            validateEntry(gameEntity.Count() > 0, "Inserted");

            var gameInsert = new Game
            {
                Id = Guid.NewGuid(),
                Name = game.Name,
                Producer = game.Producer,
                Price = game.Price
            };
            
            try
            {
                await _gameRepository.Insert(gameInsert);                
            }
            catch (Exception)
            {
                throw new DatabaseConnectionException();
            }

            return new GameViewModel
            {
                Id = gameInsert.Id,
                Name = game.Name,
                Producer = game.Producer,
                Price = game.Price
            };
        }

        public async Task Update(Guid id, GameImputModel game)
        {
            var gameEntity = await _gameRepository.Get(id);

            validateEntry(gameEntity == null, "NotInserted");

            gameEntity.Name = game.Name;
            gameEntity.Producer = game.Producer;
            gameEntity.Price = game.Price;

            try
            {
                await _gameRepository.Update(gameEntity);              
            }
            catch (Exception)
            {
                throw new DatabaseConnectionException();
            }
        }

        public async Task Update(Guid id, double price)
        {
            var gameEntity = await _gameRepository.Get(id);

            validateEntry(gameEntity == null, "NotInserted");

            gameEntity.Price = price;

            try
            {
                await _gameRepository.Update(gameEntity);              
            }
            catch (Exception)
            {
                throw new DatabaseConnectionException();
            }
        }        
        public async Task Delete(Guid id)
        {
            var game = await _gameRepository.Get(id);

            validateEntry(game == null, "NotInserted");

            try
            {
                await _gameRepository.Delete(id);            
            }
            catch (Exception)
            {
                throw new DatabaseConnectionException();
            }
        }

        public void Dispose()
        {
            _gameRepository.Dispose();
        }

        private void validateEntry(bool condition, string status)
        {
            if (condition && status == "NotInserted")
                throw new GameNotInsertedException();   
            else if (condition && status == "Inserted")
                throw new GameAlreadyInsertedException();      
        }
    }
}
