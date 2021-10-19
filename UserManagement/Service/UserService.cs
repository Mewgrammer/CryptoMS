using Microsoft.EntityFrameworkCore;
using Npgsql;
using Opw.HttpExceptions;
using UserManagement.Data;
using UserManagement.Data.Entity;
using UserManagement.Helpers;

namespace UserManagement.Service;

public class UserService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly PasswordHasher _passwordHasher;
    private readonly ILogger<UserService> _logger;

    public UserService(IServiceScopeFactory scopeFactory, ILogger<UserService> logger)
    {
        _scopeFactory = scopeFactory;
        _passwordHasher = new PasswordHasher();
        _logger = logger;
    }

    public async Task<List<User>> Find()
    {
        using var scope = _scopeFactory.CreateScope();
        await using var dbContext = scope.ServiceProvider.GetRequiredService<UserDbContext>();
        return await dbContext.Users.ToListAsync();
    }

    public async Task<User?> FindOneById(Guid id)
    {
        using var scope = _scopeFactory.CreateScope();
        await using var dbContext = scope.ServiceProvider.GetRequiredService<UserDbContext>();
        return await dbContext.Users.SingleOrDefaultAsync(u => u.Id == id);
    }

    public async Task<User?> FindOneByName(string name)
    {
        using var scope = _scopeFactory.CreateScope();
        await using var dbContext = scope.ServiceProvider.GetRequiredService<UserDbContext>();
        return await dbContext.Users
            .Include(u => u.Roles)
            .SingleOrDefaultAsync(u => u.Name == name);
    }

    public async Task<User> CreateUser(string name, string password, List<string> roles)
    {
        var hashedPassword = _passwordHasher.Hash(password);
        using var scope = _scopeFactory.CreateScope();
        await using var dbContext = scope.ServiceProvider.GetRequiredService<UserDbContext>();
        var addedUser = await dbContext.Users.AddAsync(new User
        {
            Name = name,
            Password = hashedPassword,
            Roles = roles.Select(r => new UserRole {Name = r}).ToList()
        });
        try
        {
            await dbContext.SaveChangesAsync();
        }
        catch (DbUpdateException e)
        {
            if (e.InnerException is PostgresException {SqlState: PostgresErrorCodes.UniqueViolation})
            {
                throw new ConflictException("user with same name already exists");
            }
        }
        return addedUser.Entity;
    }

    public async Task RemoveUser(Guid id)
    {
        using var scope = _scopeFactory.CreateScope();
        await using var dbContext = scope.ServiceProvider.GetRequiredService<UserDbContext>();
        var user = await dbContext.Users.SingleOrDefaultAsync(u => u.Id == id);
        if (user == null) throw new NotFoundException("user does not exist");
        dbContext.Users.Remove(user);
        await dbContext.SaveChangesAsync();
    }
}