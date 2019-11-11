using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FitnessSaveValue
{
    System.DateTime time;
    int generation;
    int[] DNA;
    float fitness;
    public FitnessSaveValue(int generation, int[] DNA, float fitness) {
        this.time = System.DateTime.Now;
        this.generation = generation;
        this.DNA = DNA;
        this.fitness = fitness;
    }

    public string toLine() {
        string delimiter = ",";

        string DNADelimiter = " ";
        string DNAString = "";
        foreach (int gen in DNA) 
            DNAString += gen + DNADelimiter;
        return time.ToString() + delimiter + generation + delimiter + DNAString + delimiter + (int)fitness;
    }
}
