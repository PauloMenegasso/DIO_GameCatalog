using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Projeto_DIO___Catálogo_de_Jogos_com_DotNet.Exceptions;
using Projeto_DIO___Catálogo_de_Jogos_com_DotNet.imputModel;
using Projeto_DIO___Catálogo_de_Jogos_com_DotNet.services;
using Projeto_DIO___Catálogo_de_Jogos_com_DotNet.viewModel;

namespace Projeto_DIO___Catálogo_de_Jogos_com_DotNet.Controllers.v1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private readonly IGameService _gameService;

        public GamesController(IGameService gameService)
        {
            _gameService = gameService;
        }

        [HttpGet]
        public async Task<ActionResult<List<GameViewModel>>> Get(
            [FromQuery, Range(1, int.MaxValue)] int page = 1,
            [FromQuery, Range(1, 50)] int quantity = 5)
        {
            var games = await _gameService.Get(page, quantity);
         
            return games.Count() == 0
                ? NoContent()
                : Ok(games); 
        }

    
        [HttpGet("{gameId:guid}")]
        public async Task<ActionResult<GameViewModel>> Get([FromRoute] Guid gameId)
        {
            var game = await _gameService.Get(gameId);

            return game == null
                ? NoContent()
                : Ok(game);
        }    

        [HttpPost]
        public async Task<ActionResult<GameViewModel>> InsertGame([FromBody] GameImputModel game)
        {
            try
            {
                var gameToInsert = await _gameService.Insert(game);

                return Ok(gameToInsert);
            }
            catch (GameAlreadyInsertedException ex)
            {
                return UnprocessableEntity("A game with this name already exists for this producer.");
            }
        }    

        [HttpPut]
        public async Task<ActionResult> UpdateGame(
            [FromRoute] Guid gameId,
            [FromBody] GameImputModel game)
        {
            try
            {
                await _gameService.Update(gameId, game);

                return Ok();
            }
            catch (GameNotInsertedException ex)
            {
                return UnprocessableEntity("Game not found in database.");
            }
        }

        [HttpPatch("{gameId:guid}/price/{price:double}")]
        public async Task<ActionResult> UpdateGamePrice(
            [FromRoute] Guid gameId,
            [FromRoute] double price)
        {
            try
            {
                await _gameService.Update(gameId, price);
            
                return Ok();
            }
            catch (GameNotInsertedException ex)
            {
                return UnprocessableEntity("Game not found in database.");
            }
        }
    
        [HttpDelete("{gameId:guid}")]
        public async Task<ActionResult> DeleteGame([FromRoute] Guid gameId)
        {
            try
            {
                await _gameService.Delete(gameId);

                return Ok();
            }
            catch (GameNotInsertedException ex)
            {
                return UnprocessableEntity("Game not found in database.");
            }
        }
    }
}
