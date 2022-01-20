using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdManager : MonoBehaviour
{

    public Timer timer;
    [SerializeField] List<GameObject> flappyBirds;
    [SerializeField] GameObject Camera;
    public int Generation = 0;

    private FlappyScript Champ1;
    private FlappyScript Champ2;
    private bool HasChamps = false;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        List<FlappyScript> LivingFlappyScripts = new List<FlappyScript>();
        int nrAlive = 0;
        foreach (var flap in flappyBirds)
        {
            if (!flap.GetComponent<FlappyScript>().Dead)
            {
                ++nrAlive;
                ScoreManagerScript.Score = flap.GetComponent<FlappyScript>().Score;
                if (!HasChamps) LivingFlappyScripts.Add(flap.GetComponent<FlappyScript>());
                if (nrAlive == 3)
                {
                    LivingFlappyScripts.Clear();
                    break;
                }
            }
        }

        if (nrAlive == 2 && !HasChamps)
        {
            Champ1 = LivingFlappyScripts[0];
            Champ2 = LivingFlappyScripts[1];
            HasChamps = true;
        }

        if (nrAlive == 0)
        {
            if (!HasChamps)
            {
                //get the furthest positions

                foreach (GameObject flappy in flappyBirds)
                {
                    if (Champ1 != null && Champ2 != null)
                    {
                        if (Champ1.transform.position.x < flappy.transform.position.x) Champ1 = flappy.GetComponent<FlappyScript>();
                        else if (Champ2.transform.position.x < flappy.transform.position.x) Champ2 = flappy.GetComponent<FlappyScript>();
                    }
                    else
                    {
                        Champ1 = flappy.GetComponent<FlappyScript>();
                        Champ2 = flappy.GetComponent<FlappyScript>();
                    }
                }
                HasChamps = true;
            }
            EndGeneration();
        }
    }
    void EndGeneration()
    {
        // move on to the next generation
        Generation++;

        Transform CameraPos = Camera.GetComponent<Camera>().transform;
        var newTrasform = Champ1.transform;
        newTrasform.position = new Vector3(CameraPos.position.x - 0.738f, 1.267f, -3.96f);

        foreach (var item in flappyBirds)
        {
            if (ScoreManagerScript.Score != 0) // if the score is 0 there are no good ai bots so make new ones
            {
                item.GetComponent<FlappyScript>().GetBestVersion(Champ1.weights, Champ2.weights); //pas the two best players 
            }
            else item.GetComponent<FlappyScript>().GenerateWeights();

            item.GetComponent<FlappyScript>().Respawn(newTrasform);

        }
        ScoreManagerScript.Score = 0;
    }
}
