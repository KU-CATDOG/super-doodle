using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class SpriteController : MonoBehaviour
{
    [SerializeField]
    [Header("페이즈 순서대로 정렬할것")]
    private GameObject[] sprites;

    private int _spriteIndex = 0;
    public int SpriteIndex
    {
        get { return _spriteIndex; }
    }

    private Animator animator;

    [Header("애니메이터의 파라미터를 쓰고, 그 타입을 넣을것")]
    public List<string> paramList = new List<string>();
    [SerializeField]
    public List<AnimatorParameterTypes> typesList = new List<AnimatorParameterTypes>();
    public enum AnimatorParameterTypes
    {
        Trigger,
        Bool,
        Float,
    }

    private void Awake()
    {
        if (sprites.Length > 0)
        {
            foreach(var spriteGameObject in sprites)
            {
                spriteGameObject.GetComponent<SpriteRenderer>().enabled = false;
            }
            sprites[_spriteIndex].GetComponent<SpriteRenderer>().enabled = true;
        }
        animator = GetComponent<Animator>();
    }

    public void SetAnimatiorParameter(string paramName, object value = null)
    {
        int paramIdx = paramList.FindIndex(e => e == paramName);
        if (paramIdx >= 0)
        {
            switch(typesList[paramIdx])
            {
                case AnimatorParameterTypes.Trigger:
                    animator.SetTrigger(paramName);
                    break;
                case AnimatorParameterTypes.Bool:
                    try
                    {
                        animator.SetBool(paramName, (bool)value);
                    }
                    catch (Exception e)
                    {
                        Debug.LogError(e);
                    }
                    break;
                case AnimatorParameterTypes.Float:
                    try
                    {
                        animator.SetFloat(paramName, (float)value);
                    }
                    catch (Exception e)
                    {
                        Debug.LogError(e);
                    }
                    break;
                default:
                    Debug.LogError($"Wrong paramType with {paramName} in {name}, check SpriteController's parameter settings");
                    break;
            }
        }
        else
        {
            Debug.LogError($"No paramName with {paramName} in {name}, check SpriteController's parameter settings");
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

        sprites[_spriteIndex].GetComponent<SpriteRenderer>().enabled = false;
        sprites[newSpriteIndex].GetComponent<SpriteRenderer>().enabled = true;
    }

    public void HideSprite()
    {
        _spriteIndex = -1;

        foreach (var spriteGameObject in sprites)
        {
            spriteGameObject.GetComponent<SpriteRenderer>().enabled = false;
        }
    }
}
