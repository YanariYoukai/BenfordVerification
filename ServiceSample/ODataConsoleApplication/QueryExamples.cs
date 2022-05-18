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
             * When the number of iterations is lower, it runs correctly.*/ 

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
                DataServiceQuery <SalesOrderLine> currentDataSet = d365.SalesOrderLines.AddQueryOption("$skip", i * 10000).AddQueryOption("$top", (i + 1) * 10000);


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
            Console.WriteLine("There are {0} ones, {1} twos, {2} threes, {3} fours, {4} fives, {5} sixes, {6} sevens, {7} eights, and {8} nines for a total of {9} at the start of each line.", ones,twos,threes, fours, fives,sixes,sevens,eights,nines, total);
        }


        public static void GetInlineQueryCount(Resources d365)
        {
            var vendorsQuery = d365.Vendors.IncludeCount();
            var vendors = vendorsQuery.Execute() as QueryOperationResponse<Vendor>;

            Console.WriteLine("Total vendors is {0}", vendors.Count);
        }

        public static void GetTopRecords(Resources d365)
        {
            var vendorsQuery = d365.Vendors.AddQueryOption("$top", "10");
            var vendors = vendorsQuery.Execute() as QueryOperationResponse<Vendor>;

            foreach (var vendor in vendors)
            {
                Console.WriteLine("Vendor with ID {0} retrived.", vendor.VendorAccountNumber);
            }
        }

        public static void FilterLinqSyntax(Resources d365)
        {
            var vendors = d365.Vendors.Where(x => x.VendorAccountNumber == "1001");

            foreach (var vendor in vendors)
            {
                Console.WriteLine("Vendor with ID {0} retrived.", vendor.VendorAccountNumber);
            }
        }

        public static void FilterSyntax(Resources d365)
        {
            var vendors = d365.Vendors.AddQueryOption("$filter", "VendorAccountNumber eq '1001'").Execute();

            foreach (var vendor in vendors)
            {
                Console.WriteLine("Vendor with ID {0} retrived.", vendor.VendorAccountNumber);
            }
        }

        public static void SortSyntax(Resources d365)
        {
            var vendors = d365.Vendors.OrderBy(x => x.VendorAccountNumber);

            foreach (var vendor in vendors)
            {
                Console.WriteLine("Vendor with ID {0} retrived.", vendor.VendorAccountNumber);
            }
        }


        public static void FilterByCompany(Resources d365)
        {
            var vendors = d365.Vendors.Where(x => x.DataAreaId == "USMF");

            foreach (var vendor in vendors)
            {
                Console.WriteLine("Vendor with ID {0} retrived.", vendor.VendorAccountNumber);
            }
        }

        public static void ExpandNavigationalProperty(Resources d365Client)
        {
            var salesOrdersWithLines = d365Client.SalesOrderHeaders.Expand("SalesOrderLine").Where(x => x.SalesOrderNumber == "012518").Take(5);

            foreach (var salesOrder in salesOrdersWithLines)
            {
                Console.WriteLine(string.Format("Sales order ID is {0}", salesOrder.SalesOrderNumber));

                foreach (var salesLine in salesOrder.SalesOrderLine)
                {
                    Console.WriteLine(string.Format("Sales order line with description {0} contains item id {1}", salesLine.LineDescription, salesLine.ItemNumber));
                }
            }
        }

        public static void FilterOnNavigationalProperty(Resources d365Client)
        {
            var salesOrderLines = d365Client.SalesOrderLines.Where(x => x.SalesOrderHeader.SalesOrderStatus == SalesStatus.Invoiced);

            foreach (var salesOrderLine in salesOrderLines)
            {
                Console.WriteLine(salesOrderLine.ItemNumber);
            }

        }
    }
}