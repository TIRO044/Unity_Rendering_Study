using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using Converter;

namespace TestRunner
{

    public class DivideCases : IEnumerable
    {
        private static string[] _abScriptPath;

        public DivideCases()
        {
            _abScriptPath = new string[]
            {
                "Assets/ASSET_BUNDLE/AB_SCRIPT/AB_SCRIPT_ANIM_DATA",
                "Assets/ASSET_BUNDLE/AB_SCRIPT/AB_SCRIPT_EFFECT"
            };
        }
        public IEnumerator GetEnumerator()
        {
            foreach (var t in _abScriptPath)
            {
                yield return new object[] { t };
            }
        }
    }

    public class DivideCases1 : IEnumerable
    {
        private static string[] _abScriptPath;

        public DivideCases1()
        {
            _abScriptPath = new string[]
            {
                "Assets/ASSET_BUNDLE/AB_SCRIPT/AB_SCRIPT_EFFECT/LUA_DAMAGE_EFFECT_TEMPLET.txt",
                "Assets/ASSET_BUNDLE/AB_SCRIPT/AB_SCRIPT_EFFECT/LUA_DAMAGE_EFFECT_TEMPLET.txt.meta"
            };
        }
        public IEnumerator GetEnumerator()
        {
            foreach (var t in _abScriptPath)
            {
                yield return new object[] { t };
            }
        }
    }

    public class DivideCases2 : IEnumerable
    {
        private static string[] _abScriptPath;

        public DivideCases2()
        {
            _abScriptPath = new string[]
            {
                "Assets/ASSET_BUNDLE/AB_SCRIPT",
                "Assets/ASSET_BUNDLE/AB_SCRIPT/AB_SCRIPT_CUTSCENE"
            };
        }
        public IEnumerator GetEnumerator()
        {
            foreach (var t in _abScriptPath)
            {
                yield return new object[] { t };
            }
        }
    }

    [TestFixture()]
    public class NameConverterEncryptionTest
    {
        private IStrConverter _est = new EasyStrConverter();

        [TestCaseSource(typeof(DivideCases1)), Order(1)]
        public void FileTest(string tc)
        {
            Debug.Log(nameof(FileTest));
            var fi = new FileInfo(tc);

            var isMetaFile = fi.Name.Contains(".meta");
            var fileName = Path.GetFileNameWithoutExtension(fi.FullName);
            if (isMetaFile)
            {
                fileName = Path.GetFileNameWithoutExtension(fileName);
            }

            var c = _est.Encryption(fileName);
            var c1 = _est.Decryption(c);

            Debug.Log($"{fileName} to {c1}");
            Assert.IsTrue(string.Equals(fileName, c1));
        }

        [TestCaseSource(typeof(DivideCases)), Order(4)]
        public void Test(string tc)
        {
            Debug.Log(nameof(Test));

            var files = Directory.GetFiles(tc);

            foreach (var f in files)
            {
                var fileName = Path.GetFileNameWithoutExtension(f);
                //메타 파일 변환 때문에 한 번 더 해줌
                fileName = Path.GetFileNameWithoutExtension(fileName);

                var c = _est.Encryption(fileName);
                var c1 = _est.Decryption(c);

                Debug.Log($"[{fileName}] -> [{c}] -> [{c1}]");

                Assert.IsTrue(string.Equals(fileName, c1));
            }
        }

        [TestCaseSource(typeof(DivideCases2)), Order(4)]
        public void TestAllFile(string tc)
        {
            Debug.Log(nameof(TestAllFile));

            var files = Directory.GetFiles(tc, "*", searchOption: SearchOption.AllDirectories);

            foreach (var f in files)
            {
                var fileName = Path.GetFileNameWithoutExtension(f);
                //메타 파일 변환 때문에 한 번 더 해줌
                fileName = Path.GetFileNameWithoutExtension(fileName);

                var c = _est.Encryption(fileName);
                var c1 = _est.Decryption(c);

                Debug.Log($"[{fileName}] -> [{c}] -> [{c1}]");

                Assert.IsTrue(string.Equals(fileName, c1));
            }
        }

        [TestCaseSource(typeof(DivideCases2)), Order(7)]
        public void EncryptionAllScript(string tc)
        {
            Debug.Log(nameof(EncryptionAllScript));
            var files = Directory.GetFiles(tc, "*.*", searchOption: SearchOption.AllDirectories)
                .Where(s => s.Contains(".txt"));

            var sb = new StringBuilder();

            foreach (var f in files)
            {
                var fullPath = Path.GetFullPath(f);
                var dirPath = Path.GetDirectoryName(fullPath);
                var isMetaFile = f.Contains(".meta");
                var extension = Path.GetExtension(f);
                var fileName = Path.GetFileNameWithoutExtension(f);
                if (isMetaFile)
                {
                    extension = Path.GetExtension(fileName);
                    fileName = Path.GetFileNameWithoutExtension(fileName);
                }

                var c = _est.Encryption(fileName);
                if (isMetaFile == false)
                {
                    var meta = $@"{dirPath}/{fileName}{extension}";
                    var meta1 = $@"{dirPath}/{c}{extension}";
                    sb.Append($"{meta} to {meta1}").AppendLine();
                    //Debug.Log($"{meta} to {meta1}");
                    if (File.Exists(meta) == false)
                    {
                        Debug.LogError($"not found {meta}");
                        continue;
                    }
                    File.Move(meta, meta1);
                }
                else
                {
                    var meta = $@"{dirPath}/{fileName}{extension}.meta";
                    var meta1 = $@"{dirPath}/{c}{extension}.meta";
                    sb.Append($"{meta} to {meta1}").AppendLine();
                    //Debug.Log($"{meta} to {meta1}");
                    if (File.Exists(meta) == false)
                    {
                        Debug.LogError($"not found {meta}");
                        continue;
                    }
                    File.Move(meta, meta1);
                }

                Debug.Log($"[{fileName}] -> [{c}]");
            }

            var path = Path.GetPathRoot(tc) + $"{nameof(EncryptionAllScript)}_Result.txt";
            Debug.Log($"root path {path}");

            using (var file = File.CreateText(path))
            {
                file.Write(sb.ToString());
            }

            AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
        }

