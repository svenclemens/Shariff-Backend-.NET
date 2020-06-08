using System;
public interface ILogger{
    void Warning(string message, string details, Exception e = null);
}