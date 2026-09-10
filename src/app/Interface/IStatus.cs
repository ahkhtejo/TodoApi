
public interface IStatus
{
    Task<Status?> GetByIdAsync(int StatusID);

    Task<List<Status>> GetAllAsync();

    Task<Status> CreateAsync(CreateStatusDto statusDto);

    Task<Status?> UpdateAsync(GetStatusDto statusDto);

    Task<bool> DeleteAsync(int StatusID);
}