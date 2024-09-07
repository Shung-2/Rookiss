using System.Collections;

namespace BlazorApp.Data
{
    public class Food
    {
        public string Name { get; set; }
        public int Price { get; set; }
    }

    public interface IFoodService
	{
		// 다양한 자료구조를 사용할 수 있으므로, IEnumerable를 사용한다.
		IEnumerable<Food> GetFoods();
	}

	public class FoodService : IFoodService
	{
        public IEnumerable<Food> GetFoods()
        {
            List<Food> foods = new List<Food>
            {
                new Food() { Name = "Bibimbap", Price = 7000 },
                new Food() { Name = "Kimbap", Price = 3000 },
                new Food() { Name = "Bossam", Price = 9000 }
            };

            return foods;
        }
    }

	public class FastFoodService : IFoodService
	{
		public IEnumerable<Food> GetFoods()
		{
			List<Food> foods = new List<Food>
			{
				new Food() { Name = "Burger", Price = 7000 },
				new Food() { Name = "Fries", Price = 3000 },
			};

			return foods;
		}
	}

	public class PaymentService
	{
		IFoodService _service;

		public PaymentService(IFoodService service)
		{
			_service = service;
		}

		// TODO
	}

	public class SingletonService : IDisposable
	{
		public Guid ID { get; set; }

		public SingletonService()
		{
			ID = Guid.NewGuid();
		}

		public void Dispose()
		{
            Console.WriteLine("SingletonServices Disposed");
		}
	}

	public class TransientService : IDisposable
	{
		public Guid ID { get; set; }

		public TransientService()
		{
			ID = Guid.NewGuid();
		}

		public void Dispose()
		{
			Console.WriteLine("TransientService Disposed");
		}
	}

	public class ScopedService : IDisposable
	{
		public Guid ID { get; set; }

		public ScopedService()
		{
			ID = Guid.NewGuid();
		}

		public void Dispose()
		{
			Console.WriteLine("ScopedService Disposed");
		}
	}
}
