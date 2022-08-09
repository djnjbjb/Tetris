using System;
using System.IO;
using UnityEngine;
using UnityEditor;

/*
	借鉴自
	https://blog.csdn.net/LLLLL__/article/details/104288456
*/

public class CountCodeLines
{
    [MenuItem("Tools/统计代码行数")]
    private static void PrintTotalLine()
    {
        string path1 = "Assets/1. Scripts";
        string path2 = "Assets/1. ScriptsForEditor";
        Debug.Log($"({path1}) 中 代码行数：{GetCodeLines(path1)}");
        Debug.Log($"({path2}) 中 代码行数：{GetCodeLines(path2)}");
    }

    private static int GetCodeLines(string path)
    {
        int allFilelineCount = 0;
        {
            string[] fileName =
                Directory.GetFiles(path, "*.cs", SearchOption.AllDirectories);
            foreach (var temp in fileName)
            {
                int fileLineCount = 0;
                StreamReader sr = new StreamReader(temp);
                while (sr.ReadLine() != null)
                {
                    fileLineCount++;
                }
                allFilelineCount += fileLineCount;
            }
        }
        return allFilelineCount;
    }
}
