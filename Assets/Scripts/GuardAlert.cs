using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GuardAlert : MonoBehaviour
{

    [SerializeField] private Transform guard;
    [SerializeField] private Tilemap valuablesTileMap;
    [SerializeField] private string emptyStandTileBaseName;
    [SerializeField] private float proximityThreshold;
    [SerializeField] private GameObject alert;
    [SerializeField] private Transform player;


    private List<Vector3> tilePositions;
    private bool alerted = false;
    private float timeSinceAlerted = 0;
    private SpriteRenderer alertSprite;

    Alert alertHandler;
    private List<Vector3Int> seenStolen;

    private int difficulty;

    // Start is called before the first frame update
    void Start()
    {

        difficulty = PlayerPrefs.GetInt("difficulty", 0);

        alertHandler = new Alert();

        seenStolen = new List<Vector3Int>();

        alertSprite = alert.GetComponent<SpriteRenderer>();

        tilePositions = new List<Vector3>();

        BoundsInt.PositionEnumerator positions = valuablesTileMap.cellBounds.allPositionsWithin;

        foreach (var position in positions) {
            Vector3Int positionVector = new Vector3Int(position.x, position.y, position.z);
            Vector3 worldPositionVector = valuablesTileMap.CellToWorld(positionVector);

            if (valuablesTileMap.HasTile(positionVector)) {
                tilePositions.Add(worldPositionVector);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 closest = new Vector3(99, 99, 99);
        float closestMagnitude = 999f;

        foreach (var tilePosition in tilePositions) {
            Vector3 distanceBetween = guard.position - tilePosition;
            if (distanceBetween.magnitude < closestMagnitude) {
                closest = tilePosition;
                closestMagnitude = distanceBetween.magnitude;
            }
        }

        if (closestMagnitude < proximityThreshold) { //guard can see a valuable
            Vector3Int cellPosition = valuablesTileMap.WorldToCell(closest);
            TileBase tileBase = valuablesTileMap.GetTile(cellPosition);
            if (tileBase.name == emptyStandTileBaseName && !seenStolen.Contains(cellPosition)) { //if its been stolen
                alerted = true;
                Debug.Log("Guard found a stolen stand");
                timeSinceAlerted = 0;
                alertHandler.Increment();
                seenStolen.Add(cellPosition);
            } else {
                timeSinceAlerted += Time.deltaTime;
                if (timeSinceAlerted > 5f) {
                    alerted = false;
                }
            }
        }

        if(alerted) {
            alertSprite.color = new Color(1,1,1,1);
        } else {
            alertSprite.color = new Color(1,1,1,0);
        }

        float distanceToPlayer = Vector3.Distance(guard.position, player.position);
        Debug.Log(distanceToPlayer);
        if(distanceToPlayer < 2f) {
            alertHandler.MaxAlert();
        }
    }
}
