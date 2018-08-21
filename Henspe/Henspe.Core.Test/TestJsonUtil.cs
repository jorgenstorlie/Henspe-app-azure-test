using System;
using Henspe.Core.Util;
using Xunit;

namespace Henspe.Core.Test
{
    public class TestJsonUtil
    {
        [Fact]
        public void TestJsonUtil_CheckForError()
        {
            string json;

            json = "{ \"glossary\": { \"title\": \"example glossary\", \"GlossDiv\": {\"title\": \"S\", \"error_message\": \"tester\" }}}";

            Assert.True(JsonUtil.CheckForError(json) == "tester");

            json = "{ \"glossary\": { \"title\": \"example glossary\", \"GlossDiv\": {\"title\": \"S\", \"error_message\": \"tester\" }}}";

            Assert.False(JsonUtil.CheckForError(json) == "test");

            json = "{ \"glossary\": { \"title\": \"example glossary\", \"GlossDiv\": {\"title\": \"S\" }}}";

            Assert.True(JsonUtil.CheckForError(json) == null);
        }
    }
}
