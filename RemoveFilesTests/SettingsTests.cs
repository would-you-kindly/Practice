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
            var parser = new Parser();
            var cmd = parser.ParseArgs(new string[] { });
            cmd.Validate();

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
            var parser = new Parser();
            var cmd = parser.ParseArgs(new string[] { Settings.LogCommand, "123" });            
        }

        [TestMethod]
        public void ParseArgsLogTrueTest()
        {
            var parser = new Parser();
            var cmd = parser.ParseArgs(new string[] { Settings.LogCommand });
            Assert.AreEqual(true, cmd.Log);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ParseArgsArgumentValueMissingTest()
        {
            var parser = new Parser();
            var cmd = parser.ParseArgs(new string[] { Settings.LogCommand, true.ToString() });            
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ParseArgsArgumentsValuesMissingTest()
        {
            var parser = new Parser();
            var cmd = parser.ParseArgs(new string[] { Settings.LogCommand, Settings.ConfirmationCommand, Settings.HelpCommand });
        }
    }
}