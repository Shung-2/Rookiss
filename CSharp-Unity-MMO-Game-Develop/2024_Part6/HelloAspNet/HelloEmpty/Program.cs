var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// 구성이 어떻게 되어 있는가?
// M - Model -      데이터 (원자재)
// V - View -       UI (인테리어)
// C - Controller - Controller (액션)

app.MapGet("/", () => "Hello World!");

app.Run();
