using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Text;


public class Debug
{
    public static void Log(string log)
    {
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine(log);
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("*********************************");
    }
    public static void LogError(string log)
    {
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine(log);
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("*********************************");
    }
    public static void Log(int messageId,IMessage message)
    {
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine($"報文ID:{messageId}\n包體:{JsonHelper.SerializeObject(message)}");
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("*********************************");
    }
}

