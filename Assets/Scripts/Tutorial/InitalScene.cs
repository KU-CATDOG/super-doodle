using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tool;
using UnityEngine.SceneManagement;
using TMPro;

public class InitalScene : MonoBehaviour
{
    [Header("Values")]
    [SerializeField] private float groundRotateSpeed = 1f;

    [Header("GameObjects")]
    [SerializeField] private Transform groundTransform;
    [SerializeField] private TextMeshPro toPressTextMesh;
    [SerializeField] private SpriteRenderer darkerRenderer;

    private KeyCode keyToPress;

    private void Awake()
    {
        MessageSystem.Instance.Subscribe<SingleKeyPressedEvent>(OnSingleKeyEvent);
        PressKeyDetector.Init();
    }

    private void Start()
    {
        var gen = new RandomKeyGenerator();
        keyToPress = gen.GetKeyCode();
        var keyText = gen.KeyCodeToString(keyToPress);
        for (int i = 1; i < keyText.Length; ++i)
        {
            if (keyText[i] >= 'A')
            {
                keyText.Insert(i, "\n");
                break;
            }
        }
        toPressTextMesh.text = keyText;
        StartCoroutine(DarkerControl(true));
    }

    private void Update()
    {
        groundTransform.Rotate(new Vector3(0, 0, groundRotateSpeed * Time.deltaTime));
    }

    private void OnDestroy()
    {
        MessageSystem.Instance.Unsubscribe<SingleKeyPressedEvent>(OnSingleKeyEvent);
    }

    private void OnSingleKeyEvent(IEvent e)
    {
        if (!(e is SingleKeyPressedEvent se)) return;

        if (se.PressedKey == keyToPress)
        {
            SoundManager.Inst.PlayEffectSound(SoundManager.Sounds.ButtonPress);
            // 몇가지의 애니메이션 후
            StartCoroutine(BeforeMoveScene("MenuScene"));
        }
        else if (se.PressedKey == KeyCode.Escape)
        {
            StartCoroutine(DarkerControl(false));
        }
    }

    private IEnumerator BeforeMoveScene(string moveTo)
    {
        yield return new WaitForSeconds(0.5f);

        SceneManager.LoadScene(moveTo);
    }

    private IEnumerator DarkerControl(bool turnOn)
    {
        float timer = 0;
        while (timer < 1f)
        {
            timer += Time.deltaTime;
            if (turnOn)
            {
                darkerRenderer.color = Color.Lerp(Color.black, Color.clear, timer);
            }
            else
            {
                darkerRenderer.color = Color.Lerp(Color.clear, Color.black, timer);
            }
            yield return null;
        }
        if (!turnOn)
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
            Application.Quit();
        }
    }
}
