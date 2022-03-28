using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using Cs.Memory;
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


    [MenuItem("ttt/싸테")]
    private static void ShiftTest()
    {
        var testPrefabName = "/MTObj";
        //var resourcePath = "/Resources";
        var resourcePath = "";
        var assetPath = "Assets";
        var path = $"{Application.dataPath}" + resourcePath + testPrefabName + ".prefab";
        
        Debug.Log($"{path}");

        var target = Path.GetFileNameWithoutExtension(path);
        var shiftPrefabName = Cypher(target, shiftRange: 5);
        Debug.Log($"shift =>  {shiftPrefabName}");
        var originPath = assetPath + resourcePath + testPrefabName + ".prefab";
        SaveAssetPath(originPath, shiftPrefabName + ".prefab");
        
        //var de = Cypher(shiftPrefabName, shiftRange: -5);
        //Debug.Log($"shift =>  {de}");
        //var dp = assetPath + resourcePath + "/" + shiftPrefabName + ".prefab";
        //SaveAssetPath(dp, "MTObj" + ".prefab");
    }

    public static string Cypher(string word, int shiftRange)
    {
        // If word is null, we just return null.
        if (string.IsNullOrEmpty(word))
            return null;

        var builder = new StringBuilder();

        foreach (var d in word)
        {
            var charCypher = ShiftChar(d, shiftRange);
            builder.Append(Convert.ToString(charCypher));
        }

        return builder.ToString();
    }


    [MenuItem("ttt/아스키")]
    public static void TestConvert()
    {
        //var test = ShiftChar('A', 5);
        //Debug.Log($"convert {test}");
        //var test1 = ShiftChar(test, -5);
        //Debug.Log($"convert {test1}");

        /////////////
        //Debug.Log($"///////////////////////////////////////");

        //var testMin111 = ShiftChar('A', 5);
        //Debug.Log($"convert {testMin111}");

        //Debug.Log($"///////////////////////////////////////");

        //var tes1t = ShiftChar('Z', 5);
        //Debug.Log($"convert {tes1t}");
        //var tes1t1 = ShiftChar(tes1t, -5);
        //Debug.Log($"convert {tes1t1}");

        ///////////////
        //Debug.Log($"///////////////////////////////////////");

        //var testMin = ShiftChar('A', -5);
        //Debug.Log($"convert {testMin}");
        //var testMin2 = ShiftChar(testMin, 5);
        //Debug.Log($"convert {testMin2}");

        //

        Debug.Log($"///////////////////////////////////////");

        var testMin111 = ShiftChar('a', 5);
        Debug.Log($"convert {testMin111}");

        Debug.Log($"///////////////////////////////////////");

        var tes1t = ShiftChar('z', 5);
        Debug.Log($"convert {tes1t}");
        var tes1t1 = ShiftChar(tes1t, -5);
        Debug.Log($"convert {tes1t1}");

        /////////////
        Debug.Log($"///////////////////////////////////////");

        var testMin = ShiftChar('a', -5);
        Debug.Log($"convert {testMin}");
        var testMin2 = ShiftChar(testMin, 5);
        Debug.Log($"convert {testMin2}");
    }

    private static void SaveAssetPath(string assetPath, string renameStr)
    {
        var result = AssetDatabase.RenameAsset(assetPath, renameStr);
        if (string.IsNullOrEmpty(result) == false)
        {
            Debug.Log(result);
            return;
        }

        Debug.Log($"Success => {renameStr}");

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    /// <summary>
    /// </summary>
    /// <param name="shiftRange"> shiftRange값 만큼 아스키 코드를 이동시킨다. </param>
    /// <returns></returns>
    private static char ShiftChar(char ch, int shiftRange)
    {
        var Z = 90;
        var A = 65;
        var z = 122;
        var a = 97;

        if (char.IsUpper(ch))
        {
            var shiftValue = ch + shiftRange;
            switch (shiftValue)
            {
                case > 'Z':
                {
                    return Convert.ToChar(A + (--shiftValue % Z));
                }
                case < 'A':
                {
                    return Convert.ToChar(Z - (A % ++shiftValue));
                }
                default:
                    return Convert.ToChar(shiftValue);
            }
        }
        else
        {
            var shiftValue = ch - shiftRange;
            switch (shiftValue)
            {
                case > 'z':
                {
                    return Convert.ToChar(a + (--shiftValue % z));
                }
                case < 'a':
                {
                    return Convert.ToChar(z - (a % ++shiftValue));
                }
                default:
                    return Convert.ToChar(shiftValue);
            }
        }
    }

    [MenuItem("ttt/Crypto Encrypt")]
    public static void Encrypt()
    {
        //var path = $"{Application.dataPath}" + "/Resources/MTObj.prefab";
        var origin = $"{Application.dataPath}" + "/Resources/";
        var path = origin + "MTObj.prefab";

        var assetPath = "Assets/Resources/";
        Debug.Log($"{path}");

        var target = Path.GetFileNameWithoutExtension(path);
        var ascii = Encoding.ASCII.GetBytes(target);
        
        Crypto2.Encrypt(ascii, ascii.Length);
        var enCodingStr  = Encoding.ASCII.GetString(ascii);
        
        var rename = AssetDatabase.RenameAsset(assetPath + "MTObj", assetPath + $"{enCodingStr}");
        Debug.Log($"rename : {rename}");
        AssetDatabase.SaveAssets();
        Debug.Log($"enCoding result : {enCodingStr}");
    }

    [MenuItem("ttt/Crypto Decrypt")]
    public static void Decrypt()
    {
        var path = $"{Application.dataPath}" + "/Resources/MTObj.prefab";
        Debug.Log($"{path}");

        var target = Path.GetFileNameWithoutExtension(path);

        var ascii = Encoding.ASCII.GetBytes(target);

        Crypto2.Decrypt(ascii, ascii.Length);
        var deCodingStr = Encoding.ASCII.GetString(ascii);
        Debug.Log($"deCoding result : {deCodingStr}");
    }
}


