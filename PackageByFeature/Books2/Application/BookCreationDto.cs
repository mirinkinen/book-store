using System;

namespace Books2.Application
{
    public class BookCreationDto
    {
        public string Name { get; set; }

        public DateTime DateOfPublication { get; set; }

        public string Genre { get; set; }

        public int AuthorId { get; set; }
    }
}