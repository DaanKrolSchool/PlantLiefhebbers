using Microsoft.Extensions.Hosting;


public class Prijs : BackgroundService
{
    private decimal _price = 0;
    private decimal _pvMultiplier = 1;
    private Product _product;
    private Product current_product;
    private decimal MaxPrijs;
    private decimal minimumPrijs;
    private decimal prijsVerandering;
    private Boolean gestart = false;
    public void SetProduct(Product product)
    {
        _product = product;
        MaxPrijs = (decimal)product.maximumPrijs;
        minimumPrijs = (decimal)product.minimumPrijs;
        prijsVerandering = (decimal)product.prijsVerandering;
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Console.WriteLine("Prijs background service test test test");
        while (!stoppingToken.IsCancellationRequested)
        {

            if (_price <= minimumPrijs)
            {
                _price = MaxPrijs;
                //_pvMultiplier *= 1.15m;

            } else {
                decimal newPrice = _price - prijsVerandering;
                _price = newPrice;
            }

            Console.WriteLine(_price);


            await Task.Delay(1000, stoppingToken);

        }
    }

    public decimal GetPrice()
    {
        return _price;
    }
}
