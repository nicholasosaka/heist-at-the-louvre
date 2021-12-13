using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;


public class AStar
{
    public class Node { // node class to hold node data
        public Node(Vector2 pos) {
            position = pos;
        }

        public Vector2 position;
        public bool traversable;
        public float f; //F = G + H
        public float g; //move cost
        public float h; //heuristic
        public Node parent;
        public void calculateF() {  //easy function to calculate F
            f = g + h;
        }
    }

    private static int worldX;
    private static int worldY;
    private List<Tilemap> tilemaps;
    private List<Node> worldNodes;
    private Grid worldGrid;

    public AStar(Grid grid) {
        worldGrid = grid;
        Component[] components = grid.GetComponentsInChildren(typeof(Tilemap));
        
        tilemaps = new List<Tilemap>();
        foreach(Component component in components) {
            tilemaps.Add((Tilemap)component);
        }

        Debug.Log(tilemaps.ToArray().Length + " tilemaps found in grid");

        worldX = 0;
        worldY = 0;

        foreach(Tilemap tilemap in tilemaps) {
            if(worldY < tilemap.size.y) {
                worldY = tilemap.size.y;
            }
            
            if(worldX < tilemap.size.x) {
                worldX = tilemap.size.x;
            }
        }

        Debug.Log("grid size: (" + worldX + ", " + worldY + ")");

        worldNodes = new List<Node>();
        Decompose();
    }

    public void Decompose() {
        int traversableCount = 0;
        float nodeCentering = 0.5f;
        float startX = -(worldX/2);
        float startY = -(worldY/2 + 1);
        
        for(int row = 0; row < worldX; row++){
            for(int col = 0; col < worldY; col++){
                float x = startX + row + nodeCentering;
                float y = startY + col + nodeCentering;

                Vector3 worldPoint = new Vector3(x, y, 1);
                Vector3Int cellPoint = worldGrid.WorldToCell(worldPoint);
                
                Vector2 nodePos = new Vector2(x, y);
                Node node = new Node(nodePos);

                Tilemap nodeTilemap = null;
                foreach(Tilemap tilemap in tilemaps) {
                    if(tilemap.HasTile(cellPoint)) {
                        nodeTilemap = tilemap;
                    }
                }
                
                if(nodeTilemap != null) {
                    node.traversable = nodeTilemap.gameObject.layer == 7;
                    if(node.traversable) {
                        traversableCount++;
                    }

                    worldNodes.Add(node);


                    // //DEBUG TODO REMOVE
                    // if(nodeTilemap.gameObject.layer == 6) { //WALLS
                    //     Vector3 debugstartX = new Vector3(node.position.x-.25f, node.position.y, 1);
                    //     Vector3 debugendX = new Vector3(node.position.x+.25f, node.position.y, 1);
                    //     Debug.DrawLine(debugstartX, debugendX, Color.red, 50f, false);
                    //     Vector3 debugstartY = new Vector3(node.position.x, node.position.y-.25f, 1);
                    //     Vector3 debugendY = new Vector3(node.position.x, node.position.y+.25f, 1);
                    //     Debug.DrawLine(debugstartY, debugendY, Color.red, 50f, false);
                    // } else if(nodeTilemap.gameObject.layer == 7) { //WALKABLE
                    //     Vector3 debugstartX = new Vector3(node.position.x-.25f, node.position.y, 1);
                    //     Vector3 debugendX = new Vector3(node.position.x+.25f, node.position.y, 1);
                    //     Debug.DrawLine(debugstartX, debugendX, Color.green, 50f, false);
                    //     Vector3 debugstartY = new Vector3(node.position.x, node.position.y-.25f, 1);
                    //     Vector3 debugendY = new Vector3(node.position.x, node.position.y+.25f, 1);
                    //     Debug.DrawLine(debugstartY, debugendY, Color.green, 50f, false);
                    // } else { //VALUABLES
                    //     Vector3 debugstartX = new Vector3(node.position.x-.25f, node.position.y, 1);
                    //     Vector3 debugendX = new Vector3(node.position.x+.25f, node.position.y, 1);
                    //     Debug.DrawLine(debugstartX, debugendX, Color.blue, 50f, false);
                    //     Vector3 debugstartY = new Vector3(node.position.x, node.position.y-.25f, 1);
                    //     Vector3 debugendY = new Vector3(node.position.x, node.position.y+.25f, 1);
                    //     Debug.DrawLine(debugstartY, debugendY, Color.blue, 50f, false);
                    // }
                }
            }
        }

        Debug.Log(worldNodes.ToArray().Length + " nodes generated, " + traversableCount + " traversable");


    }


