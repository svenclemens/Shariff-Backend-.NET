using System;
using System.Collections.Generic;

public class ServiceFactory{
    IDictionary<string, IService> ServiceMap = new Dictionary<string, IService>();

    public void RegisterService(string name, IService service = null){
        ServiceMap.Add(name, service);
    }

    public IService CreateService(string name, ServiceConfig serviceConfig){
        IService resultService;
        if(ServiceMap.ContainsKey(name)){
            resultService = ServiceMap[name];
        } else {
            resultService = (IService)System.Activator.CreateInstance(Type.GetType(name), serviceConfig);
        }

        if(serviceConfig != null){
            resultService.SetConfig(serviceConfig);
        }

        return resultService;
    }

    public IEnumerable<IService> GetServicesByName(IEnumerable<ServiceConfig> data){
        var services = new List<IService>();
        if(data != null){
            foreach(var currentService in data){
                 try
                 {
                    var service = this.CreateService(currentService.ServiceName, currentService);
                    services.Add(service);
                 }
                 catch (System.Exception)
                 {
                     continue;
                 }
            }
        }

        return services;
    }
}