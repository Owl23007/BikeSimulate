using System.Collections.Generic;
using TMPro; // ����TextMeshPro�����ռ�
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class RatePrint : MonoBehaviour
{
  
    public Button closeButton; // ���ڹر�Canvas�İ�ť
    public Canvas scoreCanvas; // ������ʾ������Canvas
    public TextMeshProUGUI scoreText; // ������ʾ������TextMeshProUGUI����

    void Start()
    {
        closeButton.onClick.AddListener(CloseCanvas);
        ShowScores();

    }
    void CloseCanvas()
    {
        // ����Canvas
        scoreCanvas.gameObject.SetActive(false);
    }
    void ShowScores()
    {
        // ��PlayerPrefs���ط���
        int index = PlayerPrefs.GetInt("Index", 1);
        Dictionary<string, int> scores = new Dictionary<string, int>();
        for (int i = 1; i < index; i++)
        {
            string time = PlayerPrefs.GetString("Time" + i, "");
            int score = PlayerPrefs.GetInt("Score" + i, 0);
            scores[time] = score;
        }

        // ����������
        var sortedScores = from entry in scores
                           orderby entry.Value descending
                           select entry;

        // ��ʾ����
        string scoreTextString = "";
        int count = 0;
        foreach (var entry in sortedScores)
        {
            if (count >= 8)
            {
                break;
            }
            scoreTextString += "Time: " + entry.Key + ", Score: " + entry.Value + "\n";
            count++;
        }
        scoreText.text = scoreTextString;
    }
}

