using System;
using System.Collections.Generic;

public class BackendConfig{
    public IEnumerable<string> Domains {get;set;}
    public IEnumerable<ServiceConfig> ServiceData {get;set;}
}