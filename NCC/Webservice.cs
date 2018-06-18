using System;
using System.Text;
using System.Security.Cryptography;
using System.ServiceModel;
using System.IO;

namespace NCC
{
    public class WebService : NeplanServiceClient
    {
        public NeplanServiceClient nepService;

        public WebService(String ws_adress, String username, String password)
            : base(GetBinding(), new EndpointAddress(ws_adress))
        {
            this.ClientCredentials.UserName.UserName = username;
            this.ClientCredentials.UserName.Password = getMd5Hash(password);
        }

        private static WSHttpBinding GetBinding()
        {
            WSHttpBinding binding = new WSHttpBinding();
            binding.Name = "WSHttpBinding_NeplanService";
            binding.Security.Mode = SecurityMode.TransportWithMessageCredential;
            binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;
            binding.Security.Message.ClientCredentialType = MessageCredentialType.UserName;
            binding.MaxReceivedMessageSize = 65536 * 100;
            return binding;
        }

        public bool UploadXML(ExternalProject project, string measurements, string path)
        {
            Stream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            string uploadName = NepMeasUpload(fs);

            Boolean x = ImportMeasuredDataFromXMlFile(project, uploadName, measurements);
            fs.Close();
            return x;
        }

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
