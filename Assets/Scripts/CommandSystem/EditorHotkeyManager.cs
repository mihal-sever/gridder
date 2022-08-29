using System;
using System.Collections.Generic;
using System.Linq;
using Sever.Gridder.CommandSystem;
using UnityEngine;

namespace Sever.Gridder.Editor
{
    public class EditorHotkeyManager : MonoBehaviour
    {
        private struct HotkeyBind
        {
            public bool IsFired { get; private set; }

            public int KeyCodesCount { get; }

            private readonly List<KeyCode> _keyCodes;

            private readonly Action _onHotkeyPress;

            public HotkeyBind(List<KeyCode> codes, Action onPress)
            {
                KeyCodesCount = codes.Count;
                _keyCodes = codes;
                _onHotkeyPress = onPress;
                IsFired = false;
            }

            public void Evaluate()
            {
                foreach (KeyCode key in _keyCodes)
                {
                    if (GetKeyPress(key))
                    {
                        continue;
                    }

                    IsFired = false;

                    return;
                }

                if (IsFired)
                {
                    return;
                }

                IsFired = true;
                Debug.LogError($"{_keyCodes[0]} {_keyCodes[1]}");
                _onHotkeyPress.Invoke();
            }


            public bool Equals(HotkeyBind other)
            {
                if (other._keyCodes.Count != _keyCodes.Count)
                {
                    return false;
                }

                foreach (KeyCode code in _keyCodes)
                {
                    if (!other._keyCodes.Contains(code))
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        private static EditorHotkeyManager _instance;
        public static EditorHotkeyManager Instance => _instance ??= FindObjectOfType<EditorHotkeyManager>();

        private static readonly List<KeyCode> _modeKeys = new()
        {
            KeyCode.RightShift,
            KeyCode.LeftShift,
            KeyCode.RightControl,
            KeyCode.LeftControl,
            KeyCode.RightAlt,
            KeyCode.LeftAlt,
            KeyCode.RightCommand,
            KeyCode.LeftCommand
        };

        private List<HotkeyBind> _binds = new();
        

        private void Start()
        {
            if (FindObjectsOfType<EditorHotkeyManager>().Length > 1)
            {
                Debug.LogError("there's more than one HotkeyManager present");
            }
            
            enabled = false;
            
            // Hotkey for change tool
            AddHotkeyBind(new List<KeyCode> {KeyCode.LeftControl, KeyCode.Z}, CommandsManager.Undo);
            AddHotkeyBind(new List<KeyCode> {KeyCode.LeftCommand, KeyCode.Z}, CommandsManager.Undo);

            AddHotkeyBind(new List<KeyCode> {KeyCode.LeftControl, KeyCode.S}, EditorController.Save);
            AddHotkeyBind(new List<KeyCode> {KeyCode.LeftCommand, KeyCode.S}, EditorController.Save);

            AddHotkeyBind(new List<KeyCode> {KeyCode.LeftControl, KeyCode.D}, EditorController.DeleteKnob);
            AddHotkeyBind(new List<KeyCode> {KeyCode.LeftCommand, KeyCode.D}, EditorController.DeleteKnob);

            AddHotkeyBind(new List<KeyCode> {KeyCode.LeftControl, KeyCode.F}, EditorController.SetKnobFinished);
            AddHotkeyBind(new List<KeyCode> {KeyCode.LeftCommand, KeyCode.F}, EditorController.SetKnobFinished);


            _binds = _binds.OrderByDescending(x => x.KeyCodesCount).ToList();
        }

        private void Update()
        {
            foreach (HotkeyBind bind in _binds)
            {
                bind.Evaluate();
                if (bind.IsFired)
                {
                    return;
                }
            }
        }
        
        private static bool GetKeyPress(KeyCode keyCode)
        {
            return _modeKeys.Contains(keyCode) ? Input.GetKey(keyCode) : Input.GetKeyDown(keyCode);
        }

        private void AddHotkeyBind(List<KeyCode> codes, Action onPress)
        {
            if (onPress == null)
            {
                throw new ArgumentNullException("onPress", "hotkey action cannot be null");
            }

            if (codes == null || codes.Count == 0)
            {
                throw new ArgumentNullException("codes", "keycode list cannot be null or contain 0 elements");
            }

            HotkeyBind newBind = new HotkeyBind(codes, onPress);

            foreach (HotkeyBind bind in _binds)
            {
                if (bind.Equals(newBind))
                {
                    throw new Exception("bind already exists");
                }
            }

            _binds.Add(newBind);
        }
    }
}