using System;
using UnityEngine;
using UnityEngine.UI;

public class BuffIconUI : MonoBehaviour
{
    [SerializeField] private Image icon;
    private string buffName;
    private string statDescription;
    private float stat;

    public void SetName(string name) { this.buffName = name; }
    public void SetIcon(Sprite icon) { this.icon.sprite = icon; }
    public void SetStatDescription(string description) { this.statDescription = description; }
    public void SetStat(float stat) { this.stat = stat; }

    public string GetName => buffName;
}
