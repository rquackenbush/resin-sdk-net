namespace ResinExplorer.Interfaces
{
    public interface ISettings
    {
        bool ShouldRememberToken { get; set; }

        string Token { get; set; }
    }
}
