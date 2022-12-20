using CSharpFunctionalExtensions;
using SEP.WebShop.Core.Entities;
using SEP.WebShop.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEP.WebShop.Core.Services
{
    public class PaymentService : IService<Payment, Guid>
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IWebShopUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public PaymentService(IPaymentRepository paymentRepository, IWebShopUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _paymentRepository = paymentRepository;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public Result Create(Payment entity)
        {
            if (_paymentRepository.FindById(entity.Id).HasValue)
                return Result.Failure("Payment with specified ID already exists");
            _paymentRepository.Add(entity);
            _unitOfWork.Commmit();
            return Result.Success();
        }

        public Result Delete(Guid id)
        {
            if (_paymentRepository.FindById(id).HasNoValue)
                return Result.Failure("Payment with specified ID does not exist");
            _paymentRepository.Remove(id);
            _unitOfWork.Commmit();
            return Result.Success();
        }

        public Result Update(Payment entity)
        {
            Maybe<Payment> payment = _paymentRepository.FindById(entity.Id);
            if (payment.HasNoValue)
                return Result.Failure("Payment with specified ID does not exist");
            _paymentRepository.Update(payment.Value.Update(entity));
            _unitOfWork.Commmit();
            return Result.Success();
        }
    }
}
