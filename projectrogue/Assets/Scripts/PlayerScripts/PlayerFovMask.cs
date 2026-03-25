using UnityEngine;

public class PlayerFovMask : MonoBehaviour
{
    [SerializeField] private PlayerVisibilityMesh visibility;

    private void LateUpdate()
    {
        if (visibility == null) return;

        transform.position = visibility.transform.position;

        float diameter = visibility.GetViewRadius * 2f * 0.5f;
        transform.localScale = new Vector3(diameter, diameter, 1f);
    }
}
