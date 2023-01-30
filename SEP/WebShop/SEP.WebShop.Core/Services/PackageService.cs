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
    public class PackageService : IService<Package, Guid>
    {
        private readonly IPackageRepository _packageRepository;
        private readonly IUnitOfWork _unitOfWork;

        public PackageService(IPackageRepository packageRepository, IUnitOfWork unitOfWork)
        {
            _packageRepository = packageRepository;
            _unitOfWork = unitOfWork;
        }

        public Result Create(Package entity)
        {
            if (_packageRepository.FindById(entity.Id).HasValue)
                return Result.Failure("Package with specified ID already exists");
            _packageRepository.Add(entity);
            _unitOfWork.Commmit();
            return Result.Success();
        }

        public Result Delete(Guid id)
        {
            if (_packageRepository.FindById(id).HasNoValue)
                return Result.Failure("Package with specified ID does not exist");
            _packageRepository.Remove(id);
            _unitOfWork.Commmit();
            return Result.Success();
        }

        public Result Update(Package entity)
        {
            Maybe<Package> package = _packageRepository.FindById(entity.Id);
            if (package.HasNoValue)
                return Result.Failure("Package with specified ID does not exist");
            _packageRepository.Update(package.Value.Update(entity));
            _unitOfWork.Commmit();
            return Result.Success();
        }

    }
}
