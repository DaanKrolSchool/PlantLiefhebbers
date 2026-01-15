using Microsoft.Extensions.Hosting;


public class Prijs : BackgroundService
{
    private decimal _priceRijnsburg = 0;
    private Product _productRijnsburg;
    private int newpidRijnsburg = -1;
    private int currentpidRijnsburg = -1;
    private decimal MaxPrijsRijnsburg;
    private decimal minimumPrijsRijnsburg;
    private decimal prijsVeranderingRijnsburg;

    private decimal _priceNaaldwijk = 0;
    private Product _productNaaldwijk;
    private int newpidNaaldwijk = -1;
    private int currentpidNaaldwijk = -1;
    private decimal MaxPrijsNaaldwijk;
    private decimal minimumPrijsNaaldwijk;
    private decimal prijsVeranderingNaaldwijk;

    private decimal _priceEelde = 0;
    private Product _productEelde;
    private int newpidEelde = -1;
    private int currentpidEelde = -1;
    private decimal MaxPrijsEelde;
    private decimal minimumPrijsEelde;
    private decimal prijsVeranderingEelde;

    private decimal _priceAalsmeer = 0;
    private Product _productAalsmeer;
    private int newpidAalsmeer = -1;
    private int currentpidAalsmeer = -1;
    private decimal MaxPrijsAalsmeer;
    private decimal minimumPrijsAalsmeer;
    private decimal prijsVeranderingAalsmeer;
    public void SetProduct(Product product)
    {

        if (product.klokLocatie == "rijnsburg")
        {
            _productRijnsburg = product;
            newpidRijnsburg = product.productId;
            MaxPrijsRijnsburg = (decimal)(product.maximumPrijs ?? product.minimumPrijs);
            minimumPrijsRijnsburg = (decimal)product.minimumPrijs;
            prijsVeranderingRijnsburg = (decimal)(product.prijsVerandering <= 0 ? 0.1f : product.prijsVerandering);
        }


        if (product.klokLocatie == "naaldwijk")
        {
            _productNaaldwijk = product;
            newpidNaaldwijk = product.productId;
            MaxPrijsNaaldwijk = (decimal)(product.maximumPrijs ?? product.minimumPrijs);
            minimumPrijsNaaldwijk = (decimal)product.minimumPrijs;
            prijsVeranderingNaaldwijk = (decimal)(product.prijsVerandering <= 0 ? 0.1f : product.prijsVerandering);
        }

        if (product.klokLocatie == "eelde")
        {
            _productEelde = product;
            newpidEelde = product.productId;
            MaxPrijsEelde = (decimal)(product.maximumPrijs ?? product.minimumPrijs);
            minimumPrijsEelde = (decimal)product.minimumPrijs;
            prijsVeranderingEelde = (decimal)(product.prijsVerandering <= 0 ? 0.1f : product.prijsVerandering);
        }

        if (product.klokLocatie == "aalsmeer")
        {
            _productAalsmeer = product;
            newpidAalsmeer = product.productId;
            MaxPrijsAalsmeer = (decimal)(product.maximumPrijs ?? product.minimumPrijs);
            minimumPrijsAalsmeer = (decimal)product.minimumPrijs;
            prijsVeranderingAalsmeer = (decimal)(product.prijsVerandering <= 0 ? 0.1f : product.prijsVerandering);
        }
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Console.WriteLine("Prijs background service test test test");
        while (!stoppingToken.IsCancellationRequested)
        {
            // Rijnsburg
            if (currentpidRijnsburg != newpidRijnsburg)
            {
                currentpidRijnsburg = newpidRijnsburg;
                _priceRijnsburg = MaxPrijsRijnsburg;
            }

            if (_priceRijnsburg <= minimumPrijsRijnsburg)
            {
                _priceRijnsburg = MaxPrijsRijnsburg;
            }
            else
            {
                _priceRijnsburg -= prijsVeranderingRijnsburg;
            }

            // Naaldwijk
            if (currentpidNaaldwijk != newpidNaaldwijk)
            {
                currentpidNaaldwijk = newpidNaaldwijk;
                _priceNaaldwijk = MaxPrijsNaaldwijk;
            }

            if (_priceNaaldwijk <= minimumPrijsNaaldwijk)
            {
                _priceNaaldwijk = MaxPrijsNaaldwijk;
            }
            else
            {
                _priceNaaldwijk -= prijsVeranderingNaaldwijk;
            }

            // Eelde
            if (currentpidEelde != newpidEelde)
            {
                currentpidEelde = newpidEelde;
                _priceEelde = MaxPrijsEelde;
            }

            if (_priceEelde <= minimumPrijsEelde)
            {
                _priceEelde = MaxPrijsEelde;
            }
            else
            {
                _priceEelde -= prijsVeranderingEelde;
            }

            // Aalsmeer
            if (currentpidAalsmeer != newpidAalsmeer)
            {
                currentpidAalsmeer = newpidAalsmeer;
                _priceAalsmeer = MaxPrijsAalsmeer;
            }

            if (_priceAalsmeer <= minimumPrijsAalsmeer)
            {
                _priceAalsmeer = MaxPrijsAalsmeer;
            }
            else
            {
                _priceAalsmeer -= prijsVeranderingAalsmeer;
            }

            //Console.WriteLine(_priceRijnsburg);


            await Task.Delay(1000, stoppingToken);

        }
    }

    public decimal GetPriceRijnsburg()
    {
        return _priceRijnsburg;
    }
    public decimal GetPriceNaaldwijk()
    {
        return _priceNaaldwijk;
    }
    public decimal GetPriceEelde()
    {
        return _priceEelde;
    }
    public decimal GetPriceAalsmeer()
    {
        return _priceAalsmeer;
    }
}
