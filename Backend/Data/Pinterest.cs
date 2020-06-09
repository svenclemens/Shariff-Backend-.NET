using System;
using System.Text.Json;

public class Pinterest : Request, IService {
    public Pinterest() : base() { }
    public Pinterest(ServiceConfig _config) : base(_config) { }
    public string GetName(){
        return @"pinterest";
    }

    public System.Net.WebRequest GetRequest(string url){
        var request = System.Net.HttpWebRequest.Create($@"https://api.pinterest.com/v1/urls/count.json?callback=x&url={System.Net.WebUtility.HtmlEncode(url)}");
        request.Method = @"GET";
        return request;
    }

    public int ExtractCount(ReadOnlySpan<byte> jsonData){
        int result = 0;
        var jsonReader = new Utf8JsonReader(jsonData);
        
        while(jsonReader.Read()){
            if(jsonReader.TokenType == JsonTokenType.PropertyName && jsonReader.GetString() == @"count"){
                jsonReader.Read();
                if(jsonReader.TokenType == JsonTokenType.Number){
                    result = jsonReader.GetInt32();
                    break;
                }
            }
        }

        return result;
    }

    public new string FilterResponse(string content) {
        // x({"url":"http://your-uri.com","count":0})
        return content.Substring(2, content.Length - 1);
    }

    public new void SetConfig(ServiceConfig config){
        base.SetConfig(config);
    }
}