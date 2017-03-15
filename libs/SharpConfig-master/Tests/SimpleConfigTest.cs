// Copyright (c) 2013-2016 Cemalettin Dervis, MIT License.
// https://github.com/cemdervis/SharpConfig

using System;
using SharpConfig;
using NUnit.Framework;
using System.IO;

namespace Tests
{
    [TestFixture]
    public sealed class SimpleConfigTest
    {
        [Test]
        public void SingleValues()
        {
            var cfg = new Configuration();

            cfg["TestSection"]["IntSetting1"].IntValue = 100;
            cfg["TestSection"]["IntSetting2"].IntValue = 200;
            cfg["TestSection"]["StringSetting1"].StringValue = "Test";

            Assert.AreEqual(cfg["TestSection"]["IntSetting1"].IntValue, 100);
            Assert.AreEqual(cfg["TestSection"]["IntSetting2"].IntValue, 200);
            Assert.AreEqual(cfg["TestSection"]["StringSetting1"].StringValue, "Test");
        }

        [Test]
        public void ArrayValues()
        {
            var cfg = new Configuration();

            var ints = new int[] { -3, -2, -1, 0, 1, 2, 3 };
            var strings = new string[] { "Hello", "World", "!" };
            var floats = new float[] { 0.5f, 1.0f, 1.5f };

            cfg["TestSection"]["IntArray"].IntValueArray = ints;
            cfg["TestSection"]["StringArray"].StringValueArray = strings;
            cfg["TestSection"]["FloatArray"].FloatValueArray = floats;

            // ints
            {
                var arr = cfg["TestSection"]["IntArray"].IntValueArray;
                Assert.AreEqual(ints.Length, arr.Length);
                for (int i = 0; i < ints.Length; i++)
                    Assert.AreEqual(ints[i], arr[i]);
            }
            // strings
            {
                var arr = cfg["TestSection"]["StringArray"].StringValueArray;
                Assert.AreEqual(strings.Length, arr.Length);
                for (int i = 0; i < strings.Length; i++)
                    Assert.AreEqual(strings[i], arr[i]);
            }
            // floats
            {
                var arr = cfg["TestSection"]["FloatArray"].FloatValueArray;
                Assert.AreEqual(floats.Length, arr.Length);
                for (int i = 0; i < floats.Length; i++)
                    Assert.AreEqual(floats[i], arr[i]);
            }
        }

        [Test]
        public void SetGetValue()
        {
            var cfg = new Configuration();

            var ints = new int[] { 1, 2, 3 };

            cfg["TestSection"]["IntSetting1"].SetValue(100);
            cfg["TestSection"]["IntSetting2"].SetValue(200);
            cfg["TestSection"]["StringSetting1"].SetValue("Test");
            cfg["TestSection"]["IntArray"].SetValue(ints);

            Assert.AreEqual(cfg["TestSection"]["IntSetting1"].GetValue(typeof(int)), 100);
            Assert.AreEqual(cfg["TestSection"]["IntSetting2"].GetValue(typeof(int)), 200);
            Assert.AreEqual(cfg["TestSection"]["StringSetting1"].GetValue(typeof(string)), "Test");

            var intsNonGeneric = cfg["TestSection"]["IntArray"].GetValueArray(typeof(int));
            var intsGeneric = cfg["TestSection"]["IntArray"].GetValueArray<int>();

            Assert.AreEqual(intsNonGeneric.Length, intsGeneric.Length);
            Assert.AreEqual(intsGeneric.Length, ints.Length);

            for (int i = 0; i < intsNonGeneric.Length; i++)
            {
                Assert.AreEqual(intsNonGeneric[i], intsGeneric[i]);
                Assert.AreEqual(intsGeneric[i], ints[i]);
            }

            // Verify that wrong usage of GetValue throws.
            Assert.Throws<InvalidOperationException>(() =>
            {
                cfg["TestSection"]["IntArray"].GetValue(typeof(int[]));
            });
            Assert.Throws<InvalidOperationException>(() =>
            {
                cfg["TestSection"]["IntArray"].GetValue(typeof(int[][]));
            });
            Assert.Throws<InvalidOperationException>(() =>
            {
                cfg["TestSection"]["IntArray"].GetValue<int[]>();
            });
            Assert.Throws<InvalidOperationException>(() =>
            {
                cfg["TestSection"]["IntArray"].GetValue<int[][]>();
            });

            // Verify that wrong usage of GetValueArray throws.
            Assert.Throws<ArgumentException>(() =>
            {
                cfg["TestSection"]["IntArray"].GetValueArray(typeof(int[]));
            });
            Assert.Throws<ArgumentException>(() =>
            {
                cfg["TestSection"]["IntArray"].GetValueArray<int[]>();
            });
            Assert.Throws<ArgumentException>(() =>
            {
                cfg["TestSection"]["IntArray"].GetValueArray(typeof(int[][]));
            });
            Assert.Throws<ArgumentException>(() =>
            {
                cfg["TestSection"]["IntArray"].GetValueArray<int[][]>();
            });
        }

