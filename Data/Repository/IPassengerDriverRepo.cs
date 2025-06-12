using nicenice.Server.Models.DTOs;
using nicenice.Server.Models;
using System;
using System.Threading.Tasks;

namespace nicenice.Server.Data.Repository
{
    public interface IPassengerDriverRepo
    {
        Task<bool> Exists(Guid userId);
        Task<bool> Register(PassengerDriverDto model);
    }
}
