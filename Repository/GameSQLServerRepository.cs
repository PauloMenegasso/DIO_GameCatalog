using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Projeto_DIO___Catálogo_de_Jogos_com_DotNet.Entities;


namespace Projeto_DIO___Catálogo_de_Jogos_com_DotNet.Repository
{
    public class GameSQLServerRepository : IGameRepository
    {
        private readonly SqlConnection _sqlConnection;

        public GameSQLServerRepository(IConfiguration configuration)
        {
            _sqlConnection = new SqlConnection(configuration.GetConnectionString("Default"));
        }

        public async Task<List<Game>> Get(int page, int quantity)
        {
            var games = new List<Game>();

            var command = $"Select * from Games order by Id ofsset {((page -1)*quantity)} rows fetch next {quantity} rows only";

            var sqlDataReader = await OpenAndQuery(command);

            while (sqlDataReader.Read())
            {
                games.Add(new Game
                {
                    Id = (Guid)sqlDataReader["Id"],
                    Name = (string)sqlDataReader["Name"],
                    Producer = (string)sqlDataReader["Producer"],
                    Price = (double)sqlDataReader["Price"]
                });
            }

            await _sqlConnection.CloseAsync();

            return games;
        }

        public async Task<Game> Get(Guid id)
        {
            Game game = null;

            var command = $"select * from Games where Id = '{id}'";

            var sqlDataReader = await OpenAndQuery(command);      

            while (sqlDataReader.Read())
            {
                game = new Game
                {
                    Id = (Guid)sqlDataReader["Id"],
                    Name = (string)sqlDataReader["Name"],
                    Producer = (string)sqlDataReader["Producer"],
                    Price = (double)sqlDataReader["Price"]                    
                };
            }

            await _sqlConnection.CloseAsync();

            return game;
        }

        public async Task<List<Game>> Get (string name, string producer)
        {
            var games = new List<Game>();

            var command = $"Select * from Games where Name = '{name}' and Producer = '{producer}'";

            var sqlDataReader = await OpenAndQuery(command);

            while (sqlDataReader.Read())
            {
                games.Add(new Game
                {
                    Id = (Guid)sqlDataReader["Id"],
                    Name = (string)sqlDataReader["Name"],
                    Producer = (string)sqlDataReader["Producer"],
                    Price = (double)sqlDataReader["Price"]
                });
            }

            await _sqlConnection.CloseAsync();

            return games;
        }

        public async Task Insert(Game game)
        {
            var command = $"insert Games (Id, Name, Producer, Price) values ('{game.Id}', '{game.Name}', '{game.Producer}', {game.Price.ToString().Replace(",", ".")})";

            await ExecuteQuery(command);
        }

        public async Task Update(Game game)
        {
            var command = $"update Games set Name = '{game.Name}', Producer = '{game.Producer}', Price = {game.Price.ToString().Replace(",", ".")} where Id = '{game.Id}'";

            await ExecuteQuery(command);
        }

        public async Task Delete(Guid id)
        {
            var command = $"delete from Games where Id = '{id}'";

            await ExecuteQuery(command);
        }

        public void Dispose()
        {
            _sqlConnection?.Close();
            _sqlConnection?.Dispose();
        }

        private async Task<SqlDataReader> OpenAndQuery(string command)
        {
            await _sqlConnection.OpenAsync();
            var sqlCommand = new SqlCommand(command, _sqlConnection);
            return await sqlCommand.ExecuteReaderAsync();
        }

        private async Task ExecuteQuery(string command)
        {
            await _sqlConnection.OpenAsync();
            var sqlCommand = new SqlCommand(command, _sqlConnection);
            sqlCommand.ExecuteNonQuery();
            await _sqlConnection.CloseAsync();
        }
    }
}
