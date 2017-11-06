using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Neuron {
    //Input = output fra tidligere neuron * vægt
    public List<float> Inputs = new List<float>();
    public int NeuronNumber;

    public float Summation;
    public float Output;
    
    public float Threshhold;
    public List<Connection> BackLinks = new List<Connection>();
    public List<Connection> Links = new List<Connection>();


    public void RandomizeNeuron() {

        Threshhold = Random.value;
    }

    void CalculateSum() {
        Summation = 0;
        foreach (float item in Inputs)
        {
            Summation += item;
        }

    }
    void calculateOutput() {
        Output = (float)sigmoid.output(Summation);

    }

    public void CleanInput() {

        Inputs.Clear();
    }

    public void Prime() {
        CalculateSum();
        calculateOutput();
        if (Links.Count>0)
        {
            foreach (Connection item in Links)
            {
                if (Output>= Threshhold)
                {
                item.SendInput(Output);

                }
                else
                {
                    item.SendInput(0);
                }
            }
        }

        Inputs.Clear();
    }


    public void Prime2(List<Neuron> visited) {
        if (BackLinks.Count>0)
        {
            visited.Add(this);
            foreach (Connection item in BackLinks)
            {
                Inputs.Add(item.GetOutput(visited));
            }
        }
        CalculateSum();
        calculateOutput();
        

    }

    public void AddInput(float inp) {
        Inputs.Add(inp);
    }

    class sigmoid
    {
        public static double output(double x)
        {
            return 1.0 / (1.0 + Mathf.Exp((float)-x));
        }

        public static double derivative(double x)
        {
            return x * (1 - x);
        }
    }

}
