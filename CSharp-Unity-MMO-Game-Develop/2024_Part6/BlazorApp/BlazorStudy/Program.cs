using BlazorStudy.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

// SPA - Single Page Application
// 페이지 전체를 다시 로드하는 방식이 아닌, 바뀌는 부분만 JavaScript와 DOM을 통해 업데이트하는 방식
// Counter나 FetchData가 아닌 페이지 전체 레이아웃을 변경해야 할 때 어디서 어떻게 해야할까?가 오늘의 핵심 주제

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
