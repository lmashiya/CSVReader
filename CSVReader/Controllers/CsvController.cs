using Microsoft.AspNetCore.Mvc;
using CSVReader.Models;
using CSVReader.ViewModels;

namespace CSVReader.Controllers;

public class CsvController : Controller
{
    [HttpGet]
    public IActionResult Upload()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Upload(IFormFile? file)
    {
        if (file == null || file.Length == 0)
        {
            TempData["Message"] = "Please select a valid CSV file.";
            return RedirectToAction("Upload");
        }

        var records = new List<CsvData>();
        try
        {
            using var stream = new StreamReader(file.OpenReadStream());
            var lines = stream.ReadToEnd()
                .Split("\n", StringSplitOptions.RemoveEmptyEntries)
                .Skip(1);

            foreach (var line in lines)
            {
                var columns = line.Split(',');
                if (columns.Length < 4) continue;

                records.Add(new CsvData
                {
                    FirstName = columns[0].Trim(),
                    LastName = columns[1].Trim(),
                    Address = columns[2].Trim(),
                    PhoneNumber = columns[3].Trim()
                });
            }
        }
        catch
        {
            TempData["Message"] = "An error occurred while processing the file.";
            return RedirectToAction("Upload");
        }

        var viewModel = new CsvResultsViewModel
        {
            GroupedLastNames = records
                .GroupBy(r => r.LastName)
                .Select(g => new GroupedLastName
                {
                    LastName = g.Key,
                    Count = g.Count()
                })
                .OrderByDescending(g => g.Count)
                .ThenBy(g => g.LastName)
                .ToList(),
            UniqueAddresses = records
                .Select(r => r.Address)
                .Distinct()
                .ToList()
        };

        return View("Results", viewModel);
    }

}