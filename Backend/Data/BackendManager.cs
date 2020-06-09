using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class BackendManager{
    public string BaseCacheKey {get;set;}
    public ICache Cache {get;set;}
    public IEnumerable<string> Domains{get;set;}
    public IEnumerable<IService> Services {get;set;}
    public ILogger Logger {get;set;}
    
    public BackendManager(){ }

    public BackendManager(string _baseCacheKey, ICache _cache, IEnumerable<string> _domains, IEnumerable<IService> _services){
        BaseCacheKey = _baseCacheKey;
        Cache = _cache;
        Domains = _domains;
        Services = _services;
    }

    public string Get(string url){
        string cacheKey = GetMD5Hash($@"{url}{BaseCacheKey}");

        if(!url.StartsWith(@"http")) {
            url = @"https://" + url;
        }

        if(!Uri.TryCreate(url, UriKind.Absolute, out _)){
            return string.Empty;
        }

        if(Cache.HasItem(cacheKey)){
            return Cache.GetItem(cacheKey);
        }

        var results = new Dictionary<IService, System.Net.HttpWebResponse>();
        // TODO: Rewrite this to run parallel
        foreach(var currentService in this.Services){
            var request = currentService.GetRequest(url);
            try
            {
                results[currentService] = (System.Net.HttpWebResponse)request.GetResponse();
            }
            catch (System.Net.WebException ex)
            {
                results[currentService] = (System.Net.HttpWebResponse)ex.Response;
            }
        }

        var counts = new Dictionary<string, int>();

        foreach(KeyValuePair<IService, System.Net.HttpWebResponse> currentKVPair in results){
            if(currentKVPair.Value.StatusCode != System.Net.HttpStatusCode.OK){
                if(this.Logger != null){
                    this.Logger.Warning(currentKVPair.Value.StatusDescription, currentKVPair.Value.ResponseUri.ToString());
                }
            } else {
                using(var contentStream = currentKVPair.Value.GetResponseStream()){
                    using(var contentReader = new System.IO.StreamReader(currentKVPair.Value.GetResponseStream())){
                        var content = currentKVPair.Key.FilterResponse(contentReader.ReadToEnd());
                        var contentSpan = new ReadOnlySpan<byte>(System.Text.Encoding.UTF8.GetBytes(content));
                        counts[currentKVPair.Key.GetName()] = currentKVPair.Key.ExtractCount(contentSpan);
                    }
                }
            }
        }

        var countJson = System.Text.Json.JsonSerializer.Serialize(counts);
        Cache.SetItem(cacheKey,  countJson);

        return countJson;
    }

    public static string GetMD5Hash(string value){
        var md5Crypt = System.Security.Cryptography.MD5.Create();
        byte[] valueHash = md5Crypt.ComputeHash(System.Text.Encoding.UTF8.GetBytes(value));

        var sb = new System.Text.StringBuilder();
        for(var i = 0; i < valueHash.Length; ++i){
            sb.Append(valueHash[i].ToString(@"x2"));
        }

        return sb.ToString();
    }
}