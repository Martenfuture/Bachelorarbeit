using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    public static UIHandler instance;

    public GameObject WaveUI;
    public GameObject EnemyUI;
    public GameObject NextWaveUI;
    public GameObject HealthBarUI;
    public GameObject DamageGradientUI;

    [SerializeField] private GameObject _gameOverUI;
    private bool _isGameOver = false;

    // Variable UI
    [Header("Variable UIs")]
    [SerializeField] GameObject VariableUI;

    [SerializeField] TextMeshProUGUI _varPlayerHealthUI;
    [SerializeField] TextMeshProUGUI _varEnemiesPerMinuteUI;
    [SerializeField] TextMeshProUGUI _varEnemiesPerWaveUI;
    [SerializeField] TextMeshProUGUI _varEnemySpeedUI;
    [SerializeField] TextMeshProUGUI _varEnemyHealthUI;
    [SerializeField] TextMeshProUGUI _varEnemyDamageUI;
    [SerializeField] TextMeshProUGUI _varWeaponDammageUI;
    [SerializeField] TextMeshProUGUI _varWeaponFireRateUI;

    private TextMeshProUGUI _waveText;
    private TextMeshProUGUI _enemyText;
    private TextMeshProUGUI _nextWaveText;
    private TextMeshProUGUI _healthBarText;

    private float _healthBarWidth;


    public PlayerInputActions PlayerControls;
    private InputAction _toggleVariableUI;


    private void Awake()
    {
        instance = this;
        PlayerControls = new PlayerInputActions();
    }

    private void OnEnable()
    {
        _toggleVariableUI = PlayerControls.Player.ToggleVariableUI;
        _toggleVariableUI.Enable();
        _toggleVariableUI.performed += ToggleVariableUI;
    }

    private void Start()
    {
        GameManager.instance.OnGameOver += GameOver;
        _waveText = WaveUI.GetComponent<TextMeshProUGUI>();
        _enemyText = EnemyUI.GetComponent<TextMeshProUGUI>();
        _nextWaveText = NextWaveUI.GetComponent<TextMeshProUGUI>();
        _healthBarText = HealthBarUI.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        _healthBarWidth = HealthBarUI.GetComponent<RectTransform>().rect.width;
    }

    public void UpdateUIWave(int waveNumber)
    {
        _waveText.text = "Wave: " + waveNumber;
    }

    public void UpdateUIEnemy(int enemyNumber)
    {
        _enemyText.text = "Enemies: " + enemyNumber;
    }

    public void NextWaveCountdown(int seconds)
    {
        StartCoroutine(NextWaveCountdownDelay(seconds));
    }

    IEnumerator NextWaveCountdownDelay(int seconds)
    {
        yield return new WaitForEndOfFrame();
        NextWaveUI.transform.parent.gameObject.SetActive(true);
        while (seconds > 0)
        {

            _nextWaveText.text = seconds.ToString();
            if (seconds % 2 == 0)
            {
                _nextWaveText.color = Color.red;
            }
            else
            {
                _nextWaveText.color = Color.white;
            }
            seconds--;

            yield return new WaitForSeconds(1);
        }
        NextWaveUI.transform.parent.gameObject.SetActive(false);
        WaveManager.instance.StartWave();
    }

    public void UpdateUIHealth(float health, bool positiv)
    {
        if (_isGameOver)
        {
            return;
        }
        StartCoroutine(DamageGradient(positiv));
        _healthBarText.text = health.ToString();

        HealthBarUI.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _healthBarWidth * health / 100);
    }

    IEnumerator DamageGradient(bool positive)
    {
        Image image = DamageGradientUI.GetComponent<Image>();

        float alpha = 0.1f;
        float valueChange = 0.025f;

        Color targetColor = positive ? new Color(0, 1, 0, 0) : new Color(1, 0, 0, 0);

        while (alpha > 0)
        {
            alpha += valueChange;

            targetColor.a = alpha;
            image.color = targetColor;

            if (alpha > 1)
            {
                valueChange *= -1;
            }

            yield return new WaitForEndOfFrame();
        }

    }

    public void UpdateUIVariable(string variableType, string variable)
    {
        switch (variableType)
        {
            case "PlayerHealth":
                _varPlayerHealthUI.text = variable;
                break;
            case "EnemiesPerMinute":
                _varEnemiesPerMinuteUI.text = variable;
                break;
            case "EnemiesPerWave":
                _varEnemiesPerWaveUI.text = variable;
                break;
            case "EnemySpeed":
                _varEnemySpeedUI.text = variable;
                break;
            case "EnemyHealth":
                _varEnemyHealthUI.text = variable;
                break;
            case "EnemyDamage":
                _varEnemyDamageUI.text = variable;
                break;
            case "WeaponDamage":
                _varWeaponDammageUI.text = variable;
                break;
            case "WeaponFireRate":
                _varWeaponFireRateUI.text = variable;
                break;
        }
    }


    private void ToggleVariableUI(InputAction.CallbackContext obj)
    {
        VariableUI.SetActive(!VariableUI.activeSelf);
    }

    private void GameOver()
    {
        _isGameOver = true;
        _gameOverUI.SetActive(true);
    }
}
