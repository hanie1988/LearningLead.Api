namespace Application.Reservations;

using Core.Entities;
using Core.Exceptions;
using Core.Interfaces;
using Application.Common;

public sealed class ReservationService(IReservationRepository repo)
{
    public async Task<Reservation> CreateReservationAsync(ReservationCreateDto dto)
    {
        if (dto.CheckIn >= dto.CheckOut)
            throw new InvalidDateRangeException();

        // var room = await _roomRepository.GetByIdAsync(dto.RoomId);

        // if (room is null)
        //     throw new RoomNotFoundException(dto.RoomId);

        var available = await repo.RoomIsAvailable(dto.RoomId, dto.CheckIn, dto.CheckOut);

        if (!available)
            throw new RoomNotAvailableException();

        var reservation = new Reservation
        {
            RoomId = dto.RoomId,
            UserId = dto.UserId,
            CheckIn = dto.CheckIn,
            CheckOut = dto.CheckOut
        };

        return await repo.AddAsync(reservation);
    }

    public async Task<bool> CancelReservationAsync(int reservationId)
    {
        var reservation = await repo.GetByIdAsync(reservationId);

        if (reservation is null)
            return false;

        reservation.IsCancelled = true;     

        await repo.UpdateAsync(reservation);

        return true;
    }
}