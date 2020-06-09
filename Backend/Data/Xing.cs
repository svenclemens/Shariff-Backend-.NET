using System;
using System.Text.Json;
public class Xing : Request, IService {
    public Xing() : base() { }
    public Xing(ServiceConfig _config) : base(_config) { }

    public string GetName(){
        return @"xing";
    }

    public System.Net.WebRequest GetRequest(string url){
        var request = System.Net.HttpWebRequest.Create($@"https://www.xing-share.com/spi/shares/statistics?url={System.Net.WebUtility.HtmlEncode(url)}");
        request.Method = @"POST";
        return request;
    }

    // unsafe public int ExtractCount(SimdJsonSharp.ParsedJsonN data){
    //     var iterator = data.CreateIterator();
    //     var key = Convert.ToSByte(@"share_counter");
        
    //     if(iterator.MoveToKey(&key) && iterator.IsInteger){
    //         return (int)iterator.GetInteger();
    //     } else {
    //         return 0;
    //     }
    // }

    public int ExtractCount(ReadOnlySpan<byte> jsonData){
        int result = 0;
        var jsonReader = new Utf8JsonReader(jsonData);
        
        while(jsonReader.Read()){
            if(jsonReader.TokenType == JsonTokenType.PropertyName && jsonReader.GetString() == @"share_counter"){
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
        return base.FilterResponse(content);
    }

    public new void SetConfig(ServiceConfig config){
        base.SetConfig(config);
    }
}