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
using Serilog;
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

    string filePath = config["Settings:LogFilePath"];
    string fileName = config["Settings:LogFileName"];

    Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.RollingFile(Path.Combine(filePath, fileName))
    .CreateLogger();

    Log.Information("InvoicConsoleApp - Started.");

    var invoiceService = serviceProvider.GetRequiredService<IInvoiceConsoleAppService>();
    Log.Information("InvoiceConsoleAppService registered");

    var existingInvoiceNumbers = invoiceService.GetListOfInvoiceNumbers();
    Log.Information("Db call to query existing records to be used for validation duplicates.");

    var invoiceDataList = ReadCsvFile(AppContext.BaseDirectory + "data.csv", existingInvoiceNumbers);

    Log.Information("Data import started..");
    invoiceService.Create(invoiceDataList);
    Log.Information("Data import finished..");

    var invoiceNumberAndQuatityViewModel = invoiceService.GetInvoiceNumberAndSumOfAssociatedLines(invoiceDataList);

    var balanceCheck = invoiceService.GetProductOfQuantityAndUnitSellingPriceExVAT(invoiceDataList);

    Console.WriteLine($"\t\t\tInvoice Number and Total Quantity");
    Console.WriteLine($"===========================================================================================");
    invoiceNumberAndQuatityViewModel?.ToList().ForEach(x => Console.WriteLine($"Invoice Number: {x.InvoiceNumber} and Total Quantity: {x.TotalQuantity}"));
    Console.WriteLine($"===========================================================================================");
    Console.WriteLine($"Sum of all InvoiceHeader.InvoiceTotal: {Math.Round((decimal)balanceCheck.HeaderInvoiceTotal, 2)}");
    Console.WriteLine($"Sum of InvoiceLines.Quantity * InvoiceLines.UnitSellingPriceExVAT: {Math.Round((decimal)balanceCheck.ProductOfQuantityAndUnitPrice, 2)}");


    static List<InvoiceHeaderViewModel> ReadCsvFile(string filePath, IEnumerable<InvoiceHeaderViewModel> existingInvoiceNumbers)
    {
        Log.Information("Reading Csv File..");
        var invoiceDataList = new List<InvoiceHeaderViewModel>();
        var invoiceHeaders = new List<InvoiceHeaderViewModel>();
        var invoiceLines = new List<InvoiceLineViewModel>();

        invoiceDataList.AddRange(existingInvoiceNumbers);
        existingInvoiceNumbers.ToList().ForEach(x => invoiceLines.AddRange(x.InvoiceLines?.ToList() ?? []));

        if (!File.Exists(filePath))
        {
            Log.Error("The specified file does not exist.");
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
                    header.InvoiceLines.Add(line);
                }
            }
        }
        Log.Information("Read Csv File successfully..");

        return invoiceDataList;
    }
}
catch (Exception ex)
{
    Console.WriteLine("An error occured. Please check system logs for further information");
    Log.Error(ex.ToString(), "An error occurred.");
}