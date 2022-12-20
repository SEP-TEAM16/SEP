using CSharpFunctionalExtensions;
using SEP.WebShop.Core.Entities;
using SEP.WebShop.Core.Entities.Enums;
using SEP.WebShop.Core.Repositories;

namespace SEP.WebShop.Core.Services
{
    public class SubscriptionService : IService<Subscription, Guid>
    {
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly ISubscriptionOptionRepository _subscriptionOptionRepository;
        private readonly IWebShopUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public SubscriptionService(ISubscriptionRepository subscriptionRepository, ISubscriptionOptionRepository subscriptionOptionRepository, IWebShopUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _subscriptionRepository = subscriptionRepository;
            _subscriptionOptionRepository = subscriptionOptionRepository;
            _userRepository = userRepository;
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

        public Result MakeSubscription(SubscriptionOption subscriptionOption, Company company)
        {
            if (_subscriptionOptionRepository.FindById(subscriptionOption.Id).HasNoValue)
                return Result.Failure("Subscription option with ID specified doesn't exist");
            if (_userRepository.FindById(company.Id).HasNoValue)
                return Result.Failure("Company with ID specified doesn't exist");
            DateTime expirationDateTime = 
                (subscriptionOption.SubscriptionType == SubscriptionType.annual) ? DateTime.Now.AddYears(1) : DateTime.Now.AddMonths(1);
            Result<Subscription> subscriptionResult = Subscription.Create(Guid.NewGuid(), expirationDateTime, SubscriptionStatus.created,  subscriptionOption, company);
            if (subscriptionResult.IsFailure)
                return Result.Failure(subscriptionResult.Error);
            _subscriptionRepository.Add(subscriptionResult.Value);
            _unitOfWork.Commmit();
            return Result.Success();
        }
    }
}
