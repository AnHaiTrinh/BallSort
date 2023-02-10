using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class FileUtils : MonoBehaviour
{
    public static void writeFile(string filePath, string content)
    {
        StreamWriter writer = new StreamWriter(filePath, true);
        writer.Write(content);
        writer.Close();
    }

    public static string readFile(string filePath)
    {
        StreamReader reader = new StreamReader(filePath);
        string content = reader.ReadToEnd();
        reader.Close();
        return content;
    }
}
