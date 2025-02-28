using EF.Models;
using Microsoft.EntityFrameworkCore;
using Service.Dto.SystemSetup;
using Service.SystemSetup.Interface;
using Service.UserManagement.Interface;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Cache
{
    public class CachedItems
    {
        private readonly IGenericCacheService _cacheService;
        private readonly IPSDBMCatalogueService _psdbmCatalogueService;
        private readonly ISupplementaryCatalogueService _supplementaryCatalogueService;
        private readonly IAccountCodeService _accountCodeService;
        private readonly IRoleService _roleService;
        private readonly int _expirationHours = 24;

        public CachedItems(IGenericCacheService cacheService, 
            IPSDBMCatalogueService catalogueService, 
            ISupplementaryCatalogueService supplementaryCatalogueService,
            IAccountCodeService accountCodeService,
            IRoleService roleService)
        {
            _cacheService = cacheService;
            _psdbmCatalogueService = catalogueService;
            _supplementaryCatalogueService = supplementaryCatalogueService;
            _roleService = roleService;
            _accountCodeService = accountCodeService;
        }

        public Task<List<PSDBMCatalogueDto>> PSDBMCatalogues =>
            _cacheService.GetOrCreateAsync("CachedPSDBMCatalogue", async () => (await (_psdbmCatalogueService.GetAllCurrent())).ToList(), TimeSpan.FromHours(_expirationHours));

        public Task<List<SupplementaryCatalogueDto>> SupplementaryCatalogues =>
           _cacheService.GetOrCreateAsync("CachedSupplementaryCatalogue", async () => (await (_supplementaryCatalogueService.GetAllCurrent())).ToList(), TimeSpan.FromHours(_expirationHours));

        public Task<List<AccountCodeDto>> AccountCodes =>
          _cacheService.GetOrCreateAsync("CachedAccountCodes", async () => (await (_accountCodeService.GetAllAsync())).ToList(), TimeSpan.FromHours(_expirationHours));
    }

}
