﻿
using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Resin.Api.Client;
using Resin.Api.Client.Domain;

namespace resin
{
    class Program
    {
        static void Main(string[] args)
        {
            string token = GetToken();

            if (!string.IsNullOrWhiteSpace(token))
            {
                TestAsync(token).GetAwaiter().GetResult();
            }
        }

        private static string GetToken()
        {
            string location = System.Reflection.Assembly.GetExecutingAssembly().Location;

            string codeDirectory = Path.GetDirectoryName(location);

            string tokenPath = Path.Combine(codeDirectory,  @"..\..\..\token.txt");

            if (File.Exists(tokenPath))
            {
                return File.ReadAllText(tokenPath);
            }

            Console.WriteLine($"Unable to find token at '{tokenPath}'.");

            return null;
        }

        private static async Task TestAsync(string token)
        {
            var client = new ResinClient(token);

            ResinApplication[] applications = await client.GetApplicationsAsync();
            DisplayObjects(applications, a => a.AppName);

            var devices = await client.GetDevicesAsync();
            DisplayObjects(devices, d => d.Name);

            //await client.RestartAsync(666, 666);
        }

        private static void DisplayObjects<TObject>(TObject[] items, Func<TObject, string> nameFunc)
        {
            var propertyInfos = typeof(TObject).GetProperties();

            foreach (var item in items)
            {
                string name = nameFunc(item);

                Console.WriteLine(name);
                Console.WriteLine("--------------------------------------");

                foreach (var propertyInfo in propertyInfos)
                {
                    string propertyName = propertyInfo.Name;

                    object propertyValue = propertyInfo.GetValue(item);

                    Console.WriteLine($"  {propertyName}: {propertyValue}");
                }

                Console.WriteLine();
            }
        }
    }
}