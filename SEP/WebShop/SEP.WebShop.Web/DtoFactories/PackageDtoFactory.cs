using SEP.WebShop.Core.Entities;
using SEP.WebShop.Web.Dto;

namespace SEP.WebShop.Web.DtoFactories
{
    public class PackageDtoFactory
    {
        public PackageDto Create(Package package) {
            return new PackageDto(package.Id, package.Name, package.Currency, package.Price);
        }
    }
}
