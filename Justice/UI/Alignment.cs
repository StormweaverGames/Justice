using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Justice.UI
{
    public enum VerticalAlignment
    {
        Top    = 1,
        Center = 2,
        Bottom = 3
    }

    public enum HorizontalAlignment
    {
        Left    = 1,
        Center  = 2,
        Right   = 3
    }

    public enum Alignment
    {
        LeftTop      = 1 | (1 << 2),
        CenterTop    = 2 | (1 << 2),
        RightTop     = 3 | (1 << 2),
        
        LeftMiddle   = 1 | (2 << 2),
        CenterMiddle = 2 | (2 << 2),
        RightMiddle  = 3 | (2 << 2),

        LeftBottom   = 1 | (3 << 2),
        CenterBottom = 2 | (3 << 2),
        RightBottom  = 3 | (3 << 2)
    };

    /// <summary>
    /// Various tools for dealing with alignments
    /// </summary>
    public static class AlignmentTools
    {
        public static Alignment GetAlignment(HorizontalAlignment horizontal, VerticalAlignment vertical)
        {
            return (Alignment)((int)horizontal | ((int)vertical << 2));
        }

        /// <summary>
        /// Handles getting the horizontal and vertical alignment elements of al alignment
        /// </summary>
        /// <param name="alignment">The alignment to get the components for</param>
        /// <param name="horizontal">The value to store the horizontal aligment in</param>
        /// <param name="vertical">The value to store the vertical alignment in</param>
        public static void GetComponents(this Alignment alignment, out HorizontalAlignment horizontal, out VerticalAlignment vertical)
        {
            horizontal = (HorizontalAlignment)((int)alignment & 3);
            vertical = (VerticalAlignment)(((int)alignment >> 2) & 3);
        }

        /// <summary>
        /// Determines the reference point for a given alignment relative to the given bounds.
        /// </summary>
        /// <example>
        /// 
        /// Alignment a = Alignment.LeftTop;
        /// Vector2 refrence = a.GetReferencePoint(new Rectangle(0, 0, 10, 10));
        /// 
        /// // reference is now (0, 0)
        ///         
        /// Alignment b = Alignment.RightTop;
        /// refrence = b.GetReferencePoint(new Rectangle(0, 0, 10, 10));
        /// 
        /// // reference is now (10, 0)
        /// 
        /// </example>
        /// <param name="alignment">The alignment to determine the point for</param>
        /// <param name="bounds">The bounds to get the point for</param>
        /// <returns>The reference point for the given alignment</returns>
        public static Vector2 GetReferencePoint(this Alignment alignment, Rectangle bounds)
        {
            HorizontalAlignment horizontal = (HorizontalAlignment)((int)alignment & 3);
            VerticalAlignment vertical = (VerticalAlignment)(((int)alignment >> 2) & 3);

            Vector2 result = new Vector2();

            switch (horizontal)
            {
                case HorizontalAlignment.Left:
                    result.X = bounds.Left;
                    break;
                case HorizontalAlignment.Center:
                    result.X = bounds.Center.X;
                    break;
                case HorizontalAlignment.Right:
                    result.X = bounds.Right;
                    break;
            }

            switch (vertical)
            {
                case VerticalAlignment.Top:
                    result.Y = bounds.Top;
                    break;
                case VerticalAlignment.Center:
                    result.Y = bounds.Center.Y;
                    break;
                case VerticalAlignment.Bottom:
                    result.Y = bounds.Bottom;
                    break;
            }

            return result;
        }

        /// <summary>
        /// Determines the bounds that a peice of content should have to align to an orgin point
        /// </summary>
        /// <param name="alignment">The aligment to solve for</param>
        /// <param name="orgin">The origin to place the content relative to</param>
        /// <param name="contentBounds">The current bounds of the content</param>
        /// <returns>The new bounds for the content</returns>
        public static Rectangle SolveBounds(this Alignment alignment, Point orgin, Rectangle contentBounds)
        {
            HorizontalAlignment horizontal = (HorizontalAlignment)((int)alignment & 3);
            VerticalAlignment vertical = (VerticalAlignment)(((int)alignment >> 2) & 3);

            Rectangle rect = new Rectangle();
            rect.Width = contentBounds.Width;
            rect.Height = contentBounds.Height;

            switch (horizontal)
            {
                case HorizontalAlignment.Left:
                    rect.X = orgin.X;
                    break;
                case HorizontalAlignment.Center:
                    rect.X = orgin.X - contentBounds.Width / 2;
                    break;
                case HorizontalAlignment.Right:
                    rect.X = orgin.X - contentBounds.Width;
                    break;
            }

            switch (vertical)
            {
                case VerticalAlignment.Top:
                    rect.Y = orgin.Y;
                    break;
                case VerticalAlignment.Center:
                    rect.Y = orgin.Y - contentBounds.Height / 2;
                    break;
                case VerticalAlignment.Bottom:
                    rect.Y = orgin.Y - contentBounds.Height;
                    break;
            }

            return rect;
        }

        /// <summary>
        /// Determines the normal that a paice of content should expand to relative to the aligment,
        /// (0, 0) is expand in all directions
        /// </summary>
        /// <example>
        /// 
        /// Alignment a = Alignment.LeftTop;
        /// Vector2 normal = a.GetContentNormal();
        /// 
        /// // normal is now (1, 1)
        ///         
        /// Alignment b = Alignment.CenterBottom;
        /// normal = b.GetContentNormal();
        /// 
        /// // normal is now (0, -1)
        /// 
        /// </example>
        /// <param name="alignment">The alignment to determine the normal for</param>
        /// <param name="bounds">The bounds to get the point for</param>
        /// <returns>The content normal for the given alignment</returns>
        public static Vector2 GetContentNormal(this Alignment alignment)
        {
            HorizontalAlignment horizontal = (HorizontalAlignment)((int)alignment & 3);
            VerticalAlignment vertical = (VerticalAlignment)(((int)alignment >> 2) & 3);

            Vector2 result = new Vector2();

            switch (horizontal)
            {
                case HorizontalAlignment.Left:
                    result.X = 1;
                    break;
                case HorizontalAlignment.Center:
                    result.X = 0;
                    break;
                case HorizontalAlignment.Right:
                    result.X = -1;
                    break;
            }

            switch (vertical)
            {
                case VerticalAlignment.Top:
                    result.Y = 1;
                    break;
                case VerticalAlignment.Center:
                    result.Y = 0;
                    break;
                case VerticalAlignment.Bottom:
                    result.Y = -1;
                    break;
            }

            return result;
        }
    }    
}
