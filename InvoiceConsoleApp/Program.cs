using CsvHelper;
using CsvHelper.Configuration;
using InviceConsoleApp.Service.Interfaces;
using InviceConsoleApp.Service.Services;
using InviceConsoleApp.Service.ViewModels;
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

    var invoiceService = serviceProvider.GetRequiredService<IInvoiceConsoleAppService>();

    var existingInvoiceNumbers = invoiceService.GetListOfInvoiceNumbers();

    var invoiceDataList = ReadCsvFile(AppContext.BaseDirectory + "data.csv", existingInvoiceNumbers);

    //invoiceService.Create(invoiceDataList);

    var invoiceNumberAndQuatityViewModel = invoiceService.GetInvoiceNumberAndSumOfAssociatedLines(invoiceDataList);

    var balanceCheck = invoiceService.GetProductOfQuantityAndUnitSellingPriceExVAT(invoiceDataList);

    Console.WriteLine($"\t\t\tInvoice Number and Total Quantity");
    Console.WriteLine($"===========================================================================================");
    invoiceNumberAndQuatityViewModel?.ToList().ForEach(x => Console.WriteLine($"Invoice Number: {x.InvoiceNumber} and Total Quantity: {x.TotalQuantity}"));
    Console.WriteLine($"===========================================================================================");
    Console.WriteLine($"Sum of all InvoiceHeader.InvoiceTotal: {balanceCheck.HeaderInvoiceTotal}");
    Console.WriteLine($"Sum of InvoiceLines.Quantity * InvoiceLines.UnitSellingPriceExVAT: {balanceCheck.ProductOfQuantityAndUnitPrice}");


    static List<InvoiceHeaderViewModel> ReadCsvFile(string filePath, IEnumerable<InvoiceHeaderViewModel> existingInvoiceNumbers)
    {
        var invoiceDataList = new List<InvoiceHeaderViewModel>();
        var invoiceHeaders = new List<InvoiceHeaderViewModel>();
        var invoiceLines = new List<InvoiceLineViewModel>();

        invoiceDataList.AddRange(existingInvoiceNumbers);
        existingInvoiceNumbers.ToList().ForEach(x => invoiceLines.AddRange(x.InvoiceLines?.ToList() ?? []));

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
                if (!invoiceDataList.Any(x => x.InvoiceNumber == record.InvoiceNumber))
                {
                    record.InvoiceLines = record.InvoiceLines ?? [];
                    invoiceDataList.Add(record);
                }

                var lineRecord = csv.GetRecord<InvoiceLineViewModel>();
                if (!invoiceLines.Any(x => x.InvoiceNumber == record.InvoiceNumber && x.Description == lineRecord.Description && x.Quantity == lineRecord.Quantity))
                {
                    invoiceLines.Add(lineRecord);
                }
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
                    if (!invoiceLines.Any(x => x.InvoiceNumber == line.InvoiceNumber && x.Description == line.Description && x.Quantity == line.Quantity))
                    {
                        header.InvoiceLines.Add(line);
                    }                   
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