using CSharpFunctionalExtensions;

namespace SEP.WebShop.Core.Repositories
{
    public interface IRepository<T, ID> where T : class where ID : IComparable
    {
        IEnumerable<T> FindAll();

        Maybe<T> FindById(ID id);

        void Update(T entity);

        void Remove(ID id);

        void Add(T entity);
    }
}
