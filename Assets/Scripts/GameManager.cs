using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private float countdownTime = 3.0f;
    private float countdown;
    private bool isCountdownInProgress = true;
    private float fadeOpacity = 0.2f;

    private PlayerController playerController;
    private ShaderController shaderController;
    private Animator playerAnimator;

    public bool CanMove { get => _canMove; set => _canMove = value; }
    public bool IsInputDisabled { get => _isInputDisabled; set => _isInputDisabled = value; }
    [SerializeField]
    private bool _canMove = false;
    [SerializeField]
    private bool _isInputDisabled = false;

    void Start()
    {
        countdown = countdownTime;
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        shaderController = GameObject.Find("CurveLevel").GetComponent<ShaderController>();
        playerAnimator = playerController.GetComponent<Animator>();
    }

    void Update()
    {
        if (!_canMove && countdown > 0)
        {
            countdown -= Time.deltaTime;
            playerAnimator.enabled= false;
            if (countdown <= 0)
            {
                _canMove = true;
                playerAnimator.enabled= true;
                countdown = countdownTime;
            }
        }
        else
        {
            isCountdownInProgress = false;
        }
    }

    void OnGUI()
    {
        GUIStyle countdownStyle = new GUIStyle(GUI.skin.GetStyle("label"));
        countdownStyle.fontSize = 50;
        countdownStyle.normal.textColor = Color.white;

        if (!_canMove)
        {
            GUI.Label(new Rect(Screen.width / 2, Screen.height / 2, 200, 500), Mathf.Round(countdown).ToString(), countdownStyle);
        }

        GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);
        buttonStyle.fontSize = 30;

        Rect buttonRect = _isInputDisabled ? new Rect((Screen.width - 200) / 2, (Screen.height - 100) / 2, 200, 100) : new Rect(Screen.width - 110, 10, 100, 50);
        if (!_isInputDisabled)
        {
            buttonStyle.fontSize = 15;
            buttonStyle.normal.background = Texture2D.linearGrayTexture;
        }

        if (GUI.Button(buttonRect, "RESTART", buttonStyle))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if (isCountdownInProgress)
        {
            GUI.color = new Color(0f, 0f, 0f, fadeOpacity);
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), Texture2D.whiteTexture);
        }
    }

    public void EndGame()
    {
        shaderController.enabled = false;
        _isInputDisabled = true;
        playerAnimator.SetBool("isInputDisabled", true);
    }

}
