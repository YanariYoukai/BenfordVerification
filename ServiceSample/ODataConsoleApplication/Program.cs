using System;
using System.Net;
using AuthenticationUtility;
using Microsoft.OData.Client;
using ODataUtility.Microsoft.Dynamics.DataEntities;
//using ODataUtility.Connected_Services.OData_Service.Microsoft.Dynamics.DataEntities;

namespace ODataConsoleApplication
{
    class Program
    {
        public static string ODataEntityPath = ClientConfiguration.Default.UriString + "data";

        static void Main(string[] args)
        {
            // To test custom entities, regenerate "ODataClient.tt" file.
            // https://blogs.msdn.microsoft.com/odatateam/2014/03/11/tutorial-sample-how-to-use-odata-client-code-generator-to-generate-client-side-proxy-class/

            
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            Uri oDataUri = new Uri(ODataEntityPath, UriKind.Absolute);
            var context = new Resources(oDataUri);
            

            context.SendingRequest2 += new EventHandler<SendingRequest2EventArgs>(delegate (object sender, SendingRequest2EventArgs e)
            {
                var authenticationHeader = OAuthHelper.GetAuthenticationHeader(useWebAppAuthentication: true);
                e.RequestMessage.SetHeader(OAuthHelper.OAuthHeader, authenticationHeader);
            });


            //TODO: Read OData enity and do some action on it

            //QueryExamples.ReadLegalEntities(context);
            //QueryExamples.ReadSalesOrderLines(context);
            // QueryExamples.CountOrderLines(context);
            //QueryExamples.CountOccurenceOfEachNumber(context);
            QueryExamples.LinqVersion(context);
        }
    }
}
