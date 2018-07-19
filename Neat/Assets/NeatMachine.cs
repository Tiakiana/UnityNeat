using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NeatMachine : MonoBehaviour {
    public static NeatMachine NEAT;
    public List<NeuralNetwork> NeuralNetworks = new List<NeuralNetwork>();
    public List<NeuralNetwork> BestNeuralNetworks = new List<NeuralNetwork>();
    public List<float[,]> SavedNeuralNetworks = new List<float[,]>();
    public List<float> fitnessesofBest = new List<float>();
    
    public GameObject Car, Begin;
    public int NumberOfBests = 5;
    public int MutationChance = 15;

    public void MakeNewRandomBatch(int number) {
        for (int i = 0; i < NumberOfBests; i++)
        {
            NeuralNetwork n = new NeuralNetwork();
            n.fitness = 0;
            BestNeuralNetworks.Add(n);
            fitnessesofBest.Add(-9999f);
        }
        

        for (int i = 0; i < number; i++)
        {
            GameObject go  = Instantiate(Car, Begin.transform.position, Quaternion.identity) as GameObject;
            go.GetComponent<NeuralNetwork>().MakeRandomAndStart();
            NeuralNetworks.Add(go.GetComponent<NeuralNetwork>());
        }

    }
    

    IEnumerator GameLoop() {
        while (true)
        {
            if (End)
            {

                SaveTheBest();
                //Destroy all cars

                foreach (NeuralNetwork item in NeuralNetworks)
                {
                    Destroy(item.gameObject);
                }

                // TakeBest And Mate them
                // Mutate the shit out of them
                List<float[,]> everybody = new List<float[,]>();
                foreach (NeuralNetwork item in NeuralNetworks)
                {
                    everybody.Add(SaveNeuralNetwork(item));
                }
                NeuralNetworks.Clear();

                MakeNewRandomBatch(100);

                // Make the best for reference
                GameObject go2 = Instantiate(Car, Begin.transform.position, Quaternion.identity) as GameObject;

                NeuralNetworks.Add(go2.GetComponent<NeuralNetwork>());
                Debug.Log("Making best");
                go2.GetComponent<NeuralNetwork>().LoadNeuralNetwork(SavedNeuralNetworks[0]);
               

                


                for (int i = 0; i < NumberOfBests; i++)
                {

                    for (int x = 0; x < 20; x++)
                    {
                        GameObject go = Instantiate(Car, Begin.transform.position, Quaternion.identity) as GameObject;
                        NeuralNetworks.Add(go.GetComponent<NeuralNetwork>());

                        if (Random.Range(1,101)<= 30)
                        {
                            go.GetComponent<NeuralNetwork>().LoadNeuralNetwork(MakeChild(SavedNeuralNetworks[i], everybody[Random.Range(0, everybody.Count)]));

                        }
                        else
                        {

                        go.GetComponent<NeuralNetwork>().LoadNeuralNetwork(MakeChild(SavedNeuralNetworks[i], SavedNeuralNetworks[Random.Range(0,NumberOfBests)]));
                        }


                        if (Random.Range(1,101)>= MutationChance)
                        {
                        go.GetComponent<NeuralNetwork>().MutateThis();
                        }

                        //Destroy(go.GetComponent<NeuralNetwork>());
                        //Start heer igen det er vigtigt! //  Du mangler at finde ud af hvordan du overleverer neurale netværker på tværs af generationer.
                       // go.AddComponent<ObjectCopier.Clone<NeuralNetwork>(BestNeuralNetworks[i])>;
                    }
                }
                // Create new batch
                End = false;
            }

            yield return new WaitForSeconds(1);
        }



    }

    public void SaveTheBest() {
        List<NeuralNetwork> SortedList = NeuralNetworks.OrderBy(o => o.fitness).ToList();
       

        SortedList.Reverse();
        for (int i = 0; i < 3; i++)
        {
            Debug.Log(i + "'s fitness is: "+SortedList[i].fitness);
        }

        for (int i = 0; i < NumberOfBests; i++)
        {
            if (SortedList[i].fitness> fitnessesofBest[4])
            {
                BestNeuralNetworks.Add(SortedList[i]);
            }
        }
        fitnessesofBest.Clear();
        


        SortedList = BestNeuralNetworks.OrderBy(o => o.fitness).ToList();
        SortedList.Reverse();
        if (SortedList.Count>NumberOfBests)
        {
            SortedList.RemoveRange(NumberOfBests, SortedList.Count-NumberOfBests);
        }

        BestNeuralNetworks.Clear();
        BestNeuralNetworks.AddRange(SortedList);
        for (int i = 0; i < NumberOfBests; i++)
        {
            fitnessesofBest.Add(BestNeuralNetworks[i].fitness);
        }

        for (int i = 0; i < 3; i++)
        {
            Debug.Log("Best of the best:" + i + "'s fitness is: " + BestNeuralNetworks[i].fitness);
        }

        SavedNeuralNetworks.Clear();
        for (int i = 0; i < BestNeuralNetworks.Count; i++)
        {
            SavedNeuralNetworks.Add(SaveNeuralNetwork(BestNeuralNetworks[i]));
        }

    }

    private void Awake()
    {
        Physics2D.IgnoreLayerCollision(8,8,true);
        NEAT = this;
    }
        bool End = false;
    IEnumerator CheckForEnd() {
        while (true)
        {
            if (!End)
            {

            bool somethingisAlive = false;
            foreach (NeuralNetwork item in NeuralNetworks)
            {
                if (!item.Dead)
                {
                    somethingisAlive = true;
                }
            }
            if (!somethingisAlive)
            {
                End = true;
            }
            }

            yield return null;
        }

    }


    public float[,] MakeChild(float[,] nn1, float[,] nn2) {
        float[,] res = new float[200,200];


        for (int x = 0; x < 200; x++)
        {
            for (int y = 0; y < 200; y++)
            {
                if (nn1[x,y] != 0 && nn2[x,y] != 0)
                {
                    if (Random.Range(0,2) == 0)
                    {
                        res[x, y] = nn1[x,y];
                    }
                    else
                    {
                        res[x, y] = nn2[x, y];
                    }

                }
                else if (nn1[x,y] != 0 && nn2[x,y] == 0)
                {
                    res[x, y] = nn1[x, y];
                }
                else if (nn1[x, y] == 0 && nn2[x, y] == 0)
                {
                    res[x, y] = nn2[x, y];
                }
            }
        }
        return res;

    }

    

    public float[,] SaveNeuralNetwork(NeuralNetwork nn) {
        float[,] savedItem = new float[200, 200];

        foreach (Connection item in nn.allconnections)
        {
            savedItem[item.In, item.Out] = item.Weight;
        }

        return savedItem;
            }


    public void LoadNeuralNet(float [,] nnSave) {
        NeuralNetwork nn = new NeuralNetwork();
        for (int x = 0; x < 200; x++)
        {
            for (int y = 0; y < 200; y++)
            {
                if (nnSave[x,y] >0)
                {

                }
            }
        }

    }

    // Use this for initialization
    void Start () {
        MakeNewRandomBatch(100);
        StartCoroutine("CheckForEnd");
        StartCoroutine("GameLoop");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    
    
}
