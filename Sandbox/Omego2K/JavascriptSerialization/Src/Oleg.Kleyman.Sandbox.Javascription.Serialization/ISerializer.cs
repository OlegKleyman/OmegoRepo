namespace Oleg.Kleyman.Sandbox.Javascription.Serialization
{
    public interface ISerializer
    {
        string Serialize<T>(T target);
        T Deserialize<T>(string json);
    }
}