using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationUtility
{
    public partial class ClientConfiguration
    {
        public static ClientConfiguration Default { get { return ClientConfiguration.OneBox; } }

        public static ClientConfiguration OneBox = new ClientConfiguration()
        {
            UriString = "https://msdemod365fo3b81909fd1afb61edevaos.cloudax.dynamics.com/",
            UserName = "nobody@on.the.net",
            Password = "",

           
            ActiveDirectoryResource = "https://msdemod365fo3b81909fd1afb61edevaos.cloudax.dynamics.com", 
            ActiveDirectoryTenant = "https://login.windows.net/autocont.cz", 
            ActiveDirectoryClientAppId = "a3c93dee-9093-4d33-ab2a-c92a9b7c8100",
            ActiveDirectoryClientAppSecret = "DBy7Q~oTrJq7MB4T_VPJPCiozjnnbFj6NNitx",      
            TLSVersion = "1.2",
        };

        public string TLSVersion { get; set; }
        public string UriString { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ActiveDirectoryResource { get; set; }
        public String ActiveDirectoryTenant { get; set; }
        public String ActiveDirectoryClientAppId { get; set; }
        public string ActiveDirectoryClientAppSecret { get; set; }
    }
}
