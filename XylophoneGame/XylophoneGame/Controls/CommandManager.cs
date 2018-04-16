using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace XylophoneGame
{
    public delegate void GameAction(ButtonAction buttonState, Vector2 amount);

    public class CommandManager
    {
        private InputListener Input;

        private Dictionary<Keys, GameAction> KeyBindings = new Dictionary<Keys, GameAction>();
        private Dictionary<MouseButton, GameAction> MouseButtonBindings = new Dictionary<MouseButton, GameAction>();

        public CommandManager()
        {
            Input = new InputListener();

            // Register events with the input listener
            Input.OnKeyDown += this.OnKeyDown;
            Input.OnKeyPressed += this.OnKeyPressed;
            Input.OnKeyUp += this.OnKeyUp;
            Input.OnMouseButtonDown += this.OnMouseButtonDown;
        }

        public void Update()
        {
            // Update polling input listener, everything else is handled by events
            Input.Update();
        }

        public void OnKeyDown(object sender, KeyboardEventArgs e)
        {
            GameAction action = KeyBindings[e.Key];

            if (action != null)
            {
                action(ButtonAction.DOWN, new Vector2(1.0f));
            }

        }

        public void OnKeyUp(object sender, KeyboardEventArgs e)
        {
            GameAction action = KeyBindings[e.Key];

            if (action != null)
            {
                action(ButtonAction.UP, new Vector2(1.0f));
            }
        }

        public void OnKeyPressed(object sender, KeyboardEventArgs e)
        {
            GameAction action = KeyBindings[e.Key];

            if (action != null)
            {
                action(ButtonAction.PRESSED, new Vector2(1.0f));
            }
        }

        //
        public void OnMouseButtonDown(object sender, MouseEventArgs e)
        {
            GameAction action = MouseButtonBindings[e.Button];

            if (action != null)
            {
                action(ButtonAction.DOWN,  new Vector2(e.CurrentState.X, e.CurrentState.Y));
            }
        }

        public void AddKeyboardBinding(Keys key, GameAction action)
        {
            // Add key to listen for when polling
            Input.AddKey(key);

            // Add the binding to the command map
            KeyBindings.Add(key, action);
        }

        public void AddMouseBinding(MouseButton button, GameAction action)
        {
            // Add key to listen for when polling
            Input.AddButton(button);

            // Add the binding to the command map
            MouseButtonBindings.Add(button, action);
        }

        public void Destroy() {
            Input = null;
        }
    }

}
