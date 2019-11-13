using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace SPMBServices
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IService1
    {
        // TODO: Add your service operations here
        [OperationContract]
        [WebInvoke(
            Method = "POST",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "daftar?username={username}&password={password}&email={email}&nohp={nohp}"
        )]
        void Daftar(string username, string password, string email, string nohp);
    }

}
