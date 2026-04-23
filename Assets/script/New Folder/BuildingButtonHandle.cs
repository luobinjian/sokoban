using UnityEngine;
using UnityEngine.UI;

public class BuildingButtonHandle : MonoBehaviour
{
    [SerializeField] BuildingObjectBase item;
    Button button;
    BuidingCreator creator;

    void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnButtonClick);
        creator = BuidingCreator.GetInstance();
    }

    public void OnButtonClick()
    {
        print(item.Type);
        creator.ObjectSelected(item);
    }
}
