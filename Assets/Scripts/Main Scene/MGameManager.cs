using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MGameManager : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI coinText;

    public void OnStartBtClk()
    {
        SceneManager.LoadScene(1);
        Time.timeScale = 1.0f;
    }

    private void Start()
    {
        coinText.text = PlayData.Instance.goodsData.coin.ToString();
        
    }
}
