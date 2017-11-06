using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeuralNetwork : MonoBehaviour {
    Sensors sensors;
    CarControl carctrl;
    public float fitness;
    public Neuron[] InputNeurons = new Neuron[5];
    public List<Neuron> HiddenNeurons = new List<Neuron>();
    public Neuron[] OutputNeurons = new Neuron[2];
    public List<Connection> allconnections = new List<Connection>();
    public Neuron[] EveryNeuron = new Neuron[200];
    public bool TestCar = false;
    public bool Dead;
    public  int hiddenneurons;

    // Use this for initialization
    void Start () {
        Threshhold = 0.7f;
       // MakeRandomAndStart();


    }

    void ChangeWeight() {

        int i = Random.Range(0,allconnections.Count);
        allconnections[i].SetRandomWeight();
    }



    void NewNeuron() {
        List<Neuron> AllNeurons = new List<Neuron>();
        AllNeurons.AddRange(HiddenNeurons);
        AllNeurons.AddRange(OutputNeurons);
        Neuron n = new Neuron();
        HiddenNeurons.Add(n);
        Connection c = new Connection();
        allconnections.Add(c);

        c.InnovationNumber = allconnections.Count;
        c.SetRandomWeight();
        n.Links.Add(c);
        c.neuronIn = n;
        if (Random.Range(0, 2) == 0)
        {
            int ix = Random.Range(0, AllNeurons.Count);
            c.neuronOut = AllNeurons[ix];
            AllNeurons[ix].BackLinks.Add(c);

        }
        else
        {
            int ix = Random.Range(0, OutputNeurons.Length);
            c.neuronOut = OutputNeurons[ix];
            OutputNeurons[ix].BackLinks.Add(c);
        }

        // C er forbudt herfra

        Connection c2 = new Connection();
        allconnections.Add(c2);

        c2.InnovationNumber = allconnections.Count;
        c2.SetRandomWeight();
        n.BackLinks.Add(c2);
        c2.neuronOut = n;
        if (Random.Range(0, 2) == 0)
        {
            int ix = Random.Range(0, AllNeurons.Count);
            c2.neuronIn = AllNeurons[ix];
            AllNeurons[ix].Links.Add(c2);

        }
        else
        {
            int ix = Random.Range(0, InputNeurons.Length);
            c2.neuronIn = InputNeurons[ix];
            InputNeurons[ix].Links.Add(c2);
        }
    }

    void DeleteNeuron() {
        int ix = Random.Range(0, HiddenNeurons.Count);
        foreach (Connection item in HiddenNeurons[ix].BackLinks)
        {
            item.neuronIn.Links.Remove(item);
            allconnections.Remove(item);
        }
        foreach (Connection item in HiddenNeurons[ix].Links)
        {
            item.neuronOut.BackLinks.Remove(item);
            allconnections.Remove(item);

        }
        HiddenNeurons.Remove(HiddenNeurons[ix]);
    }
    void CreateConnection() {
        List<Neuron> AllNeurons = new List<Neuron>();
        AllNeurons.AddRange(HiddenNeurons);
        AllNeurons.AddRange(OutputNeurons);

        Connection c = new Connection();
        allconnections.Add(c);

        c.InnovationNumber = allconnections.Count;
        c.SetRandomWeight();
        Neuron n;
        if (Random.Range(0,2) == 0)
        {
            n = InputNeurons[Random.Range(0, InputNeurons.Length)];
        }
        else
        {
            n = HiddenNeurons[Random.Range(0, HiddenNeurons.Count)];
        }

        n.Links.Add(c);
        c.neuronIn = n;
        if (Random.Range(0, 2) == 0)
        {
            int ix = Random.Range(0, AllNeurons.Count);
            c.neuronOut = AllNeurons[ix];
            AllNeurons[ix].BackLinks.Add(c);

        }
        else
        {
            int ix = Random.Range(0, OutputNeurons.Length);
            c.neuronOut = OutputNeurons[ix];
            OutputNeurons[ix].BackLinks.Add(c);
        }
        
    }

    public void MutateThis() {
        // Change Weight
        // Create new neuron
        // Delete Neuron
        // Create Connection
        int ix = Random.Range(1, 5);
        switch (ix)
        {
            case 1:
              //  Debug.Log("Changing weight");
                ChangeWeight();
                break;
            case 2:
                NewNeuron();
                break;
            case 3:
                DeleteNeuron();
                break;
            case 4:
                CreateConnection();
                // Create Connection;
                break;



            default:
                break;
        }

    }

    public float Threshhold = 0.7f;

    public void MakeRandomAndStart() {
        sensors = GetComponent<Sensors>();
        carctrl = GetComponent<CarControl>();
        for (int i = 0; i < 5; i++)
        {
            Neuron n = new Neuron();
            InputNeurons[i] = n;
            n.NeuronNumber = i;
        }

        for (int i = 0; i < 2; i++)
        {
            Neuron n = new Neuron();
            //n.RandomizeNeuron();
            n.Threshhold = Threshhold;
            OutputNeurons[i] = n;
            n.NeuronNumber = i + 5;
        }
        MakeRandomHiddenNeurons();
        SetRandomConnections();

        StartCoroutine("FeedForward");

    }

    public void MakeBasic() {
        for (int i = 0; i < 5; i++)
        {
            Neuron n = new Neuron();
            InputNeurons[i] = n;
            n.NeuronNumber = i;
            EveryNeuron [i] = n;

        }
        for (int i = 0; i < 2; i++)
        {
            Neuron n = new Neuron();
            //n.RandomizeNeuron();
            n.Threshhold = Threshhold;
            OutputNeurons[i] = n;
            n.NeuronNumber = i + 5;
            EveryNeuron [n.NeuronNumber] = n;
        }
    }

    public void LoadNeuralNetwork(float[,] nnSave) {
        sensors = GetComponent<Sensors>();
        carctrl = GetComponent<CarControl>();

        MakeBasic();

        for (int x = 0; x < 200; x++)
        {
            for (int y = 0; y < 200; y++)
            {
                if (nnSave[x, y] > 0)
                {
                    Connection c = new Connection();
                    allconnections.Add(c);
                    c.Weight = nnSave[x, y];

                    if (EveryNeuron[x] == null)
                    {
                        Neuron n = new Neuron();
                        EveryNeuron[x] = n;
                        HiddenNeurons.Add(n);
                        n.NeuronNumber = x;
                    }
                    if (EveryNeuron[y] == null)
                    {
                        Neuron n = new Neuron();
                        EveryNeuron[y] = n;
                        HiddenNeurons.Add(n);

                        n.NeuronNumber = y;
                    }
                    EveryNeuron[x].Links.Add(c);
                    c.In = x;
                    c.neuronIn = EveryNeuron[x];
                    
                    c.neuronOut = EveryNeuron[y];
                    c.Out = y;
                    EveryNeuron[y].BackLinks.Add(c);
                }
            }
        }
        StartCoroutine("FeedForward");

    }

    public void MakeRandomHiddenNeurons() {
        int neuros = Random.Range(0,100);
        HiddenNeurons = new List<Neuron>();
        for (int i = 0; i < neuros; i++)
        {
            Neuron n = new Neuron();
            // Giv den et nummer men husk at du allerede har lavet 7 neuroner i forvejen
            n.NeuronNumber = i + 7;
           // n.RandomizeNeuron();
            HiddenNeurons.Add(n);
        }

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        StopAllCoroutines();
        Dead = true;
        fitness = Fitness.ftns.GetFitness(gameObject);
    }

    public void SetRandomConnections (){
        if (HiddenNeurons == null)
        {
            Debug.Log("Hidden neurons nonexistent");
        }
        else
        {
            List<Neuron> AllNeurons = new List<Neuron>();
            AllNeurons.AddRange(HiddenNeurons);
            AllNeurons.AddRange(OutputNeurons);
            for (int i = 0; i < Random.Range(0,HiddenNeurons.Count); i++)
            {
                Connection c = new Connection();
                allconnections.Add(c);
                c.InnovationNumber = i;
                c.SetRandomWeight();
                int randomInput = Random.Range(0, 5);
                InputNeurons[randomInput].Links.Add(c);
                c.neuronIn = InputNeurons[randomInput];
                c.In = InputNeurons[randomInput].NeuronNumber;
                c.neuronOut = AllNeurons[Random.Range(0, AllNeurons.Count)];
                c.Out = c.neuronOut.NeuronNumber;
                c.neuronOut.BackLinks.Add(c);
            }

            for (int i = 0; i < Random.Range(0, HiddenNeurons.Count); i++)
            {
                Connection c = new Connection();
                allconnections.Add(c);

                c.InnovationNumber = i;
                c.SetRandomWeight();
                int randomHidden = Random.Range(0,HiddenNeurons.Count);
                HiddenNeurons[randomHidden].Links.Add(c);
                c.neuronIn = HiddenNeurons[randomHidden];
                c.In = c.neuronIn.NeuronNumber;
                if (Random.Range(0,2) == 0)
                {
                    int ix = Random.Range(0, AllNeurons.Count);
                    c.neuronOut = AllNeurons[ix];
                    c.Out = c.neuronOut.NeuronNumber;
                    AllNeurons[ix].BackLinks.Add(c);
                    
                }
                else
                {
                    int ix = Random.Range(0, OutputNeurons.Length);
                    c.neuronOut = OutputNeurons[ix];
                    c.Out = c.neuronOut.NeuronNumber;
                    OutputNeurons[ix].BackLinks.Add(c);
                }
            }
        }
    }

    
    IEnumerator FeedForward()
    {
        while (true)
        {
            
            for (int i = 0; i < 5; i++)
            {
                InputNeurons[i].CleanInput();
                InputNeurons[i].AddInput(sensors.Sensorium[i]);
            }

            foreach (Neuron item in InputNeurons)
            {
                item.Prime();
            }
            foreach (Neuron item in HiddenNeurons)
            {
                item.Prime();
            }
            foreach (Neuron item in OutputNeurons)
            {
                item.Prime();
            }

            if (OutputNeurons[0].Output>OutputNeurons[0].Threshhold)
            {
                carctrl.TurnLeft();
            }
            if (OutputNeurons[1].Output > OutputNeurons[1].Threshhold)
            {
                carctrl.TurnRight();
            }
            if (TestCar)
            {
                Debug.Log(allconnections[0].Weight);
                Debug.Log(allconnections[1].Weight);
                Debug.Log(allconnections[2].Weight);
                Debug.Log(allconnections[3].Weight);
                Debug.Log("output Neuron 0 = " + OutputNeurons[0].Output + " Output Neuron 1 = " + OutputNeurons[1].Output);
            }
            yield return null;
        }
    }


    IEnumerator FeedForward2()
    {
        while (true)
        {

            
            foreach (Neuron item in OutputNeurons)
            {
                List<Neuron> visited = new List<Neuron>();
                visited.Add(item);
                item.Prime2(visited);
            }

            if (OutputNeurons[0].Output > OutputNeurons[0].Threshhold)
            {
                carctrl.TurnLeft();
            }
            if (OutputNeurons[1].Output > OutputNeurons[1].Threshhold)
            {
                carctrl.TurnRight();
            }
            if (TestCar)
            {
                Debug.Log("output Neuron 0 = " + OutputNeurons[0].Output + " Output Neuron 1 = " + OutputNeurons[1].Output);
            }
            yield return null;
        }
    }


    void Update () {
        hiddenneurons = HiddenNeurons.Count;
	}
}
