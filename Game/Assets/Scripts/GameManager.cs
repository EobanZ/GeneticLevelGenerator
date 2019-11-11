using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : GenericSingletonClass<GameManager>
{
    private int Highscore;
  
    public PlayerStats Player;
    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI GameOverText;

    public TextMeshProUGUI GenerationText;
    public TextMeshProUGUI DNAText;
    public TextMeshProUGUI LastFitnessText;
    public TextMeshProUGUI BestFitnessText;

    public int Generation { get; set; }
    public int DNA { private get; set; }
    public int PopulationSize { get; set; }
    private int lastFitness;
    private int bestFitness;

    public int LastFitness { set { lastFitness = value; bestFitness = lastFitness > bestFitness ? lastFitness : bestFitness; } }

    void Start()
    {
    }

    private void FixedUpdate()
    {
        UpdateUI();
    }

    void Update()
    {
        
    }

    public void UpdateUI()
    {

        ScoreText.SetText(Player.CurrentScore.ToString());
        GenerationText.SetText(Generation.ToString());
        DNAText.SetText(""+DNA.ToString()+"/"+PopulationSize.ToString());
        LastFitnessText.SetText(lastFitness.ToString());
        BestFitnessText.SetText(bestFitness.ToString());
       
        if (Player.isDead())
        {
            GameOverText.SetText("Game Over");
            Invoke("LoadLauncher", 3.0f);
        }
            

    }

    private void LoadLauncher()
    {
        Destroy(Launcher.Instance.gameObject);
        SceneManager.LoadScene(0);
    }


}
