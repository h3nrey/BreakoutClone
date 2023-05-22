using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Button))]
public class LevelButton : MonoBehaviour
{
    public int sceneBuildIndex;
    Button selfBtn;
    [SerializeField] TMP_Text text;
    private void Enable() {
        selfBtn = GetComponent<Button>();
    }
    

    public void SetBuildIndex(int index) {
        sceneBuildIndex = index;
    }
    public void CallScene() {
        SceneManager.LoadScene(int.Parse(text.text) + 1);
    }

}
