using System;
using System.Collections.Generic;
using System.Text;
using ZachJohnson.Promptu.SkinApi;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Extensions;

namespace ZachJohnson.Promptu.WpfUI.DefaultSkin
{
    internal class DefaultLayoutManager : ILayoutManager
    {
        public DefaultLayoutManager()
        {
        }

        public void PositionPrompt(
            PositioningContext context,
            PositioningMode positioningMode,
            Point? suggestedPosition)
        {
            if (positioningMode == PositioningMode.FollowMouse)
            {
                Point mousePosition = Control.MousePosition;
                suggestedPosition = new Point(mousePosition.X, mousePosition.Y - context.SkinInstance.Prompt.Size.Height);
            }
            else if (positioningMode == PositioningMode.CurrentScreen)
            {
                Point mousePosition = Control.MousePosition;
                Screen currentScreen = Screen.FromPoint(mousePosition);
                suggestedPosition = new Point(
                    (currentScreen.WorkingArea.Left + (currentScreen.WorkingArea.Width / 2) - (context.SkinInstance.Prompt.Size.Width / 2)),
                    currentScreen.WorkingArea.Top + (currentScreen.WorkingArea.Height / 3) - (context.SkinInstance.Prompt.Size.Height / 2));
            }

            if (suggestedPosition != null)
            {
                context.SkinInstance.Prompt.Location = suggestedPosition.Value;
            }

            Point location = context.SkinInstance.Prompt.Location;
            Rectangle screen = Screen.FromPoint(location).WorkingArea;
            Rectangle thisPrompt = new Rectangle(location, context.SkinInstance.Prompt.Size);

            if (thisPrompt.Top < screen.Top)
            {
                thisPrompt.Y = screen.Top;
            }
            else if (thisPrompt.Bottom > screen.Bottom)
            {
                thisPrompt.Y = screen.Bottom - thisPrompt.Height;
            }

            if (thisPrompt.Width <= screen.Width)
            {
                if (thisPrompt.Left < screen.Left)
                {
                    thisPrompt.X = screen.Left;
                }
                else if (thisPrompt.Right > screen.Right)
                {
                    thisPrompt.X = screen.Right - thisPrompt.Width;
                }
            }
            else
            {
                thisPrompt.X = screen.Left;
            }

            context.SkinInstance.Prompt.Location = thisPrompt.Location;
        }

