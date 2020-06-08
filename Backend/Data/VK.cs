using System;
using System.Text.Json;

public class VK : Request, IService {
    public string GetName(){
        return @"vk";
    }

    public System.Net.WebRequest GetRequest(string url){
        var request = System.Net.HttpWebRequest.Create($@"https://vk.com/share.php?act=count&index=1&url={System.Net.WebUtility.HtmlEncode(url)}");
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
        // 'VK.Share.count(1, x);' with x being the count
        var indexOfClosingBrace = content.LastIndexOf(')');
        var indexOfLastComma = content.LastIndexOf(',');

        return content.Substring(indexOfLastComma, indexOfClosingBrace-indexOfLastComma);
    }

    public new void SetConfig(ServiceConfig config){
        base.SetConfig(config);
    }
}