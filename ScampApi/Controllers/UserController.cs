using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using ScampApi.Infrastructure;
using ScampTypes.ViewModels;
using DocumentDbRepositories;
using ProvisioningLibrary;

namespace ScampApi.Controllers
{
  [Route("api/user")]
  public class UserController : Controller
  {
    private readonly ISecurityHelper _securityHelper;
    private readonly IResourceRepository _resourceRepository;
    private readonly IUserRepository _userRepository;
    private readonly IGraphAPIProvider _graphAPIProvider;
    private static IVolatileStorageController _volatileStorageController = null;

    public UserController(ISecurityHelper securityHelper, IResourceRepository resourceRepository, IUserRepository userRepository, IVolatileStorageController volatileStorageController, IGraphAPIProvider graphAPIProvider)
    {
      _resourceRepository = resourceRepository;
      _userRepository = userRepository;
      _securityHelper = securityHelper;
      _volatileStorageController = volatileStorageController;
      _graphAPIProvider = graphAPIProvider;
    }

    // retrieves the current user
    // GET: api/user
    [HttpGet(Name = "User.CurrentUser")]
    public async Task<User> Get()
    {
      ScampUser tmpUser = null;

      // fetch user
      tmpUser = await _securityHelper.GetCurrentUser();

      //TODO: we're going to need this for authorizing requests, so we should probably cache it
      //return object for user...

      var user = new User()
      {
        Id = tmpUser.Id,
        Name = tmpUser.Name,
        Email = tmpUser.Email,
        IsSystemAdmin = tmpUser.IsSystemAdmin
      };

      return user;
    }
  }
}
