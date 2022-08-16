using System.Collections.Generic;
using System.Linq;
using Sever.Gridder.Data;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Sever.Gridder.Editor
{
    public class KnobController : MonoBehaviour, IInitializable, IPointerClickHandler
    {
        [SerializeField] private KnobColorSelector _colorSelector;
        [SerializeField] private GameObject _imagePanel;
        [SerializeField] private Transform _knobsParent;
        [SerializeField] private GameObject _knobPrefab;

        private readonly List<KnobButton> _knobs = new();
        private KnobButton _lastSelectedKnob;
        private Project _project;
        private KnobColor _knobColor;

        private const float MaxKnobPanelPositionX = 1100;


        public void Init()
        {
            _colorSelector.ColorSelected += SelectKnobColor;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.dragging)
            {
                return;
            }

            if (!eventData.hovered.Contains(_imagePanel) || _lastSelectedKnob)
            {
                if (!_lastSelectedKnob)
                {
                    return;
                }

                _lastSelectedKnob.SetSelected(false);
                _lastSelectedKnob = null;

                return;
            }

            var knob = Instantiate(_knobPrefab, eventData.position, Quaternion.identity, _knobsParent).GetComponent<KnobButton>();
            knob.Init(_project, _knobColor, MaxKnobPanelPositionX, SelectKnob);
            _knobs.Add(knob);
        }

        private void Update()
        {
            if (!_lastSelectedKnob)
            {
                return;
            }

            if (Input.GetKeyUp(KeyCode.Backspace))
            {
                DeleteKnob(_lastSelectedKnob);
                return;
            }

            if (Input.GetKeyUp(KeyCode.F))
            {
                _lastSelectedKnob.ToggleFinished();
            }
        }

        public void Save()
        {
            _project?.UpdateKnobs(_knobs.Select(x => x.GetKnobDto()).ToList());
        }

        public void Clear()
        {
            _project = null;
            _lastSelectedKnob = null;
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
                knob.Init(_project, knobDto, MaxKnobPanelPositionX, SelectKnob);
                _knobs.Add(knob);
            }
        }

        public void SelectKnob(KnobButton knob)
        {
            if (_lastSelectedKnob && _lastSelectedKnob != knob)
            {
                _lastSelectedKnob.SetSelected(false);
            }

            _lastSelectedKnob = knob;

            if (_lastSelectedKnob)
            {
                _lastSelectedKnob.transform.SetAsLastSibling();
            }
        }

        private void DeleteKnob(KnobButton knob)
        {
            _lastSelectedKnob = null;
            _knobs.Remove(knob);
            Destroy(knob.gameObject);
        }

        private void SelectKnobColor(KnobColor color)
        {
            _knobColor = color;

            if (_lastSelectedKnob)
            {
                _lastSelectedKnob.SetColor(color);
            }
        }
    }
}