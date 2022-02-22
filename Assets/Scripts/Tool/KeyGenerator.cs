using System.Collections.Generic;
using UnityEngine;

namespace Tool
{
    /// <summary>
    /// 사용할 키는 여기서 정의함
    /// </summary>
    public static class KeysUsed
    {
        private static List<KeyCode> _usableKeys;

        /// <summary>
        /// 기본적으로 인게임에서 무작위로 나와야 할때 사용되는 키들
        /// </summary>
        public static List<KeyCode> UsableKeys
        {
            get
            {
                if (_usableKeys == null)
                {
                    _usableKeys = InitUsableKeys();
                }
                return _usableKeys;
            }
        }

        /// <summary>
        /// 인게임중, 특별하게 사용되는 키들 (일시정지, 기믹 등)
        /// </summary>
        public static List<KeyCode> SpecialKeys = new List<KeyCode>(new KeyCode[]
        {
            KeyCode.Space,
            KeyCode.Escape,
        });

        private static List<KeyCode> InitUsableKeys()
        {
            List<KeyCode> keyCodes = new List<KeyCode>();
            //Escape와 Space는 사용 금지 - 기믹용/일시정지용 특수키

            if (GameManager.Inst.cn)
            {
                keyCodes.AddRange(new KeyCode[] {
                // 기타 아래 안들어가는거
                KeyCode.Delete,
                KeyCode.Numlock,
                KeyCode.CapsLock,
                KeyCode.ScrollLock,
                KeyCode.SysReq, // PrintScreen
                //KeyCode.Pause,
            });
            }        

            if (GameManager.Inst.an)
            {
                // 알파벳
                for (var i = KeyCode.A; i <= KeyCode.Z; ++i)
                {
                    keyCodes.Add(i);
                }
                // 상단 숫자버튼들
                for (var i = KeyCode.Alpha0; i <= KeyCode.Alpha9; ++i)
                {
                    keyCodes.Add(i);
                }

                keyCodes.AddRange(new KeyCode[] {
                // 좌측 기능키
                KeyCode.Tab,
                KeyCode.CapsLock,
                KeyCode.LeftShift,
                KeyCode.LeftControl,
                KeyCode.LeftAlt,

                // 우측 기능키
                KeyCode.Return, // Enter
                KeyCode.RightShift,

                // 엔터왼쪽에 기호들
                KeyCode.LeftBracket, // [
                KeyCode.RightBracket, // ]
                KeyCode.Backslash, // \
                KeyCode.Semicolon, // ;
                KeyCode.Quote, // '
                KeyCode.Comma, // ,
                KeyCode.Period, // .
                KeyCode.Slash, // /

                // 위쪽키(상단숫자부분)
                KeyCode.BackQuote, // `
                KeyCode.Minus, // -
                KeyCode.Equals, // =
                KeyCode.Backspace,
            });
            }

            if (GameManager.Inst.keypad)
            {
                // 키패드
                for (var i = KeyCode.Keypad0; i <= KeyCode.Keypad9; ++i)
                {
                    keyCodes.Add(i);
                }
            }

            if (GameManager.Inst.fn)
            {
                for (var i = KeyCode.F1; i <= KeyCode.F12; ++i)
                {
                    keyCodes.Add(i);
                }
            }
            


            return keyCodes;
        }
    }

    /// <summary>
    /// 땅에 공급해 줄 키를 생성하는 기능을 가진 인터페이스
    /// </summary>
    public interface IKeyGenerator
    {
        /// <summary>
        /// 이 제너레이터에서 나올 수 있는 키들의 집합
        /// </summary>
        public IReadOnlyCollection<KeyCode> CandidatePool { get; }

        public KeyCode GetKeyCode();

        public string KeyCodeToString(KeyCode key);
    }

    public static class EarthKeyGenerator
    {
        public static IKeyGenerator KeyGenerator { get; set; } = new RandomKeyGenerator();
    }

    public class NullKeyGenerator : IKeyGenerator
    {
        public IReadOnlyCollection<KeyCode> CandidatePool => new HashSet<KeyCode> { KeyCode.Backspace };

        public KeyCode GetKeyCode() => KeyCode.Backspace;

        public string KeyCodeToString(KeyCode key) => key.ToString();
    }

    public class RandomKeyGenerator : IKeyGenerator
    {
        private readonly List<KeyCode> pool;

        public IReadOnlyCollection<KeyCode> CandidatePool => pool;

        public RandomKeyGenerator()
        {
            pool = new List<KeyCode>(KeysUsed.UsableKeys);
        }

        public KeyCode GetKeyCode() => pool[Random.Range(0, pool.Count)];

        public string KeyCodeToString(KeyCode key) => key switch
        {
            KeyCode.Return => "Enter",

            KeyCode.LeftBracket => "[",
            KeyCode.RightBracket => "]",
            KeyCode.Backslash => "\\",
            KeyCode.Semicolon => ";",
            KeyCode.Quote => "\'",
            KeyCode.Comma => ",",
            KeyCode.Period => ".",
            KeyCode.Slash => "/",

            KeyCode.BackQuote => "`",
            KeyCode.Minus => "-",
            KeyCode.Equals => "=",

            KeyCode.SysReq => "PrintScreen",
            
            KeyCode.Alpha1 => "Upside 1",
            KeyCode.Alpha2 => "Upside 2",
            KeyCode.Alpha3 => "Upside 3",
            KeyCode.Alpha4 => "Upside 4",
            KeyCode.Alpha5 => "Upside 5",
            KeyCode.Alpha6 => "Upside 6",
            KeyCode.Alpha7 => "Upside 7",
            KeyCode.Alpha8 => "Upside 8",
            KeyCode.Alpha9 => "Upside 9",
            KeyCode.Alpha0 => "Upside 0",

            KeyCode.KeypadPeriod => "Keypad .",
            KeyCode.KeypadDivide => "Keypad /",
            KeyCode.KeypadMultiply => "Keypad *",
            KeyCode.KeypadMinus => "Keypad -",
            KeyCode.KeypadPlus => "Keypad +",

            _ => key.ToString()
        };
    }
}
