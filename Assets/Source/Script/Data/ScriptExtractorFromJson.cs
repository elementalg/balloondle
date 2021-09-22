using System;
using Balloondle.Script.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Type = Balloondle.Script.Core.Type;

namespace Balloondle.Script.Data
{
    public class ScriptExtractorFromJson
    {
        private const string EntriesKey = "entries";
        private const string TypeKey = "type";
        private const string ObjectKey = "object";
        private const string Id = "entry_id";
        private const string DurationKey = "duration";
        
        private const string ExpireEventKey = "expire_event";
        private const string ExpireEventEnabledKey = "enabled";
        private const string ExpireEventValueKey = "value";
        
        private const string TextKey = "text";
        
        private const string CharacterDataKey = "character_data";
        private const string CharacterDataIdKey = "id";
        private const string CharacterDataNameKey = "name";

        public ScriptText FromJson(string json)
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

            ScriptText scriptText = new ScriptText();
            
            JArray entriesRoot = scriptRoot.Value<JArray>(EntriesKey);
            foreach (JToken entry in entriesRoot)
            {
                Entry deserializedEntry = DeserializeEntry(entry);
                 scriptText.Write(deserializedEntry);
            }

            return scriptText;
        }

        private Entry DeserializeEntry(JToken serializedEntry)
        {
            string type = serializedEntry.Value<string>(TypeKey);
            Type entryType = (Type)Enum.Parse(typeof(Type), type);
            serializedEntry = serializedEntry.Value<JObject>(ObjectKey);

            ExpireEvent expireEvent = DeserializeExpireEvent(serializedEntry.Value<JObject>(ExpireEventKey));
            
            switch (entryType)
            {
                case Type.Silence:
                    return new SilenceEntry(serializedEntry.Value<int>(Id),
                        serializedEntry.Value<float>(DurationKey),
                        expireEvent);
                case Type.Narrative:
                    return new NarrativeEntry(serializedEntry.Value<int>(Id),
                        serializedEntry.Value<float>(DurationKey),
                        expireEvent,
                        serializedEntry.Value<string>(TextKey));
                case Type.Character:
                    Character characterData =
                        DeserializeCharacterEntry(serializedEntry.Value<JObject>(CharacterDataKey));

                    return new CharacterEntry(serializedEntry.Value<int>(Id),
                        serializedEntry.Value<float>(DurationKey), expireEvent,
                        serializedEntry.Value<string>(TextKey),
                        characterData);
                default:
                    throw new InvalidOperationException("Unknown entry type detected.");
            }
        }

        private ExpireEvent DeserializeExpireEvent(JObject serializedExpireEvent)
        {
            bool enabled = serializedExpireEvent.Value<bool>(ExpireEventEnabledKey);
            string value = serializedExpireEvent.Value<string>(ExpireEventValueKey);

            return new ExpireEvent(enabled, value);
        }
        
        private Character DeserializeCharacterEntry(JObject serializedCharacterEntry)
        {
            ulong characterId = serializedCharacterEntry.Value<ulong>(CharacterDataIdKey);
            string characterName = serializedCharacterEntry.Value<string>(CharacterDataNameKey);

            return new Character(characterId, characterName);
        }
    }
}