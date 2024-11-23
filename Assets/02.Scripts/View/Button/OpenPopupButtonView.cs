using UnityEngine;
using UnityEngine.UI;

public class OpenPopupButtonView : MonoBehaviour
{
    public Button button;
    public GameObject popupPrefab;
    public Transform parent_tr;

    public void Init(Transform parent)
    {
        parent_tr = parent;
    }

    public GameObject OnPopup()
    {
        return Instantiate(popupPrefab, parent_tr);
    }
}
