using InventoryManagement.Application.DTOs.Base;
using InventoryManagement.Domain.Entities;
using MediatR;

namespace InventoryManagement.Application.Consumptions.Commands;

public class RegisterConsumptionCommand(Consumption consumption) : IRequest<ResultResponse<Consumption>>
{
    public Consumption Consumption = consumption;
}