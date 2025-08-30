using API.Types;

var builder = WebApplication.CreateBuilder(args);
builder.AddGraphQL()
    .AddTypes()
    .AddType<BookType>()
    .AddType<AuthorType>();

var app = builder.Build();
app.MapGraphQL();
app.RunWithGraphQLCommands(args);