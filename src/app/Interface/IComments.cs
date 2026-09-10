
public interface IComments
{
    Task<GetCommentsDto?> GetByIdAsync(int CommentID);

    Task<List<GetCommentsDto>> GetAllAsync();

    Task<Comments> CreateAsync(CreateCommentDto CommentDto);

    Task<GetCommentsDto?> UpdateAsync(Comments Comment);

    Task<bool> DeleteAsync(int CommentID);

}