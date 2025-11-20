namespace Application.Reservations;

using Core.Entities;
using Core.Interfaces;
using Application.Common;

public sealed class ReservationService(IReservationRepository repo)
{
    public async Task<Reservation> CreateReservationAsync(ReservationCreateDto dto)
    {
        //check is canceled
        var available = await repo.RoomIsAvailable(dto.RoomId, dto.CheckIn, dto.CheckOut);

        if (!available)
            throw new AppException("Room is not available for the selected dates.", 409);

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