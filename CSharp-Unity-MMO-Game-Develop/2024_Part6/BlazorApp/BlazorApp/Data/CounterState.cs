namespace BlazorApp.Data
{
    public class CounterState
    {
        public int _count = 0;

        public Action OnStateChanged;

        public int Count
        {
            get => _count;
            set
            {
                _count = value;
                Refresh();
            }
        }

        void Refresh() => OnStateChanged?.Invoke();
    }
}
