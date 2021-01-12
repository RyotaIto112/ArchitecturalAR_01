using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace ARUI {
    public class UiTopController : MonoBehaviour, UiInitializer {
        [SerializeField] protected CanvasGroup content;
        [SerializeField] protected List<Button> OpStateBtn;
        protected int opStateCount => Enum.GetNames(typeof(OparateState)).Length;
        protected bool isActive;

        void OnValidate() {
            Assert.IsTrue(opStateCount == OpStateBtn.Count);
        }

        void Start(){
            NextOpState((int)OparateState.Spawn);
        }

        public bool IsActive () => isActive;

        public bool Initialize () {
            ToggleContent (false);
            return true;
        }

        public void NextOpState(int next){
            UiController.currOpState = (OparateState)next;
            for(int i = 0; i < opStateCount; i++) {
                var flag = i == next;
                OpStateBtn[i].gameObject.SetActive(flag);
                OpStateBtn[i].interactable = flag;
            }
        }

        public void ToggleContent (bool flag) {
            content.alpha = flag ? 1 : 0;
            content.blocksRaycasts = flag;
            content.interactable = flag;
            isActive = flag;
        }
    }
}