namespace Cs.Memory
{
    using Cs.Core.Util;

    public static class Crypto2
    {
        private const ulong OddMask = 0x5555_5555_5555_5555;
        private const ulong EvenMask = 0xaaaa_aaaa_aaaa_aaaa;

        private static readonly ulong[] MaskList = new ulong[]
        {
            0xc257fad792eebf73,
            0x04189e34ceb9a62d,
            0xcb6578523f2f30ec,
            0x2b627f4954cf04eb,
            0x55b7063d67e08dc1,
            0x75fa03fa2b9cecc1,
            0x0de766a582eec302,
            0x0c59262af8b70a40,
            0x749d7540354d2b91,
            0xd92341e44f542370,
            0xd7a98aec31b0003e,
            0x3e13af4726627247,
            0xe6f11e7aa2960a8a,
            0x6139148e3b9d771c,
            0x6a0bf16f055abebe,
            0xfd2af98e29c91d37,
        };

        public static void Encrypt(byte[] buffer, int size)
        {
            Encrypt(buffer, offset: 0, size);
        }

        public static void Decrypt(byte[] buffer, int size)
        {
            Decrypt(buffer, offset: 0, size);
        }

        public static void Encrypt(byte[] buffer, int offset, int size)
        {
            int maskIndex = 0;
            int processed = 0;

            while (processed < size)
            {
                ulong mask = MaskList[maskIndex];
                int position = offset + processed;
                int remainSize = size - processed;
                if (remainSize >= sizeof(long))
                {
                    ulong twisted = buffer.DirectToUint64(position) ^ mask;

                    ulong oddValue = twisted & OddMask;
                    ulong evenValue = twisted & EvenMask;
                    ulong switched = (evenValue >> 1) | (oddValue << 1);

                    switched = switched & 0xffff_ffff_0000_0000
                        | ((switched & 0xff00_0000) >> 8)
                        | ((switched & 0x00ff_0000) << 8)
                        | ((switched & 0xff00) >> 8)
                        | ((switched & 0x00ff) << 8);

                    switched.DirectWriteTo(buffer, position);
                    processed += sizeof(long);
                }
                else
                {
                    for (int i = 0; i < remainSize; ++i)
                    {
                        buffer[position + i] ^= (byte)(mask & (0xfful << i) >> i);
                    }

                    processed += remainSize;
                }

                maskIndex = (maskIndex + 1) % MaskList.Length;
            }
        }

