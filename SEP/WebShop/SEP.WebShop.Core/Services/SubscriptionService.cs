using CSharpFunctionalExtensions;
using SEP.WebShop.Core.Entities;
using SEP.WebShop.Core.Repositories;

namespace SEP.WebShop.Core.Services
{
    public class SubscriptionService : IService<Subscription, Guid>
    {
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly IUnitOfWork _unitOfWork;

        public SubscriptionService(ISubscriptionRepository subscriptionOptionRepository, IUnitOfWork unitOfWork)
        {
            _subscriptionRepository = subscriptionOptionRepository;
            _unitOfWork = unitOfWork;
        }
        public Result Create(Subscription entity)
        {
            if (_subscriptionRepository.FindById(entity.Id).HasValue)
                return Result.Failure("Subscription with specified ID already exists");
            _subscriptionRepository.Add(entity);
            _unitOfWork.Commmit();
            return Result.Success();
        }

        public Result Delete(Guid id)
        {
            if (_subscriptionRepository.FindById(id).HasNoValue)
                return Result.Failure("Subscription with specified ID does not exist");
            _subscriptionRepository.Remove(id);
            _unitOfWork.Commmit();
            return Result.Success();
        }

        public Result Update(Subscription entity)
        {
            Maybe<Subscription> subscription = _subscriptionRepository.FindById(entity.Id);
            if (subscription.HasNoValue)
                return Result.Failure("Subscription with specified ID does not exist");
            _subscriptionRepository.Update(subscription.Value.Update(entity));
            _unitOfWork.Commmit();
            return Result.Success();
        }
    }
}
