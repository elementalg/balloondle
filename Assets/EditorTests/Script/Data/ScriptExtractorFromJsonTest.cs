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
                                 "          'duration': 1" +
                                 "      }" +
                                 "  }," +
                                 "  {" +
                                 "      'type': 'Narrative'," +
                                 "      'object':" +
                                 "      {" +
                                 "          'duration': 4," +
                                 "          'text': 'Too many credit cards...'" +
                                 "      }" +
                                 "  }," +
                                 "  {" +
                                 "      'type': 'Character'," +
                                 "      'object':" +
                                 "      {" +
                                 "          'duration': 4," +
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

            ScriptContainer extractedScriptContainer = _extractor.FromJson(exampleJson);
            
            Assert.True(extractedScriptContainer.HasNext());

            Entry entryBeingRead = extractedScriptContainer.ReadNext();
            
            Assert.IsInstanceOf<SilenceEntry>(entryBeingRead);
            SilenceEntry firstEntry = (SilenceEntry)entryBeingRead;
            Assert.True(Mathf.Approximately(1f, firstEntry.Duration));

            entryBeingRead = extractedScriptContainer.ReadNext();
            Assert.IsInstanceOf<NarrativeEntry>(entryBeingRead);
            NarrativeEntry secondEntry = (NarrativeEntry)entryBeingRead;
            Assert.True(Mathf.Approximately(4f, secondEntry.Duration));
            Assert.AreEqual("Too many credit cards...", ((NarrativeEntry)secondEntry).Text);

            entryBeingRead = extractedScriptContainer.ReadNext();
            Assert.IsInstanceOf<CharacterEntry>(entryBeingRead);
            CharacterEntry thirdEntry = (CharacterEntry)entryBeingRead;
            Assert.True(Mathf.Approximately(4f, thirdEntry.Duration));
            Assert.AreEqual("I DECLARE BANKRUPTCY", ((CharacterEntry)thirdEntry).Text);
            Assert.AreEqual(0, thirdEntry.CharacterData.Id);
            Assert.AreEqual("Michael Scott", thirdEntry.CharacterData.Name);
        }
    }
}