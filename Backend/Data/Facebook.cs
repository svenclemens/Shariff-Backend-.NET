using System;
using System.Text.Json;

public class Facebook : Request, IService {
    public string GetName(){
        return @"facebook";
    }

    public System.Net.WebRequest GetRequest(string url){
        var accessToken = System.Net.WebUtility.HtmlEncode(this.config.AppId) + @"|" + System.Net.WebUtility.HtmlEncode(this.config.Secret);
        var request = System.Net.HttpWebRequest.Create($@"https://graph.facebook.com/v7.0/?id={System.Net.WebUtility.HtmlEncode(url)}&fields=engagement&access_token={accessToken}");
        request.Method = @"GET";
        return request;
    }

    public int ExtractCount(ReadOnlySpan<byte> jsonData){
        int result = 0;
        var jsonReader = new Utf8JsonReader(jsonData);
        
        while(jsonReader.Read()){
            if(jsonReader.TokenType == JsonTokenType.PropertyName){
                if(jsonReader.GetString() == @"reaction_count" || jsonReader.GetString() == @"comment_count" || jsonReader.GetString() == @"share_count"){
                    jsonReader.Read();
                    if(jsonReader.TokenType == JsonTokenType.Number){
                        result += jsonReader.GetInt32();
                    }
                }
            }
        }

        return result;
    }

    public new string FilterResponse(string content) {
        return content;
    }

    public new void SetConfig(ServiceConfig config){
        if(string.IsNullOrEmpty(config.AppId) || string.IsNullOrEmpty(config.Secret)){
            throw new ArgumentNullException(@"The Facebook app_id and secret must not be empty.");
        }

        base.SetConfig(config);
    }
}