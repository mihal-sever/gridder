using UnityEngine;
using UnityEngine.EventSystems;

namespace Sever.Gridder.UI
{
    public class HoverIndicatorEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        [SerializeField] private GameObject _hoveredIndicator;
        
        
        private void Awake()
        {
            _hoveredIndicator.SetActive(false);
        }

        private void OnDisable()
        {
            _hoveredIndicator.SetActive(false);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _hoveredIndicator.SetActive(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _hoveredIndicator.SetActive(false);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _hoveredIndicator.SetActive(false);
        }
    }
}