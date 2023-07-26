using Api_Dapper_Postgrade.Dto;
using Api_Dapper_Postgrade.Repositories;
using Dapper;
using Moq;
using Npgsql;

namespace BoardGamesTesting
{
    public class DapperBoardGameRepositoryTests
    {
        // Mock the NpgsqlConnection
        private readonly Mock<NpgsqlConnection> mockConnection;

        // Mock the DapperBoardGameRepository 
        private readonly DapperBoardGameRepository boardGameRepository;

        public DapperBoardGameRepositoryTests()
        {
            mockConnection = new Mock<NpgsqlConnection>();
            boardGameRepository = new DapperBoardGameRepository() { connection = mockConnection.Object };
        }

        [Fact]
        public async Task Add_ValidBoardGame_Success()
        {
            // Arrange
            var gameToAdd = new BoardGameDto
            {
                Id = 1,
                Name = "Test Game",
                MinPlayers = 2,
                MaxPlayers = 4,
                AverageDuration = 60
            };

            // Act
            await boardGameRepository.Add(gameToAdd);

            // Assert
            mockConnection.Verify(
                conn => conn.ExecuteAsync(
                    It.IsAny<string>(),
                    It.Is<BoardGameDto>(g =>
                        g.Id == gameToAdd.Id &&
                        g.Name == gameToAdd.Name &&
                        g.MinPlayers == gameToAdd.MinPlayers &&
                        g.MaxPlayers == gameToAdd.MaxPlayers &&
                        g.AverageDuration == gameToAdd.AverageDuration
                    ),
                    null,
                    null,
                    System.Threading.CancellationToken.None
                ),
                Times.Once
            );
        }

        [Fact]
        public async Task GetAll_ReturnsListOfBoardGames()
        {
            // Arrange
            var expectedGames = new List<BoardGameDto>
        {
            new BoardGameDto { Id = 1, Name = "Game 1", MinPlayers = 2, MaxPlayers = 4, AverageDuration = 60 },
            new BoardGameDto { Id = 2, Name = "Game 2", MinPlayers = 1, MaxPlayers = 6, AverageDuration = 90 }
        };

            mockConnection
                .Setup(conn => conn.QueryAsync<BoardGameDto>(It.IsAny<string>()))
                .ReturnsAsync(expectedGames);

            // Act
            var result = await boardGameRepository.GetAll();

            // Assert
            Assert.Equal(expectedGames.Count, result.Count());
            Assert.Equal(expectedGames[0].Name, result.First().Name);
        }

        // Similar tests for Get, Update, and Delete methods can be written.
    }

    public class DapperAdditionalDbOperationsTests
    {
        // Mock the NpgsqlConnection
        private readonly Mock<NpgsqlConnection> mockConnection;

        // Mock
        private readonly DapperAdditionalDbOperations additionalDbOperations;

        public DapperAdditionalDbOperationsTests()
        {
            mockConnection = new Mock<NpgsqlConnection>();
            additionalDbOperations = new DapperAdditionalDbOperations() { connection = mockConnection.Object };
        }

        [Fact]
        public async Task CreateTableIfNotExists_Success()
        {
            // Arrange

            // Act
            await additionalDbOperations.CreateTableIfNotExists();

            // Assert
            mockConnection.Verify(
                conn => conn.ExecuteAsync(
                    It.IsAny<string>(),
                    null,
                    null,
                    null,
                    System.Threading.CancellationToken.None
                ),
                Times.Once
            );
        }

        [Fact]
        public async Task GetVersion_ReturnsVersionString()
        {
            // Arrange
            var expectedVersion = "PostgreSQL 13.4";

            mockConnection
                .Setup(conn => conn.ExecuteScalarAsync<string>(It.IsAny<string>()))
                .ReturnsAsync(expectedVersion);

            // Act
            var result = await additionalDbOperations.GetVersion();

            // Assert
            Assert.Equal(expectedVersion, result);
        }
    }
}