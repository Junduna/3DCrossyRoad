using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] GameObject grass;
    [SerializeField] GameObject road;
    [SerializeField] int extent = 7;
    [SerializeField] int frontDistance = 10;
    [SerializeField] int backDistance = -5;
    [SerializeField] int maxSameTerrainRepeat = 3;

    Dictionary<int, TerrainBlok> map = new Dictionary<int, TerrainBlok>(50);
    TMP_Text gameOverText;
    private void Start()
    {
        // setup gameover panel
        gameOverPanel.SetActive(false);
        gameOverText = gameOverPanel.GetComponentInChildren<TMP_Text>();

        //belakang
        for (int z = backDistance; z <= 0; z++)
        {
            CreateTerrain(grass, z);
        }

        //depan
        for(int z = 1; z <= frontDistance; z++) 
        {
            //random value grass dan road
            var prefab = GetNextRandomTerrainPrefab(z);

            // Instantiate blocknya
            CreateTerrain(prefab, z);
        }

        player.SetUp(backDistance, extent);
    }
    
    private int playerLastMaxTravel;
    private void Update()
    {
        // cek player masih hidup gak??
        if(player.IsDie && gameOverPanel.activeInHierarchy == false)
            StartCoroutine(ShowGameOverPanel()); 
    
        // infinite terrain system
        if (player.MaxTravel==playerLastMaxTravel)
        return;
        
        playerLastMaxTravel = player.MaxTravel;

        // bikin ke depan
        var randomTbPrefab = GetNextRandomTerrainPrefab(player.MaxTravel+frontDistance);
        CreateTerrain(randomTbPrefab, player.MaxTravel+frontDistance);

        // hapus yang belakang
        var lastTB = map[player.MaxTravel-1 + backDistance];

        // TerrainBlok LastTB;
        // int lastPos = player.MaxTravel;
        // foreach(var (pos, value) in map)
        // {
        //     if(pos<lastPos)
        //     {
        //         lastPos = pos;
        //         lastTB = tb
        //     }
        // }

        // hapus dari daftar, klo gak remove nnti jd null
        map.Remove(player.MaxTravel-1 + backDistance);
        Destroy(lastTB.gameObject);

        player.SetUp(player.MaxTravel + backDistance, extent);
    }

    IEnumerator ShowGameOverPanel()
    {
        yield return new WaitForSeconds(3);

        Debug.Log("GameOver");
        gameOverText.text = "YOUR SCORE : " + player.MaxTravel;
        gameOverPanel.SetActive(true);
    }
    
    private void CreateTerrain(GameObject prefab, int zPos)
    {
        var go = Instantiate(prefab, new Vector3(0, 0, zPos), Quaternion.identity);
        var tb = go.GetComponent<TerrainBlok>();
        tb.Build(extent);

        map.Add(zPos, tb);
        Debug.Log(map[zPos] is Road);
    }

    private GameObject GetNextRandomTerrainPrefab(int nextPos)
    {
        bool isUniform = true;
        var tbRef = map[nextPos - 1];
        for (int distance = 2; distance <= maxSameTerrainRepeat; distance++)
        {
            if (map[nextPos - distance].GetType() != tbRef.GetType())
            {
                isUniform = false;
                break;
            }
        }

        if (isUniform)
        {
            if (tbRef is Grass)
                return road;
            else
                return grass;
        }

        //random generate block dengan chance 50%
        return Random.value > 0.5f ? road : grass;
    }
}
