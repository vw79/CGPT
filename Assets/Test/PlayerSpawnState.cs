using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerSpawnState
{
    public enum SpawnType { AtSpawn }
    public static SpawnType NextSpawn = SpawnType.AtSpawn;
}

