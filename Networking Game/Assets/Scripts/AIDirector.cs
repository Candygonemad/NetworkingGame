using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDirector : MonoBehaviour
{
    [HideInInspector]
    public bool serverStarted;
    [HideInInspector]
    public List<EnemyPattern> patterns;
    [HideInInspector]
    public float nextTime;

    private float timeBetweenPatterns;
    [HideInInspector]
    public GameObject[] players;

    //Information
    [HideInInspector]
    public float averageHealth;
    [HideInInspector]
    public int numberOfPatternsSurvived;
    [HideInInspector]
    public int enemiesHit;
    [HideInInspector]
    public int bulletsShot;
    [HideInInspector]
    public float weaponAccuracy;

    //Weights That Change
    [HideInInspector]
    public float weightHealth;
    [HideInInspector]
    public float weightSurvived;
    [HideInInspector]
    public float weightAccuracy;

    //Output Patterns
    [HideInInspector]
    public int weightTotal;
    //private enum Difficulty { Easy, Medium, Hard };
    //private Difficulty difficulty;
    private int difficultyPatterns;
    [HideInInspector]
    public List<EnemyPattern> currentPatterns;

    private bool nextWave = true;

    public int numPlayers = 2;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!serverStarted)
        {
            if (GameObject.FindGameObjectsWithTag("Player").Length >= numPlayers)
                ServerStarted();
            else
                return;
        }
            

        if (Time.time < nextTime)
            return;
        if(nextWave)
        {
            nextWave = false;
            //Debug.Log("Next Wave");
            //Get Information
            //Debug.Log("Getting Information...");
            //Health
            averageHealth = 0;
            //Debug.Log("Players Length: " + players.Length);
            for (int i = 0; i < players.Length; i++)
            {
                averageHealth += players[i].GetComponent<Health>().currentHealth.Value;
            }

            averageHealth /= players.Length;

            //Accuracy
            weaponAccuracy = (float)enemiesHit / (float)bulletsShot;

            //Patterns Survived
            numberOfPatternsSurvived++;

            //Apply Information
            //Debug.Log("Applying Information...");
            //Health
            if (averageHealth < 4 * players.Length)
            {
                weightHealth = .5f;
            }
            else if (averageHealth < 7 * players.Length)
            {
                weightHealth = 1.25f;
            }
            else
            {
                weightHealth = 2.0f;
            }

            //Accuracy
            if (weaponAccuracy < .25f)
            {
                weightAccuracy = 1.5f;
            }
            else if (averageHealth < .6)
            {
                weightAccuracy = 2.5f;
            }
            else
            {
                weightAccuracy = 3.0f;
            }

            //Patterns Survived
            if (numberOfPatternsSurvived < 7 * players.Length)
            {
                weightSurvived = 2.5f;
            }
            else if (numberOfPatternsSurvived < 18 * players.Length)
            {
                weightSurvived = 3.75f;
            }
            else
            {
                weightSurvived = 5.0f;
            }

            weightTotal = Mathf.RoundToInt(weightHealth + weightAccuracy + weightSurvived);

            //Spawn Using Patterns
            //Debug.Log("Getting Number of Patterns...");
            if (weightTotal < 6.5f)
            {
                difficultyPatterns = 6;
            }
            else if (weightTotal < 8)
            {
                difficultyPatterns = 10;
            }
            else
            {
                difficultyPatterns = 16;
            }

            //Go through and choose patterns based on remaining difficultyPatterns and assign a relative amount of enemies
            //Debug.Log("Choosing Patterns to Spawn...");
            //Debug.Log("Difficulty Patterns: " + difficultyPatterns);
            float difficultNextTime = 0;
            while (difficultyPatterns > 0 && Time.time > difficultNextTime)
            {
                int difficulty = Random.Range(1, 3);
                if (difficulty > difficultyPatterns)
                    difficulty = Random.Range(1, difficultyPatterns);

                //Debug.Log("Number of Patterns: " + patterns.Count);
                EnemyPattern difficultyPattern = patterns[Random.Range(0, patterns.Count)];
                switch (difficulty)
                {
                    case 1: EasySpawn(difficultyPattern); break;
                    case 2: MediumSpawn(difficultyPattern); break;
                    case 3: HardSpawn(difficultyPattern); break;
                }

                difficultyPatterns -= difficulty;

                if (difficulty == 1)
                {
                    difficultNextTime = Time.time + 2;
                }
                else if (difficulty == 2)
                {
                    difficultNextTime = Time.time + 1;
                }
                else
                {
                    difficultNextTime = Time.time;
                }
                //Debug.Log("Difficulty Patterns: " + difficultyPatterns);
            }

            nextTime = Time.time + timeBetweenPatterns;
            nextWave = true;
        }
        

        
    }

    public void ServerStarted()
    {
        serverStarted = true;

        timeBetweenPatterns = 4.0f;
        nextTime = Time.time;
        players = GameObject.FindGameObjectsWithTag("Player");
        GameObject patternParent = GameObject.Find("Patterns");
        //Debug.Log("Pattern Parent: " + patternParent);
        List<GameObject> patternObjects = new List<GameObject>();

        int children = patternParent.transform.childCount;
        for (int i = 0; i < children; ++i)
        {
            if(patternParent.transform.GetChild(i).gameObject.activeSelf)
                patternObjects.Add(patternParent.transform.GetChild(i).gameObject);
        }

        //Debug.Log("Pattern Object Count: " + patternObjects.Count);

        for (int i = 0; i < patternObjects.Count; i++)
        {
            patterns.Add(patternObjects[i].GetComponent<EnemyPattern>());
        }

        //Debug.Log("Patterns Count: " + patterns.Count);

        for (int i = 0; i < patterns.Count; i++)
        {
            for(int n = 0; n < players.Length; n++)
            {
                patterns[i].players.Add(players[n]);
            }
        }
    }

    public void EasySpawn(EnemyPattern difficultyPattern)
    {
        if (difficultyPattern.pattern == MovementPattern.line)
        {
            difficultyPattern.ActivateLineSpawners(Random.Range(4, 8));
        }
        else if (difficultyPattern.pattern == MovementPattern.circle)
        {
            difficultyPattern.ActivateCircleSpawners(Random.Range(4, 8));
        }
        else if (difficultyPattern.pattern == MovementPattern.follow)
        {
            difficultyPattern.ActivateFollowSpawners(4);
        }
    }

    public void MediumSpawn(EnemyPattern difficultyPattern)
    {
        if (difficultyPattern.pattern == MovementPattern.line)
        {
            difficultyPattern.ActivateLineSpawners(Random.Range(8, 12));
        }
        else if (difficultyPattern.pattern == MovementPattern.circle)
        {
            difficultyPattern.ActivateCircleSpawners(Random.Range(8, 12));
        }
        else if (difficultyPattern.pattern == MovementPattern.follow)
        {
            difficultyPattern.ActivateFollowSpawners(8);
        }
    }

    public void HardSpawn(EnemyPattern difficultyPattern)
    {
        if (difficultyPattern.pattern == MovementPattern.line)
        {
            difficultyPattern.ActivateLineSpawners(Random.Range(12, 16));

        }
        else if (difficultyPattern.pattern == MovementPattern.circle)
        {
            difficultyPattern.ActivateCircleSpawners(Random.Range(12, 16));
        }
        else if (difficultyPattern.pattern == MovementPattern.follow)
        {
            difficultyPattern.ActivateFollowSpawners(12);
        }
    }

    private IEnumerator WaitForPattern(int difficulty)
    {
        if (difficulty == 1)
        {
            yield return new WaitForSeconds(Random.Range(3, 5));
        }
        else if (difficulty == 2)
        {
            yield return new WaitForSeconds(Random.Range(2, 4));
        }
        else
        {
            yield return new WaitForSeconds(Random.Range(1, 3));
        }
    }
}
