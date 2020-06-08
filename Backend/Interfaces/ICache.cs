public interface ICache{
    void SetItem(string key, string value);
    string GetItem(string key);
    bool HasItem(string key);
}