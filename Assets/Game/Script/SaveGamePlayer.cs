using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class SaveGamePlayer
{
    public SerializableVector3 playerPosition;
    public float score;
    public float head;
    public Dictionary<int, bool> buyGuns = new Dictionary<int, bool>();
    public Dictionary<int, Gun> gundictionary = new Dictionary<int, Gun>();
    public List<string> destroyedObjects = new List<string>();
    public Dictionary<string, int> gateOnMonster = new Dictionary<string, int>();
    /*public Dictionary<string, SerializableVector3> monster = new Dictionary<string, SerializableVector3>();*/
    public SaveGamePlayer(SerializableVector3 position, float scores, float heath, Dictionary<int, bool> BuyGuns, Dictionary<int, Gun> gunDictionary, List<string> destroyed, Dictionary<string, int> GateOnMonster/*, Dictionary<string, SerializableVector3> Monster*/)
    {
        playerPosition = position;
        score = scores;
        head = heath;
        buyGuns = BuyGuns;
        gundictionary = gunDictionary;
        destroyedObjects = destroyed;
        gateOnMonster = GateOnMonster;
        /*monster = Monster;*/
    }
}