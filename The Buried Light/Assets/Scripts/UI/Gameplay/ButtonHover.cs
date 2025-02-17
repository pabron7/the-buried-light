using UnityEngine;
using UnityEngine.EventSystems;

    public class ButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private Vector3 originalScale;
        public float hoverScaleFactor = 1.1f;

        void Start()
        {
            originalScale = transform.localScale;
        }

        /// <summary>
        /// Method called when the pointer enters the button. Scales the button up by the hover scale factor
        /// and plays the hover sound.
        /// </summary>
        public void OnPointerEnter(PointerEventData eventData)
        {
            transform.localScale = originalScale * hoverScaleFactor;

        
        }

        /// <summary>
        /// Method called when the pointer exits the button. Reverts the button back to its original scale.
        /// </summary>
        public void OnPointerExit(PointerEventData eventData)
        {
            transform.localScale = originalScale;
        }
    }