using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Connection {
    public int InnovationNumber;
    public Neuron neuronOut, neuronIn;
    public int In, Out;
    public float Weight;

    public void SetRandomWeight() {
        Weight = Random.value;

    }
    public void SendInput(float inp) {
        neuronOut.AddInput(inp * Weight);

    }
    public float GetOutput(List<Neuron> visited) {
        if (!visited.Contains(neuronIn))
        {
            neuronIn.Prime2(visited);
            return neuronIn.Output* Weight;
        }
        else
        {
            return 0;
        }
    }



}
