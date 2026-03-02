using UnityEngine;

// https://www.youtube.com/watch?v=rDZztBWGMIs&t=2s

[System.Serializable]
public class SaveData
{
    // Player related data
    public Vector3 playerPosistion;
    public float playerSpeed;
    public float playerIFrameSeconds;
    public float playerMaxHealth;
    public float playerHealth;
    public int playerCoinCount;
    public int playerKeyCount;
    public int playerWeaponId;

    // In future we would also need to save things like entire generated level information, enemies, player stats, etc...
}
