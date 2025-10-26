using Common.Domain;
using Common.Infra;
using Domain.Orders;
using Domain.OrderItems;

namespace TestData;

public static class TestDataContainer
{
    // System user ID for test data.
    public static Guid SystemUserId => Guid.Parse("00000000-0000-0000-0000-000000000001");

    // Organization IDs for test data.
    public static Guid AuthorizedOrganization1 => Guid.Parse("10000000-0000-0000-0000-000000000001");
    public static Guid AuthorizedOrganization2 => Guid.Parse("10000000-0000-0000-0000-000000000002");

    // Order IDs for test data.
    public static Guid Order1Id => Guid.Parse("8E6A9434-87F5-46B2-A6C3-522DC35D8EEF");
    public static Guid Order2Id => Guid.Parse("7D5B8323-76E4-35A1-B5B2-411CB24C7DEE");
    public static Guid Order3Id => Guid.Parse("6C4A7212-65D3-24F0-A4A1-300BA13B6CDD");

    // Order Item IDs for test data.
    public static Guid OrderItem1Id => Guid.Parse("A125C5BD-4F8E-4794-9C36-76E401FB4F24");
    public static Guid OrderItem2Id => Guid.Parse("B234D6CE-5E9F-5805-AD47-87F512FC5E35");
    public static Guid OrderItem3Id => Guid.Parse("C345E7DF-6FAE-6916-BE58-98E623ED6F46");

    public static IEnumerable<Order> GetOrders()
    {
        var orders = new Order[]
        {
            new Order("John Smith", new DateOnly(2025, 10, 4), AuthorizedOrganization2).SetId(Order1Id),
            new Order("Jane Doe", new DateOnly(2025, 10, 9), AuthorizedOrganization2).SetId(Order2Id),
            new Order("Bob Johnson", new DateOnly(2025, 10, 11), AuthorizedOrganization1).SetId(Order3Id),
        };

        return orders;
    }

    public static IEnumerable<OrderItem> GetOrderItems(IEnumerable<Order> orders)
    {
        var ordersList = orders.ToList();
        var orderItems = new List<OrderItem>();

        // Add specific order items with known IDs for easier testing (these will be first when sorted by ID)
        orderItems.Add(new OrderItem(Order1Id, "The Great Gatsby", 1, 15.99m).SetId(OrderItem1Id));
        orderItems.Add(new OrderItem(Order1Id, "To Kill a Mockingbird", 2, 12.50m).SetId(OrderItem2Id));
        orderItems.Add(new OrderItem(Order2Id, "1984 by George Orwell", 1, 14.99m).SetId(OrderItem3Id));

        // Generate deterministic order items for all orders with book titles
        var bookTitles = GetDeterministicBookTitles();
        for (int i = 1; i <= 100; i++)
        {
            var orderItem = new OrderItem(
                GetDeterministicOrder(ordersList, i).Id,
                bookTitles[i - 1],
                GetDeterministicQuantity(i),
                GetDeterministicUnitPrice(i));
            
            // Set deterministic ID based on the index
            orderItem.SetId(GetDeterministicOrderItemId(i));
            orderItems.Add(orderItem);
        }

        return orderItems;
    }

    private static List<string> GetDeterministicBookTitles()
    {
        // Generate a deterministic list of 100 book titles
        var titles = new List<string>
        {
            "Pride and Prejudice", "The Catcher in the Rye", "The Hobbit", "Harry Potter and the Sorcerer's Stone",
            "The Lord of the Rings", "The Chronicles of Narnia", "Animal Farm", "Brave New World",
            "The Odyssey", "The Iliad", "Moby-Dick", "War and Peace",
            "Crime and Punishment", "The Brothers Karamazov", "Jane Eyre", "Wuthering Heights",
            "Great Expectations", "A Tale of Two Cities", "The Picture of Dorian Gray", "Dracula",
            "Frankenstein", "Alice's Adventures in Wonderland", "The Adventures of Tom Sawyer", "The Adventures of Huckleberry Finn",
            "Little Women", "Anne of Green Gables", "The Secret Garden", "Treasure Island",
            "Robinson Crusoe", "Gulliver's Travels", "Don Quixote", "The Count of Monte Cristo",
            "The Three Musketeers", "Les Misérables", "The Hunchback of Notre-Dame", "Madame Bovary",
            "Anna Karenina", "The Metamorphosis", "The Trial", "The Stranger",
            "The Plague", "One Hundred Years of Solitude", "Love in the Time of Cholera", "The Old Man and the Sea",
            "For Whom the Bell Tolls", "A Farewell to Arms", "The Sun Also Rises", "The Grapes of Wrath",
            "Of Mice and Men", "East of Eden", "Catch-22", "Slaughterhouse-Five",
            "The Great Gatsby", "To Kill a Mockingbird", "1984", "Fahrenheit 451",
            "Lord of the Flies", "The Hunger Games", "Divergent", "The Maze Runner",
            "Twilight", "The Da Vinci Code", "Angels and Demons", "The Alchemist",
            "Life of Pi", "The Kite Runner", "A Thousand Splendid Suns", "The Book Thief",
            "The Help", "The Fault in Our Stars", "Gone Girl", "The Girl on the Train",
            "Big Little Lies", "Where the Crawdads Sing", "Educated", "Becoming",
            "Sapiens", "The Immortal Life of Henrietta Lacks", "Unbroken", "The Glass Castle",
            "Wild", "Eat, Pray, Love", "The Power of Now", "Thinking, Fast and Slow",
            "Outliers", "The Tipping Point", "Blink", "Quiet",
            "Daring Greatly", "Atomic Habits", "The 7 Habits of Highly Effective People", "How to Win Friends and Influence People",
            "Rich Dad Poor Dad", "The Lean Startup", "Zero to One", "Good to Great"
        };

        // Ensure we have exactly 100 titles
        while (titles.Count < 100)
        {
            titles.Add($"Classic Book #{titles.Count + 1}");
        }

        return titles;
    }

    private static int GetDeterministicQuantity(int orderItemId)
    {
        // Use modulo to create deterministic quantity between 1 and 5 (more realistic for books)
        return 1 + orderItemId % 5;
    }

    private static decimal GetDeterministicUnitPrice(int orderItemId)
    {
        // Use modulo to create deterministic price between 9.99 and 34.99 (typical book prices)
        return 9.99m + (orderItemId % 25);
    }

    private static Order GetDeterministicOrder(List<Order> orders, int orderItemId)
    {
        // Use modulo to deterministically assign orders based on order item ID
        return orders[orderItemId % orders.Count];
    }

    private static Guid GetDeterministicOrderItemId(int index)
    {
        // Generate deterministic GUID based on index
        var bytes = new byte[16];
        var indexBytes = BitConverter.GetBytes(index);
        Array.Copy(indexBytes, bytes, Math.Min(indexBytes.Length, bytes.Length));
        return new Guid(bytes);
    }
}