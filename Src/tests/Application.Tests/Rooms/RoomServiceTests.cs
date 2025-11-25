using Xunit;
using FluentAssertions;
using Moq;
using Application.Rooms;
using Core.Interfaces;
using Application.Queries.Rooms;
using Core.Filters;
using Core.Entities;
using Application.Tests.Common;

public class RoomServiceTests: TestingHelper
{
    private readonly Mock<IRoomRepository> _roomRepoMock;
    private readonly Mock<IRoomQueryRepository> _queryRepoMock;
    private readonly RoomService _service;

    public RoomServiceTests()
    {
        _roomRepoMock = new Mock<IRoomRepository>();
        //_queryRepoMock = new Mock<IRoomQueryRepository>(MockBehavior.Strict);
        _queryRepoMock = new Mock<IRoomQueryRepository>();

        _service = new RoomService(_roomRepoMock.Object, _queryRepoMock.Object);
    }

    [Fact]
    public async Task SearchAsync_Should_Map_Filter_And_Return_PagedResult()
    {
        // Arrange
        var dto = new RoomFilterDto
        {
            HotelId = 1,
            MinCapacity = 2,
            MaxCapacity = 4,
            MinPrice = 100,
            MaxPrice = 300,
            SortBy = "price",
            SortDirection = "asc",
            Page = 2,
            PageSize = 10
        };

        var expectedFilter = new RoomFilter
        {
            HotelId = 1,
            MinCapacity = 2,
            MaxCapacity = 4,
            MinPrice = 100,
            MaxPrice = 300,
            SortBy = "price",
            SortDirection = "asc",
            Page = 2,
            PageSize = 10
        };

        var rooms = new List<Room>
        {
            new Room { Id = 10, HotelId = 1, RoomNumber = "201", Capacity = 3, PricePerNight = 150 },
            new Room { Id = 11, HotelId = 1, RoomNumber = "202", Capacity = 4, PricePerNight = 200 },
        };

        _queryRepoMock
            .Setup(r => r.CountAsync(It.IsAny<RoomFilter>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(2);

        _queryRepoMock
            .Setup(r => r.SearchAsync(It.IsAny<RoomFilter>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(rooms);

        // Act
        var result = await _service.SearchAsync(dto, CancellationToken.None);

        // Assert
        result.TotalCount.Should().Be(2);
        result.Items.Should().HaveCount(2);

        result.Items[0].Id.Should().Be(10);
        result.Items[0].PricePerNight.Should().Be(150);

        result.Page.Should().Be(2);
        result.PageSize.Should().Be(10);

        // Verify calls happened with correct filter
        _queryRepoMock.Verify(r =>
            r.CountAsync(It.Is<RoomFilter>(f =>
                f.HotelId == expectedFilter.HotelId &&
                f.MinCapacity == expectedFilter.MinCapacity &&
                f.MaxCapacity == expectedFilter.MaxCapacity &&
                f.MinPrice == expectedFilter.MinPrice &&
                f.MaxPrice == expectedFilter.MaxPrice &&
                f.SortBy == expectedFilter.SortBy &&
                f.SortDirection == expectedFilter.SortDirection &&
                f.Page == expectedFilter.Page &&
                f.PageSize == expectedFilter.PageSize
            ), It.IsAny<CancellationToken>()),
            Times.Once
        );
    }
}