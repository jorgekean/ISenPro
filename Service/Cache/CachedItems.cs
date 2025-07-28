using EF.Models.UserManagement;
using Service.Dto.SystemSetup;
using Service.SystemSetup.Interface;
using Service.UserManagement.Interface;

namespace Service.Cache
{
    public class CachedItems
    {
        private readonly IGenericCacheService _cacheService;
        private readonly IPSDBMCatalogueService _psdbmCatalogueService;
        private readonly ISupplementaryCatalogueService _supplementaryCatalogueService;
        private readonly IAccountCodeService _accountCodeService;
        private readonly IRoleService _roleService;
        private readonly IDepartmentService _departmentService;
        private readonly int _expirationHours = 24;

        public CachedItems(IGenericCacheService cacheService,
            IPSDBMCatalogueService catalogueService,
            ISupplementaryCatalogueService supplementaryCatalogueService,
            IAccountCodeService accountCodeService,
            IRoleService roleService,
            IDepartmentService departmentService)
        {
            _cacheService = cacheService;
            _psdbmCatalogueService = catalogueService;
            _supplementaryCatalogueService = supplementaryCatalogueService;
            _roleService = roleService;
            _accountCodeService = accountCodeService;
            _departmentService = departmentService;
        }

        public Task<List<PSDBMCatalogueDto>> GetPSDBMCatalogues(int year) =>
            _cacheService.GetOrCreateAsync($"CachedPSDBMCatalogue_{year}", async () =>
                (await _psdbmCatalogueService.GetAllCurrent(year)).ToList(), TimeSpan.FromHours(_expirationHours));

        public Task<List<SupplementaryCatalogueDto>> GetSupplementaryCatalogues(int year) =>
           _cacheService.GetOrCreateAsync($"CachedSupplementaryCatalogue_{year}", async () =>
               (await _supplementaryCatalogueService.GetAllCurrent(year)).ToList(), TimeSpan.FromHours(_expirationHours));

        public Task<List<AccountCodeDto>> AccountCodes =>
          _cacheService.GetOrCreateAsync("CachedAccountCodes", async () =>
              (await _accountCodeService.GetAllAsync()).ToList(), TimeSpan.FromHours(_expirationHours));


        public Task<List<DepartmentDto>> Departments =>
         _cacheService.GetOrCreateAsync("CachedDepartments", async () =>
             (await _departmentService.GetAllAsync()).ToList(), TimeSpan.FromHours(_expirationHours));
    }
}
