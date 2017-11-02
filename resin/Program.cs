namespace resin
{
    class Program
    {
        static void Main(string[] args)
        {
            //string token = GetToken();
            //string token = @"eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpZCI6MjcxNTMsInVzZXJuYW1lIjoibHVib21pcl9sYXNraSIsImVtYWlsIjoibHVib21pci5sYXNraUBzZGUuY3oiLCJjcmVhdGVkX2F0IjoiMjAxNy0xMC0yNVQxMDoxNDoxOC41ODJaIiwiZmlyc3RfbmFtZSI6Ikx1Ym9taXIiLCJsYXN0X25hbWUiOiJMYXNraSIsImNvbXBhbnkiOiIiLCJhY2NvdW50X3R5cGUiOiJwcm9mZXNzaW9uYWwiLCJqd3Rfc2VjcmV0IjoiSElSUURSNDdRSU1YN1FGU1hKSFFWNExXQk4zRE1BNUIiLCJoYXNfZGlzYWJsZWRfbmV3c2xldHRlciI6dHJ1ZSwic29jaWFsX3NlcnZpY2VfYWNjb3VudCI6W10sImhhc1Bhc3N3b3JkU2V0Ijp0cnVlLCJuZWVkc1Bhc3N3b3JkUmVzZXQiOmZhbHNlLCJwdWJsaWNfa2V5Ijp0cnVlLCJmZWF0dXJlcyI6W10sImludGVyY29tVXNlck5hbWUiOiJsdWJvbWlyX2xhc2tpIiwiaW50ZXJjb21Vc2VySGFzaCI6Ijc2YmQ2MjkxNTMxMzQ4YzM2NzgxZWI2MTA5MmE5NzI4YTQ1Yzk0MmE0N2YyOWYwOGYyN2I0ZDA5MmQ1Njc3YzgiLCJwZXJtaXNzaW9ucyI6W10sImF1dGhUaW1lIjoxNTA4OTI2NDc2MzM2LCJhY3RvciI6MTk4Njk1MSwiaWF0IjoxNTA5MzgxNjYxLCJleHAiOjE1MDk5ODY0NjF9.hfThhjaWOTtFGvYrC64WHk_4Ito_oSfLpnj1Su_dLMg";

            //if (!string.IsNullOrWhiteSpace(token))
            //{
            //    TestAsync(token).GetAwaiter().GetResult();
            //}
        }

        //private static string GetToken()
        //{
        //    string location = System.Reflection.Assembly.GetExecutingAssembly().Location;

        //    string codeDirectory = Path.GetDirectoryName(location);

        //    string tokenPath = Path.Combine(codeDirectory, @"..\..\..\token.txt");

        //    if (File.Exists(tokenPath))
        //    {
        //        return File.ReadAllText(tokenPath);
        //    }

        //    Console.WriteLine($"Unable to find token at '{tokenPath}'.");

        //    return null;
        //}

        //private static async Task TestAsync(string token)
        //{
        //    var tokenProvider = new SimpleTokenProvider(token);

        //    var client = new ResinApiClient(tokenProvider);

        //    //var user = await client.GetUserAsync();

        //    //Console.WriteLine(user.Id);

        //    //await client.CreateApplicationAsync("testing2", "artik10");

        //    ResinApplication[] applications = await client.GetApplicationsAsync();
        //    //DisplayObjects(applications, a => a.AppName);

        //    //var stagingApplication = await client.GetApplicationAsync("ScadaStaging");

        //    //string key = await client.GetProvisioningKeyAsync(stagingApplication.Id);

        //    //Console.WriteLine(key);

        //    string uuid = Guid.NewGuid().ToString("N");

        //    var deviceId = await client.RegisterDeviceAsync(applications[0].Id, uuid);

        //    //Console.WriteLine($"DeviceId: {deviceId}");



        //    //int applicationId = applications[0].Id;

        //    //var variables = await client.GetApplicationEnvironmentVariablesAsync(applicationId);

        //    var devices = await client.GetDevicesAsync();

        //    foreach (var device in devices)
        //    {
        //        Console.WriteLine(device.Name);

        //        //var application = await device.Application.GetAsync();

        //        //Console.WriteLine($"Device {device.Id}: {application.AppName}");

        //        // var user = await device.User.GetAsync();

        //        Console.WriteLine($"  user: [{user.Id}] {user.Username}");


        //        var user2 = await client.GetUserAsync(user.Id);

        //        Console.WriteLine($" user2: {user2.Username}");

        //        //var application2 = await client.GetApplicationAsync(application.Id);

        //        //                Console.WriteLine($" application2: {application2.AppName}");

        //        //var childDevices = await application.Devices.GetAsync();

        //        //foreach (var childDevice in childDevices)
        //        //{
        //        //    Console.WriteLine($"     {childDevice.Name}");
        //        //}

        //    }

        //    //DisplayObjects(devices, d => d.Id.ToString());

        //    //int deviceId = devices[0].Id;

        //    //string status = await client.GetStatusAsync(deviceId);

        //    //Console.WriteLine(status);

        //    //await client.AddNoteAsync(deviceId, "Hello!!!!!!!!!!");

        //    //var variables = await client.GetDeviceEnvironmentalVariablesAsync(deviceId);

        //    //foreach (var variable in variables)
        //    //{
        //    //   Console.WriteLine($" {variable.Name}: {variable.Value}");
        //    //}

        //    //var devices = await client.GetDevicesAsync();
        //    //DisplayObjects(devices, d => d.Name);

        //    //await client.RestartDeviceAsync(666, 666);
        //}

        ////private static void DisplayObjects<TObject>(TObject[] items, Func<TObject, string> nameFunc)
        ////{
        ////    var propertyInfos = typeof(TObject).GetProperties();

        ////    foreach (var item in items)
        ////    {
        ////        string name = nameFunc(item);

        ////        Console.WriteLine(name);
        ////        Console.WriteLine("--------------------------------------");

        ////        foreach (var propertyInfo in propertyInfos)
        ////        {
        ////            string propertyName = propertyInfo.Name;

        ////            object propertyValue = propertyInfo.GetValue(item);

        ////            Console.WriteLine($"  {propertyName}: {propertyValue}");
        ////        }

        ////        Console.WriteLine();
        ////    }
        ////}
    }
}
