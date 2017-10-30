using ResinExplorer.Interfaces;

namespace ResinExplorer.Core
{
    public class UserSettings : ISettings
    {
        public bool ShouldRememberToken
        {
            get { return Properties.Settings.Default.ShouldRememberToken; }
            set
            {
                if (Properties.Settings.Default.ShouldRememberToken != value)
                {
                    Properties.Settings.Default.ShouldRememberToken = value;
                    Properties.Settings.Default.Save();
                }
            }
        }

        public string Token
        {
            get
            {
                //var bytes = Encoding.UTF8.GetBytes(Properties.Settings.Default.Token);
                //string base64 = Convert.ToBase64String(bytes);
                return DPAPI.Decrypt(Properties.Settings.Default.Token);
                //return "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpZCI6MjcxNTMsInVzZXJuYW1lIjoibHVib21pcl9sYXNraSIsImVtYWlsIjoibHVib21pci5sYXNraUBzZGUuY3oiLCJjcmVhdGVkX2F0IjoiMjAxNy0xMC0yNVQxMDoxNDoxOC41ODJaIiwiZmlyc3RfbmFtZSI6Ikx1Ym9taXIiLCJsYXN0X25hbWUiOiJMYXNraSIsImNvbXBhbnkiOiIiLCJhY2NvdW50X3R5cGUiOiJwcm9mZXNzaW9uYWwiLCJqd3Rfc2VjcmV0IjoiSElSUURSNDdRSU1YN1FGU1hKSFFWNExXQk4zRE1BNUIiLCJoYXNfZGlzYWJsZWRfbmV3c2xldHRlciI6dHJ1ZSwic29jaWFsX3NlcnZpY2VfYWNjb3VudCI6W10sImhhc1Bhc3N3b3JkU2V0Ijp0cnVlLCJuZWVkc1Bhc3N3b3JkUmVzZXQiOmZhbHNlLCJwdWJsaWNfa2V5Ijp0cnVlLCJmZWF0dXJlcyI6W10sImludGVyY29tVXNlck5hbWUiOiJsdWJvbWlyX2xhc2tpIiwiaW50ZXJjb21Vc2VySGFzaCI6Ijc2YmQ2MjkxNTMxMzQ4YzM2NzgxZWI2MTA5MmE5NzI4YTQ1Yzk0MmE0N2YyOWYwOGYyN2I0ZDA5MmQ1Njc3YzgiLCJwZXJtaXNzaW9ucyI6W10sImF1dGhUaW1lIjoxNTA4OTI2NDc2MzM2LCJhY3RvciI6MTk4Njk1MSwiaWF0IjoxNTA5MDE2NDI2LCJleHAiOjE1MDk2MjEyMjZ9.xAoFZHadqcGix36PTjgsrgAkYlxH03PTdSujMq8fOpc";
            }
            set
            {

                //var bytes = Encoding.UTF8.GetBytes(value);
                //string base64 = Convert.ToBase64String(bytes);
                //Properties.Settings.Default.Token = DPAPI.Encrypt(base64);
                Properties.Settings.Default.Token = DPAPI.Encrypt(value);
                Properties.Settings.Default.Save();
            }
        }
    }
}
