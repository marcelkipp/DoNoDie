using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World_Builder : MonoBehaviour
{
    public bool spawnEnemies = true;
    public bool spawnDiamonds = true;
    public bool spawnPowerups = true;

    public Terrain Terrain;
    public LayerMask TerrainLayer;
    public static float TerrainLeft, TerrainRight, TerrainTop, TerrainBottom, TerrainWidth, TerrainLength, TerrainHeight;

    public GameObject pickupObject;
    public GameObject[] powerups;
    public GameObject[] enemies;

    public bool ShowCursor = true;

    public float offsetDiamond = 0f;
    public float offsetPowerup = 0f;
    public float offsetEnemy = 0f;

    public int maxPowerups = 50;
    public int minPowerups = 30;
    public int maxDiamonds = 200;
    public int minDiamonds = 50;
    public int amountOfStartingEnemys = 50;
    public int minEnemys = 30;

    private int currentAmoutOfDiamonds;
    private int currentAmountOfPowerups;
    private int currentAmountOfEnemies;

    private float minYSpawn = 90.38f;
    private float maxYSpawn = 105f;

    private void Awake()
    {
        TerrainLeft = Terrain.transform.position.x;
        TerrainBottom = Terrain.transform.position.z;
        TerrainWidth = Terrain.terrainData.size.x;
        TerrainLength = Terrain.terrainData.size.z;
        TerrainHeight = Terrain.terrainData.size.y;
        TerrainRight = TerrainLeft + TerrainWidth;
        TerrainTop = TerrainBottom + TerrainLength;

        if(spawnDiamonds) InitDiamondSpwaner();
        if(spawnEnemies) InitEnemySpwaner();
        if(spawnPowerups) InitPowerupSpawner();

    }

    private void Start()
    {
        Cursor.visible = ShowCursor;
    }

    private void Update()
    {
        checkSpawner();
    }

    public void InitDiamondSpwaner()
    {
        InstanciateAtRandomPosition(pickupObject, maxDiamonds, offsetDiamond);
        currentAmoutOfDiamonds = maxDiamonds;
    }

    public void InitPowerupSpawner()
    {
        InstanciateAtRandomPosition(getRandomPowerup(), maxPowerups, offsetPowerup);
        currentAmountOfPowerups = maxPowerups;
    }

    public void InitEnemySpwaner()
    {
        InstanciateAtRandomPosition(getRandomEnemy(), amountOfStartingEnemys, offsetEnemy);
        currentAmountOfEnemies = amountOfStartingEnemys;
    }


    // Überprüft ob es noch genug Objekte in der Welt gibt
    // wenn nicht werden neue gespawnt
    public void checkSpawner()
    {
        if (spawnDiamonds && currentAmoutOfDiamonds < minDiamonds)
        {
            InstanciateAtRandomPosition(pickupObject, 1, offsetDiamond);
        }

        if (spawnPowerups && currentAmountOfPowerups < minPowerups)
        {
            InstanciateAtRandomPosition(getRandomPowerup(), 1, offsetDiamond);
        }
    }

    public void InstanciateAtRandomPosition(GameObject Resource, int amount, float offset)
    {

        var i = 0;
        float terrainHeight = 0f;
        RaycastHit hit;
        float randomPositionX, randomPositionY, randomPositionZ;
        Vector3 randomPosition = Vector3.zero;
        float rotY = 0f;

        do
        {
            
            randomPositionX = Random.Range(TerrainLeft, TerrainRight);
            randomPositionZ = Random.Range(TerrainBottom, TerrainTop);

            if (Physics.Raycast(new Vector3(randomPositionX, 9999f, randomPositionZ), Vector3.down, out hit, Mathf.Infinity, TerrainLayer))
            {
                terrainHeight = hit.point.y;
            }

            randomPositionY = terrainHeight + offset;

            // Objekt nur spawnen wenn es in den angegebenen Grenzen liegt
            if (randomPositionY >= minYSpawn && randomPositionY <= maxYSpawn)
            {
                i++;
                randomPosition = new Vector3(randomPositionX, randomPositionY, randomPositionZ);

                // Nur der Diamand muss vor dem instanzieren gedreht werden um 270 Grad
                if (Resource.name == "Pickup_Diamond") rotY = 270f;

                Instantiate(Resource, randomPosition, Quaternion.Euler(rotY, Random.Range(0, 360), 0));
            }

        } while (i < amount); 
        
    }

    public GameObject getRandomPowerup()
    {
        return powerups[Random.Range(0, powerups.Length)];
    }

    public GameObject getRandomEnemy()
    {
        return enemies[Random.Range(0, enemies.Length)];
    }

    public void lowerCurrentDiamonds()
    {
        currentAmoutOfDiamonds--;
        Debug.Log("Diamonds: " + currentAmoutOfDiamonds); 
    }

    public void lowerCurrentPowerups()
    {
        currentAmountOfPowerups--;
        Debug.Log("Powerups: " + currentAmountOfPowerups);
    }
}
