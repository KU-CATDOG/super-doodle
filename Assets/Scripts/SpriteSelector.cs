using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteSelector : MonoBehaviour
{
    [SerializeField]
    [Header("페이즈 순서대로 정렬할것")]
    private GameObject[] sprites;

    private int _spriteIndex = 0;
    public int SpriteIndex
    {
        get { return _spriteIndex; }
    }

    private void Awake()
    {
        if (sprites.Length > 0)
        {
            foreach(var spriteGameObject in sprites)
            {
                spriteGameObject.SetActive(false);
            }
            sprites[_spriteIndex].SetActive(true);
        }
    }

    public void SetSprite(int newSpriteIndex)
    {
        if (newSpriteIndex >= sprites.Length || newSpriteIndex < 0)
        {
            Debug.LogError($"{gameObject.name}'s sprite array length is only {sprites.Length}, (your input is {newSpriteIndex})");
            return;
        }

        if (newSpriteIndex == _spriteIndex)
        {
            return;
        }

        sprites[_spriteIndex].SetActive(false);
        sprites[newSpriteIndex].SetActive(true);
    }

    public void HideSprite()
    {
        _spriteIndex = -1;

        foreach (var spriteGameObject in sprites)
        {
            spriteGameObject.SetActive(false);
        }
    }
}
