using System;

namespace Backend
{
    public class Backend
    {
        private BackendManager backendManager = null;

        public Backend(BackendConfig _config){
            var domains = _config.Domains;
            var baseCacheKey = BackendManager.GetMD5Hash(System.Text.Json.JsonSerializer.Serialize(_config));
            var cache = new BasicCache();
            var serviceFactory = new ServiceFactory();

            backendManager = new BackendManager(baseCacheKey, cache, domains, serviceFactory.GetServicesByName(_config.ServiceData));
        }

        public string Get(string url){
            return backendManager.Get(url);
        }
    }
}
