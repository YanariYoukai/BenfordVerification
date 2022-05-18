using Microsoft.OData.Client;
using ODataUtility.Microsoft.Dynamics.DataEntities;
//using ODataUtility.Connected_Services.OData_Service.Microsoft.Dynamics.DataEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ODataConsoleApplication
{
    public static class QueryExamples
    {
        public static void ReadLegalEntities(Resources d365)
        {
            foreach (var legalEntity in d365.LegalEntities.AsEnumerable())
            {
                Console.WriteLine("Name: {0}", legalEntity.Name);
            }
        }

        public static void ReadSalesOrderLines(Resources d365)
        {
            foreach (var salesLine in d365.SalesOrderLines)
            {
                Console.WriteLine(salesLine.LineAmount);
            }
        }

        public static void CountOrderLines(Resources d365)
        {
            Console.WriteLine("There are {0} total sales records.", d365.SalesOrderLines.Count());
        }

        public static void CountOccurenceOfEachNumber(Resources d365)
        {
            /* Very primitive first working draft. The number of iterations the loop is meant to run for is correct, but the connection eventually times out.
             * When the number of iterations is lower, it runs correctly. Hopefully this issue gets fixed once the logic gets transferred to LINQ.*/

            int ones = 0;
            int twos = 0;
            int threes = 0;
            int fours = 0;
            int fives = 0;
            int sixes = 0;
            int sevens = 0;
            int eights = 0;
            int nines = 0;
            int total = 0;

            int collectionSize = d365.SalesOrderLines.Count();
            int loopIterations = collectionSize % 10000 == 0 ? collectionSize / 10000 : (collectionSize / 10000) + 1;

            for (int i = 0; i < 6; i++)
            {
                DataServiceQuery<SalesOrderLine> currentDataSet = d365.SalesOrderLines
                    .AddQueryOption("$skip", i * 10000).AddQueryOption("$top", (i + 1) * 10000);


                foreach (var line in currentDataSet)
                {
                    var firstNumber = line.LineAmount < 0 ? line.LineAmount * -1 : line.LineAmount;
                    switch (firstNumber.ToString()[0])
                    {
                        case '1':
                            ones++;
                            total++;
                            break;
                        case '2':
                            twos++;
                            total++;
                            break;
                        case '3':
                            threes++;
                            total++;
                            break;
                        case '4':
                            fours++;
                            total++;
                            break;
                        case '5':
                            fives++;
                            total++;
                            break;
                        case '6':
                            sixes++;
                            total++;
                            break;
                        case '7':
                            sevens++;
                            total++;
                            break;
                        case '8':
                            eights++;
                            total++;
                            break;
                        case '9':
                            nines++;
                            total++;
                            break;
                        default:
                            continue;
                    }
                }
            }

            Console.WriteLine(
                "There are {0} ones, {1} twos, {2} threes, {3} fours, {4} fives, {5} sixes, {6} sevens, {7} eights, and {8} nines for a total of {9} at the start of each line.",
                ones, twos, threes, fours, fives, sixes, sevens, eights, nines, total);
        }

        public static void LinqVersion(Resources d365)
        {
            Dictionary<char, int> result = new Dictionary<char, int>()
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
            int collectionSize = d365.SalesOrderLines.Count();
            int loopIterations = collectionSize % 10000 == 0 ? collectionSize / 10000 : (collectionSize / 10000) + 1;

            for (int i = 0; i < loopIterations; i++)
            {
                DataServiceQuery<SalesOrderLine> currentDataSet = d365.SalesOrderLines
                    .AddQueryOption("$skip", i * 10000).AddQueryOption("$top", (i + 1) * 10000);

                var input = currentDataSet
                    .Select(line => new {line.LineAmount})
                    .ToArray();

                var tempDictionary = input
                    .Select(x => x.LineAmount < 0 ? x.LineAmount * -1 : x.LineAmount)
                    .ToArray()
                    .Select(x => x.ToString()[0])
                    .GroupBy(c => c)
                    .Select(y => new {Number = y.Key, Count = y.Count()})
                    .ToDictionary(y => y.Number, y => y.Count);

                foreach (var kvPair in tempDictionary)
                {
                    result[kvPair.Key] += kvPair.Value;
                }
            }

            foreach (var row in result)
            {
                Console.WriteLine($"Key {row.Key} occured {row.Value} times as the leading digit.");
            }
        }
    }
}