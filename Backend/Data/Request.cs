public class Request{
    protected ServiceConfig config;

    public Request(){}
    public Request(ServiceConfig _config){
        config = _config;
    }

    public void SetConfig(ServiceConfig _config){
        config = _config;
    }

    public System.Net.WebRequest CreateRequest(string url, string method = "GET"){
        var request = System.Net.HttpWebRequest.Create(url);
        request.Method = method;
        return request;
    }

    public string FilterResponse(string content){
        return content;
    }
}