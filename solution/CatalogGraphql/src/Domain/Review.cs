using Common.Domain;

namespace Domain;

public class Review : Entity
{
    public string Title { get; set; }

    public string Body { get; set; }

    public Book Book { get; set; }
    
    public Guid BookId { get; set; }
}