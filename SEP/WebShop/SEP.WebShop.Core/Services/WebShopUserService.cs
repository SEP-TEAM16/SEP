using CSharpFunctionalExtensions;
using SEP.WebShop.Core.Entities;
using SEP.WebShop.Core.Repositories;

namespace SEP.WebShop.Core.Services
{
    public class WebShopUserService : IService<WebShopUser, Guid>
    {
        private readonly IWebShopUserRepository _webShopUserRepository;
        private readonly IUnitOfWork _unitOfWork;

        public WebShopUserService(IWebShopUserRepository webShopUserRepository, IUnitOfWork unitOfWork)
        {
            _webShopUserRepository = webShopUserRepository;
            _unitOfWork = unitOfWork;
        }

        public Result Create(WebShopUser entity)
        {
            if (_webShopUserRepository.FindById(entity.Id).HasValue)
                return Result.Failure("User with specified ID already exists");
            if (_webShopUserRepository.FindByUsername(entity.Username).HasValue)
                return Result.Failure("User with specified username already exists");
            _webShopUserRepository.Add(entity);
            _unitOfWork.Commmit();
            return Result.Success();
        }

        public Result Delete(Guid id)
        {
            if (_webShopUserRepository.FindById(id).HasNoValue)
                return Result.Failure("User with specified ID does not exist");
            _webShopUserRepository.Remove(id);
            _unitOfWork.Commmit();
            return Result.Success();
        }

        public Result Update(WebShopUser entity)
        {
            Maybe<WebShopUser> webShopUser = _webShopUserRepository.FindById(entity.Id);
            if (webShopUser.HasNoValue)
                return Result.Failure("User with specified ID does not exist");
            _webShopUserRepository.Update(webShopUser.Value.Update(entity));
            _unitOfWork.Commmit();
            return Result.Success();
        }
    }
}
