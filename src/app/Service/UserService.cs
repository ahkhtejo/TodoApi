
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

public class UserService : IUser
{
    private readonly AppDbContext _dbContext;


    public UserService(AppDbContext dbContext)
    {
        _dbContext = dbContext;

    }
    public async Task<User> CreateAsync(AddUserReqest user)
    {
        User AddedUser = new User
        {

            Name = user.Name,
            Email = user.Email,
            BirthDate = user.BirthDate,
            UserType = user.UserType
        };
        _dbContext.Users.Add(AddedUser);
        await _dbContext.SaveChangesAsync();
        return AddedUser;
    }

    public async Task<bool> DeleteAsync(int UserId)
    {
        var user = _dbContext.Users.FirstOrDefault(u => u.Id == UserId);
        if (user is null)
        {
            return false;
        }
        _dbContext.Remove(user);
        return true;
    }

    public async Task<List<User>> GetAllAsync()
    {
        return _dbContext.Users.ToList();
    }

    public async Task<User?> GetByIdAsync(int UserId)
    {
        var user = _dbContext.Users.FirstOrDefault(u => u.Id == UserId);

        return user;

    }

    public async Task<User?> UpdateAsync(User user)
    {
        var DbUser = await GetByIdAsync(user.Id);
        if (DbUser is not null)
        {
            DbUser.Name = user.Name;
            DbUser.BirthDate = user.BirthDate;
            DbUser.UserType = user.UserType;
            _dbContext.Entry(DbUser).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }
        return DbUser;
    }
}