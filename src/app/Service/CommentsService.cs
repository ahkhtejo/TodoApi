
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

public class CommentsService : IComments
{
    private readonly AppDbContext _dbContext;
    private readonly UserService _userService;
    public CommentsService(AppDbContext dbContext, UserService userService)
    {
        _dbContext = dbContext;
        _userService = userService;
    }

    public async Task<Comments> CreateAsync(CreateCommentDto CommentDto)
    {
        User user = await _userService.GetByIdAsync(CommentDto.UserId);

        if (user is null)
        {
            throw new IndexOutOfRangeException("User ID was not Founed");
        }

        Comments comments = new Comments
        {
            Body = CommentDto.Body,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now,
            UserId = user.Id,
            TodoTaskID = CommentDto.TodoTaskID

        };

        _dbContext.comments.Add(comments);
        await _dbContext.SaveChangesAsync();
        return comments;
    }

    public async Task<bool> DeleteAsync(int CommentID)
    {

        var comment = await _dbContext.comments.SingleOrDefaultAsync(c => c.Id == CommentID);

        if (comment is not null)
        {
            _dbContext.comments.Remove(comment);
            await _dbContext.SaveChangesAsync();
            return true;
        }
        return false;
    }

    public async Task<List<GetCommentsDto>> GetAllAsync()
    {
        var allComments = await _dbContext.comments
        .Include(c => c.users)
        .Include(c => c.todoTask)
        .ToListAsync();
        var commentsList = allComments
        .Select(item => CommentDtoMapping.MapCommetToDto(item))
        .ToList();
        return commentsList;
    }

    public async Task<GetCommentsDto?> GetByIdAsync(int CommentID)
    {
        var comment = await _dbContext.comments.Include(c => c.users).Include(c => c.todoTask).SingleOrDefaultAsync(c => c.Id == CommentID);

        if (comment == null)
        {
            return null;
        }

        return CommentDtoMapping.MapCommetToDto(comment);
    }

    public async Task<GetCommentsDto?> UpdateAsync(Comments Comment)
    {
        var exstsingComment = await _dbContext.comments.SingleOrDefaultAsync(c => c.Id == Comment.Id);
        exstsingComment?.Body = Comment.Body;
        exstsingComment?.UserId = Comment.UserId;
        _dbContext.Entry(exstsingComment).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();
        return CommentDtoMapping.MapCommetToDto(exstsingComment);
    }
}