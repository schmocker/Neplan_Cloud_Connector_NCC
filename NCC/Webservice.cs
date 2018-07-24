using System;
using System.Text;
using System.Security.Cryptography;
using System.ServiceModel;
using System.IO;

namespace NCC
{
    // create the class "WebService" and inherit from class "NeplanServiceClient"
    public class WebService : NeplanServiceClient
    {
        public NeplanServiceClient nepService;

        // constructor method for class "WebService"
        public WebService(String ws_adress, String username, String password, int maxReturnSize)
            // instantiation of the super class "WebService"
            // - calls private method GetBinding to get a valid WSHttpBinding
            //   with the given maxReturnSize for requests
            // - instantiats an new EndpointAddress with the given URL of ws_adress
            : base(GetBinding(maxReturnSize), new EndpointAddress(ws_adress))
        {
            // set the username
            this.ClientCredentials.UserName.UserName = username;
            // hash and set the username, use the getM5Hash method provided by NEPLAN
            this.ClientCredentials.UserName.Password = getMd5Hash(password);
        }

        // GetBinding method to create the WSHttpBinding to instantiat the super
        // class "NeplanServiceClient"
        private static WSHttpBinding GetBinding(int maxReturnSize)
        {
            // create a new WSHttpBinding and set the required name
            WSHttpBinding binding = new WSHttpBinding();
            binding.Name = "WSHttpBinding_NeplanService";

            // set some variables to match the NEPLAN conditions
            binding.Security.Mode = SecurityMode.TransportWithMessageCredential;
            binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;
            binding.Security.Message.ClientCredentialType = MessageCredentialType.UserName;

            // set the MaxReceivedMessageSize to the given maxReturnSize
            binding.MaxReceivedMessageSize = maxReturnSize;

            // set the ReceiveTimeout and SendTimeout to 20 minutes
            // - could be implemented the same way as the maxReturnSize
            // default is 1 minute, which is not always enough
            System.TimeSpan ts = new System.TimeSpan(0, 20, 0);
            binding.ReceiveTimeout = ts;
            binding.SendTimeout = ts;

            // return the created WSHttpBinding
            return binding;
        }

        // optional: helper method to upload nepmeas xml files
        public bool UploadXML(ExternalProject project, string measurements, string path)
        {
            // create a new FileStream with the given path
            Stream fs = new FileStream(path, FileMode.Open, FileAccess.Read);

            // create a new uploadName using the provided method NepMeasUpload
            // from the super class
            string uploadName = NepMeasUpload(fs);

            // upload the nepmeas files to neplan cloud using the provided
            // method ImportMeasuredDataFromXMlFile from the super class
            Boolean x = ImportMeasuredDataFromXMlFile(project, uploadName, measurements);

            // close the FileStream
            fs.Close();

            // return the returned Boolean
            return x;
        }
        // more helper methods could be implemented

        // getM5Hash method provided by NEPLAN to hash passwords
        private static string getMd5Hash(string input)
        {
            #region Not important for the training
            // Create a new instance of the MD5CryptoServiceProvider object.
            MD5 md5Hasher = MD5.Create();

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
                sBuilder.Append(data[i].ToString("x2"));

            // Return the hexadecimal string.
            return sBuilder.ToString();
            #endregion
        }

    }
}
