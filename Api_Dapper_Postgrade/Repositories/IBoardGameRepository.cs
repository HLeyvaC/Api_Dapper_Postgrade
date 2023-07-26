using Api_Dapper_Postgrade.Dto;

namespace Api_Dapper_Postgrade.Repositories
{
    public interface IBoardGameRepository
    {
        Task<IEnumerable<BoardGameDto>> GetAll();

        Task<BoardGameDto> Get(int id);

        Task Add(BoardGameDto game);

        Task Update(int id, BoardGameDto game);

        Task Delete(int id);

    }

    public interface IAdditionalDbOperations
    {
        Task<string> GetVersion();

        Task CreateTableIfNotExists();
    }
}
