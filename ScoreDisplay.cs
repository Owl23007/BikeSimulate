using UnityEngine;
using TMPro; // ����TextMeshPro�����ռ�
using System.Collections;

public class ScoreDisplay : MonoBehaviour
{
    private TextMeshProUGUI scoreText; // ʹ��TextMeshProUGUI����Text
    public Action Action;
    void Start()
    {
        scoreText = GetComponent<TextMeshProUGUI>(); // ��ȡTextMeshProUGUI���
        
        StartCoroutine(UpdateScore());
    }

    IEnumerator UpdateScore()
    {
        yield return new WaitForSeconds(1);
        while (true)
        {
            float score = Action.GetScore();
            int putscore = (int)score;
            
            // ��ȡ�÷�
            
           
          
            // �����ı�
            scoreText.text = "Score: " + putscore;

            // �ȴ�1��
            yield return new WaitForSeconds(1);
        }
    }
}
