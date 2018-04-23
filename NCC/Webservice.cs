using System;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Cryptography;
using System.ServiceModel;
using System.Text;

namespace NCC
{
    public class Webservice : NeplanServiceClient
    {

        public NeplanServiceClient nepService;

        public Webservice(String ws_adress, String username, String password)
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
            return binding;
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
        public Dictionary<string, Object> GetMethodSpecifications(string MethodName)
        {
            MethodInfo method = this.GetType().GetMethod(MethodName);
            ParameterInfo[] parameters = method.GetParameters();

            Dictionary<string, Object> mehodSpecification = new Dictionary<string, Object>();

            Dictionary<string, Object>[] Inputs = new Dictionary<String, Object>[parameters.Length];
            for (int i = 0; i < parameters.Length; i++)
            {
                Inputs[i] = new Dictionary<string, Object>();
                Inputs[i].Add("Name", parameters[i].Name);
                Inputs[i].Add("Type", parameters[i].ParameterType.ToString());
            }
            mehodSpecification.Add("Inputs", Inputs);
            mehodSpecification.Add("Output", method.ReturnType.ToString());
            return mehodSpecification;
        }
    }
}
