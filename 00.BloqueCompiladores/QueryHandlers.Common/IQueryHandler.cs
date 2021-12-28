namespace QueryHandlers.Common
{
    public interface IQueryHandler<in T> where T : QueryParameter
    {
        QueryResult Handle(T parameters);
    }
}
