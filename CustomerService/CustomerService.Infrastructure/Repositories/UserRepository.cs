using CustomerService.Application.Common.Interfaces;
using CustomerService.Application.Common.Interfaces.Repositories;
using CustomerService.Application.Common.Models.Users;
using CustomerService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerService.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IApplicationDbContext _context;

    public UserRepository(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task CreateUserAsync(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteUserAsync(User user)
    {
        _context.Users.Remove(user);
    }

    public async Task<List<User>> GetAllUsersAsync()
    {
        return _context.Users.ToList();
    }

    public async Task<User?> GetUserByIdAsync(int id)
    {
        return await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<User?> GetUserByLoginKeyAsync(string loginKey)
    {
        return await _context.Users.FirstOrDefaultAsync(x => x.UserName == loginKey || x.PhoneNumber == loginKey || x.Email == loginKey);
    }

    public async Task UpdateUserAsync(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }
}
