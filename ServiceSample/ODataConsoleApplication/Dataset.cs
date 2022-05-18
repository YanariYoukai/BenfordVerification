using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.OData.Client;
using ODataUtility.Microsoft.Dynamics.DataEntities;

namespace ODataConsoleApplication
{
    public class Dataset
    {
        protected double collectionSize;
        private bool populated;
        protected Dictionary<char, int> data;
        private Dictionary<char, double> expectedValues;
        private Dictionary<char, double> permissibleDeviations;

        public Dataset()
        {
            collectionSize = 0;
            populated = false;
            data = new Dictionary<char, int>()
            {
                {'1', 0},
                {'2', 0},
                {'3', 0},
                {'4', 0},
                {'5', 0},
                {'6', 0},
                {'7', 0},
                {'8', 0},
                {'9', 0}
            };
            expectedValues = new Dictionary<char, double>()
            {
                {'1', 30.1},
                {'2', 17.6},
                {'3', 12.5},
                {'4', 9.7},
                {'5', 7.9},
                {'6', 6.7},
                {'7', 5.8},
                {'8', 5.1},
                {'9', 4.6}
            };
            permissibleDeviations = new Dictionary<char, double>()
            {
                {'1', 1.505},
                {'2', 0.88},
                {'3', 0.625},
                {'4', 0.485},
                {'5', 0.395},
                {'6', 0.335},
                {'7', 0.29},
                {'8', 0.255},
                {'9', 0.23}
            };
        }

        private void PopulateDataset(Resources d365)
        {
            collectionSize = d365.SalesOrderLines.Count();

            //If the size of the dataset is not an exact multiple of 10 000, we need to include an additional one.
            var loopIterations = collectionSize % 10000 == 0 ? collectionSize / 10000 : (collectionSize / 10000) + 1;


            for (int i = 0; i < (int) loopIterations; i++)
            {
                Console.WriteLine("Working on dataset {0} of {1}...", i + 1, (int) loopIterations);
                DataServiceQuery<SalesOrderLine> currentDataSet = d365.SalesOrderLines
                    .AddQueryOption("$skip", i * 10000).AddQueryOption("$top", (i + 1) * 10000);

                var tempDictionary = currentDataSet
                    .Select(line => new {line.LineAmount})
                    .ToArray()
                    .Select(x => x.LineAmount < 0 ? x.LineAmount * -1 : x.LineAmount)
                    .ToArray()
                    .Select(x => x.ToString()[0])
                    .GroupBy(c => c)
                    .Select(y => new {Number = y.Key, Count = y.Count()})
                    .ToDictionary(y => y.Number, y => y.Count);

                foreach (var kvPair in tempDictionary)
                {
                    data[kvPair.Key] += kvPair.Value;
                }
            }
        }

        public void AttemptDatasetCreation(Resources d365)
        {
            //TODO: Look into disabling individual stack traces and only print the exception message after last unsuccessful try, if time permits
            int attempts = 5;

            while (true)
            {
                try
                {
                    PopulateDataset(d365);
                    populated = true;
                    break;
                }
                catch (Exception e)
                {
                    if (--attempts == 0)
                    {
                        Console.WriteLine("Creation of dataset failed due to following exception:");
                        Console.WriteLine(e.Message);
                        break;
                    }
                }
            }
        }

        public void PrintResults()
        {
            if (populated)
            {
                Console.WriteLine("There are {0} entries in the dataset.", collectionSize);
                bool suspiciousFlag = false;
                foreach (var row in data)
                {
                    double actualValue = (data[row.Key] / collectionSize) * 100;
                    if (Math.Abs(expectedValues[row.Key] - actualValue) > permissibleDeviations[row.Key])
                    {
                        suspiciousFlag = true;
                    }

                    Console.WriteLine("Leading number {0} occurs in {1:0.00}% of cases. Expected occurence is {2}%.",
                        row.Key, actualValue, expectedValues[row.Key]);
                }

                Console.WriteLine(suspiciousFlag
                    ? "There is a significant deviation from the expected values. Further investigation is recommended."
                    : "The dataset falls within an acceptable statistical deviation.");
            }
            else
            {
                Console.WriteLine("The dataset is not populated. Use AttemptDatasetCreation() first.");
            }
        }
    }
}