using System.Collections.Generic;

public class BasicCache : ICache
{
    private Dictionary<string, string> internalMemory;

    public BasicCache(){
        internalMemory = new Dictionary<string, string>();
    }

    public string GetItem(string key)
    {
        return internalMemory[key];
    }

    public bool HasItem(string key)
    {
        return internalMemory.ContainsKey(key);
    }

    public void SetItem(string key, string value)
    {
        internalMemory.Add(key, value);
    }
}