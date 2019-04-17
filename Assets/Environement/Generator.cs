using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public enum RoomType
{
    Start,
    End,
    Item,
    Nothing
}

public class Room
{
    public SortedDictionary<string, Room> neighbor;
    public Vector2Int position;
    public RoomType type;
    public string roomId;

    public Room(int x, int y, RoomType type = RoomType.Nothing)
    {
        position = new Vector2Int(x, y);
        this.type = type;
        roomId = (new System.Random().Next(0, Directory.GetFiles("Assets/Resources/", "*.prefab", SearchOption.TopDirectoryOnly).Length)).ToString("D2");
        neighbor = new SortedDictionary<string, Room>();
    }

    public string PrefabName()
    {
        string str = Convert.ToInt32(type).ToString() + "/";
        foreach(string s in neighbor.Keys)
        {
            str += s;
        }
        int tmp = Directory.GetFiles("Assets/Resources/" + str, "*.prefab", SearchOption.TopDirectoryOnly).Length;
        roomId = (new System.Random().Next(0, tmp)).ToString("D2");
        str += "/Room_" + roomId;
        return str;
    }

}

public class Generator : MonoBehaviour
{
    [SerializeField]
    private int NumberOfRoom;
    [SerializeField]
    private int RoomSize;
    List<Room> createdRoom;

    void Start()
    {
        DataStorage.Floor ++;
        Gen();
        PrintDungeon();
    }

    void Gen()
    {
        createdRoom = new List<Room>();
        Room currentRoom = new Room(0, 0, RoomType.Start); // Mettre RoomType.Start quand les salles seront créer.
        createdRoom.Add(currentRoom);
        var ran = new System.Random();
        while(createdRoom.Count < NumberOfRoom)
        {
            string str = Convert.ToInt32(Convert.ToString(ran.Next(0, 16), 2)).ToString("D4");
            int n = 3;
            foreach(char c in str)
            {
                if(c == '1')
                {
                    Room tmp;
                    string direction;
                    switch(n)
                    {
                        case 3:
                            tmp = new Room(currentRoom.position.x, currentRoom.position.y + 1);
                            direction = "N";
                            break;
                        case 2:
                            tmp = new Room(currentRoom.position.x + 1, currentRoom.position.y);
                            direction = "E";
                            break;
                        case 1:
                            tmp = new Room(currentRoom.position.x, currentRoom.position.y - 1);
                            direction = "S";
                            break;
                        case 0:
                            tmp = new Room(currentRoom.position.x - 1, currentRoom.position.y);
                            direction = "W";
                            break;
                        default:
                            tmp = new Room(0, 0);
                            direction = "";
                            break;
                    }
                    if(!CreatedRoomContain(tmp))
                    {
                        switch(direction)
                        {
                            case "N":
                                tmp.neighbor.Add("S", currentRoom);
                                break;
                            case "E":
                                tmp.neighbor.Add("W", currentRoom);
                                break;
                            case "S":
                                tmp.neighbor.Add("N", currentRoom);
                                break;
                            case "W":
                                tmp.neighbor.Add("E", currentRoom);
                                break;
                        }
                        if(createdRoom.Count + 1 == NumberOfRoom)
                        {
                            tmp.type = RoomType.End;
                        }
                        createdRoom.Add(tmp);
                        currentRoom.neighbor.Add(direction, tmp);
                    }
                }
                n--;
            }
            int rng = new System.Random().Next(0, currentRoom.neighbor.Count);
            foreach(Room room in currentRoom.neighbor.Values)
            {
                if(rng == 0)
                {
                    currentRoom = room;
                    break;
                }
                else
                {
                    rng--;
                }
            }
        }
    }

    void PrintDungeon()
    {
        List<GameObject> lgo = new List<GameObject>();
        foreach(Room room in createdRoom)
        {
            string str = room.PrefabName();
            GameObject tmp = (GameObject)Instantiate(Resources.Load(str));
            tmp.transform.position = new Vector3(room.position.x * RoomSize, room.position.y * RoomSize);
            lgo.Add(tmp);
        }
    }

    void DebugDungeon()
    {
        string str = "";
        foreach(Room room in createdRoom)
        {
            str += "[" + room.position.x + ", " + room.position.y + "]; ";
            foreach(Room r in room.neighbor.Values)
            {
                str += "(" + r.position.x + ", " + r.position.y + "); ";
            }
            str += "| ";
        }
        print(str);
    }

    bool CreatedRoomContain(Room room)
    {
        foreach(Room r in createdRoom)
        {
            if(room.position.x == r.position.x && room.position.y == r.position.y)
                return true;
        }
        return false;
    }
}
