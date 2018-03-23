using System;
using Microsoft.Xna.Framework;

namespace WareHouse3
{
    public class Origin
    {
        public enum FrameOrigin {
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

        public static Vector2 GetOrigin(int width, int height, FrameOrigin type) {
            switch (type) {
				case FrameOrigin.topLeft:
					return new Vector2(0.0f, 0.0f);
				case FrameOrigin.topCenter:
					return new Vector2((width / 2.0f), 0.0f);
				case FrameOrigin.topRight:
					return new Vector2(width, 0.0f);
                case FrameOrigin.left:
                    return new Vector2(0.0f, (height / 2.0f));
                case FrameOrigin.center:
                    return new Vector2((width / 2.0f), (height / 2.0f));
                case FrameOrigin.right:
                    return new Vector2(width, (height / 2.0f));
                case FrameOrigin.bottomLeft:
                    return new Vector2(0.0f, height);
				case FrameOrigin.bottomCenter:
					return new Vector2((width / 2.0f), height);
                case FrameOrigin.bottomRight:
					return new Vector2(width, height);
                default:
                    return new Vector2(0.0f, 0.0f);
            }
        }
        

    }
}
