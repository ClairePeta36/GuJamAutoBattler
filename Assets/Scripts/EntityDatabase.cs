using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Entity Database", menuName = "EntityDatabase")]
public class EntityDatabase : ScriptableObject
{
    [System.Serializable]
    public struct EntityData
    {
        public BaseEntity prefab;
        public string name;
        public Sprite imageBackground;

        public int cost;
        public string tribe;
        public Sprite iconTribe;
        public int health;
        public int attack;
        public int quantity;
        public string ability;
    }

    public List<EntityData> allEntities;
}