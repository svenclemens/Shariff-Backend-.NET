using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace shariff.Controllers
{
    [ApiController]
    [Route("")]
    public class DefaultController : Controller
    {
        static private BackendConfig Configuration;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public DefaultController(IWebHostEnvironment environment){
            _hostingEnvironment = environment;
            var configFilePath = $@"{environment.ContentRootPath}\config.json";
            if(System.IO.File.Exists(configFilePath) && TryLoadConfigFile(configFilePath, environment, out BackendConfig config)){
                Configuration = config;
            } else {
                Configuration = new BackendConfig(){
                    Domains = Array.Empty<string>(),
                    ServiceData = new [] {new ServiceConfig(){ ServiceName = @"facebook", AppId = @"foo", Secret = @"bar"} }
                };
            }
        }

        private bool TryLoadConfigFile(string configFilePath, IWebHostEnvironment environment, out BackendConfig config)
        {
            try
            {
                using(var sr = new System.IO.StreamReader(configFilePath)){
                    var content = sr.ReadToEnd();
                    config = System.Text.Json.JsonSerializer.Deserialize<BackendConfig>(content);
                }
            }
            catch (System.Exception)
            {
                config = null;
                return false;
            }

            return true;
        }

        [HttpGet()]
        [Route("")]
        public async Task<ActionResult<string>> Get()
        {
            return string.Empty;
        }

        [HttpGet()]
        [Route("/interactions/")]
        public async Task<ActionResult<string>> GetInteractions([FromQuery(Name = @"url")]string url = @"")
        {
            Response.Headers.Add(@"Content-type", @"application/json");
            var backend =  new Backend.Backend(Configuration);
            return backend.Get(url);
        }
    }
}
