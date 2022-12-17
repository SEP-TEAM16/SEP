using CSharpFunctionalExtensions;
using SEP.WebShop.Core.Entities;
using SEP.WebShop.Core.Repositories;

namespace SEP.WebShop.Core.Services
{
    public class SubscriptionOptionService : IService<SubscriptionOption, Guid>
    {
        private readonly ISubscriptionOptionRepository _subscriptionOptionRepository;
        private readonly IUnitOfWork _unitOfWork;

        public SubscriptionOptionService(ISubscriptionOptionRepository subscriptionOptionRepository, IUnitOfWork unitOfWork)
        {
            _subscriptionOptionRepository = subscriptionOptionRepository;
            _unitOfWork = unitOfWork;
        }
        public Result Create(SubscriptionOption entity)
        {
            if (_subscriptionOptionRepository.FindById(entity.Id).HasValue)
                return Result.Failure("Subscription option with specified ID already exists");
            _subscriptionOptionRepository.Add(entity);
            _unitOfWork.Commmit();
            return Result.Success();
        }

        public Result Delete(Guid id)
        {
            if (_subscriptionOptionRepository.FindById(id).HasNoValue)
                return Result.Failure("Subscription option with specified ID does not exist");
            _subscriptionOptionRepository.Remove(id);
            _unitOfWork.Commmit();
            return Result.Success();
        }

        public Result Update(SubscriptionOption entity)
        {
            Maybe<SubscriptionOption> subscriptionOption = _subscriptionOptionRepository.FindById(entity.Id);
            if (subscriptionOption.HasNoValue)
                return Result.Failure("Subscription option with specified ID does not exist");
            _subscriptionOptionRepository.Update(subscriptionOption.Value.Update(entity));
            _unitOfWork.Commmit();
            return Result.Success();
        }
    }
}
