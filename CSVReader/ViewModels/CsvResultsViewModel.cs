using System.Collections.Generic;

namespace CSVReader.ViewModels;
    public class CsvResultsViewModel
    {
        public List<GroupedLastName> GroupedLastNames { get; set; }
        public List<string> UniqueAddresses { get; set; }
    }

    public class GroupedLastName
    {
        public string LastName { get; set; }
        public int Count { get; set; }
    }
