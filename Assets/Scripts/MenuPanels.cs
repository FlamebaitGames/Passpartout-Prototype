using UnityEngine;
using System.Collections;

public class MenuPanels : MonoBehaviour {
    [SerializeField]
    private GameObject paintingCanvasPanel;
    [SerializeField]
    private GameObject bottomBarMoneyPanel;
    [SerializeField]
    private GameObject weeklyPanel;
    [SerializeField]
    private GameObject mainMenuPanel;

    public enum Panel
    {
        NONE = 0,
        PAINTING_CANVAS = 1 << 1,
        MONEY_PANEL = 1 << 2,
        WEEKLY_PANEL = 1 << 3,
        MAIN_MENU_PANEL = 1 << 4,
        ALL = MAIN_MENU_PANEL | WEEKLY_PANEL | MONEY_PANEL | PAINTING_CANVAS
    }

    public void SetPanelsToShow(Panel panel)
    {
        paintingCanvasPanel.SetActive((panel & Panel.PAINTING_CANVAS) == Panel.PAINTING_CANVAS);
        bottomBarMoneyPanel.SetActive((panel & Panel.MONEY_PANEL) == Panel.MONEY_PANEL);
        weeklyPanel.SetActive((panel & Panel.WEEKLY_PANEL) == Panel.WEEKLY_PANEL);
        mainMenuPanel.SetActive((panel & Panel.MAIN_MENU_PANEL) == Panel.MAIN_MENU_PANEL);
    }

    void Start()
    {
        Debug.Assert(paintingCanvasPanel != null && bottomBarMoneyPanel != null && weeklyPanel != null && mainMenuPanel != null, "Not all MenuPanels have been set!");
    }
}
