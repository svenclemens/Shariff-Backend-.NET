using System;
using System.Text.Json;

public class AddThis : Request, IService {
    public string GetName(){
        return @"addthis";
    }

    public System.Net.WebRequest GetRequest(string url){
        var request = System.Net.HttpWebRequest.Create($@"http://api-public.addthis.com/url/shares.json?url={System.Net.WebUtility.HtmlEncode(url)}");
        request.Method = @"GET";
        return request;
    }

    public int ExtractCount(ReadOnlySpan<byte> jsonData){
        int result = 0;
        var jsonReader = new Utf8JsonReader(jsonData);
        
        while(jsonReader.Read()){
            if(jsonReader.TokenType == JsonTokenType.PropertyName && jsonReader.GetString() == @"shares"){
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