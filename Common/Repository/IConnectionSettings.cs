namespace Common.Repository
{
    public interface IConnectionSettings
    {
        string Alias { get; set; }

        string ConfigFile { get; set; }

        string ConnectionString { get; set; }

        string Password { get; set; }

        string User { get; set; }
    }
}