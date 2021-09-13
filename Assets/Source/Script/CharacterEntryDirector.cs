using System;
using Balloondle.Script.Core;
using Balloondle.Script.Viewer;
using UnityEngine;

namespace Balloondle.Script
{
    public class CharacterEntryDirector : IEntryDirector
    {
        private CharacterEntry _entry;
        private CharacterEntryStyleReferences _styleReferences;
        private Canvas _canvas;

        private InOutController _characterInOut;
        private InOutController _messageInOut;
        
        public CharacterEntryDirector(CharacterEntry entry, EntryStyleComponent entryStyle, Canvas canvas)
        {
            _entry = entry;
            _styleReferences = GameObject.Instantiate(entryStyle.m_Prefab, canvas.transform)
                .GetComponent<CharacterEntryStyleReferences>();

            _characterInOut = _styleReferences.m_CharacterImage.GetComponent<InOutController>();
            _messageInOut = _styleReferences.m_MessageBox.GetComponent<InOutController>();

            _messageInOut.m_InEnd += () => _messageInOut.InText();
            _characterInOut.m_InEnd += () => _characterInOut.InText();
        }

        public void In()
        { 
            _characterInOut.In();
            _messageInOut.In();
        }

        public void Update()
        {
            
        }

        public void UpdateText(CharacterEntry updatedEntry)
        {
            if (_entry.CharacterData.Id != updatedEntry.CharacterData.Id)
            {
                throw new InvalidOperationException("Entry is not assigned to the same character.");
            }
            
            _styleReferences.m_MessageText.text = updatedEntry.Text;
            _entry = updatedEntry;
        }

        public void Out()
        {
            _characterInOut.Out();
            _characterInOut.OutText();
            _messageInOut.Out();
            _messageInOut.OutText();
            
            GameObject.Destroy(_styleReferences.gameObject, 1f);
        }
    }
}