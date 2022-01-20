using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class FlappyScript : MonoBehaviour
{
    // public AudioClip FlyAudioClip, DeathAudioClip, ScoredAudioClip;
    public Sprite GetReadySprite;
    public float RotateUpSpeed = 1, RotateDownSpeed = 1;
    public GameObject IntroGUI, DeathGUI;
    public Collider2D restartButtonGameCollider;
    public float VelocityPerJump = 3;
    public float XSpeed = 1;
    public GameObject Timer;

    public GameObject Spawner;
    public bool Dead = false;


    public List<List<List<float>>> weights = new List<List<List<float>>>();
    public int Score = 0;

    //private variables
    const int m_MutationProbability = 20; // mutate every 1/2
    const int m_InputNodes = 3;
    const int m_HiddenNodes = 4;
    const int m_OuputNodes = 1;

    // Use this for initialization
    void Start()
    {
        GenerateWeights();

    }

    FlappyYAxisTravelState flappyYAxisTravelState;

    enum FlappyYAxisTravelState
    {
        GoingUp, GoingDown
    }

    Vector3 birdRotation = Vector3.zero;
    // Update is called once per frame



    #region GameCode
    void Update()
    {
        // Debug.ClearDeveloperConsole();
        //handle back key in Windows Phone
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();

        if (GameStateManager.GameState == GameState.Intro)
        {
            MoveBirdOnXAxis();
            if (WasTouchedOrClicked())
            {
                BoostOnYAxis();
                GameStateManager.GameState = GameState.Playing;
                IntroGUI.SetActive(false);
                Score = 0;
            }
        }
        else if (GameStateManager.GameState == GameState.Playing && !Dead)
        {
            MoveBirdOnXAxis();
            if (NeuralNetworkFlap() && GetComponent<Rigidbody2D>().velocity.y <= 0)
            {
                BoostOnYAxis();
            }


        }
        else if (GameStateManager.GameState == GameState.Dead)
        {
            Vector2 contactPoint = Vector2.zero;

            if (Input.touchCount > 0)
                contactPoint = Input.touches[0].position;
            if (Input.GetMouseButtonDown(0))
                contactPoint = Input.mousePosition;

            //check if user wants to restart the game
            if (restartButtonGameCollider == Physics2D.OverlapPoint
                (Camera.main.ScreenToWorldPoint(contactPoint)))
            {
                GameStateManager.GameState = GameState.Intro;
                Application.LoadLevel(Application.loadedLevelName);
            }
        }
    }



    void FixedUpdate()
    {

        //just jump up and down on intro screen
        if (GameStateManager.GameState == GameState.Intro)
        {
            if (GetComponent<Rigidbody2D>().velocity.y < -1) //when the speed drops, give a boost
                GetComponent<Rigidbody2D>().AddForce(new Vector2(0, GetComponent<Rigidbody2D>().mass * 5500 * Time.deltaTime)); //lots of play and stop
                                                                                                                                //and play and stop etc to find this value, feel free to modify
        }
        else if (GameStateManager.GameState == GameState.Playing || GameStateManager.GameState == GameState.Dead)
        {
            FixFlappyRotation();
        }
    }

    bool WasTouchedOrClicked()
    {
        if (Input.GetButtonUp("Jump") || Input.GetMouseButtonDown(0) ||
            (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Ended))
            return true;
        else
            return false;
    }

    void MoveBirdOnXAxis()
    {
        transform.position += new Vector3(Time.deltaTime * XSpeed, 0, 0);
    }

    void BoostOnYAxis()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(0, VelocityPerJump);
        // GetComponent<AudioSource>().PlayOneShot(FlyAudioClip);
    }
    float GetYSpeed()
    {
        return GetComponent<Rigidbody2D>().velocity.y;
    }


    private void FixFlappyRotation()
    {
        if (GetComponent<Rigidbody2D>().velocity.y > 0) flappyYAxisTravelState = FlappyYAxisTravelState.GoingUp;
        else flappyYAxisTravelState = FlappyYAxisTravelState.GoingDown;

        float degreesToAdd = 0;

        switch (flappyYAxisTravelState)
        {
            case FlappyYAxisTravelState.GoingUp:
                degreesToAdd = 6 * RotateUpSpeed;
                break;

            case FlappyYAxisTravelState.GoingDown:
                degreesToAdd = -3 * RotateDownSpeed;
                break;

            default:
                break;
        }
        //solution with negative eulerAngles found here: http://answers.unity3d.com/questions/445191/negative-eular-angles.html

        //clamp the values so that -90<rotation<45 *always*
        birdRotation = new Vector3(0, 0, Mathf.Clamp(birdRotation.z + degreesToAdd, -90, 45));
        transform.eulerAngles = birdRotation;
    }


    void OnTriggerEnter2D(Collider2D col)
    {


        if (GameStateManager.GameState == GameState.Playing)
        {

            if (col.gameObject.tag == "Pipeblank") //pipeblank is an empty gameobject with a collider between the two pipes
            {
                // GetComponent<AudioSource>().PlayOneShot(ScoredAudioClip);
                Score++;
            }
            else if (col.gameObject.tag == "Pipe")
            {
                FlappyDies();
            }

        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {

        if (col.gameObject.tag == "Flappy")
        {
            Physics2D.IgnoreCollision(this.GetComponent<Collider2D>(), col.collider);
        }
        else if (col.gameObject.tag == "Floor")
        {
            FlappyDies();
        }

        //if (GameStateManager.GameState == GameState.Playing)
        //{
        //    if (col.gameObject.tag == "Floor")
        //    {
        //        FlappyDies();
        //    }
        //}
    }

    void FlappyDies()
    {
        Dead = true;
        // GameStateManager.GameState = GameState.Dead;
        // DeathGUI.SetActive(true);
        // GetComponent<AudioSource>().PlayOneShot(DeathAudioClip);
    }
    #endregion
    //my code
    #region Mycode

    GameObject GetPipe()
    {
        SpawnerScript s = Spawner.GetComponent<SpawnerScript>();
        var item = s.GetClosestPipe(transform);

        return item;
    }
    bool NeuralNetworkFlap()
    {

        var bird = GetComponent<Rigidbody2D>();

        //make correct neural network
        List<List<float>> NeuralNetwork = new List<List<float>>();

        NeuralNetwork.Add(new List<float>());
        NeuralNetwork.Add(new List<float>());
        NeuralNetwork.Add(new List<float>());

        for (int i = 0; i < m_InputNodes; i++) NeuralNetwork[0].Add(0);
        for (int i = 0; i < m_HiddenNodes; i++) NeuralNetwork[1].Add(0);
        for (int i = 0; i < m_OuputNodes; i++) NeuralNetwork[2].Add(0); //add the correct nodes

        //handle input layer
        NeuralNetwork[0][0] = -bird.velocity.y; //set the velocity of the bird as an input

        var pipe = GetPipe(); //get the closest pipe if there is one

        if (pipe == null)
        {
            NeuralNetwork[0][1] = 0;
            NeuralNetwork[0][2] = 0;
        }
        else
        {
            NeuralNetwork[0][1] = pipe.transform.position.y - bird.position.y; //add the distance to the pipe as a second input variable
            if (pipe.GetComponent<PipeMovement>().direction) NeuralNetwork[0][2] = 0; //pipe movement was another input variable but this is default -> I have disabled the pipe movement
            else NeuralNetwork[0][2] = 0;
        }
        //we loop over all of the nodes, we take their input into account and multiply them with their weights, then we have some hidden layers
        //the hidden layers also get multiplied by their weights so we get theire outcome
        for (int i = 0; i < NeuralNetwork.Count - 1; i++)
        {
            for (int j = 0; j < NeuralNetwork[i + 1].Count; j++)
            {
                for (int k = 0; k < NeuralNetwork[i].Count; k++)
                {
                    NeuralNetwork[i + 1][j] += NeuralNetwork[i][k] * weights[i][k][j];
                }
                if (NeuralNetwork[i + 1][j] <= 0) NeuralNetwork[i + 1][j] = (float)(Math.Pow(2, NeuralNetwork[i + 1][j]) - 1);
                else NeuralNetwork[i + 1][j] = 1 - (float)(Math.Pow(2, -NeuralNetwork[1 + i][j]));
            }
        }
        return 0 <= NeuralNetwork[2][0]; // return output node if positive -> Jump
    }
    public void GenerateWeights()
    {
        //give all the weights random values
        weights.Clear();
        var ww1 = new List<List<float>>();
        for (int i = 0; i < m_InputNodes; i++)
        {
            var w1 = new List<float>();
            for (int j = 0; j < m_HiddenNodes; j++)
            {
                w1.Add(0);
            }
            ww1.Add(w1);
        }
        var ww2 = new List<List<float>>();

        for (int i = 0; i < m_HiddenNodes; i++)
        {
            var w2 = new List<float>();
            for (int j = 0; j < m_OuputNodes; j++)
            {
                w2.Add(0);
            }
            ww2.Add(w2);
        }
        weights.Add(ww1);
        weights.Add(ww2);
        for (int i = 0; i < weights.Count; i++)
        {
            for (int j = 0; j < weights[i].Count; j++)
            {
                for (int k = 0; k < weights[i][j].Count; k++)
                {
                    weights[i][j][k] = Timer.GetComponent<Timer>().GetRandomFloat();
                }
            }
        }
    }
    public List<List<List<float>>> GetWeights()
    {
        return weights;
    }

    public void GetBestVersion(List<List<List<float>>> FirstBirdWeights, List<List<List<float>>> SecondBirdWeights)
    { // simulate natural selection -> take the two best of my flock and scramble their values to make a new generation
        float random = 0;
        for (int i = 0; i < weights.Count; i++)
        {
            for (int j = 0; j < weights[i].Count; j++)
            {
                for (int k = 0; k < weights[i][j].Count; k++)
                {
                    random = Timer.GetComponent<Timer>().GetRandomInteger(1, m_MutationProbability + 1);
                    if (random == 1)
                    {
                        weights[i][j][k] = weights[i][j][k] * Timer.GetComponent<Timer>().GetRandomFloat(0.8f, 1.2f);
                    }
                    else if (random % 2 == 0) weights[i][j][k] = FirstBirdWeights[i][j][k];
                    else weights[i][j][k] = SecondBirdWeights[i][j][k];
                }
            }
        }
    }
    public void Respawn(Transform transform)
    {
        Dead = false;
        Score = 0;
        GetComponent<Transform>().position = transform.position;
        var pipes = GameObject.FindGameObjectsWithTag("Pipe");
        var pipeblanks = GameObject.FindGameObjectsWithTag("Pipeblank");
        foreach (var pipe in pipes) Destroy(pipe);
        foreach (var item in pipeblanks) Destroy(item);
    }
    #endregion


}