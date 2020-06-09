using System;
using System.Text.Json;

public class Reddit : Request, IService {
    public Reddit() : base() { }
    public Reddit(ServiceConfig _config) : base(_config) { }
    public string GetName(){
        return @"reddit";
    }

    public System.Net.WebRequest GetRequest(string url){
        var request = System.Net.HttpWebRequest.Create($@"https://www.reddit.com/api/info.json?url={System.Net.WebUtility.HtmlEncode(url)}");
        request.Method = @"GET";
        return request;
    }

    public int ExtractCount(ReadOnlySpan<byte> jsonData){
        int result = 0;
        var jsonReader = new Utf8JsonReader(jsonData);
        
        while(jsonReader.Read()){
            if(jsonReader.TokenType == JsonTokenType.PropertyName && jsonReader.GetString() == @"score"){
                jsonReader.Read();
                if(jsonReader.TokenType == JsonTokenType.Number){
                    result += jsonReader.GetInt32();
                }
            }
        }

        return result;
    }

    public new string FilterResponse(string content) {
        // 'VK.Share.count(1, x);' with x being the count
        var indexOfClosingBrace = content.LastIndexOf(')');
        var indexOfLastComma = content.LastIndexOf(',');

        return content.Substring(indexOfLastComma, indexOfClosingBrace-indexOfLastComma);
    }

    public new void SetConfig(ServiceConfig config){
        base.SetConfig(config);
    }
}