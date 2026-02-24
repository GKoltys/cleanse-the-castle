using TMPro;
using Unity.VisualScripting;
using UnityEngine;

// https://www.youtube.com/watch?v=iD1_JczQcFY
public class DamagePopUp : MonoBehaviour
{
    private static DamagePopUp prefab;
    private TextMeshPro textMesh;

    private float disappearTimer = 1f;
    private Color textColor;

    private void Awake()
    {
        textMesh = GetComponent<TextMeshPro>();
    }

    public static DamagePopUp Create(Vector3 position, float damageAmount)
    {
        if (prefab == null)
        {
            prefab = Resources.Load<DamagePopUp>("UI/DamagePopUp");
        }

        Transform damagePopUpTransform = Instantiate(prefab.GetComponent<Transform>(), position, Quaternion.identity);
        DamagePopUp damagePopUp = damagePopUpTransform.GetComponent<DamagePopUp>();
        damagePopUp.Setup(damageAmount);

        return damagePopUp;
    }

    public void Setup(float damageAmount)
    {
        textMesh.SetText(damageAmount.ToString());
        textColor = textMesh.color;
    }

    private void Update()
    {
        float moveYSpeed = 1f;
        transform.position += new Vector3(0, moveYSpeed * Time.deltaTime);

        disappearTimer -= Time.deltaTime;
        if (disappearTimer < 0f)
        {
            float disappearSpeed = 3f;
            textColor.a -= disappearSpeed * Time.deltaTime;
            textMesh.color = textColor;
            if (textColor.a < 0f)
            {
                Destroy(gameObject);
            }
        }
    }
}