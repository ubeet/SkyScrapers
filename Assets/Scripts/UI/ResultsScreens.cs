using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResultsScreens : MonoBehaviour
{
    [SerializeField] protected CanvasGroup winStateScreen;
    [SerializeField] private Button acceptButton;
    [SerializeField] private float alphaGrow = 0.1f;
    [SerializeField] private TMP_Text stateText;
    [SerializeField] public GameObject body;
    [SerializeField] private TimerPresenter timerPresenter;

    protected bool HasState = false;
    protected bool WinState;
    protected float Alpha;


    public virtual void OnAcceptClick()
    {
        OpenMainMenu();
    }

    protected virtual void OnEnable()
    {
        acceptButton.onClick.AddListener(OnAcceptClick);
    }
    protected virtual void OnDisable() { acceptButton.onClick.RemoveListener(OnAcceptClick); }

    private void Start()
    {
        HasState = false;
        WinState = false;
        Alpha = 0;
    }

    public virtual void SetWinStateScreen(bool state)
    {
        timerPresenter.transform.SetParent(body.transform);
        winStateScreen.gameObject.SetActive(true);
        if(stateText != null)
            stateText.text = state ? stateText.text : "you lose";
        WinState = state;
        HasState = true;
    }

    public void OpenMainMenu()
    {
        FindObjectOfType<SceneFader>().FadeToScene(0);
    }

    private void Update()
    {
        if (HasState && Alpha <= 1)
        {
            winStateScreen.alpha = Alpha += alphaGrow;
        }
    }
}
