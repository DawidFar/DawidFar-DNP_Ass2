namespace ApiContracts.Comments
{
    public class CreateCommentDto
    {
        public required Guid PostId { get; set; }
        public required Guid UserId { get; set; }
        public required string Content { get; set; }
    }
}