        [Test]
        public void SectionAdditionAndRemoval()
        {
            var cfg = new Configuration();

            var section1 = new Section("Section1");
            var section2 = new Section("Section2");

            cfg.Add(section1);
            cfg.Add(section2);
            cfg["Section3"]["Setting1"].SetValue(0);

            Assert.IsTrue(cfg.Contains(section1));
            Assert.IsTrue(cfg.Contains(section2));
            Assert.IsTrue(cfg.Contains("Section1"));
            Assert.IsTrue(cfg.Contains("Section2"));
            Assert.IsTrue(cfg.Contains("Section3"));
            Assert.IsTrue(cfg["Section3"].Contains("Setting1"));

            cfg.Add(new Section("Section1"));
            cfg.Add(new Section("Section1"));

            {
                var sections = cfg.GetSectionsNamed("Section1");
                int actualCount = 0;
                foreach (var sec in sections)
                {
                    Assert.AreEqual(sec.Name, "Section1");
                    actualCount++;
                }

                Assert.AreEqual(actualCount, 3);
            }

            Assert.IsTrue(cfg.Remove("Section1"));
            cfg.RemoveAllNamed("Section1");

            Assert.IsTrue(!cfg.Contains("Section1"));
        }

        [Test]
        public void SetValueOverload()
        {
            var cfg = new Configuration();

            object[] obj = new object[] { 1, 2, 3 };

            var setting = cfg["TestSection"]["TestSetting"];
            setting.SetValue(obj);

            // GetValue() should throw, because the setting is an array now.
            // It should notify us to use GetValueArray() instead.
            Assert.Throws<InvalidOperationException>(() =>
            {
                setting.GetValue(typeof(int));
            });

            // Now get the array object and check.
            object[] intsNonGeneric = setting.GetValueArray(typeof(int));
            int[] intsGeneric = setting.GetValueArray<int>();

            Assert.AreEqual(obj.Length, intsGeneric.Length);
            Assert.AreEqual(intsGeneric.Length, intsNonGeneric.Length);

            for (int i = 0; i < obj.Length; i++)
            {
                Assert.AreEqual(obj[i], intsGeneric[i]);
                Assert.AreEqual(intsGeneric[i], intsNonGeneric[i]);
            }
        }

        [Test]
        public void SaveAndLoadComments()
        {
            var cfgStr =
                "# Line1" + Environment.NewLine +
                "; Line2" + Environment.NewLine +
                "#" + Environment.NewLine +
                "# Line4" + Environment.NewLine +
                "[Section] # InlineComment1" + Environment.NewLine +
                "Setting = Value ; InlineComment2" + Environment.NewLine +
                Environment.NewLine +
                "# Line1   " + Environment.NewLine +
                "#Line2 " + Environment.NewLine +
                "## ###" + Environment.NewLine +
                ";Line4" + Environment.NewLine +
                "[Section2]" + Environment.NewLine +
                "Setting=\"Val;#ue\"# InlineComment3";

            var cfg = Configuration.LoadFromString(cfgStr);

            SaveAndLoadComments_Check(cfg);

            var filename = Path.GetTempFileName();
            try
            {
                cfg.SaveToFile(filename);
                cfg = Configuration.LoadFromFile(filename);
                SaveAndLoadComments_Check(cfg);
            }
            finally
            {
                if (File.Exists(filename))
                    File.Delete(filename);
            }
        }

