using Api_Dapper_Postgrade.Controllers;
using Api_Dapper_Postgrade.Repositories;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace UnitTesting
{
    public class BoardTesting
    {
        private readonly BoardGameController _controller;
        private readonly IBoardGameRepository _repository;
        private readonly IBoardGameRepository _repository2;
        public BoardTesting()
        {
            _repository = new DapperBoardGameRepository();
            _controller = new BoardGameController(_repository, (IAdditionalDbOperations)_repository2);
        }

        [Fact]
        public void Get_OK()
        {
            var result = _controller.Get();
            Assert.IsType<OkObjectResult>(result);

        }
    }
}