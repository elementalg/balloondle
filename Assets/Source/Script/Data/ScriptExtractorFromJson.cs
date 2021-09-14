using System;
using Balloondle.Script.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Balloondle.Script.Data
{
    public class ScriptExtractorFromJson
    {
        private const string EntriesKey = "entries";
        private const string EntryTypeKey = "type";
        private const string EntryObjectKey = "object";
        private const string EntryDurationKey = "duration";
        
        private const string EntryExpireEventKey = "expire_event";
        private const string EntryExpireEventEnabledKey = "enabled";
        private const string EntryExpireEventValueKey = "value";
        
        private const string EntryTextKey = "text";
        
        private const string EntryCharacterDataKey = "character_data";
        private const string EntryCharacterDataIdKey = "id";
        private const string EntryCharacterDataNameKey = "name";

        public ScriptContainer FromJson(string json)
        {
            if (json == null)
            {
                throw new ArgumentException("Expected non-null json string.");
            }

            if (json.Length == 0)
            {
                throw new ArgumentException("Expected non-empty json string.");
            }

            JObject scriptRoot = JsonConvert.DeserializeObject<JObject>(json);

            ScriptContainer scriptContainer = new ScriptContainer();
            
            JArray entriesRoot = scriptRoot.Value<JArray>(EntriesKey);
            foreach (JToken entry in entriesRoot)
            {
                Entry deserializedEntry = DeserializeEntry(entry);
                scriptContainer.Write(deserializedEntry);
            }

            return scriptContainer;
        }

        private Entry DeserializeEntry(JToken serializedEntry)
        {
            string type = serializedEntry.Value<string>(EntryTypeKey);
            EntryType entryType = (EntryType)Enum.Parse(typeof(EntryType), type);
            serializedEntry = serializedEntry.Value<JObject>(EntryObjectKey);

            ExpireEvent expireEvent = DeserializeExpireEvent(serializedEntry.Value<JObject>(EntryExpireEventKey));
            
            switch (entryType)
            {
                case EntryType.Silence:
                    return new SilenceEntry(serializedEntry.Value<float>(EntryDurationKey), expireEvent);
                case EntryType.Narrative:
                    return new NarrativeEntry(serializedEntry.Value<float>(EntryDurationKey), expireEvent,
                        serializedEntry.Value<string>(EntryTextKey));
                case EntryType.Character:
                    Character characterData =
                        DeserializeCharacterEntry(serializedEntry.Value<JObject>(EntryCharacterDataKey));

                    return new CharacterEntry(serializedEntry.Value<float>(EntryDurationKey), expireEvent,
                        serializedEntry.Value<string>(EntryTextKey),
                        characterData);
                default:
                    throw new InvalidOperationException("Unknown entry type detected.");
            }
        }

        private ExpireEvent DeserializeExpireEvent(JObject serializedExpireEvent)
        {
            bool enabled = serializedExpireEvent.Value<bool>(EntryExpireEventEnabledKey);
            string value = serializedExpireEvent.Value<string>(EntryExpireEventValueKey);

            return new ExpireEvent(enabled, value);
        }
        
        private Character DeserializeCharacterEntry(JObject serializedCharacterEntry)
        {
            ulong characterId = serializedCharacterEntry.Value<ulong>(EntryCharacterDataIdKey);
            string characterName = serializedCharacterEntry.Value<string>(EntryCharacterDataNameKey);

            return new Character(characterId, characterName);
        }
    }
}