        private static void SaveAndLoadComments_Check(Configuration cfg)
        {
            Assert.AreEqual(2, cfg.SectionCount);
            Assert.IsTrue(cfg.Contains("Section"));
            Assert.IsTrue(cfg.Contains("Section2"));
            Assert.IsTrue(cfg.Contains("Section", "Setting"));
            Assert.IsTrue(cfg.Contains("Section2", "Setting"));

            var section = cfg["Section"];
            var section2 = cfg["Section2"];

            Assert.IsNotNull(section.PreComment);
            Assert.IsNotNull(section2.PreComment);

            Assert.AreEqual(
                "Line1" + Environment.NewLine +
                "Line2" + Environment.NewLine +
                Environment.NewLine +
                "Line4",
                section.PreComment
                );

            Assert.AreEqual(
                "Line1" + Environment.NewLine +
                "Line2" + Environment.NewLine +
                "# ###" + Environment.NewLine +
                "Line4",
                section2.PreComment
                );

            Assert.AreEqual("InlineComment1", section.Comment);
            Assert.AreEqual("InlineComment2", section["Setting"].Comment);

            Assert.IsNull(section2.Comment);
            Assert.AreEqual("InlineComment3", section2["Setting"].Comment);

            Assert.AreEqual("Value", section["Setting"].StringValue);
            Assert.AreEqual("\"Val;#ue\"", section2["Setting"].StringValue);
        }

        [Test]
        public void ArrayParsing()
        {
            var cfg = new Configuration();
            var section = cfg["Section"];

            section["Setting1"].StringValue = "{1,2,3}";
            section["Setting2"].StringValue = "   {1,2,3}   ";
            section["Setting3"].StringValue = " d {1,2,3} d ";
            section["Setting4"].StringValue = "{ 1,2,   3  }";
            section["Setting5"].StringValue = "{ 123, 456, 789 }";
            section["Setting6"].StringValue = "{}";
            section["Setting7"].StringValue = "{,}";
            section["Setting8"].StringValue = "{13,}";
            section["Setting9"].StringValue = "{{1},{2},{3}}";
            section["Setting10"].StringValue = "{ {123}, 456, {{789}} }";
            section["Setting11"].StringValue = "{\"12,34\", 5678}";
            section["Setting12"].StringValue = "{\"{123}\", 456}";

            AssertArraysAreEqual(new[] { "1", "2", "3" }, section["Setting1"].StringValueArray);
            AssertArraysAreEqual(new[] { "1", "2", "3" }, section["Setting2"].StringValueArray);

            Assert.IsFalse(section["Setting3"].IsArray);
            Assert.AreEqual("d {1,2,3} d", section["Setting3"].StringValue);

            AssertArraysAreEqual(new[] { "1", "2", "3" }, section["Setting4"].StringValueArray);
            AssertArraysAreEqual(new[] { "123", "456", "789" }, section["Setting5"].StringValueArray);

            Assert.IsTrue(section["Setting6"].IsArray);
            Assert.AreEqual(0, section["Setting6"].ArraySize);

            Assert.IsFalse(section["Setting7"].IsArray);
            Assert.IsFalse(section["Setting8"].IsArray);

            AssertArraysAreEqual(new[] { "{1}", "{2}", "{3}" }, section["Setting9"].StringValueArray);
            AssertArraysAreEqual(new[] { "{123}", "456", "{{789}}" }, section["Setting10"].StringValueArray);
            AssertArraysAreEqual(new[] { "\"12,34\"", "5678" }, section["Setting11"].StringValueArray);
            AssertArraysAreEqual(new[] { "\"{123}\"", "456" }, section["Setting12"].StringValueArray);
        }

        private static void AssertArraysAreEqual(string[] expected, string[] actual)
        {
            Assert.AreEqual(expected.Length, actual.Length);
            for (int i = 0; i < expected.Length; ++i)
                Assert.AreEqual(expected[i], actual[i]);
        }
    }
}
