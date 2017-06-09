using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Repository.Data;
using Repository.Logic;

namespace MyCollectionDataBase.Controllers
{
    [Route("api/user")]
    public class ApiUserController : Controller
    {
        private UserRepository userRepository = new UserRepository(new UserSQLContext());

        [HttpGet]
        public IEnumerable<string> Get()
        {
            try
            {
                return userRepository.GetUsernames();
            }
            catch (Exception e)
            {
                return new List<string>(); //return empty string
            }
        }
    }
}