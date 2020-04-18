using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    public class PopupMenu : MonoBehaviour, IDeselectHandler
    {
        public void OnDeselect(BaseEventData eventData) {
            transform.gameObject.SetActive(false);
        }
    }
}
