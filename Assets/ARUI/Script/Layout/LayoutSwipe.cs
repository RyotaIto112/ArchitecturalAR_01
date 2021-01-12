using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ARUI.Layout {
    [RequireComponent (typeof (RectTransform))]
    public class LayoutSwipe : MonoBehaviour {
        [SerializeField] protected float width;
        [SerializeField] protected float limit;
        protected RectTransform outerRect;
        protected List<RectTransform> contents;

        void Start () {
            outerRect = GetComponent<RectTransform> ();
            contents = new List<RectTransform> ();
            foreach (Transform c in outerRect.transform)
                contents.Add (c.GetComponent<RectTransform> ());
            width = contents.Sum (c => c.rect.width);
            outerRect.sizeDelta = new Vector2(width, outerRect.sizeDelta.y);
            limit = Mathf.Max(width - Screen.width, 0) * 2;
        }

        void Update () {
            var ap = outerRect.anchoredPosition;
            ap = new Vector2(Mathf.Clamp(ap.x, -limit, 0),ap.y);
            outerRect.anchoredPosition = ap;
        }
    }
}