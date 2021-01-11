using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Score_Counter : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public static int score;
    public bool isEscape;

    public GameObject menuScreen;
    public GameObject victoryMsg;

    // Start is called before the first frame update
    void Start() {
        score = 0;
    }

    // Update is called once per frame
    void Update() {
        scoreText.text = score.ToString();

        if (score == 3) {
            menuScreen.SetActive(true);
            victoryMsg.SetActive(true);
        } else {
            if (isEscape) {
                menuScreen.SetActive(true);
                victoryMsg.SetActive(false);

            } else {
                menuScreen.SetActive(false);
                victoryMsg.SetActive(false);
            }

            if (Input.GetKeyDown(KeyCode.Escape)) {
                if (isEscape) {
                    isEscape = false;
                } else {
                    isEscape = true;
                }
            }
        }

        
    }
}
