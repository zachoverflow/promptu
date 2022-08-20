using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Collections.ObjectModel;

namespace ZachJohnson.Promptu
{
    internal static class ValidHotkeyKeys
    {
        private static List<ValidHotkeyKey> validKeys;
        private static ReadOnlyCollection<ValidHotkeyKey> readonlyValidKeys;

        static ValidHotkeyKeys()
        {
            validKeys = new List<ValidHotkeyKey>(new ValidHotkeyKey[]
            {
                new ValidHotkeyKey("A", Keys.A),
                new ValidHotkeyKey("B", Keys.B),
                new ValidHotkeyKey("C", Keys.C),
                new ValidHotkeyKey("D", Keys.D),
                new ValidHotkeyKey("E", Keys.E),
                new ValidHotkeyKey("F", Keys.F),
                new ValidHotkeyKey("G", Keys.G),
                new ValidHotkeyKey("H", Keys.H),
                new ValidHotkeyKey("I", Keys.I),
                new ValidHotkeyKey("J", Keys.J),
                new ValidHotkeyKey("K", Keys.K),
                new ValidHotkeyKey("L", Keys.L),
                new ValidHotkeyKey("N", Keys.N),
                new ValidHotkeyKey("M", Keys.M),
                new ValidHotkeyKey("O", Keys.O),
                new ValidHotkeyKey("P", Keys.P),
                new ValidHotkeyKey("Q", Keys.Q),
                new ValidHotkeyKey("R", Keys.R),
                new ValidHotkeyKey("S", Keys.S),
                new ValidHotkeyKey("T", Keys.T),
                new ValidHotkeyKey("U", Keys.U),
                new ValidHotkeyKey("V", Keys.V),
                new ValidHotkeyKey("W", Keys.W),
                new ValidHotkeyKey("X", Keys.X),
                new ValidHotkeyKey("Y", Keys.Y),
                new ValidHotkeyKey("Z", Keys.Z),
                new ValidHotkeyKey("0", Keys.D0),
                new ValidHotkeyKey("1", Keys.D1),
                new ValidHotkeyKey("2", Keys.D2),
                new ValidHotkeyKey("3", Keys.D3),
                new ValidHotkeyKey("4", Keys.D4),
                new ValidHotkeyKey("5", Keys.D5),
                new ValidHotkeyKey("6", Keys.D6),
                new ValidHotkeyKey("7", Keys.D7),
                new ValidHotkeyKey("8", Keys.D8),
                new ValidHotkeyKey("9", Keys.D9),
                new ValidHotkeyKey("Num Pad 0", Keys.NumPad0),
                new ValidHotkeyKey("Num Pad 1", Keys.NumPad1),
                new ValidHotkeyKey("Num Pad 2", Keys.NumPad2),
                new ValidHotkeyKey("Num Pad 3", Keys.NumPad3),
                new ValidHotkeyKey("Num Pad 4", Keys.NumPad4),
                new ValidHotkeyKey("Num Pad 5", Keys.NumPad5),
                new ValidHotkeyKey("Num Pad 6", Keys.NumPad6),
                new ValidHotkeyKey("Num Pad 7", Keys.NumPad7),
                new ValidHotkeyKey("Num Pad 8", Keys.NumPad8),
                new ValidHotkeyKey("Num Pad 9", Keys.NumPad9),
                new ValidHotkeyKey("F1", Keys.F1),
                new ValidHotkeyKey("F2", Keys.F2),
                new ValidHotkeyKey("F3", Keys.F3),
                new ValidHotkeyKey("F4", Keys.F4),
                new ValidHotkeyKey("F5", Keys.F5),
                new ValidHotkeyKey("F6", Keys.F6),
                new ValidHotkeyKey("F7", Keys.F7),
                new ValidHotkeyKey("F8", Keys.F8),
                new ValidHotkeyKey("F9", Keys.F9),
                new ValidHotkeyKey("F10", Keys.F10),
                new ValidHotkeyKey("F11", Keys.F11),
                new ValidHotkeyKey("F12", Keys.F12),
                new ValidHotkeyKey("Left Arrow", Keys.Left),
                new ValidHotkeyKey("Right Arrow", Keys.Right),
                new ValidHotkeyKey("Up Arrow", Keys.Up),
                new ValidHotkeyKey("Down Arrow", Keys.Down),
                new ValidHotkeyKey("`", Keys.Oem3),
                new ValidHotkeyKey("-", Keys.OemMinus),
                new ValidHotkeyKey("=", Keys.Oemplus), // test
                new ValidHotkeyKey("[", Keys.Oem4),
                new ValidHotkeyKey("]", Keys.Oem6),
                new ValidHotkeyKey("\\", Keys.Oem5),
                new ValidHotkeyKey(";", Keys.Oem1),
                new ValidHotkeyKey("'", Keys.Oem7),
                new ValidHotkeyKey(",", Keys.Oemcomma),
                new ValidHotkeyKey(".", Keys.OemPeriod),
                new ValidHotkeyKey("/", Keys.Oem2),
                new ValidHotkeyKey("Enter", Keys.Enter),
                new ValidHotkeyKey("Space", Keys.Space),
                new ValidHotkeyKey("Backspace", Keys.Back),
                new ValidHotkeyKey("Home", Keys.Home),
                new ValidHotkeyKey("End", Keys.End),
                new ValidHotkeyKey("Page Up", Keys.PageUp),
                new ValidHotkeyKey("Page Down", Keys.PageDown),
                new ValidHotkeyKey("Delete", Keys.Delete),
                new ValidHotkeyKey("Insert", Keys.Insert),
                new ValidHotkeyKey("Escape", Keys.Escape)
            });

            readonlyValidKeys = new ReadOnlyCollection<ValidHotkeyKey>(validKeys);
        }

        public static ReadOnlyCollection<ValidHotkeyKey> ValidKeys
        {
            get { return readonlyValidKeys; }
        }

        public static string GetStringRepresentation(Keys key)
        {
            foreach (ValidHotkeyKey hotkeyKey in validKeys)
            {
                if (hotkeyKey.AssociatedKey == key)
                {
                    return hotkeyKey.ToString();
                }
            }

            return "Unknown";
        }

        public static ValidHotkeyKey Map(Keys key)
        {
            foreach (ValidHotkeyKey item in ValidHotkeyKeys.ValidKeys)
            {
                if (key == item.AssociatedKey)
                {
                    //try
                    //{
                    //    //this.lastHotkeyKey = hotkeyKey;
                    //    this.settingUnderlyingHotkeyKeyValue = true;
                    //    this.hotkeyKey.SelectedItem = item;
                    //}
                    //finally
                    //{
                    //    this.settingUnderlyingHotkeyKeyValue = false;
                    //}

                    return item;
                }
            }

            throw new ArgumentException("No hotkey matched the provided hotkey.");
        }
    }
}
