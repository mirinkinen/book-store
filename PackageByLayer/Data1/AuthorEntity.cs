using System;
using System.Collections.Generic;

namespace Data1
{
    internal class AuthorEntity
    {
        public AuthorEntity()
        {
            Books = new List<BookEntity>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime DateOfBirth { get; set; }

        public List<BookEntity> Books { get; }
    }
}