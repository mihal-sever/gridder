using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DefaultNamespace
{
    public class KnobController : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private GameObject _imagePanel;
        [SerializeField] private Transform _knobsParent;
        [SerializeField] private GameObject _knobPrefab;

        private readonly List<KnobButton> _knobs = new();
        private KnobButton _lastSelectedButton;
        private float _maxKnobPanelPositionX;
        

        private void Awake()
        {
            _maxKnobPanelPositionX = _imagePanel.GetComponent<RectTransform>().rect.size.x - 200;
            
            EventBus.UserInputValidated += () => { enabled = true;};
            EventBus.ProjectLoaded += data => RestoreProject(data.knobs);
            
            enabled = false;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!eventData.hovered.Contains(_imagePanel) || _lastSelectedButton)
            {
                if (_lastSelectedButton)
                {
                    _lastSelectedButton.IsSelected = false;
                    _lastSelectedButton = null;
                }

                return;
            }

            var knob = Instantiate(_knobPrefab, eventData.position, Quaternion.identity, _knobsParent).GetComponent<KnobButton>();
            knob.Init(_maxKnobPanelPositionX, OnButtonSelected, OnButtonDeleted);
            _knobs.Add(knob);
        }
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                SaveProject();
            }
        }
        
        private void OnApplicationQuit()
        {
            SaveProject();
        }

        private void SaveProject()
        {
            var knobsDto = _knobs.Select(x => new KnobDto()
            {
                isFinished = x.IsFinished,
                x = x.AnchoredPosition.x,
                y = x.AnchoredPosition.y
            }).ToList();

            var projectDto = new ProjectDto()
            {
                canvasWidth = ProjectData.CanvasWidth,
                gridStepMm = ProjectData.GridStepMm,
                imagePath = ProjectData.ImagePath,
                knobs = knobsDto
            };
            
            FileManager.SaveProject(projectDto);
        }

        private void OnButtonSelected(KnobButton knob)
        {
            if (_lastSelectedButton && _lastSelectedButton != knob)
            {
                _lastSelectedButton.IsSelected = false;
            }

            _lastSelectedButton = knob;
        }
        
        private void OnButtonDeleted(KnobButton knob)
        {
            _lastSelectedButton = null;
            _knobs.Remove(knob);
            Destroy(knob.gameObject);
        }

        private void RestoreProject(List<KnobDto> knobs)
        {
            foreach (KnobDto knobDto in knobs)
            {
                var knob = Instantiate(_knobPrefab, _knobsParent).GetComponent<KnobButton>();
                var anchoredPosition = new Vector2(knobDto.x, knobDto.y);
                knob.Init(knobDto.isFinished, anchoredPosition, _maxKnobPanelPositionX, OnButtonSelected, OnButtonDeleted);
                _knobs.Add(knob);
            }
        }
    }
}