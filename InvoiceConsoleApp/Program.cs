using AutoMapper;
using CsvHelper;
using CsvHelper.Configuration;
using InviceConsoleApp.Service.Interfaces;
using InviceConsoleApp.Service.Services;
using InviceConsoleApp.Service.ViewModels;
using InvoiceConsoleApp.Infra.CrossCutting.IoC;
using InvoiceConsoleApp.Infra.Data.Context;
using InvoiceConsoleApp.Infra.Data.Interfaces;
using InvoiceConsoleApp.Infra.Data.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;

try
{
    var config = new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory())
             .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
             .Build();

    var serviceProvider = new ServiceCollection()
        .AddSingleton<IConfiguration>(config)
        .AddDbContext<InvoiceConsoleAppDbContext>(options =>
            options.UseSqlServer(config.GetConnectionString("DefaultConnection")))
        .AddScoped<IInvoiceConsoleAppService, InvoiceConsoleAppService>()
        .AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies())
        .AddScoped<IInvoiceHeaderRepository, InvoiceHeaderRepository>()
        .AddScoped<IInvoiceLineRepository, InvoiceLineRepository>()
                .BuildServiceProvider();

    //RegisterServices(serviceProvider);

    //static void RegisterServices(ServiceCollection services)
    //{
    //    // Adding dependencies from another layers (isolated from Presentation)
    //    SimpleInjectorBootStrapper.RegisterServices(services);
    //}

    var invoiceDataList = ReadCsvFile(AppContext.BaseDirectory + "data.csv");

    var invoiceService = serviceProvider.GetRequiredService<IInvoiceConsoleAppService>();

    //invoiceService.Create(invoiceDataList);

    var total = invoiceService.GetProductOfQuantityAndUnitSellingPriceExVAT();

    static List<InvoiceHeaderViewModel> ReadCsvFile(string filePath)
    {
        var invoiceDataList = new List<InvoiceHeaderViewModel>();
        var invoiceLines = new List<InvoiceLineViewModel>();
        var invoiceNumbers = new HashSet<string>();

        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException("The specified file does not exist.", filePath);
        }

        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true,  // Specify that the first row is header columns
            MissingFieldFound = null,
            Delimiter = ","
        };

        using (var reader = new StreamReader(filePath))
        using (var csv = new CsvReader(reader, config))
        {
            csv.Context.RegisterClassMap<InvoiceHeaderMap>();
            csv.Context.RegisterClassMap<InvoiceLineMap>();

            // Read records
            while (csv.Read())
            {
                var record = csv.GetRecord<InvoiceHeaderViewModel>();
                if (invoiceNumbers.Add(record.InvoiceNumber))
                {
                    record.InvoiceLines = new List<InvoiceLineViewModel>();
                    invoiceDataList.Add(record);
                }

                var lineRecord = csv.GetRecord<InvoiceLineViewModel>();
                invoiceLines.Add(lineRecord);
            }
        }

        // Group InvoiceLines by InvoiceNumber and add them to InvoiceHeaders
        var groupedLines = invoiceLines
            .GroupBy(l => l.InvoiceNumber)
            .Select(g => new
            {
                InvoiceNumber = g.Key,
                Lines = g.ToList()
            });

        foreach (var group in groupedLines)
        {
            var header = invoiceDataList.FirstOrDefault(h => h.InvoiceNumber == group.InvoiceNumber);
            if (header != null)
            {
                if (header.InvoiceLines == null)
                {
                    header.InvoiceLines = new List<InvoiceLineViewModel>();
                }
                foreach (var line in group.Lines)
                {
                    header.InvoiceLines.Add(line);
                }
            }
        }

        return invoiceDataList;
    }
}
catch (Exception ex)
{
    Console.WriteLine(ex.ToString());
}