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
        toPressTextMesh.text = gen.KeyCodeToString(keyToPress);
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
    }

    private IEnumerator BeforeMoveScene(string moveTo)
    {
        yield return new WaitForSeconds(0.5f);

        SceneManager.LoadScene(moveTo);
    }
}
