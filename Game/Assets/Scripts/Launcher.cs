using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.UI;
using UnityEditor;
using TMPro;

public class Launcher : GenericSingletonClass<Launcher>
{
    public int populationSize = 8;
    public float mutationRate = 0.01f;
    public int elitism = 3;
    public int blockSize = 16;

    public OptionsScriptableObject optionsSO;

    public Slider poplulatioSlider, mutationSlider, elitismSlider, blocksizeSlider;
    public TextMeshProUGUI populationText, mutationText, elitismText, blocksizeText;

    void Start()
    {
        //EditorUtility.SetDirty(optionsSO);

        populationSize = optionsSO.populationSize;
        mutationRate = optionsSO.mutationRate;
        elitism = optionsSO.elitism;
        blockSize = optionsSO.blockSize;
       
        DontDestroyOnLoad(this.gameObject);
        poplulatioSlider.value = populationSize;
        mutationSlider.value = mutationRate;
        elitismSlider.value = (float)elitism / (float)populationSize;
        blocksizeSlider.value = blockSize;
      
        OnSlider();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Reset()
    {
        File.Delete(Application.dataPath + "/" + "Genetic_Save");

    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void OnSlider()
    {
        populationText.SetText("Population Size: " + poplulatioSlider.value.ToString());
        mutationText.SetText("Mutation Rate: " + (mutationSlider.value * 100).ToString("#0") + "%");
        elitismText.SetText("Elitism Rate: " + (elitismSlider.value*100).ToString("#0") +"%");
        blocksizeText.SetText("Block Size: " + blocksizeSlider.value.ToString());
    }

    public void OnSave()
    {
        populationSize = (int)poplulatioSlider.value;
        mutationRate = mutationSlider.value;
        elitism = (int)(elitismSlider.value * populationSize);
        blockSize = (int)blocksizeSlider.value;

        optionsSO.populationSize = populationSize;
        optionsSO.mutationRate = mutationRate;
        optionsSO.elitism = elitism;
        optionsSO.blockSize = blockSize;
        Reset();
    }
}
