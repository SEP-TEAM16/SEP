using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SEP.WebShop.Core.Repositories;
using SEP.WebShop.Core.Services;
using SEP.WebShop.Persistence.Repositories;
using SEP.WebShop.Web.DtoFactories;

namespace SEP.WebShop.Web.Controllers
{
    [Route("api/package")]
    [ApiController]
    public class PackageController : ControllerBase
    {
        private readonly ILogger<PackageController> _logger;
        private readonly PackageService _packageService;
        private readonly IPackageRepository _packageRepository;
        private readonly PackageDtoFactory _packageDtoFactory;

        public PackageController(ILogger<PackageController> logger, PackageService packageService, IPackageRepository packageRepository) {
            _logger = _logger;
            _packageService = packageService;
            _packageRepository = packageRepository;
            _packageDtoFactory = new PackageDtoFactory();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetAll() {
            
            return Ok(_packageRepository.FindAll().Select(item => _packageDtoFactory.Create(item)));
        }
    }
}
