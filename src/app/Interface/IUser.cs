public interface IUser
{
    Task<User?> GetByIdAsync(int UserId);

    Task<List<User>> GetAllAsync();

    Task<User> CreateAsync(AddUserReqest user);

    Task<User?> UpdateAsync(User user);

    Task<bool> DeleteAsync(int UserId);

}