    public List<Node> GetPath(Vector2 startPoint, Vector2 endPoint) {

        List<Node> openList = new List<Node>();
        List<Node> closedList = new List<Node>();

        Node startingNode = GetClosestNodeToPoint(startPoint);

        startingNode.parent = null; //starting node has no parent, redundant line but added for clarity
        openList.Add(startingNode);

        bool isPathFound = false;
        while(openList.ToArray().Length > 0 && !isPathFound) {


            //find best node to explore, call it q.
            Node q = openList[0];
            foreach(Node openNode in openList) {
                if(openNode.f < q.f) {
                    q = openNode;
                }
            }

            openList.Remove(q);

            List<Node> successors = new List<Node>();
            foreach(Node node in GetTraversableNeighbors(q)) {
                if(!closedList.Contains(node)) {
                    node.parent = q;
                    successors.Add(node);
                }
            }

            foreach(Node successor in successors) { //TODO optimize

                if(successor.position == GetClosestNodeToPoint(endPoint).position) {
                    isPathFound = true;
                    List<Node> path = GeneratePath(successor, startingNode);
                    Debug.Log("Found path of length " + path.ToArray().Length);
                    return path;
                }

                successor.g = q.g + Vector3.Distance(successor.position, q.position);
                successor.h = ManhattanDistance(successor, GetClosestNodeToPoint(endPoint));
                successor.calculateF();

                bool badSuccessor = false; //assuming good successor

                //if there's a better successor in the open list, it's a bad successor
                foreach(Node openNode in openList) {
                    if(openNode.position == successor.position && openNode.f < successor.f) {
                            badSuccessor = true;
                    }
                }

                foreach(Node closedNode in closedList) {
                    if(closedNode.position == successor.position && closedNode.f < successor.f) {
                        badSuccessor = true;
                    }
                }

                if(!badSuccessor) {
                    openList.Add(successor);
                }

                closedList.Add(q);

            }

        }

        Debug.Log("Failed to find path");
        return new List<Node>(); //empty path on failure
    }


    private List<Node> GeneratePath(Node node, Node startingNode) {
        List<Node> path = new List<Node>();
        path.Add(node);
        Node head = node;
        bool stopFlag = false;
        while(head.parent.position != startingNode.position && !stopFlag) {
            if(head.traversable) {
                path.Add(head.parent);
                head = head.parent;
            } else {
                stopFlag = true;
            }
        }

        path.Add(startingNode);
        // path.Reverse();

        return path;
    }
    private List<Node> GetTraversableNeighbors(Node node) {

        List<Node> neighbors = new List<Node>();
        List<Node> traversableNeighbors = new List<Node>();

        float x = node.position.x;
        float y = node.position.y;

        neighbors.Add(GetClosestNodeToPoint(new Vector2(x+1, y)));
        neighbors.Add(GetClosestNodeToPoint(new Vector2(x, y+1)));
        neighbors.Add(GetClosestNodeToPoint(new Vector2(x-1, y)));
        neighbors.Add(GetClosestNodeToPoint(new Vector2(x, y-1)));

        foreach(Node neighbor in neighbors) { 
            if(neighbor.traversable) {
                traversableNeighbors.Add(neighbor);
            }
        }

        return traversableNeighbors;
    }


    private float ManhattanDistance(Node a, Node b) {
        return Math.Abs(a.position.x - b.position.x) + Math.Abs(a.position.y - b.position.y);
    }

    private Node GetClosestNodeToPoint(Vector2 point) {
        Node best = worldNodes[0];
        float bestDist = Vector2.Distance(point, best.position);

        foreach(Node node in worldNodes) {
            if(Vector2.Distance(point, node.position) < bestDist) {
                bestDist = Vector2.Distance(point, node.position);
                best = node;
            }
        }
        
        return best;
    }

    public static Node GetClosestNodeToPoint(Vector2 point, List<Node> nodes) {
        Node best = nodes[0];
        float bestDist = Vector2.Distance(point, best.position);

        foreach(Node node in nodes) {
            if(Vector2.Distance(point, node.position) < bestDist) {
                bestDist = Vector2.Distance(point, node.position);
                best = node;
            }
        }
        
        return best;
    }


}