        public void PositionSuggestionProvider(PositioningContext context)
        {
            Rectangle protectedArea = new Rectangle(context.SkinInstance.Prompt.Location, context.SkinInstance.Prompt.Size);

            IInfoBox parameterHelpBox = context.InfoBoxes.ParameterHelpBox;
            if (parameterHelpBox != null && parameterHelpBox.Visible)
            {
                protectedArea = protectedArea.GetBoundingRectangleWith(new Rectangle(parameterHelpBox.Location, parameterHelpBox.ActualSize));
            }

            System.Drawing.Rectangle screen = Screen.GetWorkingArea(protectedArea);
            screen = Taskbar.RemoveBoundsFromScreenIfNecessary(screen);

            Size previousSize = context.SkinInstance.SuggestionProvider.SaveSize;

            int maxItemCount;
            if (previousSize.Height > 0)
            {
                maxItemCount = previousSize.Height / context.SkinInstance.SuggestionProvider.ItemHeight;
            }
            else
            {
                maxItemCount = 10;
            }

            if (maxItemCount < 1)
            {
                maxItemCount = 1;
            }

            int proposedHeight = context.SkinInstance.SuggestionProvider.ItemHeight * (context.SkinInstance.SuggestionProvider.ItemCount);

            if (context.SkinInstance.SuggestionProvider.ItemCount > maxItemCount)
            {
                proposedHeight = context.SkinInstance.SuggestionProvider.ItemHeight * maxItemCount;
            }

            int lowerSpace = screen.Bottom - protectedArea.Bottom;

            Point location = new Point(protectedArea.X, protectedArea.Bottom);

            if (lowerSpace < proposedHeight)
            {
                int upperSpace = protectedArea.Top - screen.Top;

                if (upperSpace > lowerSpace)
                {
                    if (upperSpace < proposedHeight)
                    {
                        proposedHeight = (int)Math.Floor((double)upperSpace / (double)context.SkinInstance.SuggestionProvider.ItemHeight) * context.SkinInstance.SuggestionProvider.ItemHeight;
                    }

                    location = new Point(protectedArea.X, protectedArea.Top - (context.SkinInstance.SuggestionProvider.Padding.Vertical + proposedHeight));
                    //(this.Height - this.ClientSize.Height)
                }
                else
                {
                    proposedHeight = (int)Math.Floor((double)lowerSpace / (double)context.SkinInstance.SuggestionProvider.ItemHeight) * context.SkinInstance.SuggestionProvider.ItemHeight;
                }
            }

            int minWidth = context.SkinInstance.SuggestionProvider.MinimumWidth; //SystemInformation.VerticalScrollBarWidth + this.customListBox.ImageSize.Width;

            context.SkinInstance.SuggestionProvider.Location = location;

            context.SkinInstance.SuggestionProvider.Size = new Size(
                (previousSize.Width < minWidth ? minWidth : previousSize.Width),
                proposedHeight + context.SkinInstance.SuggestionProvider.Padding.Vertical);

            //context.Skin.SuggestionProvider.SaveSize = previousSize;

            //populationInfo.Suggester.SizeAndPositionYourself(
            //    protectedArea,
            //    new List<Rectangle>(),
            //    Globals.CurrentProfile.SuggesterSize);

            System.Windows.Interop.IWin32Window prompt = context.SkinInstance.Prompt.UIObject as System.Windows.Interop.IWin32Window;
            System.Windows.Interop.IWin32Window suggester = context.SkinInstance.SuggestionProvider.UIObject as System.Windows.Interop.IWin32Window;

            if (prompt != null && suggester != null)
            {
                NativeMethods.SetOwner(prompt, suggester);
            }
        }

        public void PositionItemInfoBox(PositioningContext context, IInfoBox itemInfoBox, out Size preferredSize)
        {
            Rectangle rect = context.SkinInstance.SuggestionProvider.GetItemBounds(context.SkinInstance.SuggestionProvider.SelectedIndex);
            itemInfoBox.MaxWidth = GetWidthOfThreeFifthsOfScreen(context.SkinInstance.Prompt.Location) - 5;
            itemInfoBox.Refresh();
            preferredSize = itemInfoBox.ActualSize;

            //int maximumWidth = GetWidthOfThreeFifthsOfScreen(context.Skin.Prompt.Location) - 5;

            Rectangle suggesterFootprint = new Rectangle(context.SkinInstance.SuggestionProvider.Location, context.SkinInstance.SuggestionProvider.Size);

            //Rectangle screen = Screen.GetWorkingArea(suggesterFootprint);

            //int rightSpace = screen.Right - suggesterFootprint.Right - 5;
            //int leftSpace = suggesterFootprint.Left - screen.Left - 5;

            //if (maximumWidth > rightSpace)
            //{pace)
            //    {
            //    if (leftSpace > rightS
            //        if (maximumWidth > leftSpace)
            //        {
            //            maximumWidth = leftSpace;
            //        }
            //    }
            //    else
            //    {
            //        maximumWidth = rightSpace;
            //    }
            //}

            //if (preferredSize.Width > maximumWidth)
            //{
            //    preferredSize = itemInfoBox.GetPreferredSize(new Size(maximumWidth, 0));

            //    if (preferredSize.Width > maximumWidth)
            //    {
            //        preferredSize.Width = maximumWidth;
            //    }
            //}

            //itemInfoBox.Size = preferredSize;
            //informationBox.Size = new Size(5, 5);

            itemInfoBox.Location = PositionRightOrLeft(
                suggesterFootprint,
                context.SkinInstance.SuggestionProvider.Location.Y + rect.Y + ((rect.Height - itemInfoBox.ActualSize.Height) / 2) - context.SkinInstance.SuggestionProvider.Padding.Top,
                preferredSize);
        }

