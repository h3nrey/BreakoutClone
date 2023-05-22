using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelsChooseController : MonoBehaviour
{
    int lastLevelIndex;
    [SerializeField] int totalLevels;
    [SerializeField] GameObject levelButtonPrefab;
    [SerializeField] Transform startPos;
    [SerializeField] Transform btnGrid;

    private void Awake() {
        btnGrid.GetChild(0).GetComponent<Image>().color = Color.white;
        btnGrid.GetChild(0).GetComponent<Button>().interactable = true;
        print(btnGrid.GetChild(0).name);
        if (PlayerPrefs.HasKey("lastLevelPassed")) {
            lastLevelIndex = PlayerPrefs.GetInt("lastLevelPassed");

            for(int i = 0; i < lastLevelIndex - 1; i++) {
                int totalOfButtons = btnGrid.childCount;

                btnGrid.GetChild(i).GetComponent<Image>().color = Color.white;
                btnGrid.GetChild(i).GetComponent<Button>().interactable = true;
                print(btnGrid.GetChild(i).name);
                //btnGrid.GetChild(i).GetComponent<LevelButton>().SetBuildIndex(i);
            }
        }

        //GenerateLevelButtons();

    }

    [Button("Create Buttons")]
    public void GenerateLevelButtons() {
        for(int i = 1; i <= totalLevels; i++) {
            //Vector2 btnPos = new Vector2(startPos.position.x + i, startPos.position.y);
            GameObject button = Instantiate(levelButtonPrefab, btnGrid);
            button.transform.GetChild(0).GetComponent<TMP_Text>().text = $"{i}";
            button.GetComponent<Image>().color = Color.gray;
        }
    }
}
