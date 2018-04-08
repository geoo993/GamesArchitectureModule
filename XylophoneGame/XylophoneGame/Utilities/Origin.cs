using System;
using Microsoft.Xna.Framework;

namespace XylophoneGame
{
    public class FrameOrigin
    {
        public enum OriginType {
            left,
            right,
            center,
            topLeft,
            topRight,
            topCenter,
            bottomLeft,
            bottomRight,
            bottomCenter
        }

        public static Vector2 GetOrigin(int width, int height, OriginType type) {
            switch (type) {
				case OriginType.topLeft:
					return new Vector2(0.0f, 0.0f);
				case OriginType.topCenter:
					return new Vector2((width / 2.0f), 0.0f);
				case OriginType.topRight:
					return new Vector2(width, 0.0f);
                case OriginType.left:
                    return new Vector2(0.0f, (height / 2.0f));
                case OriginType.center:
                    return new Vector2((width / 2.0f), (height / 2.0f));
                case OriginType.right:
                    return new Vector2(width, (height / 2.0f));
                case OriginType.bottomLeft:
                    return new Vector2(0.0f, height);
				case OriginType.bottomCenter:
					return new Vector2((width / 2.0f), height);
                case OriginType.bottomRight:
					return new Vector2(width, height);
                default:
                    return new Vector2(0.0f, 0.0f);
            }
        }
        

    }
}
