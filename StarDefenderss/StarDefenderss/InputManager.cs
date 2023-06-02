using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace StarDefenderss;

public static class InputManager
{
    private static KeyboardState currentKeyboardState;
    private static KeyboardState previousKeyboardState;
    private static MouseState currentMouseState;
    private static MouseState previousMouseState;

    public static void Update()
    {
        previousKeyboardState = currentKeyboardState;
        currentKeyboardState = Keyboard.GetState();

        previousMouseState = currentMouseState;
        currentMouseState = Mouse.GetState();
    }

    public static bool IsKeyPressed(Keys key)
    {
        return currentKeyboardState.IsKeyDown(key) && previousKeyboardState.IsKeyUp(key);
    }

    public static bool IsMouseLeftButtonPressed()
    {
        return currentMouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released;
    }
    public static bool IsMouseRightButtonPressed()
    {
        return currentMouseState.RightButton == ButtonState.Pressed && previousMouseState.RightButton == ButtonState.Released;
    }

    public static Point GetMousePosition()
    {
        return new Point(currentMouseState.X, currentMouseState.Y);
    }
}
