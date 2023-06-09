using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    public static UIHandler instance;

    public GameObject WaveUI;
    public GameObject EnemyUI;
    public GameObject NextWaveUI;
    public GameObject HealthBarUI;
    public GameObject DamageGradientUI;

    private TextMeshProUGUI _waveText;
    private TextMeshProUGUI _enemyText;
    private TextMeshProUGUI _nextWaveText;
    private TextMeshProUGUI _healthBarText;

    private float _healthBarWidth;


    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
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
        StartCoroutine(DamageGradient(positiv));
        _healthBarText.text = health.ToString();

        HealthBarUI.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _healthBarWidth * health / 100);
    }

    IEnumerator DamageGradient(bool positive)
    {
        Image image = DamageGradientUI.GetComponent<Image>();

        float alpha = 0.1f;
        float valueChange = 0.05f;

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
}
