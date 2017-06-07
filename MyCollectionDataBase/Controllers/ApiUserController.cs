﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Repository.Data;
using Repository.Logic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyCollectionDataBase.Controllers
{
    [Route("api/user")]
    public class ApiUserController : Controller
    {
        private UserRepository userRepository = new UserRepository(new UserSQLContext());

        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            try
            {
                return userRepository.GetUsernames();
            }
            catch (Exception e)
            {
                return new List<string>();
            }
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}