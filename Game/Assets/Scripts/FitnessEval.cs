using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FitnessEval {

    float fintnessVal;
    public FitnessEval(float time, int score) {
        this.fintnessVal = (1/time)*score;
    }

    public float getFitness() {
        return this.fintnessVal;
    }
}
