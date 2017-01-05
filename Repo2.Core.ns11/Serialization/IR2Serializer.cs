namespace Repo2.Core.ns11.Serialization
{
    public interface IR2Serializer
    {
        string  ToString       (object obj);
        T       FromString <T> (string text);
    }
}
