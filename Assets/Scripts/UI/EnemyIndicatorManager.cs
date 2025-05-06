using UnityEngine;

public class EnemyIndicatorManager : MonoBehaviour
{
    [SerializeField] private GameObject visualIndicator;
    
    private Canvas canvas;


    private void Start()
    {
        canvas = GetComponent<Canvas>(); 
    }

    public void SpawnIndicator(GameObject enemy)
    { 

        // Instantiate the indicator in the canvas
        GameObject indicator = Instantiate(visualIndicator, canvas.transform);

        // Assign the enemy and player to the indicator's script
        EnemyIndicatorUI indicatorScript = indicator.GetComponent<EnemyIndicatorUI>();

        indicatorScript.enemy = enemy;

    }
}