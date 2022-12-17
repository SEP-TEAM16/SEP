using CSharpFunctionalExtensions;

namespace SEP.WebShop.Core.Services
{
    public interface IService<T, ID> where T : class where ID : IComparable
    {
        Result Create(T entity);
        Result Update(T entity);
        Result Delete(ID id);

    }
}
