using System;
using UnityEngine;
using UnityEngine.UI;

namespace Sever.Gridder.UI
{
    public class PopUp : MonoBehaviour, IInitializable
    {
        private static PopUp _instance;
        public static PopUp Instance => _instance ??= FindObjectOfType<PopUp>(true);

        [SerializeField] private Button _yes;
        [SerializeField] private Button _no;

        
        public void Init()
        {
            gameObject.SetActive(false);
        }

        public void ShowPopUp(Action yesCallback)
        {
            _yes.onClick.RemoveAllListeners();
            _no.onClick.RemoveAllListeners();

            _yes.onClick.AddListener(()=>
            {
                yesCallback();
                BlurOverlay.Instance.Deactivate();
                gameObject.SetActive(false);
            });
            
            _no.onClick.AddListener(()=>
            {
                BlurOverlay.Instance.Deactivate();
                gameObject.SetActive(false);
            });

            gameObject.SetActive(true);
            BlurOverlay.Instance.Activate();
        }
    }
}