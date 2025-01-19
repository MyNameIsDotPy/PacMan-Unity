using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScrGameController : MonoBehaviour
{
    public ScrPlayer player;
    public TextMeshProUGUI vidasText;
    public TextMeshProUGUI scoreText;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        vidasText.text = "Vidas: " + player.vidas;
        scoreText.text = "Score: " + player.score;
    }
}
