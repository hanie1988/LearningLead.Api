namespace Infrastructure.Repositories;

using Core.Entities;
using Core.Filters;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Core.Interfaces;
using Infrastructure.QueryExtensions;

public sealed class RoomQueryRepository(AppDbContext db): IRoomQueryRepository
{   
    public async Task<int> CountAsync(RoomFilter filter, CancellationToken ct)
    {
        var query = db.Rooms.AsQueryable();

        query = query.ApplyFilter(filter);

        return await query.CountAsync(ct);
    }

    public async Task<List<Room>> SearchAsync(RoomFilter filter, CancellationToken ct)
    {
        var query = db.Rooms.AsQueryable();

        query = query.ApplyFilter(filter);

        return await query
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync(ct);
    }
}