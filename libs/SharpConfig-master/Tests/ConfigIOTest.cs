// Copyright (c) 2013-2016 Cemalettin Dervis, MIT License.
// https://github.com/cemdervis/SharpConfig

using System.IO;
using SharpConfig;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public sealed class ConfigIOTest
    {
        private static Configuration CreateExampleConfig()
        {
            var cfg = new Configuration();
            cfg["TestSection"]["IntSetting1"].IntValue = 100;
            cfg["TestSection"]["IntSetting2"].IntValue = 200;
            cfg["TestSection"]["StringSetting1"].StringValue = "Test";
            return cfg;
        }

        private static void ValidateExampleConfig(Configuration cfg)
        {
            Assert.AreEqual(cfg["TestSection"]["IntSetting1"].IntValue, 100);
            Assert.AreEqual(cfg["TestSection"]["IntSetting2"].IntValue, 200);
            Assert.AreEqual(cfg["TestSection"]["StringSetting1"].StringValue, "Test");
        }

        [Test]
        public void WriteAndReadConfig_File()
        {
            var cfg = CreateExampleConfig();

            string filename = Path.GetTempFileName();

            cfg.SaveToFile(filename);
            FileAssert.Exists(filename, "Failed to create the test config file.");

            cfg = Configuration.LoadFromFile(filename);
            File.Delete(filename);

            ValidateExampleConfig(cfg);
        }

        [Test]
        public void WriteAndReadConfig_Stream()
        {
            var cfg = CreateExampleConfig();

            var stream = new MemoryStream();
            cfg.SaveToStream(stream);

            stream.Position = 0;

            cfg = Configuration.LoadFromStream(stream);
            stream.Dispose();

            ValidateExampleConfig(cfg);
        }

        [Test]
        public void WriteAndReadConfig_BinaryFile()
        {
            var cfg = CreateExampleConfig();

            string filename = Path.GetTempFileName();

            cfg.SaveToBinaryFile(filename);
            FileAssert.Exists(filename, "Failed to create the test config file.");

            cfg = Configuration.LoadFromBinaryFile(filename);
            File.Delete(filename);

            ValidateExampleConfig(cfg);
        }

        [Test]
        public void WriteAndReadConfig_BinaryStream()
        {
            var cfg = CreateExampleConfig();

            var stream = new MemoryStream();
            cfg.SaveToBinaryStream(stream);

            stream.Position = 0;

            cfg = Configuration.LoadFromBinaryStream(stream);
            stream.Dispose();

            ValidateExampleConfig(cfg);
        }
    }
}
