using UnityEngine;
using TMPro; // ����TextMeshPro�����ռ�
using System.Collections;
using UnityEngine.SceneManagement;

public class ReturnControl : MonoBehaviour
{
   
    private TextMeshProUGUI scoreText; // ʹ��TextMeshProUGUI����Text
    public Action Action;
    void Start()
    {
        
        scoreText = GetComponent<TextMeshProUGUI>(); // ��ȡTextMeshProUGUI���
        StartCoroutine(CountTime());
        StartCoroutine(UpdateScore());
    }
    IEnumerator CountTime()
    {
        for (int i = 3; i >0; i--)
        {
            
            scoreText.text = "QE����������б\r\n       W����\r\n      ADת��\r\n            " + i;
            yield return new WaitForSeconds(1);
        }
    }

    IEnumerator UpdateScore()
    {
        yield return new WaitForSeconds(3);
        
        while (true)
        {
            float score = Action.GetScore();
            int putscore = (int)score;
            if (Action.isTrue)
            {
                int index = PlayerPrefs.GetInt("Index", 1);
                // ��ȡ��ǰʱ��
                string currentTime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                // ��ʱ��ͷ������浽PlayerPrefs
                PlayerPrefs.SetString("Time" + index, currentTime);
                PlayerPrefs.SetInt("Score" + index, putscore);
                
                index++; // ��������
                PlayerPrefs.SetInt("Index", index);
                PlayerPrefs.Save(); // ȷ�����ݱ�����

                scoreText.text = "\n\n\nFinal Score: " + putscore;
                yield return new WaitForSeconds(3);

                SceneManager.LoadScene("StartScene");
            }
            // �����ı�
            scoreText.text =  " ";

            // �ȴ�1��
            yield return new WaitForSeconds(1);
        }
    }
}
