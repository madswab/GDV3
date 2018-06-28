using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DungeonOverview : MonoBehaviour {

    public NavMeshSurface surface;

    public List<GameObject> shapes;
    public List<GameObject> roomItems;
    public List<GameObject> enemies;

    private DungeonGenerator dungeonGenerator;
    private bool[,] visitedCells;
    private Dictionary<char, GameObject> dungeonGameObjects = new Dictionary<char, GameObject>();

    private void Start () {
        dungeonGenerator = GetComponent<DungeonGenerator>();

        visitedCells = new bool[dungeonGenerator.mapX, dungeonGenerator.mapY]; //dungeonGenerator.LoopDungeon();

        LoadDungeonCharacterMappings();
        dungeonGenerator.InitializeDungeon();
        visitedCells = dungeonGenerator.LoopDungeon();
        dungeonGenerator.DisplayDungeon();

        //
        InstantiateDungeonPieces();
        BuildNavMesh();
    }

    private void LoadDungeonCharacterMappings() {
		foreach (GameObject dungeonShape in shapes) {
			char gameObjectCharacter = dungeonShape.GetComponent<DungeonMapCharacter>().mapCharacter;
			dungeonGameObjects.Add(gameObjectCharacter, dungeonShape);
		}
	}

    private void InstantiateDungeonPieces() {

		for (int y = 1; y < dungeonGenerator.mapY - 1; y++) {
			for (int x = 1; x < dungeonGenerator.mapX - 1; x++) {
				char ch = dungeonGenerator.dungeon [x, y];

				if (dungeonGameObjects.ContainsKey(ch) == false || visitedCells [x, y] == false) {
					continue;
                }
                else if (roomItems.Count > 0 && ch == '@' || ch == '╧' || ch == '╤' || ch == '╟' || ch == '╢'){
                    GameObject dungeonGameObject = dungeonGameObjects[ch];
                    GameObject piece = Instantiate(dungeonGameObject, new Vector3(x * 12, 0, y * -12), dungeonGameObject.transform.rotation);
                    GameObject decoration = null;
                    //GameObject randomDec = roomItems[Random.Range(0, roomItems.Count)];

                    if (ch == '@') {
                        decoration = Instantiate(roomItems[Random.Range(0, roomItems.Count)], new Vector3(x * 12, 0, y * -12), Quaternion.identity);
                    }
                    else{
                        decoration = Instantiate(roomItems[Random.Range(2, roomItems.Count)], new Vector3(x * 12, 0, y * -12), Quaternion.identity);
                    }

                    if (decoration.name.Contains("EnemyRoomItem") && enemies.Count > 0){ // kan beter
                        GameObject enemy = Instantiate(enemies[Random.Range(0, enemies.Count)], decoration.transform.position + new Vector3(0, 1, 0), Quaternion.identity);
                        enemy.transform.parent = transform;
                    }

                    Vector3 randomRotItem = decoration.transform.eulerAngles;
                    randomRotItem.y = Random.Range(0, 360);
                    decoration.transform.eulerAngles = randomRotItem;

                    piece.transform.parent = transform;
                    decoration.transform.parent = transform;
                }
                else {
					GameObject dungeonGameObject = dungeonGameObjects[ch];
				    GameObject piece = Instantiate (dungeonGameObject, new Vector3 (x * 12, 0, y * -12), dungeonGameObject.transform.rotation);
                    piece.transform.parent = transform;  
                }
			}
		}
	}

    private void BuildNavMesh(){

        surface.BuildNavMesh();

    }
}
