namespace Core.Interfaces;

using Core.Filters;
using Core.Entities;
public interface IRoomQueryRepository
{
    Task<int> CountAsync(RoomFilter filter, CancellationToken ct);
    Task<List<Room>> SearchAsync(RoomFilter filter, CancellationToken ct);
}