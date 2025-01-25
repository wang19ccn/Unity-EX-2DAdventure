using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour, IInteractable
{
    private SpriteRenderer spriteRenderer;
    public Sprite openSpirte;
    public Sprite closeSprite;
    public bool isDone;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        spriteRenderer.sprite = isDone ? openSpirte : closeSprite;
    }

    public void TriggerAction()
    {
        Debug.Log("Open Chest!");
        if (!isDone)
        {
            OpenChest();
        }
    }

    private void OpenChest()
    {
        spriteRenderer.sprite = openSpirte;
        isDone = true;
        this.gameObject.tag = "Untagged"; // 宝箱交互打开后，隐藏按钮提示
    }
}
