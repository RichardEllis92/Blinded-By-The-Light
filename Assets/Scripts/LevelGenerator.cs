using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public GameObject layoutRoom;
    public Color startRoomColor, endRoomColor, shopRoomColor, healRoomColor;

    public int amountOfRooms;
    public bool isShop;
    public bool isHealRoom;
    public int shopMinDistance, shopMaxDistance;
    public int healRoomMinDistance, healRoomMaxDistance;

    public Transform generatorPoint;

    public enum Direction { Up, Right, Down, Left};
    public Direction selectedDirection;

    public float xOffset = 18f, yOffset = 10f;

    public LayerMask whatIsRoom;

    private GameObject _endRoom, _shopRoom, _healRoom;

    private readonly List<GameObject> _layoutRoomObjects = new List<GameObject>();

    public RoomPrefabs rooms;

    private readonly List<GameObject> _generatedOutlines = new List<GameObject>();

    public RoomCenter centerStart, centerEnd, centerShop, centerHealRoom;
    public RoomCenter[] potentialCenters;

    public bool firstSpawn;
    void Start()
    {
        Instantiate(layoutRoom, generatorPoint.position, generatorPoint.rotation).GetComponent<SpriteRenderer>().color = startRoomColor;

        selectedDirection = (Direction)Random.Range(0, 4);
        MoveGenerationPoint();
        
        for(int i = 0; i < amountOfRooms; i++)
        {
            GameObject newRoom =  Instantiate(layoutRoom, generatorPoint.position, generatorPoint.rotation);

            _layoutRoomObjects.Add(newRoom);

            if(i + 1 == amountOfRooms)
            {
                newRoom.GetComponent<SpriteRenderer>().color = endRoomColor;
                _layoutRoomObjects.RemoveAt(_layoutRoomObjects.Count - 1);

                _endRoom = newRoom;
            }

            selectedDirection = (Direction)Random.Range(0, 4);
            MoveGenerationPoint();

            while(Physics2D.OverlapCircle(generatorPoint.position, 0.2f, whatIsRoom))
            {
                MoveGenerationPoint();
            }

        }

        if (isShop && _layoutRoomObjects.Count > 0)
        {
            int shopSelector = Random.Range(shopMinDistance, Mathf.Min(shopMaxDistance + 1, _layoutRoomObjects.Count));
            _shopRoom = _layoutRoomObjects[shopSelector];
            _layoutRoomObjects.RemoveAt(shopSelector);
            _shopRoom.GetComponent<SpriteRenderer>().color = shopRoomColor;
        }

        if (isHealRoom && _layoutRoomObjects.Count > 0)
        {
            int healRoomSelector = Random.Range(healRoomMinDistance, Mathf.Min(healRoomMaxDistance + 1, _layoutRoomObjects.Count));
            _healRoom = _layoutRoomObjects[healRoomSelector];
            _layoutRoomObjects.RemoveAt(healRoomSelector);
            _healRoom.GetComponent<SpriteRenderer>().color = healRoomColor;
        }

        //create room outlines
        CreateRoomOutline(Vector3.zero);
        foreach(GameObject room in _layoutRoomObjects)
        {
            CreateRoomOutline(room.transform.position);
        }
        CreateRoomOutline(_endRoom.transform.position);

        if (isShop)
        {
            CreateRoomOutline(_shopRoom.transform.position);
        }
        if (isHealRoom)
        {
            CreateRoomOutline(_healRoom.transform.position);
        }

        foreach (GameObject outline in _generatedOutlines)
        {
            bool generateCenter = true;
            
            if(outline.transform.position == Vector3.zero)
            {
                Instantiate(centerStart, outline.transform.position, transform.rotation).theRoom = outline.GetComponent<Room>();

                generateCenter = false;
            }

            if(outline.transform.position == _endRoom.transform.position)
            {
                Instantiate(centerEnd, outline.transform.position, transform.rotation).theRoom = outline.GetComponent<Room>();

                generateCenter = false;
            }

            if (isShop)
            {
                if (outline.transform.position == _shopRoom.transform.position)
                {
                    Instantiate(centerShop, outline.transform.position, transform.rotation).theRoom = outline.GetComponent<Room>();

                    generateCenter = false;
                }
            }

            if (isHealRoom)
            {
                if (outline.transform.position == _healRoom.transform.position)
                {
                    Instantiate(centerHealRoom, outline.transform.position, transform.rotation).theRoom = outline.GetComponent<Room>();

                    generateCenter = false;
                }
            }

            if (generateCenter)
            {
                int centerSelect = Random.Range(0, potentialCenters.Length);
                Instantiate(potentialCenters[centerSelect], outline.transform.position, transform.rotation).theRoom = outline.GetComponent<Room>();
            }
        }
    }
    
    private void MoveGenerationPoint()
    {
        switch(selectedDirection)
        {
            case Direction.Up:
                generatorPoint.position += new Vector3(0f, yOffset, 0f);
                break;
            case Direction.Down:
                generatorPoint.position += new Vector3(0f, -yOffset, 0f);
                break;
            case Direction.Right:
                generatorPoint.position += new Vector3(xOffset, 0f, 0f);
                break;
            case Direction.Left:
                generatorPoint.position += new Vector3(-xOffset, 0f, 0f);
                break;
        }
    }

    private void CreateRoomOutline(Vector3 roomPosition)
    {
        bool roomAbove = Physics2D.OverlapCircle(roomPosition + new Vector3(0f, yOffset, 0f), 0.2f, whatIsRoom);
        bool roomBelow = Physics2D.OverlapCircle(roomPosition + new Vector3(0f, -yOffset, 0f), 0.2f, whatIsRoom);
        bool roomLeft = Physics2D.OverlapCircle(roomPosition + new Vector3(-xOffset, 0f, 0f), 0.2f, whatIsRoom);
        bool roomRight = Physics2D.OverlapCircle(roomPosition + new Vector3(xOffset, 0f, 0f), 0.2f, whatIsRoom);

        int directionCount = 0;

        if(roomAbove)
        {
            directionCount++;
        }
        if (roomBelow)
        {
            directionCount++;
        }
        if (roomLeft)
        {
            directionCount++;
        }
        if (roomRight)
        {
            directionCount++;
        }

        switch(directionCount)
        {
            case 0:
                Debug.LogError("Found no room exists!");
                break;
            case 1:

                if (roomAbove)
                {
                    _generatedOutlines.Add(Instantiate(rooms.singleUp, roomPosition, transform.rotation));
                }
                if (roomBelow)
                {
                    _generatedOutlines.Add(Instantiate(rooms.singleDown, roomPosition, transform.rotation));
                }
                if (roomLeft)
                {
                    _generatedOutlines.Add(Instantiate(rooms.singleLeft, roomPosition, transform.rotation));
                }
                if (roomRight)
                {
                    _generatedOutlines.Add(Instantiate(rooms.singleRight, roomPosition, transform.rotation));
                }

                break;
            case 2:
                if(roomAbove && roomBelow)
                {
                    _generatedOutlines.Add(Instantiate(rooms.doubleUpDown, roomPosition, transform.rotation));
                }
                if (roomLeft && roomRight)
                {
                    _generatedOutlines.Add(Instantiate(rooms.doubleLeftRight, roomPosition, transform.rotation));
                }
                if (roomAbove && roomRight)
                {
                    _generatedOutlines.Add(Instantiate(rooms.doubleUpRight, roomPosition, transform.rotation));
                }
                if (roomRight && roomBelow)
                {
                    _generatedOutlines.Add(Instantiate(rooms.doubleRightDown, roomPosition, transform.rotation));
                }
                if (roomBelow && roomLeft)
                {
                    _generatedOutlines.Add(Instantiate(rooms.doubleDownLeft, roomPosition, transform.rotation));
                }
                if (roomLeft && roomAbove)
                {
                    _generatedOutlines.Add(Instantiate(rooms.doubleLeftUp, roomPosition, transform.rotation));
                }
                break;
            case 3:
                if (roomAbove && roomRight && roomBelow)
                {
                    _generatedOutlines.Add(Instantiate(rooms.tripleUpRightDown, roomPosition, transform.rotation));
                }
                if (roomRight && roomBelow && roomLeft)
                {
                    _generatedOutlines.Add(Instantiate(rooms.tripleRightDownLeft, roomPosition, transform.rotation));
                }
                if (roomBelow && roomLeft && roomAbove)
                {
                    _generatedOutlines.Add(Instantiate(rooms.tripleDownLeftUp, roomPosition, transform.rotation));
                }
                if (roomLeft && roomAbove && roomRight)
                {
                    _generatedOutlines.Add(Instantiate(rooms.tripleLeftUpRight, roomPosition, transform.rotation));
                }
                break;
            case 4:
                if (roomBelow && roomLeft && roomAbove && roomRight)
                {
                    _generatedOutlines.Add(Instantiate(rooms.fourway, roomPosition, transform.rotation));
                }
                break;

        }
    }
}

[System.Serializable]
public class RoomPrefabs
{
    public GameObject singleUp, singleDown, singleRight, singleLeft, 
        doubleUpDown, doubleLeftRight, doubleUpRight, doubleRightDown, doubleDownLeft, doubleLeftUp,
        tripleUpRightDown, tripleRightDownLeft, tripleDownLeftUp, tripleLeftUpRight,
        fourway;
}