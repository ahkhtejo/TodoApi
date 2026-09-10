
using Microsoft.EntityFrameworkCore;

public class StatusSarvice : IStatus
{
    private readonly AppDbContext _dbContext;
    public StatusSarvice(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Status> CreateAsync(CreateStatusDto statusDto)
    {
        Status status = new Status
        {
            StatusName = statusDto.StatusName,
            StatusDescription = statusDto.StatusDescription
        };
        _dbContext.Statuses.Add(status);
        await _dbContext.SaveChangesAsync();
        return status;
    }

    public async Task<bool> DeleteAsync(int StatusID)
    {
        var status = await _dbContext.Statuses.SingleOrDefaultAsync(s => s.Id == StatusID);
        if (status is not null)
        {
            _dbContext.Statuses.Remove(status);
            await _dbContext.SaveChangesAsync();
            return true;

        }
        return false;
    }

    public async Task<List<Status>> GetAllAsync()
    {
        List<Status> statuses = await _dbContext.Statuses.ToListAsync();
        return statuses;
    }

    public async Task<Status?> GetByIdAsync(int StatusID)
    {
        var status = await _dbContext.Statuses.SingleOrDefaultAsync(s => s.Id == StatusID);
        if (status is null)
        {
            return null;
        }
        return status;
    }

    public async Task<Status?> UpdateAsync(GetStatusDto statusDto)
    {
        var status = await _dbContext.Statuses.SingleOrDefaultAsync(s => s.Id == statusDto.Id);
        if (status is null)
        {
            return null;
        }
        status.StatusName = statusDto.StatusName;
        status.StatusDescription = statusDto.StatusDescription;

        _dbContext.Entry(status).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();
        return status;
    }
}