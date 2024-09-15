using System;
using System.ComponentModel.DataAnnotations;

namespace BlazorStudy.Data
{
	public class WeatherForecast
	{
		public DateTime Date { get; set; }

		[Required(ErrorMessage = "Need TemperatureC!")]
		[Range(typeof(int), "-100", "100")]
		public int TemperatureC { get; set; }

		public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

		[Required(ErrorMessage = "Need Summary!")]
		[StringLength(10, MinimumLength = 2, ErrorMessage = "Summary must be between 2 and 10 characters.")]
        public string? Summary { get; set; }
	}
}
