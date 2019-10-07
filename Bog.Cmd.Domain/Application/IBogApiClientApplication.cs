namespace Bog.Cmd.Domain.Application
{
    public interface IBogApiClientApplication
    {
        int Execute(params string[] args);
    }
}