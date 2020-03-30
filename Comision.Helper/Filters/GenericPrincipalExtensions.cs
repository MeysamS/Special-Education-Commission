using System.Linq;
using System.Security.Claims;
using System.Security.Principal;

namespace Comision.Helper.Filters
{
    public static class GenericPrincipalExtensions
    {
        public static string FullName(this IPrincipal user)
        {
            if (user.Identity.IsAuthenticated)
            {
                var claimsIdentity = user.Identity as ClaimsIdentity;
                if (claimsIdentity != null)
                    foreach (var claim in claimsIdentity.Claims)
                    {
                        if (claim.Type == "FullName") return claim.Value;
                    }
                return "";
            }
            return "";
        }

        public static string FirstName(this IPrincipal user)
        {
            if (user.Identity.IsAuthenticated)
            {
                var claimsIdentity = user.Identity as ClaimsIdentity;
                if (claimsIdentity != null)
                    foreach (var claim in claimsIdentity.Claims)
                    {
                        if (claim.Type == "FirstName") return claim.Value;
                    }
                return "";
            }
            return "";
        }

        public static string LastName(this IPrincipal user)
        {
            if (user.Identity.IsAuthenticated)
            {
                var claimsIdentity = user.Identity as ClaimsIdentity;
                if (claimsIdentity != null)
                    foreach (var claim in claimsIdentity.Claims)
                    {
                        if (claim.Type == "LastName") return claim.Value;
                    }
                return "";
            }
            return "";
        }

        public static string AuthenType(this IPrincipal user)
        {
            if (!user.Identity.IsAuthenticated) return "";
            var claimsIdentity = user.Identity as ClaimsIdentity;
            if (claimsIdentity == null) return "";
            foreach (var claim in claimsIdentity.Claims.Where(claim => claim.Type == "AuthenType"))
            {
                return claim.Value;
            }
            return "";
        }
        public static string LevelId(this IPrincipal user)
        {
            if (!user.Identity.IsAuthenticated) return "";
            var claimsIdentity = user.Identity as ClaimsIdentity;
            if (claimsIdentity == null) return "";
            foreach (var claim in claimsIdentity.Claims.Where(claim => claim.Type == "LevelId"))
            {
                return claim.Value;
            }
            return "";
        }
        public static string LevelProgram(this IPrincipal user)
        {
            if (!user.Identity.IsAuthenticated) return "";
            var claimsIdentity = user.Identity as ClaimsIdentity;
            if (claimsIdentity == null) return "";
            foreach (var claim in claimsIdentity.Claims.Where(claim => claim.Type == "LevelProgram"))
            {
                return claim.Value;
            }
            return "";
        }
        public static string OrganName(this IPrincipal user)
        {
            if (!user.Identity.IsAuthenticated) return "";
            var claimsIdentity = user.Identity as ClaimsIdentity;
            if (claimsIdentity == null) return "";
            foreach (var claim in claimsIdentity.Claims.Where(claim => claim.Type == "OrganName"))
            {
                return claim.Value;
            }
            return "";
        }
        public static string Avatar(this IPrincipal user)
        {
            if (!user.Identity.IsAuthenticated) return "";
            var claimsIdentity = user.Identity as ClaimsIdentity;
            if (claimsIdentity == null) return "";
            foreach (var claim in claimsIdentity.Claims.Where(claim => claim.Type == "Avatar"))
            {
                return claim.Value;
            }
            return "";
        }
        public static string ListSigner(this IPrincipal user)
        {
            if (!user.Identity.IsAuthenticated) return "";
            var claimsIdentity = user.Identity as ClaimsIdentity;
            if (claimsIdentity == null) return "";
            foreach (var claim in claimsIdentity.Claims.Where(claim => claim.Type == "ListSigner"))
            {
                return claim.Value;
            }
            return "";
        }
        public static string Logo(this IPrincipal user)
        {
            if (!user.Identity.IsAuthenticated) return "";
            var claimsIdentity = user.Identity as ClaimsIdentity;
            if (claimsIdentity == null) return "";
            foreach (var claim in claimsIdentity.Claims.Where(claim => claim.Type == "Logo"))
            {
                return claim.Value;
            }
            return "";
        }
        public static string IsAdmin(this IPrincipal user)
        {
            if (!user.Identity.IsAuthenticated) return "";
            var claimsIdentity = user.Identity as ClaimsIdentity;
            if (claimsIdentity == null) return "";
            foreach (var claim in claimsIdentity.Claims.Where(claim => claim.Type == "IsAdmin"))
            {
                return claim.Value;
            }
            return "";
        }
        public static string IsPersonel(this IPrincipal user)
        {
            if (!user.Identity.IsAuthenticated) return "";
            var claimsIdentity = user.Identity as ClaimsIdentity;
            if (claimsIdentity == null) return "";
            foreach (var claim in claimsIdentity.Claims.Where(claim => claim.Type == "IsPersonel"))
            {
                return claim.Value;
            }
            return "";
        }
    }
}