using Api_Dapper_Postgrade.Dto;
using Dapper;
using Npgsql;



namespace Api_Dapper_Postgrade.Repositories
{
    public class DapperBoardGameRepository : IBoardGameRepository
    {
        private const string CONNECTION_STRING = "Host=localhost:5432;" +
                    "Username=postgres;" +
                    "Password=@Hleyva21;" +
                    "Database=postgresDB";

        private const string TABLE_NAME = "Games";
        private readonly NpgsqlConnection connection;

        public DapperBoardGameRepository()
        {
            connection = new NpgsqlConnection(CONNECTION_STRING);
            connection.Open();
        }

        public async Task Add(BoardGameDto game)
        {
            string commandText = $"INSERT INTO {TABLE_NAME} (id, Name, MinPlayers, MaxPlayers, AverageDuration) VALUES (@id, @name, @minPl, @maxPl, @avgDur)";

            var queryArguments = new
            {
                id = game.Id,
                name = game.Name,
                minPl = game.MinPlayers,
                maxPl = game.MaxPlayers,
                avgDur = game.AverageDuration
            };

            await connection.ExecuteAsync(commandText, queryArguments);
        }

        public async Task<IEnumerable<BoardGameDto>> GetAll()
        {
            string commandText = $"SELECT * FROM {TABLE_NAME}";
            var games = await connection.QueryAsync<BoardGameDto>(commandText);

            return games;
        }

        public async Task<BoardGameDto> Get(int id)
        {
            string commandText = $"SELECT * FROM {TABLE_NAME} WHERE ID = @id";

            var queryArgs = new { Id = id };
            var game = await connection.QueryFirstAsync<BoardGameDto>(commandText, queryArgs);
            return game;
        }

        public async Task Update(int id, BoardGameDto game)
        {
            var commandText = $@"UPDATE {TABLE_NAME}
                SET Name = @name, MinPlayers = @minPl, MaxPlayers = @maxPl, AverageDuration = @avgDur
                WHERE id = @id";

            var queryArgs = new
            {
                id = game.Id,
                name = game.Name,
                minPl = game.MinPlayers,
                maxPl = game.MaxPlayers,
                avgDur = game.AverageDuration
            };

            await connection.ExecuteAsync(commandText, queryArgs);
        }

        public async Task Delete(int id)
        {
            string commandText = $"DELETE FROM {TABLE_NAME} WHERE ID=(@p)";

            var queryArguments = new { p = id };

            await connection.ExecuteAsync(commandText, queryArguments);
        }

    }

    public class DapperAdditionalDbOperations : IAdditionalDbOperations
    {
        private const string CONNECTION_STRING = "Host=localhost:5432;" +
                   "Username=postgres;" +
                   "Password=@Hleyva21;" +
                   "Database=postgresDB";

        private const string TABLE_NAME = "Games";
        private readonly NpgsqlConnection connection;

        public DapperAdditionalDbOperations()
        {
            connection = new NpgsqlConnection(CONNECTION_STRING);
            connection.Open();
        }

        public async Task CreateTableIfNotExists()
        {
            var sql = $"CREATE TABLE if not exists {TABLE_NAME}" +
                     $"(" +
                     $"id serial PRIMARY KEY, " +
                     $"Name VARCHAR (200) NOT NULL, " +
                     $"MinPlayers SMALLINT NOT NULL, " +
                     $"MaxPlayers SMALLINT, " +
                     $"AverageDuration SMALLINT" +
                     $")";

            await connection.ExecuteAsync(sql);
        }

        public async Task<string> GetVersion()
        {
            var commandText = "SELECT version()";

            var value = await connection.ExecuteScalarAsync<string>(commandText);
            return value;
        }
    }
}
