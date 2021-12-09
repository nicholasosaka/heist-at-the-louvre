using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class Valuables : MonoBehaviour
{

    [SerializeField] private Transform player;

    [SerializeField] private Slider treasureTrackerSlider;

    [SerializeField] private Tilemap valuablesTileMap;

    [SerializeField] private float proximityThreshold;
    [SerializeField] private string valuableTileName;

    [SerializeField] private TileBase emptyStandTileBase;

    private List<Vector3> tilePositions;
    private int score = 0;


    // Start is called before the first frame update
    void Start()
    {
        tilePositions = new List<Vector3>();

        BoundsInt.PositionEnumerator positions = valuablesTileMap.cellBounds.allPositionsWithin;

        foreach (var position in positions) {
            Vector3Int positionVector = new Vector3Int(position.x, position.y, position.z);
            Vector3 worldPositionVector = valuablesTileMap.CellToWorld(positionVector);

            if (valuablesTileMap.HasTile(positionVector)) {
                tilePositions.Add(worldPositionVector);
            }
        }

        treasureTrackerSlider.maxValue = tilePositions.ToArray().Length;
        Debug.Log(treasureTrackerSlider.maxValue);

    }

    // Update is called once per frame
    void Update()
    {


        if(Input.GetKeyDown("space")) {
            Vector3 closest = new Vector3(99, 99, 99);
            float closestMagnitude = 999f;

            foreach (var tilePosition in tilePositions) {
                Vector3 distanceBetween = player.position - tilePosition;
                if (distanceBetween.magnitude < closestMagnitude) {
                    closest = tilePosition;
                    closestMagnitude = distanceBetween.magnitude;
                }
            }

            if (closestMagnitude < proximityThreshold) { //we grab this valuable
                //make sure it hasn't already been grabbed
                Vector3Int cellPosition = valuablesTileMap.WorldToCell(closest);
                TileBase tileBase = valuablesTileMap.GetTile(cellPosition);
                if (tileBase.name == valuableTileName) {
                    valuablesTileMap.SetTile(cellPosition, emptyStandTileBase);
                    score += 1;
                    treasureTrackerSlider.value = score;
                    Debug.Log(treasureTrackerSlider.value);
                }
            }
        }
    }
}
