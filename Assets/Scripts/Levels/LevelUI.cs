using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelUI : MonoBehaviour
{
    private Button button;
    private TextMeshProUGUI levelText;

    public void InitButton(Level level)
    {
        button = GetComponent<Button>();
        levelText = GetComponentInChildren<TextMeshProUGUI>();

        button.onClick.AddListener(()  => 
        {
            LevelSystem.Instance.SetLevel(level);
            SceneManager.LoadScene(1); 
        });
    }

    public void SetIDText(int id)
    {
        levelText.text = id.ToString();
    }
}
