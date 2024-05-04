using ContentManagementService.Business.Interface;

namespace ContentManagementService.Business.Implementation
{
    public class BaseService : IBaseService
    {
        protected readonly IUserResolver _userResolver;

        public BaseService(IUserResolver userResolver)
        {
            _userResolver = userResolver;
        }
    }
}
