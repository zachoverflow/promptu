// Copyright 2022 Zach Johnson
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

namespace System.Drawing.Extensions
{
    using System.Drawing;

    internal static class RectangleExtensions
    {
        public static bool IntersectsWithInYDimension(this Rectangle rectangle, Rectangle rect)
        {
            return rectangle.IntersectsWithInYDimension(rect, 0);
        }

        public static bool IntersectsWithInYDimension(this Rectangle rectangle, Rectangle rect, int margin)
        {
            return (rectangle.Top >= rect.Top - margin && rectangle.Top <= rect.Bottom + margin) || (rectangle.Bottom >= rect.Top - margin && rectangle.Bottom <= rect.Bottom + margin);
        }

        public static bool IntersectsWithInXDimension(this Rectangle rectangle, Rectangle rect)
        {
            return rectangle.IntersectsWithInXDimension(rect, 0);
        }

        public static bool IntersectsWithInXDimension(this Rectangle rectangle, Rectangle rect, int margin)
        {
            return (rectangle.Left >= rect.Left - margin && rectangle.Left <= rect.Right + margin) || (rectangle.Right >= rect.Left - margin && rectangle.Right <= rect.Right + margin);
        }

        public static Point GetCenter(this Rectangle rectangle)
        {
            return new Point(rectangle.Left + ((rectangle.Right - rectangle.Left) / 2), rectangle.Top + ((rectangle.Bottom - rectangle.Top) / 2));
        }

        public static Rectangle MovingCenter(this Rectangle rectangle, Point newCenter)
        {
            Point currentCenter = rectangle.GetCenter();
            return new Rectangle(
                new Point(rectangle.Left - currentCenter.X + newCenter.X, rectangle.Top - currentCenter.Y + newCenter.Y), 
                rectangle.Size);
        }

        public static bool IntersectsWith(this Rectangle rectangle, Rectangle rect, int margin)
        {
            Rectangle marginized = new Rectangle(rect.Left - margin, rect.Top - margin, rect.Width + (2 * margin), rect.Height + (2 * margin));
            return rectangle.IntersectsWith(marginized);
        }

        public static bool IntersectsWithOrContains(this Rectangle rectangle, Rectangle rect)
        {
            return rectangle.IntersectsWith(rect) || rectangle.Contains(rect);
        }

        public static Rectangle ClockwiseQuadrantalRotation(this Rectangle rect, int numberOfQuadrants)
        {
            int reallyRotating = numberOfQuadrants % 4;
            if (reallyRotating == 0 || reallyRotating == 2)
            {
                return rect;
            }
            else
            {
                Point center = new Point(rect.Left + (rect.Width / 2), rect.Top + (rect.Height / 2));
                return new Rectangle(center.X - (rect.Height / 2), center.Y - (rect.Width / 2), rect.Height, rect.Width);
            }
        }

        public static Rectangle CornerIntersection(this Rectangle rectangle1, Rectangle rectangle2)
        {
            if (rectangle1.IntersectsWith(rectangle2) && rectangle1 != rectangle2)
            {
                Rectangle intersection = new Rectangle(rectangle1.Location, rectangle1.Size);
                intersection.Intersect(rectangle2);
                bool rectangle1IsHorizontal = true;
                bool rectangle2IsHorizontal = false;
                if (rectangle1.Height > rectangle1.Width)
                {
                    rectangle1IsHorizontal = false;
                }

                if (rectangle2.Width > rectangle2.Height)
                {
                    rectangle2IsHorizontal = true;
                }

                if (rectangle1IsHorizontal && rectangle2IsHorizontal == false)
                {
                    intersection.Width = rectangle2.Width;
                    intersection.Height = rectangle1.Height;
                }
                else if (rectangle1IsHorizontal == false && rectangle2IsHorizontal)
                {
                    intersection.Width = rectangle1.Width;
                    intersection.Height = rectangle2.Height;
                }

                return intersection;
            }

            throw new ArgumentException("the rectangles do not intersect");
        }

        public static Rectangle GetBoundingRectangleWith(this Rectangle thisRect, Rectangle rect)
        {
            int left = thisRect.Left;
            int right = thisRect.Right;
            int top = thisRect.Top;
            int bottom = thisRect.Bottom;

            if (rect.Left < left)
            {
                left = rect.Left;
            }

            if (rect.Top < top)
            {
                top = rect.Top;
            }

            if (rect.Right > right)
            {
                right = rect.Right;
            }

            if (rect.Bottom > bottom)
            {
                bottom = rect.Bottom;
            }

            return new Rectangle(left, top, right - left, bottom - top);
        }
    }
}
