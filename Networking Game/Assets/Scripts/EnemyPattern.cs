using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public enum MovementPattern { line, circle, follow }
public enum LineDirection { Down, Left, Right }

public class EnemyPattern : MonoBehaviour
{
    

    //How enemies will move
    [Header("Presets")]
    public MovementPattern pattern;

    //Where to Spawn
    [SerializeField]
    private List<GameObject> spawners;
    [SerializeField]
    private bool showSpawners;
    [SerializeField]
    public GameObject enemyType;

    //Details
    [SerializeField]
    private bool canShoot;
    public List<GameObject> players;
    [SerializeField]
    private bool showPlayers;

    //Line
    [SerializeField]
    private LineDirection currentDirection;

    //Circle
    [SerializeField]
    private Vector2 radius;
    [SerializeField]
    private bool clockwise;

    //Follow
    //Use the player if canShoot is false

/*
    #region Editor
#if UNITY_EDITOR

    [CustomEditor(typeof(EnemyPattern))]
    public class EnemyPatternEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EnemyPattern enemyPattern = (EnemyPattern)target;

            DrawDetails(enemyPattern);

            //DrawSpawnerList(enemyPattern);

            if (enemyPattern.pattern == MovementPattern.line)
            {
                DrawLinePattern(enemyPattern);
            }
            else if (enemyPattern.pattern == MovementPattern.circle)
            {
                DrawCirclePattern(enemyPattern);
            }
            else if (enemyPattern.pattern == MovementPattern.follow)
            {
                DrawFollowPattern(enemyPattern);
            }
        }

        static void DrawDetails(EnemyPattern enemyPattern)
        {
            serializedObject.FindProperty("")
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Details", EditorStyles.boldLabel);

            enemyPattern.canShoot = EditorGUILayout.Toggle("Can Shoot", enemyPattern.canShoot);

            if(enemyPattern.pattern != MovementPattern.follow && enemyPattern.canShoot)
            {
                enemyPattern.showPlayers = EditorGUILayout.Foldout(enemyPattern.showPlayers, "Players");

                if (enemyPattern.showPlayers)
                {
                    EditorGUI.indentLevel++;

                    int size = Mathf.Max(0, EditorGUILayout.IntField("Size", enemyPattern.players.Count));

                    while (size > enemyPattern.players.Count)
                    {
                        enemyPattern.players.Add(null);
                    }

                    while (size < enemyPattern.players.Count)
                    {
                        enemyPattern.players.RemoveAt(enemyPattern.players.Count - 1);
                    }

                    for (int i = 0; i < enemyPattern.players.Count; i++)
                    {
                        enemyPattern.players[i] = EditorGUILayout.ObjectField("Element " + i, enemyPattern.players[i], typeof(GameObject), true) as GameObject;
                    }

                    EditorGUI.indentLevel--;
                }
            }
        }

        static void DrawSpawnerList(EnemyPattern enemyPattern)
        {
            enemyPattern.showSpawners = EditorGUILayout.Foldout(enemyPattern.showSpawners, "Spawners");

            if (enemyPattern.showSpawners)
            {
                EditorGUI.indentLevel++;

                List<GameObject> list = enemyPattern.spawners;
                int size = Mathf.Max(0, EditorGUILayout.IntField("Size", enemyPattern.players.Count));

                while (size > enemyPattern.players.Count)
                {
                    enemyPattern.players.Add(null);
                }

                while (size < enemyPattern.players.Count)
                {
                    enemyPattern.players.RemoveAt(enemyPattern.players.Count - 1);
                }

                for (int i = 0; i < enemyPattern.players.Count; i++)
                {
                    enemyPattern.players[i] = EditorGUILayout.ObjectField("Element " + i, enemyPattern.players[i], typeof(GameObject), true) as GameObject;
                }

                EditorGUI.indentLevel--;
            }
        }

        static void DrawLinePattern(EnemyPattern enemyPattern)
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Line", EditorStyles.boldLabel);

            EditorGUI.indentLevel++;

            enemyPattern.currentDirection = (LineDirection)EditorGUILayout.EnumFlagsField("Move Direction", new LineDirection());

            EditorGUI.indentLevel--;
        }

        static void DrawCirclePattern(EnemyPattern enemyPattern)
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Circle", EditorStyles.boldLabel);

            EditorGUI.indentLevel++;

            enemyPattern.radius = EditorGUILayout.Vector2Field("Radius", enemyPattern.radius);

            enemyPattern.clockwise = EditorGUILayout.Toggle("Clockwise", enemyPattern.clockwise);

            EditorGUI.indentLevel--;
        }

        static void DrawFollowPattern(EnemyPattern enemyPattern)
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Follow", EditorStyles.boldLabel);

            EditorGUI.indentLevel++;
            enemyPattern.showPlayers = EditorGUILayout.Foldout(enemyPattern.showPlayers, "Players");

            if (enemyPattern.showPlayers)
            {
                EditorGUI.indentLevel++;

                List<GameObject> list = enemyPattern.players;
                int size = Mathf.Max(0, EditorGUILayout.IntField("Size", enemyPattern.players.Count));

                while (size > enemyPattern.players.Count)
                {
                    enemyPattern.players.Add(null);
                }

                while (size < enemyPattern.players.Count)
                {
                    enemyPattern.players.RemoveAt(enemyPattern.players.Count - 1);
                }

                for (int i = 0; i < enemyPattern.players.Count; i++)
                {
                    enemyPattern.players[i] = EditorGUILayout.ObjectField("Element " + i, enemyPattern.players[i], typeof(GameObject), true) as GameObject;
                }

                EditorGUI.indentLevel--;
            }
                
            EditorGUI.indentLevel--;
        }
    }

#endif
    #endregion
*/


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActivateLineSpawners(int numEnemies)
    {
        for(int i = 0; i < spawners.Count; i++)
        {
            //Debug.Log("Enemy Type: " + enemyType);
            //Debug.Log("Current Direction: " + currentDirection);
            spawners[i].GetComponent<EnemySpawner>().Spawn(numEnemies / spawners.Count, enemyType, currentDirection, Random.Range(1.0f, 3.0f));
        }
    }

    public void ActivateCircleSpawners(int numEnemies)
    {
        for (int i = 0; i < spawners.Count; i++)
        {
            spawners[i].GetComponent<EnemySpawner>().Spawn(numEnemies / spawners.Count, enemyType, radius, clockwise, Random.Range(1.0f, 3.0f));
        }
    }

    public void ActivateFollowSpawners(int numEnemies)
    {
        Debug.Log("Players: " + players.Count);
        for (int i = 0; i < spawners.Count; i++)
        {
            spawners[i].GetComponent<EnemySpawner>().Spawn(numEnemies / spawners.Count, enemyType, players, Random.Range(1.0f, 3.0f));
        }
    }
}
