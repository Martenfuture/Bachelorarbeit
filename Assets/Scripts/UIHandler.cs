using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIHandler : MonoBehaviour
{
    public static UIHandler instance;

    public GameObject WaveUI;
    public GameObject EnemyUI;
    public GameObject NextWaveUI;

    private TextMeshProUGUI WaveText;
    private TextMeshProUGUI EnemyText;
    private TextMeshProUGUI NextWaveText;


    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        WaveText = WaveUI.GetComponent<TextMeshProUGUI>();
        EnemyText = EnemyUI.GetComponent<TextMeshProUGUI>();
        NextWaveText = NextWaveUI.GetComponent<TextMeshProUGUI>();
    }

    public void UpdateUIWave(int waveNumber)
    {
        WaveText.text = "Wave: " + waveNumber;
    }

    public void UpdateUIEnemy(int enemyNumber)
    {
        EnemyText.text = "Enemies: " + enemyNumber;
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

            NextWaveText.text = seconds.ToString();
            if (seconds % 2 == 0)
            {
                NextWaveText.color = Color.red;
            }
            else
            {
                NextWaveText.color = Color.white;
            }
            seconds--;

            yield return new WaitForSeconds(1);
        }
        NextWaveUI.transform.parent.gameObject.SetActive(false);
        WaveManager.instance.StartWave();
    }
}
