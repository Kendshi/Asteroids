using UnityEngine;
using UnityEngine.UI;
using JetBrains.Annotations;

public class Menu : MonoBehaviour
{
    [SerializeField] private GameInitialiser _gameInitialiser;
    [SerializeField] private InputFromPlayer _input;
    [SerializeField] private Text _buttonControlText;
    [SerializeField] private Button _buttonContinue;

    private bool _isGameStarted;

    void Start()
    {
        _buttonContinue.interactable = false;
    }

    private void OnEnable()
    {
        Time.timeScale = 0f;
    }

    private void OnDisable()
    {
        Time.timeScale = 1f;
    }

    [UsedImplicitly]
    public void ButtonContinue()
    {
        gameObject.SetActive(false);
    }

    [UsedImplicitly]
    public void ButtonStart()
    {
        if (_isGameStarted)
        {
            _gameInitialiser.RestartGame();
        }
        else
        {
            _isGameStarted = true;
            _gameInitialiser.StartGame();
            _buttonContinue.interactable = true;
        }
        gameObject.SetActive(false);
    }

    [UsedImplicitly]
    public void ButtonControl()
    {
        if (_input.ChangeControlMode())
            _buttonControlText.text = "”правление: клавиатура + мышь";
        else
            _buttonControlText.text = "”правление: клавиатура";
    }

    [UsedImplicitly]
    public void ButtonExit()
    {
        Application.Quit();
    }
}
