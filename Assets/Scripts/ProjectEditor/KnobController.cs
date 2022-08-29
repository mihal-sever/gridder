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
        private Project _project;
        private KnobColor _knobColor;

        private const float MaxKnobPanelPositionX = 1100;

        public KnobButton LastSelectedKnob { get; private set; }
        

        public void Init()
        {
            EventBus.ColorSelected += SetKnobColor;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.dragging)
            {
                return;
            }

            if (!eventData.hovered.Contains(_imagePanel) || LastSelectedKnob)
            {
                if (!LastSelectedKnob)
                {
                    return;
                }

                LastSelectedKnob.SetSelected(false);
                LastSelectedKnob = null;

                return;
            }

            Debug.LogError($"add knob");
            EditorController.AddKnob(eventData.position);
        }

        public KnobButton AddKnob(Vector3 position)
        {
            Debug.LogError($"add knob {_knobColor}");
            var knob = Instantiate(_knobPrefab, position, Quaternion.identity, _knobsParent).GetComponent<KnobButton>();
            knob.Init(_project, _knobColor, MaxKnobPanelPositionX, EditorController.SelectKnob);
            _knobs.Add(knob);
            return knob;
        }
        
        public void AddKnob(KnobDto knobDto)
        {
            var knob = Instantiate(_knobPrefab, _knobsParent).GetComponent<KnobButton>();
            knob.Init(_project, knobDto, MaxKnobPanelPositionX, EditorController.SelectKnob);
            _knobs.Add(knob);
        }
        
        public void DeleteKnob(KnobButton knob)
        {
            LastSelectedKnob = null;
            _knobs.Remove(knob);
            Destroy(knob.gameObject);
        }

        public void Save()
        {
            _project?.UpdateKnobs(_knobs.Select(x => x.GetKnobDto()).ToList());
        }

        public void Clear()
        {
            _project = null;
            LastSelectedKnob = null;
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
                AddKnob(knobDto);
            }
        }
        
        public void SelectKnob(KnobButton knob)
        {
            if (LastSelectedKnob && LastSelectedKnob != knob)
            {
                LastSelectedKnob.SetSelected(false);
            }

            LastSelectedKnob = knob;

            if (LastSelectedKnob)
            {
                LastSelectedKnob.transform.SetAsLastSibling();
            }
        }

        private void SetKnobColor(KnobColor color)
        {
            _knobColor = color;

            if (LastSelectedKnob)
            {
                LastSelectedKnob.Color = color;
            }
        }
    }
}