using DocumentDbRepositories;
using Microsoft.AspNet.Mvc;
using ScampApi.Infrastructure;
using System.Threading.Tasks;

namespace ScampApi.Controllers.Controllers
{
    //[Authorize]
    [Route("api/settings")]
    public class Settings : Controller
    {
        private ILinkHelper _linkHelper;
        private readonly ISystemSettingsRepository _settingsRepository;

        public Settings(ILinkHelper linkHelper, ISystemSettingsRepository settingsRepository)
        {
            _linkHelper = linkHelper;
            _settingsRepository = settingsRepository;
        }

        // Retrieve a list of system administrators
        [HttpGet("sitestyles")]
        public async Task<string> Get()
        {
            string rtnResult = string.Empty;

            // fetch site styles
            var settingDocument = await _settingsRepository.GetSiteStyleSettings();
            if (settingDocument != null)
                rtnResult = settingDocument.Settings;

            return rtnResult;
        }
    }
}
