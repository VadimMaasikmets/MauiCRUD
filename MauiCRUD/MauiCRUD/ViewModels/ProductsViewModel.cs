using CommunityToolkit.Mvvm.ComponentModel;
using Data;
using Models;
using System.Collections.ObjectModel;


namespace ViewModels
{
    public partial class ProductsViewModel : ObservableObject
    {
        private readonly DatabaseContext _context;
        public ProductsViewModel(DatabaseContext context)
        {
            _context = context;
        }
        [ObservableProperty]
        private ObservableCollection<Product> _products = new();

        [ObservableProperty]
        private Product _operatingProduct = new();

        [ObservableProperty]
        private bool _isBusy;

        [ObservableProperty]
        private string _busyText;

        public async Task LoadProductAsync()
        {
            await ExecuteAsync(async () =>
            {
                var products = await _context.GetAllAsync<Product>();
                if (products != null && products.Any())
                {
                    Products ??= new ObservableCollection<Product>();
                    foreach (var product in products)
                    {
                        Products.Add(product);
                    }
                }
            }, "Fetching products...");
        }
        private async Task ExecuteAsync(Func<Task> operation, string? busyText = null)
        {
            IsBusy = true;
            BusyText = busyText ?? "Processing...";
            try
            {
                await operation?.Invoke();

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                IsBusy = false;
                BusyText = "Processing...";
            }
        }
        [RelayCommand]
        private void SetOperatingProduct(Product? product) => OperatingProduct = product ?? new();
        [RelayCommand]
        private async Task SaveProductAsync()
        {
            if (OperatingProduct != null)
                return;
            var (isValid, errorMessage) = OperatingProduct.Validate();
            if (!isValid)
            {
                await Shell.Current.DisplayAlert("Validation Error", errorMessage, "OK");
                return;
            }
            var busy
        }

    }
}
