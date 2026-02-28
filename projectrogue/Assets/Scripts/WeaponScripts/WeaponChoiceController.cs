using UnityEngine;

public class WeaponChoiceController : MonoBehaviour
{
    private PickUpWeapon chosenPickup;
    private PlayerBase player;
    [SerializeField] private WeaponDatabase weaponDatabase;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBase>();
        chosenPickup = GetWeaponChoiceOnLoad();
        
        if (chosenPickup != null ) chosenPickup.gameObject.SetActive(false);
    }

    private PickUpWeapon GetWeaponChoiceOnLoad()
    {
        foreach (PickUpWeapon pickedUp in GetComponentsInChildren<PickUpWeapon>())
        {
            if (pickedUp.weapon == weaponDatabase.GetWeaponById(player.GetWeaponId)) return pickedUp;
        }

        return null;
    }

    public void UpdateWeapon(PickUpWeapon newPickup)
    {
        if (chosenPickup != null) chosenPickup.gameObject.SetActive(true);
        chosenPickup = newPickup;
        chosenPickup.gameObject.SetActive(false);

        player.SetWeapon(newPickup.weapon);
        SaveController.Instance.SaveGame(); // Saving all player data on weapon change since this is just as costly as only changing the weaponId
    }
}
