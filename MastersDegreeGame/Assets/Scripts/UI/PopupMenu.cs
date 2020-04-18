using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    public class PopupMenu : MonoBehaviour
    {
        public void OnPointerDown(PointerEventData eventData) {
            Debug.Log("DOWN");

        }

        public void OnPointerUp(PointerEventData eventData) {
            Debug.Log("UP");
        }
    }
}
