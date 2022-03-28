using System;
using System.IO;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using Converter;

namespace TestRunner
{
    [TestFixture()]
    public class NameConverter
    {
        private static IStrTransformer _est = new EasyStrTransformer();

        [Test()]
        [TestCase("Assets/ASSET_BUNDLE/AB_LOADING_LOC_THA/")]
        public void 네이밍_변환(string tc)
        {
            var files = Directory.GetFiles(tc);
            Log($" --------- Start FileChange : {files.Length} --------- ");
            Log($"file Count : {files.Length}");

            for (var index = 0; index < files.Length; index++)
            {
                var f = files[index];
                var extension = Path.GetExtension(f);
                var fileName = Path.GetFileNameWithoutExtension(f);
                fileName = Path.GetFileNameWithoutExtension(fileName);

                var c = _est.Encryption(fileName);
                var renamed = $"{c}{extension}";
                //Debug.LogWarning($"{fileName} ===> {renamed}");

                var c1 = _est.Decryption(c);
                var renamed1 = $"{c1}{extension}";
                Log($"[{fileName}] -> [{c}] -> [{c1}]");

                Assert.IsTrue(string.Equals(fileName, c1));
                //AssetDatabase.RenameAsset(f, renamed);
            }

            //AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
        }

        [Test()]
        [TestCase("")]
        public void a부터z까지(string tc)
        {
            for (var index = 97; index < 123; index++)
            {
                for (int i = 0; i < 1000; i++)
                {
                    var f = Convert.ToChar(index);
                    var c = _est.ShiftChar(f, i);
                    var cc = _est.ShiftChar(c, -i);
                    Log($"[{f}] -> [{c}] -> [{cc}] :: i {i}");

                    var t = Equals(f, cc);
                    Assert.IsTrue(t);
                }
            }

            AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
        }

        [Test()]
        [TestCase("")]
        public void A부터Z까지(string tc)
        {
            for (var index = 65; index < 91; index++)
            {
                for (int i = 0; i < 1000; i++)
                {
                    var f = Convert.ToChar(index);
                    var c = _est.ShiftChar(f, i);
                    var cc = _est.ShiftChar(c, -i);
                    Log($"[{f}] -> [{c}] -> [{cc}] :: i {i}");

                    var t = Equals(f, cc);
                    Assert.IsTrue(t);
                }
            }

            AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
        }

        [Test()]
        [TestCase("")]
        public void zero부터nine까지(string tc)
        {
            for (var index = 48; index < 57; index++)
            {
                for (int i = 0; i < 1000; i++)
                {
                    var f = Convert.ToChar(index);
                    var c = _est.ShiftChar(f, i);
                    var cc = _est.ShiftChar(c, -i);
                    Log($"[{f}] -> [{c}] -> [{cc}] :: i {i}");

                    var t = Equals(f, cc);
                    Assert.IsTrue(t);
                }
            }

            AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
        }

        private void Log(string result)
        {
            Debug.Log(result);
        }
    }
}
