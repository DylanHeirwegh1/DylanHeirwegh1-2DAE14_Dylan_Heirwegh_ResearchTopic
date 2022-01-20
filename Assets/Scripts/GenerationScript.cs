using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GenerationScript : MonoBehaviour
{
    public GameObject FlappyManager;
    public Text GenerationText;
    // Update is called once per frame
    void Update()
    {
        GenerationText.text = "Generation: " + FlappyManager.GetComponent<BirdManager>().Generation.ToString();

    }
}
