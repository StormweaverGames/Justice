using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Justice.Controls
{
    /// <summary>
    /// A static class used to access advanced keyboard functionality
    /// </summary>
    public static class KeyboardManager
    {
        private static KeyboardState myPreviousKeyState;
        private static KeyboardState myCurrentKeyState;

        private static Dictionary<Keys, Action<Keys, ButtonState>> myKeyboardListeners;

        public static event EventHandler<TextInputEventArgs> OnTextEntered;

        static KeyboardManager()
        {
            myPreviousKeyState = Keyboard.GetState();
            myCurrentKeyState = Keyboard.GetState();

            myKeyboardListeners = new Dictionary<Keys, Action<Keys, ButtonState>>();
        }

        public static void ListenTo(GameWindow window)
        {
            window.TextInput += WindowCharacterReceived;
        }

        private static void WindowCharacterReceived(object sender, TextInputEventArgs e)
        {
            OnTextEntered?.Invoke(sender, e);
        }

        public static void AddListener(Keys key, Action<Keys, ButtonState> listener)
        {
            if (!myKeyboardListeners.ContainsKey(key))
                myKeyboardListeners.Add(key, listener);
            else
                myKeyboardListeners[key] += listener;
        }

        public static void RemoveListener(Keys key, Action<Keys, ButtonState> listener)
        {
            if (myKeyboardListeners.ContainsKey(key))
                myKeyboardListeners[key] -= listener;
        }

        public static void Poll()
        {
            myPreviousKeyState = myCurrentKeyState;
            myCurrentKeyState = Keyboard.GetState();

            Keys[] pressedNow = myCurrentKeyState.GetPressedKeys();
            Keys[] pressedPrev = myPreviousKeyState.GetPressedKeys();

            for(int index = 0; index < pressedNow.Length; index ++)
                if (myPreviousKeyState.IsKeyUp(pressedNow[index]) && myKeyboardListeners.ContainsKey(pressedNow[index]))
                    myKeyboardListeners[pressedNow[index]](pressedNow[index], ButtonState.Pressed);

            for (int index = 0; index < pressedPrev.Length; index++)
                if (myCurrentKeyState.IsKeyUp(pressedPrev[index]) && myKeyboardListeners.ContainsKey(pressedPrev[index]))
                    myKeyboardListeners[pressedPrev[index]](pressedNow[index], ButtonState.Released);
        }

        public static bool IsKeyDown(Keys key)
        {
            return myCurrentKeyState.IsKeyDown(key);
        }

        public static bool IsKeyUp(Keys key)
        {
            return myCurrentKeyState.IsKeyUp(key);
        }

        public static bool IsKeyPressed(Keys key)
        {
            return myCurrentKeyState.IsKeyDown(key) && myPreviousKeyState.IsKeyUp(key);
        }

        public static bool IsKeyReleased(Keys key)
        {
            return myCurrentKeyState.IsKeyUp(key) && myPreviousKeyState.IsKeyDown(key);
        }
    }
}
