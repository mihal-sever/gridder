using System.Collections.Generic;
using System.Linq;
using Sever.Gridder.Data;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Sever.Gridder.Editor
{
    public class KnobController : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private GameObject _imagePanel;
        [SerializeField] private Transform _knobsParent;
        [SerializeField] private GameObject _knobPrefab;

        private readonly List<KnobButton> _knobs = new();
        public Project _project;
        private KnobColor _knobColor;
        private Button _cleanKnobsButton;

        private const float MaxKnobPanelPositionX = 1100;

        public KnobButton LastSelectedKnob { get; private set; }


        public void Init(Button cleanAllKnobsButton)
        {
            _cleanKnobsButton = cleanAllKnobsButton;
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

            EditorController.AddKnob(eventData.position);
        }

        public KnobButton AddKnob(Vector3 position)
        {
            var knob = Instantiate(_knobPrefab, position, Quaternion.identity, _knobsParent).GetComponent<KnobButton>();
            knob.Init(_project, _knobColor, MaxKnobPanelPositionX, EditorController.SelectKnob);
            _knobs.Add(knob);
            _cleanKnobsButton.interactable = true;
            return knob;
        }

        public void AddKnob(KnobDto knobDto)
        {
            var knob = Instantiate(_knobPrefab, _knobsParent).GetComponent<KnobButton>();
            knob.Init(_project, knobDto, MaxKnobPanelPositionX, EditorController.SelectKnob);
            _knobs.Add(knob);
            _cleanKnobsButton.interactable = true;
        }

        public void DeleteKnob(KnobButton knob)
        {
            LastSelectedKnob = null;
            _knobs.Remove(knob);
            _cleanKnobsButton.interactable = _knobs.Count > 0;
            Destroy(knob.gameObject);
        }

        public void Save()
        {
            _project?.UpdateKnobs(GetKnobsDto());
        }

        public List<KnobDto> GetKnobsDto()
        {
            return _knobs.Select(x => x.GetKnobDto()).ToList();
        }

        public void DeleteAllKnobs()
        {
            _cleanKnobsButton.interactable = false;
            LastSelectedKnob = null;
            foreach (KnobButton knob in _knobs)
            {
                Destroy(knob.gameObject);
            }

            _knobs.Clear();
        }

        public void Clear()
        {
            _project = null;
            DeleteAllKnobs();
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