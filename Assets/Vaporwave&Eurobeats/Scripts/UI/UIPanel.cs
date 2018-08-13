using UnityEngine;
using DG.Tweening;
namespace Scripts.UI {
    public class UIPanel : UIElement {
        public CanvasGroup Group;
        public float ShowAlpha = 1;
        public float HideAlpha = 0;
        public float TransitionDuration = 1;

        protected override void SnapShow() {
            Group.alpha = ShowAlpha;
            Group.interactable = true;
        }

        protected override void SnapHide() {
            Group.alpha = HideAlpha;
            Group.interactable = false;
        }

        protected override void OnShow() {
            Group.DOFade(ShowAlpha, TransitionDuration);
            Group.interactable = true;
        }

        protected override void OnHide() {
            Group.DOFade(HideAlpha, TransitionDuration);
            Group.interactable = false;
        }
    }
}