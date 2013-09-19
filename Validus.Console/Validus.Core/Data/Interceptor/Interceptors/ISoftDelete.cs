namespace Validus.Core.Data.Interceptor.Interceptors
{
    public interface ISoftDelete
    {
        bool IsDeleted { get; set; }
    }
}