        [TestCaseSource(typeof(DivideCases2)), Order(8)]
        public void DecryptionAllScript(string tc)
        {
            Debug.Log(nameof(DecryptionAllScript));

            var files = Directory.GetFiles(tc, "*.*", searchOption: SearchOption.AllDirectories)
                .Where(s => s.Contains(".txt"));

            foreach (var f in files)
            {
                var fullPath = Path.GetFullPath(f);
                var dirPath = Path.GetDirectoryName(fullPath);
                var isMetaFile = f.Contains(".meta");
                var extension = Path.GetExtension(f);
                var fileName = Path.GetFileNameWithoutExtension(f);
                if (isMetaFile)
                {
                    extension = Path.GetExtension(fileName);
                    fileName = Path.GetFileNameWithoutExtension(fileName);
                }

                var c = _est.Decryption(fileName);
                if (isMetaFile == false)
                {
                    var meta = $@"{dirPath}/{fileName}{extension}";
                    var meta1 = $@"{dirPath}/{c}{extension}";
                    Debug.Log($"{meta} to {meta1}");
                    if (File.Exists(meta) == false)
                    {
                        Debug.LogError($"not found {meta}");
                        continue;
                    }
                    File.Move(meta, meta1);
                }
                else
                {
                    var meta = $@"{dirPath}/{fileName}{extension}.meta";
                    var meta1 = $@"{dirPath}/{c}{extension}.meta";
                    Debug.Log($"{meta} to {meta1}");
                    if (File.Exists(meta) == false)
                    {
                        Debug.LogError($"not found {meta}");
                        continue;
                    }
                    File.Move(meta, meta1);
                }

                Debug.Log($"[{fileName}] -> [{c}]");
            }

            AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
        }

        [TestCaseSource(typeof(DivideCases)), Order(5)]
        public void EncryptionTest(string tc)
        {
            Debug.Log(nameof(EncryptionTest));
            var files = Directory.GetFiles(tc, "*", searchOption: SearchOption.AllDirectories);

            foreach (var f in files)
            {
                var isMetaFile = f.Contains(".meta");
                var extension = Path.GetExtension(f);
                var fileName = Path.GetFileNameWithoutExtension(f);
                if (isMetaFile)
                {
                    extension = Path.GetExtension(fileName);
                    fileName = Path.GetFileNameWithoutExtension(fileName);
                }

                var c = _est.Encryption(fileName);
                if (isMetaFile == false)
                {
                    var meta = $"{tc}/{fileName}{extension}";
                    var meta1 = $"{tc}/{c}{extension}";
                    Debug.Log($"{meta} to {meta1}");
                    File.Move(meta, meta1);
                }
                else
                {
                    var meta = $"{tc}/{fileName}{extension}.meta";
                    var meta1 = $"{tc}/{c}{extension}.meta";
                    Debug.Log($"{meta} to {meta1}");
                    File.Move(meta, meta1);
                }

                Debug.Log($"[{fileName}] -> [{c}]");
            }

            AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
        }

        [TestCaseSource(typeof(DivideCases)), Order(6)]
        public void DecryptionTest(string tc)
        {
            Debug.Log(nameof(DecryptionTest));

            var files = Directory.GetFiles(tc, "*", searchOption: SearchOption.AllDirectories);

            foreach (var f in files)
            {
                var isMetaFile = f.Contains(".meta");
                var extension = Path.GetExtension(f);
                var fileName = Path.GetFileNameWithoutExtension(f);
                if (isMetaFile)
                {
                    extension = Path.GetExtension(fileName);
                    fileName = Path.GetFileNameWithoutExtension(fileName);
                }

                var c = _est.Decryption(fileName);
                if (isMetaFile == false)
                {
                    var meta = $"{tc}/{fileName}{extension}";
                    var meta1 = $"{tc}/{c}{extension}";
                    Debug.Log($"{meta} to {meta1}");
                    File.Move(meta, meta1);
                }
                else
                {
                    var meta = $"{tc}/{fileName}{extension}.meta";
                    var meta1 = $"{tc}/{c}{extension}.meta";
                    Debug.Log($"{meta} to {meta1}");
                    File.Move(meta, meta1);
                }

                Debug.Log($"[{fileName}] -> [{c}]");
            }

            AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
        }
    }

    [TestFixture()]
    public class NameConverter
    {
        private static IStrConverter _est = new EasyStrConverter();

        [Test()]
        [TestCase("Assets/ASSET_BUNDLE/AB_LOADING_LOC_THA/")]
        public void 네이밍_변환(string tc)
        {
            var files = Directory.GetFiles(tc);

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
