using System;
using Sever.Gridder.CommandSystem;
using Sever.Gridder.Data;
using Sever.Gridder.UI;
using UnityEngine;

namespace Sever.Gridder.Editor
{
    public class EditorController : MonoBehaviour
    {
        private static EditorController _instance;
        private static EditorController Instance => _instance ??= FindObjectOfType<EditorController>();
        
        [SerializeField] private KnobController _knobController;
        [SerializeField] private EditorScreen _editorScreen;


        public static void AddKnob(Vector3 position)
        {
            var command = new AddKnobCommand(Instance._knobController, position);
            CommandsManager.Add(command);
        }
        
        public static void DeleteKnob()
        {
            if (!Instance._knobController.LastSelectedKnob)
            {
                Debug.LogError($"no selected knob");
                return;
            }

            var command = new DeleteKnobCommand(Instance._knobController);
            CommandsManager.Add(command);
        }

        public static void DeleteAllKnobs(KnobController knobController)
        {
            var command = new DeleteAllKnobsCommand(knobController);
            CommandsManager.Add(command);
        }

        public static void SetKnobColor(KnobColor color, KnobColor previousColor)
        {
            var command = new SetKnobColorCommand(color, previousColor);
            CommandsManager.Add(command);
        }

        public static void SetKnobFinished()
        {
            if (!Instance._knobController.LastSelectedKnob)
            {
                return;
            }
            
            var command = new SetKnobFinishedCommand(Instance._knobController.LastSelectedKnob);
            CommandsManager.Add(command);
        }

        public static void SelectKnob(KnobButton knob)
        {
            var command = new SelectKnobCommand(Instance._knobController, knob);
            CommandsManager.Add(command);
        }

        public static void ChangeSettings(Project project, ProjectSettings settings, Action onApplied)
        {
            if (project.Settings.IsNull())
            {
                project.UpdateSettings(settings);
                onApplied?.Invoke();
                return;
            }
            
            var command = new ChangeSettingsCommand(project, settings, onApplied);
            CommandsManager.Add(command);
        }

        public static void Save()
        {
            Instance._editorScreen.Save();
        }
    }
}