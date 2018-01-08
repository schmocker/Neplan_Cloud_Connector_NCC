using NeplanMqttService.NeplanService;
using System.Security.Cryptography;
using System.Xml;
using System.Globalization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeplanMqttService
{
    class Webservice
    {

        public NeplanServiceClient nepService;
        public ExternalProject ext;

        public Webservice(string username, string password, string project)
        {
            nepService = new NeplanServiceClient();                                 //instantiates the neplaservice
            nepService.ClientCredentials.UserName.UserName = username;              //give the username       
            nepService.ClientCredentials.UserName.Password = getMd5Hash(password);  //give the password
            try
            {
                nepService.Open();                              //open the service
                Console.WriteLine("Opened service");
                ext = nepService.GetProject(project, null, null, null);   //get the project             
                if (ext != null)
                    Console.WriteLine("Got project");
                else
                    Console.WriteLine("Cannot get project");

            }
            catch
            {
                Console.WriteLine("Cannot open service");
            }
        }

        public void CloseWebservice()
        {
            try
            {
                nepService.Close();         //close the service
                Console.WriteLine("Closed service");
            }
            catch
            {
                Console.WriteLine("Cannot close service");
            }
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
