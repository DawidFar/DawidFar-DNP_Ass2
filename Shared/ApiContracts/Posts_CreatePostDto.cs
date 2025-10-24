namespace ApiContracts.Posts
{
    public class CreatePostDto
    {
        public required Guid AuthorId { get; set; }
        public required string Title { get; set; }
        public required string Content { get; set; }
    }
}