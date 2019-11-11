using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System;
using UnityEngine;

public class SaveFitnessEval: GenericSingletonClass<SaveFitnessEval>
{
    public void SaveFitnessValue(FitnessSaveValue data) {
        StreamWriter outStream = System.IO.File.AppendText(Application.dataPath + "/" + "FitnessEval.csv");
        outStream.WriteLine(data.toLine());
        outStream.Close();
    }

}
