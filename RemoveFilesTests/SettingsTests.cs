using Microsoft.VisualStudio.TestTools.UnitTesting;
using RemoveFiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoveFiles.Tests
{
    [TestClass]
    public class SettingsTests
    {
        [TestMethod]
        public void ParseArgsAllDefaultsTest()
        {
            var settings = new Settings();
            var cmd = settings.ParseArgs(new string[] { });

            Assert.AreEqual(Command.DefaultConfirmation, cmd.Confirmation);
            Assert.AreEqual(Command.DefaultConnectionString, cmd.ConnectionString);
            Assert.AreEqual(Command.DefaultLog, cmd.Log);
            Assert.AreEqual(Command.DefaultPath, cmd.Path);
            Assert.AreEqual(Command.DefaultPrimaryKeyFieldName, cmd.PrimaryKeyFieldName);
            Assert.AreEqual(Command.DefaultTableName, cmd.TableName);
            Assert.AreEqual(Command.DefaultUrlFieldName, cmd.UrlFieldName);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]        
        public void ParseArgsLogInvalidTest()
        {
            var settings = new Settings();
            var cmd = settings.ParseArgs(new string[] { Settings.LogCommand, "123" });            
        }

        [TestMethod]
        public void ParseArgsLogTrueTest()
        {
            var settings = new Settings();
            var cmd = settings.ParseArgs(new string[] { Settings.LogCommand, "true" });
            Assert.AreEqual("true", cmd.Log);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ParseArgsArgumentValueMissingTest()
        {
            var settings = new Settings();
            var cmd = settings.ParseArgs(new string[] { Settings.LogCommand });            
        }

    }
}