using Api.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace ChoMoi.Extensions
{
    public class ClaimsTransformer : IClaimsTransformation
    {
        //private readonly IPrincipal _principal;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMemoryCache _cache;
        public ClaimsTransformer(IUnitOfWork unitOfWork, /*IPrincipal principal,*/ IHttpContextAccessor httpContextAccessor, IMemoryCache cache)
        {
            _unitOfWork = unitOfWork;
            //_principal = principal;
            _httpContextAccessor = httpContextAccessor;
            _cache = cache;
        }

        public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            if (principal.Identity.IsAuthenticated)
            {
                var currentPrincipal = (ClaimsIdentity)principal.Identity;//_principal.Identity;

                var ci = (ClaimsIdentity)principal.Identity;
                var cacheKey = ci.Name;

                if (_cache.TryGetValue(cacheKey, out List<Claim> claims))
                {
                    currentPrincipal.AddClaims(claims);
                }
                else
                {
                    claims = new List<Claim>();
                    //var isUserSystemAdmin = await _repository.IsUserAdmin(ci.Name);
                    //if (isUserSystemAdmin)
                    //{
                    //    var c = new Claim(ClaimTypes.Role, "SystemAdmin");
                    //    claims.Add(c);
                    //}

                    _cache.Set(cacheKey, claims);
                    currentPrincipal.AddClaims(claims);

                    //foreach (var claim in ci.Claims)
                    //{
                    //    currentPrincipal.AddClaim(claim);
                    //}
                }
            }

            return await Task.FromResult(principal);
        }
    }
}