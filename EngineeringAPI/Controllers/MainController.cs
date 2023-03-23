using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using EngineeringAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EngineeringAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MainController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public MainController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet] 
        [Route("/HelloWorld")] 
        public string HelloWorld()
        {
            return "Hello World!";
        }
    }
}
//TODO post survey
