using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Generator : MonoBehaviour
{
    [Header("Genetic Algorithm")]
    [SerializeField] GameObject player;
    [SerializeField] int populationSize = 200;
    [SerializeField] float mutationRate = 0.01f;
    [SerializeField] int elitism = 5;
    [SerializeField] int blockSize = 16;
    [SerializeField] GameObject[] prefabs;
    [SerializeField] float genomSize = 0.16f;
    [SerializeField] GameObject World;
    [SerializeField] GameObject StartPlatform;
 
    private Vector3 positionOffset = new Vector3(0f, 0f , 0f);
    private float currentTime = 0f;
    private int CurrentScore = 0;
    private int dnaIndex = 0;
    private List<FitnessEval> fitnessEvals = new List<FitnessEval>();
    private List<GameObject> DNA_Prefabs = new List<GameObject>();
    private GeneticAlgorithm<int> ga;
    private System.Random random;
    private bool isOnDivider = false;
    private GameObject dividerInstance;

    private GameManager gm; 

    private string path;


    private void Start()
    {
        populationSize = Launcher.Instance.populationSize;
        mutationRate = Launcher.Instance.mutationRate;
        elitism = Launcher.Instance.elitism;
        blockSize = Launcher.Instance.blockSize;

        random = new System.Random();

        ga = new GeneticAlgorithm<int>(populationSize, blockSize, random, GetRandomInteger, FitnessFunction, elitism, mutationRate);

        path = Application.dataPath + "/" + "Genetic_Save";
        ga.LoadGeneration(path);

      
        
        //Create End Platform of DNA level
        dividerInstance = Instantiate(StartPlatform, new Vector3((genomSize * blockSize)+1.95f, StartPlatform.transform.position.y, 0), Quaternion.identity);
        dividerInstance.GetComponent<BoxCollider2D>().enabled = true;

        /*
        *just for evaluation purpose 
        */
        //autoEvaluateFitnessFunction();
        

        //Ui
        gm = GameManager.Instance;
        gm.PopulationSize = populationSize;
        gm.Generation = ga.Generation;

        StartCoroutine(SpawnNextDNA());
    
    }

    private void autoEvaluateFitnessFunction()
    {
        dnaIndex = 0;
        for (int i = 0; i < 10000; i++) {
            if (dnaIndex == populationSize) {
                dnaIndex = 0;
                ga.NewGeneration();
                fitnessEvals.Clear();
            }
            DNA<int> dna = ga.Population[dnaIndex];
            float bestPossibleTime = 1.0f;
            FitnessEval passedDNAEvaluation = new FitnessEval(bestPossibleTime, getBestPossibleScore(dna));
            fitnessEvals.Add(passedDNAEvaluation);
            SaveFitnessEval.Instance.SaveFitnessValue(new FitnessSaveValue(ga.Generation , dna.Genes, passedDNAEvaluation.getFitness()));

            dnaIndex++;
        }
    }

    private int getBestPossibleScore(DNA<int> dna) {
        int score = 0;
        foreach (int gen in dna.Genes) {
            switch (gen) {
                case 2: score+=100; break;
                case 3: score+=10; break;
                case 4: score+=110; break;
                case 8: score+=100; break;
                case 9: score+=10; break;
                case 11: score+=100; break;
                case 12: score+=10; break;
            }
        } 
        return score;
    }

    private float FitnessFunction(int index)
    {
        return fitnessEvals[index].getFitness();
    }

    private int GetRandomInteger()
    {
        int i = random.Next(prefabs.Length);
        return i;
    }

    IEnumerator SpawnNextDNA()
    {
        Debug.Log("DNA index: " + dnaIndex);
        gm.DNA = dnaIndex + 1;
        //Delete old dna
        foreach (var item in DNA_Prefabs)
        {
            Destroy(item);
        }

        DNA_Prefabs.Clear();

        DNA<int> dna = ga.Population[dnaIndex];

        string log = "Creating block from DNA:";
        for (int i = 0; i < blockSize; i++)
            log += dna.Genes[i];
        Debug.Log(log);

        positionOffset.x = World.transform.position.x;
        for (int i = 0; i < blockSize; i++)
        {
            
            positionOffset.y = prefabs[dna.Genes[i]].transform.position.y;
            DNA_Prefabs.Add(Instantiate(prefabs[dna.Genes[i]], positionOffset, Quaternion.identity, World.transform));
            positionOffset.x += genomSize;
            yield return new WaitForSeconds(0.07f);
        }

        dnaIndex++;
        
        StopCoroutine("SpawnNextDNA");


    }


    private void calcFitnessForOldDNA() {
        float time = Time.realtimeSinceStartup;
        int score = player.GetComponent<PlayerStats>().CurrentScore;
        if (dnaIndex > 0) {
            FitnessEval passedDNAEvaluation = new FitnessEval(time-currentTime, score-CurrentScore);
            fitnessEvals.Add(passedDNAEvaluation);
            SaveFitnessEval.Instance.SaveFitnessValue(new FitnessSaveValue(ga.Generation , ga.Population[dnaIndex-1].Genes, passedDNAEvaluation.getFitness()));
            gm.LastFitness = Mathf.RoundToInt(passedDNAEvaluation.getFitness());
        }
        currentTime = time;
        CurrentScore = score;
    }

    public void EnterDivider()
    {
        //Wegen Bug: Event wird 2mal getriggert
        if(!isOnDivider)
        {
         
            calcFitnessForOldDNA();

            
            
            if (dnaIndex == populationSize)
            {    

                
                ga.NewGeneration();
                ga.SaveGeneration(path);

                dnaIndex = 0;
                fitnessEvals.Clear();
                gm.Generation = ga.Generation;
            }

            StartCoroutine(SpawnNextDNA());
            Vector3 offset = player.transform.position -dividerInstance.transform.position;
            //Debug.Log(offset);
            player.transform.position = Vector3.zero + offset;

            isOnDivider = true;
        }
    }

    public void ExitDivider()
    {
        //Wegen Bug: Event wird 2mal getriggert
        if (isOnDivider)
        {
         
            isOnDivider = false;
        }
        
    }
}
