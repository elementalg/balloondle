using System;
using Balloondle.Script.Core;
using Balloondle.Script.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using UnityEngine;

namespace EditorTests.Script.Data
{
    public class ScriptExtractorFromJsonTest
    {
        private ScriptExtractorFromJson _extractor;

        [SetUp]
        public void Initialize()
        {
            _extractor = new ScriptExtractorFromJson();
        }
        
        [Test]
        public void IsNewtonsoftJsonValidatingOurSuppositions()
        {
            string exampleJson = "{'title': 'whatever', 'author': 'ee', 'container': {'a': 1, 'b': 2}}";
            
            JObject example = JsonConvert.DeserializeObject<JObject>(exampleJson);
            
            Assert.NotNull(example);
            Assert.AreEqual("whatever", example.Value<string>("title"));
            Assert.AreEqual("ee", example.Value<string>("author"));

            JObject container = example.Value<JObject>("container");
            Assert.IsInstanceOf<JObject>(container);
        }

        [Test]
        public void ExceptionOnEmptyJsonString()
        {
            Assert.Throws<ArgumentException>(() => _extractor.FromJson(""));
        }

        [Test]
        public void ExtractsExpectedScriptFromJsonString()
        {
            string exampleJson = "{ 'title': 'Template Script', 'author': 'Le Baguette', 'entries':" +
                                 "[" +
                                 "  {" +
                                 "      'type': 'Silence'," +
                                 "      'object':" +
                                 "      {" +
                                 "          'entry_id': 1," +
                                 "          'duration': 1," +
                                 "          'expire_event':" +
                                 "          {" +
                                 "              'enabled': false," +
                                 "              'value': ''" +
                                 "          }" +
                                 "      }" +
                                 "  }," +
                                 "  {" +
                                 "      'type': 'Narrative'," +
                                 "      'object':" +
                                 "      {" +
                                 "          'entry_id': 2," +
                                 "          'duration': 4," +
                                 "          'expire_event':" +
                                 "          {" +
                                 "              'enabled': false," +
                                 "              'value': ''" +
                                 "          }," +
                                 "          'text': 'Too many credit cards...'" +
                                 "      }" +
                                 "  }," +
                                 "  {" +
                                 "      'type': 'Character'," +
                                 "      'object':" +
                                 "      {" +
                                 "          'entry_id': 3," +
                                 "          'duration': 4," +
                                 "          'expire_event':" +
                                 "          {" +
                                 "              'enabled': true," +
                                 "              'value': 'S04_E08_09:02'" +
                                 "          }," +
                                 "          'text': 'I DECLARE BANKRUPTCY'," +
                                 "          'character_data':" +
                                 "          {" +
                                 "              'id': 0," +
                                 "              'name':'Michael Scott'" +
                                 "          }" +
                                 "      }" +
                                 "  }" +
                                 "]" +
                                 "}";

            ScriptText extractedScriptText = _extractor.FromJson(exampleJson);
            
            Assert.True(extractedScriptText.HasNext());

            Entry entryBeingRead = extractedScriptText.ReadNext();
            
            Assert.IsInstanceOf<SilenceEntry>(entryBeingRead);
            SilenceEntry firstEntry = (SilenceEntry)entryBeingRead;
            Assert.AreEqual(1, firstEntry.Id);
            Assert.True(Mathf.Approximately(1f, firstEntry.Duration));
            Assert.False(firstEntry.Expire.Enabled);

            entryBeingRead = extractedScriptText.ReadNext();
            Assert.IsInstanceOf<NarrativeEntry>(entryBeingRead);
            NarrativeEntry secondEntry = (NarrativeEntry)entryBeingRead;
            Assert.AreEqual(2, secondEntry.Id);
            Assert.True(Mathf.Approximately(4f, secondEntry.Duration));
            Assert.False(firstEntry.Expire.Enabled);
            Assert.AreEqual("Too many credit cards...", (secondEntry).Text);

            entryBeingRead = extractedScriptText.ReadNext();
            Assert.IsInstanceOf<CharacterEntry>(entryBeingRead);
            CharacterEntry thirdEntry = (CharacterEntry)entryBeingRead;
            Assert.AreEqual(3, thirdEntry.Id);
            Assert.True(Mathf.Approximately(4f, thirdEntry.Duration));
            Assert.True(thirdEntry.Expire.Enabled);
            Assert.AreEqual("S04_E08_09:02", thirdEntry.Expire.Value);
            Assert.AreEqual("I DECLARE BANKRUPTCY", thirdEntry.Text);
            Assert.AreEqual(0, thirdEntry.CharacterData.Id);
            Assert.AreEqual("Michael Scott", thirdEntry.CharacterData.Name);
        }
    }
}