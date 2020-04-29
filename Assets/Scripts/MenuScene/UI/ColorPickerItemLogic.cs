using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ColorPickerItemLogic :
    MonoBehaviour,
    IPointerEnterHandler,
    IPointerExitHandler
{
    bool isIn = false;
    bool isUnlocked = false;
    [SerializeField] TooltipActions tooltipUnlocked = default;
    [SerializeField] TooltipActions tooltipLocked = default;
    [SerializeField]TooltipActions selectedTooltip = default;
    [SerializeField] RawImage image = default;
    MenuBikeColorSelector menuBikeColorSelector = default;


    private void Start() {
        menuBikeColorSelector = MenuBikeColorSelector.instance;
        int count = transform.parent.parent.GetSiblingIndex();
        var ppc = new PlayerPrefsController();
        isUnlocked = count<=ppc.killCount;
        selectedTooltip = isUnlocked?tooltipUnlocked:tooltipLocked;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isIn = true;
        selectedTooltip.MakeVisible();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isIn = false;
        selectedTooltip.MakeHidden();
    }

    private void Update() {
        if(!isIn || !isUnlocked) return;
        if(Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Mouse0)) menuBikeColorSelector.SetAccentColor(image.color);
    }

    public void Init(Color color){
        image.color = color;
    }
    
}
