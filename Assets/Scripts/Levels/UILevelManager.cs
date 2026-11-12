using UnityEngine;

public class UILevelManager : MonoBehaviour
{
    [SerializeField] private RectTransform levelUIContainer;
    [SerializeField] private LevelUI levelUIPrefab;
    [SerializeField] private Level[] levels;

    private void Start()
    {
        for (int i = 0; i < levels.Length; i++)
        {
            LevelUI levelUI = Instantiate(levelUIPrefab, levelUIContainer);

            levelUI.InitButton(levels[i]);
            levelUI.SetIDText(i + 1);
        }
    }
}