        public void PositionParameterHelpBox(PositioningContext context, IInfoBox parameterHelpBox, out Size preferredSize)
        {
            //preferredSize = parameterHelpBox.GetPreferredSize(Size.Empty);

            parameterHelpBox.MaxWidth = GetWidthOfThreeFifthsOfScreen(context.SkinInstance.Prompt.Location);
            parameterHelpBox.Refresh();
            preferredSize = parameterHelpBox.ActualSize;

            //if (preferredSize.Width > maximumWidth)
            //{
            //    preferredSize = parameterHelpBox.GetPreferredSize(new Size(maximumWidth, 0));

            //    if (preferredSize.Width > maximumWidth)
            //    {
            //        preferredSize.Width = maximumWidth;
            //    }
            //}

            //parameterHelpBox.Size = preferredSize;

            parameterHelpBox.Location = Position(
                new Rectangle(context.SkinInstance.Prompt.Location, context.SkinInstance.Prompt.Size),
                preferredSize);
        }

        private static int GetWidthOfThreeFifthsOfScreen(Point point)
        {
            Screen screen = Screen.FromPoint(point);
            return (screen.WorkingArea.Width / 5) * 3;
        }

        private static Point PositionRightOrLeft(Rectangle protectedArea, int y, Size size)
        {
            System.Drawing.Rectangle screen = Screen.GetWorkingArea(protectedArea);

            int rightSpace = screen.Right - protectedArea.Right;

            Point location = new Point(protectedArea.Right, y);

            if (rightSpace < size.Width)
            {
                int leftSpace = protectedArea.Left - screen.Left;

                if (leftSpace > rightSpace)
                {
                    location = new Point(protectedArea.Left - size.Width, y);
                }
            }

            if (location.X + size.Width > screen.Right - 5)
            {
                location.X = screen.Right - size.Width - 5;
            }

            return location;
        }

        private static Point Position(Rectangle protectedArea, Size size)
        {
            System.Drawing.Rectangle screen = Screen.GetWorkingArea(protectedArea);

            int lowerSpace = screen.Bottom - protectedArea.Bottom;

            Point location = new Point(protectedArea.X, protectedArea.Bottom);

            if (lowerSpace < size.Height)
            {
                int upperSpace = protectedArea.Top - screen.Top;

                if (upperSpace > lowerSpace)
                {
                    location = new Point(protectedArea.X, protectedArea.Top - size.Height);
                }
            }

            if (location.X + size.Width > screen.Right - 5)
            {
                location.X = screen.Right - size.Width - 5;
            }

            return location;
        }

        public void PositionProgressBox(PositioningContext context, IInfoBox progressBox, out Size preferredSize)
        {
            //preferredSize = progressBox.GetPreferredSize(Size.Empty);

            progressBox.MaxWidth = GetWidthOfThreeFifthsOfScreen(context.SkinInstance.Prompt.Location);
            progressBox.Refresh();
            preferredSize = progressBox.ActualSize;

            progressBox.Location = Position(new Rectangle(
                context.SkinInstance.Prompt.Location,
                context.SkinInstance.Prompt.Size), preferredSize);

            //progressBox.Size = preferredSize;
        }

        public void PositionInfoBox(PositioningContext context, IInfoBox box, out Size preferredSize)
        {
            preferredSize = box.GetPreferredSize(Size.Empty);
            box.Size = preferredSize;
            box.Location = Position(new Rectangle(
                context.SkinInstance.Prompt.Location,
                context.SkinInstance.Prompt.Size), preferredSize);
        }
    }
}
