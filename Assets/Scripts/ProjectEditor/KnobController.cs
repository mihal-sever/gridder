using System.Collections.Generic;
using Sever.Gridder.Data;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Sever.Gridder.Editor
{
    public class KnobController : MonoBehaviour, IInitializable, IPointerClickHandler
    {
        [SerializeField] private GameObject _imagePanel;
        [SerializeField] private Transform _knobsParent;
        [SerializeField] private GameObject _knobPrefab;

        private readonly List<KnobButton> _knobs = new();
        private KnobButton _lastSelectedButton;
        private float _maxKnobPanelPositionX;
        private Project _project;
        

        public void Init()
        {
            _maxKnobPanelPositionX = _imagePanel.GetComponent<RectTransform>().rect.size.x - 200;
            
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!eventData.hovered.Contains(_imagePanel) || _lastSelectedButton)
            {
                if (!_lastSelectedButton)
                {
                    return;
                }
                
                _lastSelectedButton.SetSelected(false);
                _lastSelectedButton = null;

                return;
            }

            var knob = Instantiate(_knobPrefab, eventData.position, Quaternion.identity, _knobsParent).GetComponent<KnobButton>();
            knob.Init(_project, _maxKnobPanelPositionX, SelectKnob);
            _knobs.Add(knob);
        }

        private void Update()
        {
            if (!_lastSelectedButton)
            {
                return;
            }

            if (Input.GetKeyUp(KeyCode.Backspace))
            {
                DeleteKnob(_lastSelectedButton);
                return;
            }

            if (Input.GetKeyUp(KeyCode.F))
            {
                _lastSelectedButton.ToggleFinished();
            }
        }
        
        public void Clear()
        {
            _project = null;
            _lastSelectedButton = null;
            foreach (KnobButton knob in _knobs)
            {
                Destroy(knob.gameObject);
            }
            _knobs.Clear();
        }

        public void SetProject(Project project)
        {
            if (_project != project)
            {
                Clear();
            }
            
            _project = project;
            
            if (_project.Knobs is not {Count: > 0})
            {
                return;
            }
            
            foreach (KnobDto knobDto in _project.Knobs)
            {
                var knob = Instantiate(_knobPrefab, _knobsParent).GetComponent<KnobButton>();
                var anchoredPosition = new Vector2(knobDto.x, knobDto.y);
                knob.Init(_project, knobDto.isFinished, anchoredPosition, _maxKnobPanelPositionX, SelectKnob);
                _knobs.Add(knob);
            }
        }
        
        private void SelectKnob(KnobButton knob)
        {
            if (_lastSelectedButton && _lastSelectedButton != knob)
            {
                _lastSelectedButton.SetSelected(false);
            }

            _lastSelectedButton = knob;
        }
        
        private void DeleteKnob(KnobButton knob)
        {
            _lastSelectedButton = null;
            _knobs.Remove(knob);
            Destroy(knob.gameObject);
        }
    }
}