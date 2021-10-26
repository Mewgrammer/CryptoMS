namespace Shared.Models;

public class ServerConfiguration
{
    public string ServiceName { get; set; }
    public string Protocol { get; set; } = "http";
    public string Host { get; set; } = "localhost";
    public int Port { get; set; } = 5000;

    public string Url => $"{Protocol}://{Host}:{Port}";
}