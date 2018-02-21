﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ChaseCameraGameLab3.MacOS
{
    public delegate void GameAction (ButtonState buttonState, Vector2 amount);
    
    public class CommandManager
    {
        private InputListener m_Input;
        private Dictionary<Keys, GameAction> m_KeyBindings = new Dictionary<Keys, GameAction>();
        
        public CommandManager()
        {
            m_Input = new InputListener();
            
            // Register events with the input listener
            m_Input.OnKeyDown += this.OnKeyDown;
            m_Input.OnKeyPressed += this.OnKeyPressed;
            m_Input.OnKeyUp += this.OnKeyUp;
        }
        
        public void Update()
        {
            // Update polling input listener, everything else is handled by events
            m_Input.Update();
        }
        
        public void OnKeyDown(object sender, KeyboardEventArgs e)
        {
            GameAction action = m_KeyBindings[e.Key];
            
 
            if (action != null)
            {
                action(ButtonState.Pressed, new Vector2(1.0f));
            }
        }
        
        public void OnKeyUp(object sender, KeyboardEventArgs e)
        {
            GameAction action = m_KeyBindings[e.Key];
 
            if (action != null)
            {
                action(ButtonState.Released, new Vector2(1.0f));
            }
        }
        
        public void OnKeyPressed(object sender, KeyboardEventArgs e)
        {
            GameAction action = m_KeyBindings[e.Key];
 
            if (action != null)
            {
                action(ButtonState.Pressed, new Vector2(1.0f));
            }
        }
        
        public void AddKeyboardBinding(Keys key, GameAction action)
        {
            // Add key to listen for when polling
            m_Input.AddKey(key);
            
            // Add the binding to the command map
            m_KeyBindings.Add(key, action);
        }

    }
}
