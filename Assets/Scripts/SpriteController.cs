using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class SpriteController : MonoBehaviour
{
    [SerializeField]
    [Header("페이즈 순서대로 정렬할것")]
    private SpriteRenderer[] sprites;

    [SerializeField]
    [Header("처치시 찢어질 조각들을 넣으면 됨, 0에는 부모넣을것, 1은 중력영향 안받는애로")]
    private SpriteRenderer[] cutSprites;

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
                spriteGameObject.enabled = false;
            }
            sprites[_spriteIndex].enabled = true;
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

        sprites[_spriteIndex].enabled = false;
        sprites[newSpriteIndex].enabled = true;
    }

    public void HideSprite()
    {
        _spriteIndex = -1;

        foreach (var spriteGameObject in sprites)
        {
            spriteGameObject.enabled = false;
        }
    }

    public void KillCutSprite()
    {
        SetAnimatiorParameter("OnDead");

        var throwDirection = new Vector2(UnityEngine.Random.Range(-1, 1), UnityEngine.Random.Range(0.3f, 1));

        StartCoroutine(ThrowSprite(cutSprites[2].transform, throwDirection, 25, 1));
    }

    IEnumerator ThrowSprite(Transform tr, Vector2 dir, float speed, float throwTime)
    {
        float timer = 0;
        Vector2 velocity = dir.normalized * speed * Time.deltaTime;
        float gravity = 0.1f;

        while (timer < throwTime)
        {
            timer += Time.deltaTime;

            velocity.y -= gravity * Time.deltaTime;
            tr.position += new Vector3(velocity.x, velocity.y, (-5 - tr.position.z));

            yield return null;
        }
    }
}
