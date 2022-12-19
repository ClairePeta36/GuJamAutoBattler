﻿using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Entity Database", menuName = "EntityDatabase")]
public class EntityDatabase : ScriptableObject
{
    [System.Serializable]
    public struct EntityData
    {
        public BaseEntity prefab;
        public string name;
        public Sprite icon;

        public int cost;
    }

    public List<EntityData> allEntities;
}