using System;
using System.Collections.Generic;
using Balloondle.Script.Core;
using Balloondle.Script.Viewer;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.Script
{
    [CustomEditor(typeof(ScriptStyle))]
    public class ScriptStyleInspector : UnityEditor.Editor
    {
        private const string ApplyChangesButton = "ApplyChangesButton";
        private const string NewComponentButton = "NewComponentButton";
        private const string ComponentsList = "ComponentsList";

        private const string NewComponentsListItem = "NewComponentsListItem";

        private const string ComponentDeleteButton = "ComponentDeleteButton";

        private VisualElement _inspector;
        private VisualTreeAsset _componentAsset;

        private ScrollView _componentsList;

        private readonly Dictionary<Foldout, EntryStyleComponent> _components =
            new Dictionary<Foldout, EntryStyleComponent>();

        private ulong _componentsListItemNextId;

        public override VisualElement CreateInspectorGUI()
        {
            _inspector = new VisualElement();

            VisualTreeAsset visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(
                AssetDatabase.GUIDToAssetPath("eef3e30d67ee87646a8b3f162a261bc3"));
            visualTree.CloneTree(_inspector);

            _componentAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(
                AssetDatabase.GUIDToAssetPath("a8bc05c8b8f7b6148987a229d8b1dbe5"));

            FindComponentsList();
            LoadExistingComponents();

            AttachCallbacksToElements();

            return _inspector;
        }

#nullable enable
        private static VisualElement? GetElementByNameFromParentRecursively(VisualElement parent, string elementName)
        {
            foreach (VisualElement child in parent.Children())
            {
                if (child.name.Equals(elementName, StringComparison.InvariantCultureIgnoreCase))
                {
                    return child;
                }

                if (child.childCount > 0)
                {
                    VisualElement? recursive = GetElementByNameFromParentRecursively(child, elementName);

                    if (recursive != null)
                    {
                        return recursive;
                    }
                }
            }

            return null;
        }
#nullable disable

        private void FindComponentsList()
        {
            VisualElement listElement = GetElementByNameFromParentRecursively(_inspector, ComponentsList);

            if (!(listElement is ScrollView))
            {
                throw new InvalidOperationException("ComponentsList is not a ListView.");
            }

            _componentsList = (ScrollView)listElement;
            listElement.RegisterCallback<MouseDownEvent>((a1) => { }, TrickleDown.TrickleDown);
        }

        private void LoadExistingComponents()
        {
            if (((ScriptStyle)target)._components != null)
            {
               foreach (EntryStyleComponent styleComponent in ((ScriptStyle)target)._components)
               {
                   Foldout foldout = CloneFromVisualTreeAComponentsFoldout();
                   foldout.name = $"ComponentsListItem{_componentsListItemNextId}";
                   foldout.text = $"#{_componentsListItemNextId} - Components";
                   _componentsListItemNextId++;
   
                   LoadValuesIntoEntryStyleComponent(foldout, styleComponent);
                   BindFieldsToEntryStyleComponent(foldout, styleComponent);
                   AttachDeleteCapacityToDeleteButton(foldout);
   
                   _componentsList.Add(foldout);
                   _components.Add(foldout, styleComponent);
               }
            }
        }

        private void LoadValuesIntoEntryStyleComponent(Foldout foldout, EntryStyleComponent styleComponent)
        {
            TextField componentName = (TextField)GetElementByNameFromParentRecursively(
                                          foldout, "ComponentName") ??
                                      throw new NullReferenceException("Could not find component's name field.");
            componentName.value = styleComponent.Name;
            
            EnumField componentEntryType = (EnumField)GetElementByNameFromParentRecursively(
                                               foldout, "ComponentEntryType") ??
                                           throw new NullReferenceException("Could not find component's entry type.");
            componentEntryType.Init(EntryType.Silence);
            componentEntryType.value = styleComponent.EntryType;
            
            ObjectField componentPrefab = (ObjectField)GetElementByNameFromParentRecursively(
                                              foldout, "ComponentPrefab") ??
                                          throw new NullReferenceException("Could not find component's prefab field.");
            componentPrefab.objectType = typeof(GameObject);
            componentPrefab.value = styleComponent.Prefab;
        }
        
        private void AttachCallbacksToElements()
        {
            AttachCallbackToApplyChangesButton();
            AttachCallbackToNewComponentButton();
        }

        private void AttachCallbackToApplyChangesButton()
        {
            VisualElement applyChangesButton = GetElementByNameFromParentRecursively(_inspector, ApplyChangesButton) ??
                                               throw new InvalidOperationException(
                                                   "Couldn't find apply changes button by name.");

            if (!(applyChangesButton is ToolbarButton button))
            {
                throw new InvalidOperationException("ApplyChangesButton is not a ToolbarButton");
            }

            button.clicked += SynchronizeComponentsWithScriptStyle;
        }

        private void SynchronizeComponentsWithScriptStyle()
        {
            ScriptStyle targetStyle = (ScriptStyle)target;
            targetStyle._components.Clear();

            foreach (EntryStyleComponent entryStyleComponent in _components.Values)
            {
                EntryStyleComponent styleComponent = new EntryStyleComponent(
                    entryStyleComponent.Name,
                    entryStyleComponent.EntryType,
                    entryStyleComponent.Prefab);

                targetStyle._components.Add(styleComponent);
            }

            targetStyle.DebugComponents();
        }

        private void AttachCallbackToNewComponentButton()
        {
            VisualElement newComponentButton = GetElementByNameFromParentRecursively(_inspector, NewComponentButton) ??
                                               throw new InvalidOperationException(
                                                   "Couldn't find new component button by name.");

            if (!(newComponentButton is ToolbarButton button))
            {
                throw new InvalidOperationException("NewComponentButton is not a ToolbarButton.");
            }

            button.clicked += CreateComponentListItem;
        }

        private void CreateComponentListItem()
        {
            Foldout foldout = CloneFromVisualTreeAComponentsFoldout();

            foldout.name = $"ComponentsListItem{_componentsListItemNextId}";
            foldout.text = $"#{_componentsListItemNextId} - Components";
            _componentsListItemNextId++;

            EntryStyleComponent entryStyleComponent = new EntryStyleComponent();

            BindFieldsToEntryStyleComponent(foldout, entryStyleComponent);
            AttachDeleteCapacityToDeleteButton(foldout);

            _componentsList.Add(foldout);
            _components.Add(foldout, entryStyleComponent);
        }

        private Foldout CloneFromVisualTreeAComponentsFoldout()
        {
            VisualElement componentListItem = new VisualElement();
            _componentAsset.CloneTree(componentListItem);

            Foldout foldout = (Foldout)GetElementByNameFromParentRecursively(
                                  componentListItem, NewComponentsListItem) ??
                              throw new NullReferenceException("Unexpected null value assigned to 'foldout'.");

            return foldout;
        }

        private void BindFieldsToEntryStyleComponent(Foldout foldout,
            EntryStyleComponent entryStyleComponent)
        {
            TextField componentName = (TextField)GetElementByNameFromParentRecursively(
                                          foldout, "ComponentName") ??
                                      throw new NullReferenceException("Could not find component's name field.");
            componentName.RegisterCallback((FocusOutEvent focusOut) =>
            {
                entryStyleComponent.Name = componentName.text;
            });
            
            EnumField componentEntryType = (EnumField)GetElementByNameFromParentRecursively(
                                               foldout, "ComponentEntryType") ??
                                           throw new NullReferenceException("Could not find component's entry type.");
            componentEntryType.Init(EntryType.Silence);
            componentEntryType.RegisterCallback((FocusOutEvent focusOut) =>
            {
                entryStyleComponent.EntryType = (EntryType)componentEntryType.value;
            });
            
            ObjectField componentPrefab = (ObjectField)GetElementByNameFromParentRecursively(
                                              foldout, "ComponentPrefab") ??
                                          throw new NullReferenceException("Could not find component's prefab field.");
            componentPrefab.objectType = typeof(GameObject);
            componentPrefab.RegisterCallback((FocusOutEvent focusOut) =>
            {
                entryStyleComponent.Prefab = (GameObject)componentPrefab.value;
            });
        }

        private void AttachDeleteCapacityToDeleteButton(Foldout foldoutOfTheComponent)
        {
            Button deleteButton = (Button)GetElementByNameFromParentRecursively(
                                      foldoutOfTheComponent, ComponentDeleteButton) ??
                                  throw new NullReferenceException(
                                      "Unexpected null value assigned to 'deleteButton'.");

            deleteButton.clicked += () => { DeleteComponentListItem(foldoutOfTheComponent); };
        }

        private void DeleteComponentListItem(Foldout foldoutOfTheComponent)
        {
            _components.Remove(foldoutOfTheComponent);
            _componentsList.Remove(foldoutOfTheComponent);
        }
    }
}