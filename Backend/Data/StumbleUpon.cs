using System;
using System.Text.Json;

public class StumbleUpon : Request, IService {
    public StumbleUpon() : base() { }
    public StumbleUpon(ServiceConfig _config) : base(_config) { }
    public string GetName(){
        return @"stumbleupon";
    }

    public System.Net.WebRequest GetRequest(string url){
        var request = System.Net.HttpWebRequest.Create($@"https://www.stumbleupon.com/services/1.01/badge.getinfo?url={System.Net.WebUtility.HtmlEncode(url)}");
        request.Method = @"GET";
        return request;
    }

    public int ExtractCount(ReadOnlySpan<byte> jsonData){
        int result = 0;
        var jsonReader = new Utf8JsonReader(jsonData);
        
        while(jsonReader.Read()){
            if(jsonReader.TokenType == JsonTokenType.PropertyName && jsonReader.GetString() == @"views"){
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
        return content;
    }

    public new void SetConfig(ServiceConfig config){
        base.SetConfig(config);
    }
}