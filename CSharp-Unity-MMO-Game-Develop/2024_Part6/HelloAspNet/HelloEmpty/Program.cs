var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// ������ ��� �Ǿ� �ִ°�?
// M - Model -      ������ (������)
// V - View -       UI (���׸���)
// C - Controller - Controller (�׼�)

app.MapGet("/", () => "Hello World!");

app.Run();
