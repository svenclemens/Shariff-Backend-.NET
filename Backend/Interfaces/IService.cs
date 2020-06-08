using System;

public interface IService{
    System.Net.WebRequest GetRequest(string url);
    int ExtractCount(ReadOnlySpan<byte> data); //todo: change type accordingly
    string GetName();
    string FilterResponse(string content);
    void SetConfig(ServiceConfig config);
}