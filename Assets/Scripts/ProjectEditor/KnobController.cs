using System.Collections.Generic;
using GridMapper.Data;
using GridMapper.UI;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GridMapper
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
            
            EventBus.ProjectSelected += OpenProject;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!eventData.hovered.Contains(_imagePanel) || _lastSelectedButton)
            {
                if (!_lastSelectedButton)
                {
                    return;
                }
                
                _lastSelectedButton.IsSelected = false;
                _lastSelectedButton = null;

                return;
            }

            var knob = Instantiate(_knobPrefab, eventData.position, Quaternion.identity, _knobsParent).GetComponent<KnobButton>();
            knob.Init(_project, _maxKnobPanelPositionX, SelectKnob, DeleteKnob);
            _knobs.Add(knob);
        }

        private void OpenProject(Project project)
        {
            if (_project != project)
            {
                // clear previous project
            }
            
            _project = project;
            
            if (project.Knobs is not {Count: > 0})
            {
                return;
            }
            
            foreach (KnobDto knobDto in project.Knobs)
            {
                var knob = Instantiate(_knobPrefab, _knobsParent).GetComponent<KnobButton>();
                var anchoredPosition = new Vector2(knobDto.x, knobDto.y);
                knob.Init(_project, knobDto.isFinished, anchoredPosition, _maxKnobPanelPositionX, SelectKnob, DeleteKnob);
                _knobs.Add(knob);
            }
        }
        
        private void SelectKnob(KnobButton knob)
        {
            if (_lastSelectedButton && _lastSelectedButton != knob)
            {
                _lastSelectedButton.IsSelected = false;
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