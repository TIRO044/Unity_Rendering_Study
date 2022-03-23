using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using UnityEditor;

public class Test
{
    [MenuItem("ttt/tt")]
    public static void Test1()
    {
        var obj = Resources.Load<GameObject>("MTObj");
        if (obj == null)
        {
            return;
        }

        var instance = MonoBehaviour.Instantiate(obj);
    }


    [MenuItem("ttt/CheckPath")]
    private static void CheckFile()
    {
        var path = $"{Application.dataPath}" + "/Resources/MTObj.prefab";
        Debug.Log($"{path}");
        Debug.Log(File.Exists(path) ? "exist" : "not exist");

        var target = Path.GetFileNameWithoutExtension(path);
        var ascii = Encoding.ASCII.GetBytes(target);

        var encoding = TestEncoding(ascii);
        var encodingStr = Encoding.ASCII.GetString(encoding);
        Debug.Log($"encodingStr : {encodingStr}");

        var decoding = TestDecoding(encoding);
        var decodingStr = Encoding.ASCII.GetString(decoding);
        Debug.Log($"decodingStr : {decodingStr}");
    }

    private static byte[] TestEncoding(byte[] asciiArr)
    {
        Debug.Log("--------- TestEncoding ---------");

        var encodingBuffer = new byte[asciiArr.Length];
        var sb = new StringBuilder();
        // test encoding
        for (var index = 0; index < asciiArr.Length; index++)
        {
            var b = asciiArr[index]++;
            //Debug.Log($"ascii : => {b}");
            //Debug.Log($"ascii convert : => 0x{b:x}");
            //Debug.Log($"char : => {(char)b}");
            //var t = 0xff & (b >> 2);

            //encodingBuffer[index] = (byte)(b >> 2);
            encodingBuffer[index] = b;
            sb.Append(encodingBuffer[index]).Append(',');
        }

        Debug.Log($"encoding result {sb}");

        return encodingBuffer;
    }

    private static byte[] TestDecoding(byte[] asciiArr)
    {
        Debug.Log("--------- TestDecoding ---------");

        var decodingBuffer = new byte[asciiArr.Length];
        var sb = new StringBuilder();

        // test decoding
        for (var index = 0; index < decodingBuffer.Length; index++)
        {
            var b = asciiArr[index]--;
            //decodingBuffer[index] = (byte)(b << 2);
            decodingBuffer[index] = b;
            sb.Append(decodingBuffer[index]).Append(',');
        }

        Debug.Log($"decoding result {sb}");

        return decodingBuffer;
    }


    [MenuItem("ttt/cypher test")]
    private static void ShiftTest()
    {
        var path = $"{Application.dataPath}" + "/Resources/MTObj.prefab";
        Debug.Log($"{path}");
        Debug.Log(File.Exists(path) ? "exist" : "not exist");

        var target = Path.GetFileNameWithoutExtension(path);
        var cy = Cypher(target);
        Debug.Log($"cy : {cy}");

        var de = deCypher(cy);
        Debug.Log($"de cy : {de}");
    }

    public static string Cypher(string word)
    {
        // If word is null, we just return null.
        if (string.IsNullOrEmpty(word))
            return null;

        StringBuilder builder = new StringBuilder();

        foreach (var d in word)
        {
            var dd = d + 200;
            var ddd = Convert.ToChar(dd);

            //Debug.Log($"origin : {d}, +2 : {dd}, Convert {ddd}");

            var charCypher = ddd;
            builder.Append(Convert.ToString(charCypher));
        }

        return builder.ToString();
    }

    public static string deCypher(string word)
    {
        // If word is null, we just return null.
        if (string.IsNullOrEmpty(word))
            return null;

        StringBuilder builder = new StringBuilder();

        foreach (var d in word)
        {
            var dd = d - 200;
            var ddd = Convert.ToChar(dd);

            //Debug.Log($"origin : {d}, +2 : {dd}, Convert {ddd}");

            var charCypher = ddd;
            builder.Append(Convert.ToString(charCypher));
        }

        return builder.ToString();
    }

    private static char CharShift(char a, int shiftRange)
    {
        if (char.IsUpper(a))
        {
            var shift = a - shiftRange;

            if (shift > 57)
            {
                shift = 57 - shiftRange / 25;
            }

            if (shift < 32)
            {
                shift = 32 + shiftRange;
            }
        }
        else
        {

        }

        return 's';
    }

    private static void CheckLower()
    {

    }
}

