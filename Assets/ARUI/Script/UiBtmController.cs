using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ARUI {
    public class UiBtmController : MonoBehaviour, UiInitializer {
        [SerializeField] protected CanvasGroup baseCanvas;
        [SerializeField] protected CanvasGroup objCanvas;
        protected bool isActive;

        public bool IsActive() => isActive;

        public bool Initialize () {
            ShowBaseInterface();
            return true;
        }

        public void ShowObjSelector()   => ToggleContent(true);
        public void ShowBaseInterface() => ToggleContent(false);

        void ToggleContent (bool flag) {
            objCanvas.alpha = flag ? 1 : 0;
            objCanvas.blocksRaycasts = flag;
            objCanvas.interactable = flag;
            baseCanvas.alpha = !flag ? 1 : 0;
            baseCanvas.blocksRaycasts = !flag;
            baseCanvas.interactable = !flag;
            isActive = flag;
        }
    }
}