        public static void Decrypt(byte[] buffer, int offset, int size)
        {
            int maskIndex = 0;
            int processed = 0;

            while (processed < size)
            {
                ulong mask = MaskList[maskIndex];
                int position = offset + processed;
                int remainSize = size - processed;
                if (remainSize >= sizeof(long))
                {
                    ulong switched = buffer.DirectToUint64(position);

                    switched = switched & 0xffff_ffff_0000_0000
                        | ((switched & 0xff00_0000) >> 8)
                        | ((switched & 0x00ff_0000) << 8)
                        | ((switched & 0xff00) >> 8)
                        | ((switched & 0x00ff) << 8);

                    ulong oddValue = switched & OddMask;
                    ulong evenValue = switched & EvenMask;
                    switched = (evenValue >> 1) | (oddValue << 1);

                    ulong twisted = switched ^ mask;

                    twisted.DirectWriteTo(buffer, position);
                    processed += sizeof(long);
                }
                else
                {
                    for (int i = 0; i < remainSize; ++i)
                    {
                        buffer[position + i] ^= (byte)(mask & (0xfful << i) >> i);
                    }

                    processed += remainSize;
                }

                maskIndex = (maskIndex + 1) % MaskList.Length;
            }
        }
    }
}

namespace Cs.Core.Util
{
    using System;

    public static class NumericExt
    {
        public static int Clamp(this int val, int min, int max)
        {
            return Math.Min(Math.Max(val, min), max);
        }

        public static long Clamp(this long val, long min, long max)
        {
            return Math.Min(Math.Max(val, min), max);
        }

        public static T Clamp<T>(this T val, T min, T max)
            where T : class, IComparable<T>
        {
            if (val.CompareTo(min) < 0)
            {
                return min;
            }

            return val.CompareTo(max) > 0 ? max : val;
        }

        public static ushort DirectToUint16(this byte[] buffer, int startIndex)
        {
            // faster then BitConverter.ToUInt16
            return (ushort)((((uint)buffer[startIndex + 1]) << 8) |
                buffer[startIndex + 0]);
        }

        public static uint DirectToUint32(this byte[] buffer, int startIndex)
        {
            // faster then BitConverter.ToUInt32
            return (((((((uint)buffer[startIndex + 3]) << 8) |
                buffer[startIndex + 2]) << 8) |
                buffer[startIndex + 1]) << 8) |
                buffer[startIndex + 0];
        }

        public static ulong DirectToUint64(this byte[] buffer, int startIndex)
        {
            // faster then BitConverter.ToUInt64
            return (((((((((((((((ulong)buffer[startIndex + 7]) << 8) |
                buffer[startIndex + 6]) << 8) |
                buffer[startIndex + 5]) << 8) |
                buffer[startIndex + 4]) << 8) |
                buffer[startIndex + 3]) << 8) |
                buffer[startIndex + 2]) << 8) |
                buffer[startIndex + 1]) << 8) |
                buffer[startIndex + 0];
        }

        public static void DirectWriteTo(this int data, byte[] buffer, int position)
        {
            // faster then Buffer.BlockCopy(BitConverter.GetBytes(data), 0, buffer, position, sizeof(int))
            buffer[position] = (byte)data;
            buffer[position + 1] = (byte)(data >> 8);
            buffer[position + 2] = (byte)(data >> 16);
            buffer[position + 3] = (byte)(data >> 24);
        }

        public static void DirectWriteTo(this long data, byte[] buffer, int position)
        {
            // faster then Buffer.BlockCopy(BitConverter.GetBytes(data), 0, buffer, position, sizeof(long))
            buffer[position] = (byte)data;
            buffer[position + 1] = (byte)(data >> 8);
            buffer[position + 2] = (byte)(data >> 16);
            buffer[position + 3] = (byte)(data >> 24);
            buffer[position + 4] = (byte)(data >> 32);
            buffer[position + 5] = (byte)(data >> 40);
            buffer[position + 6] = (byte)(data >> 48);
            buffer[position + 7] = (byte)(data >> 56);
        }

        public static void DirectWriteTo(this ulong data, byte[] buffer, int position)
        {
            // faster then Buffer.BlockCopy(BitConverter.GetBytes(data), 0, buffer, position, sizeof(ulong))
            buffer[position] = (byte)data;
            buffer[position + 1] = (byte)(data >> 8);
            buffer[position + 2] = (byte)(data >> 16);
            buffer[position + 3] = (byte)(data >> 24);
            buffer[position + 4] = (byte)(data >> 32);
            buffer[position + 5] = (byte)(data >> 40);
            buffer[position + 6] = (byte)(data >> 48);
            buffer[position + 7] = (byte)(data >> 56);
        }
    }
}
