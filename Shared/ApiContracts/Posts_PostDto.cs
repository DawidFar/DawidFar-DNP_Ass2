namespace ApiContracts.Posts
{
    public class PostDto
    {
        public Guid Id { get; set; }
        public Guid AuthorId { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? AuthorName { get; set; }
    }
}