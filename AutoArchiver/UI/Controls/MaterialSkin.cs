/*
==============================================================================
The MIT License (MIT)

Copyright © 2014 Ignace Maes

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
==============================================================================
*/

#region Imports

using System;
using System.Linq;
using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;
using System.ComponentModel;
using MaterialSkin.Controls;
using MaterialSkin.Animations;
using System.Drawing.Drawing2D;
using System.Collections.Generic;
using System.Runtime.InteropServices;

#endregion
namespace MaterialSkin
{
    #region MaterialSkinManager

    public class MaterialSkinManager
    {
        //Singleton instance
        private static MaterialSkinManager instance;

        //Forms to control
        private readonly List<Form> formsToManage = new List<Form>();

        //Theme
        private Themes theme;
        public Themes Theme
        {
            get { return theme; }
            set
            {
                theme = value;
                UpdateBackgrounds();
            }
        }

        private ColorScheme colorScheme;
        public ColorScheme ColorScheme
        {
            get { return colorScheme; }
            set
            {
                colorScheme = value;
                UpdateBackgrounds();
            }
        }

        public enum Themes : byte
        {
            LIGHT,
            DARK
        }

        //Constant color values
        private static readonly Color PRIMARY_TEXT_BLACK = Color.FromArgb(222, 0, 0, 0);
        private static readonly Brush PRIMARY_TEXT_BLACK_BRUSH = new SolidBrush(PRIMARY_TEXT_BLACK);
        public static Color SECONDARY_TEXT_BLACK = Color.FromArgb(138, 0, 0, 0);
        public static Brush SECONDARY_TEXT_BLACK_BRUSH = new SolidBrush(SECONDARY_TEXT_BLACK);
        private static readonly Color DISABLED_OR_HINT_TEXT_BLACK = Color.FromArgb(66, 0, 0, 0);
        private static readonly Brush DISABLED_OR_HINT_TEXT_BLACK_BRUSH = new SolidBrush(DISABLED_OR_HINT_TEXT_BLACK);
        private static readonly Color DIVIDERS_BLACK = Color.FromArgb(31, 0, 0, 0);
        private static readonly Brush DIVIDERS_BLACK_BRUSH = new SolidBrush(DIVIDERS_BLACK);

        private static readonly Color PRIMARY_TEXT_WHITE = Color.FromArgb(255, 255, 255, 255);
        private static readonly Brush PRIMARY_TEXT_WHITE_BRUSH = new SolidBrush(PRIMARY_TEXT_WHITE);
        public static Color SECONDARY_TEXT_WHITE = Color.FromArgb(179, 255, 255, 255);
        public static Brush SECONDARY_TEXT_WHITE_BRUSH = new SolidBrush(SECONDARY_TEXT_WHITE);
        private static readonly Color DISABLED_OR_HINT_TEXT_WHITE = Color.FromArgb(77, 255, 255, 255);
        private static readonly Brush DISABLED_OR_HINT_TEXT_WHITE_BRUSH = new SolidBrush(DISABLED_OR_HINT_TEXT_WHITE);
        private static readonly Color DIVIDERS_WHITE = Color.FromArgb(31, 255, 255, 255);
        private static readonly Brush DIVIDERS_WHITE_BRUSH = new SolidBrush(DIVIDERS_WHITE);

        // Checkbox colors
        private static readonly Color CHECKBOX_OFF_LIGHT = Color.FromArgb(138, 0, 0, 0);
        private static readonly Brush CHECKBOX_OFF_LIGHT_BRUSH = new SolidBrush(CHECKBOX_OFF_LIGHT);
        private static readonly Color CHECKBOX_OFF_DISABLED_LIGHT = Color.FromArgb(66, 0, 0, 0);
        private static readonly Brush CHECKBOX_OFF_DISABLED_LIGHT_BRUSH = new SolidBrush(CHECKBOX_OFF_DISABLED_LIGHT);

        private static readonly Color CHECKBOX_OFF_DARK = Color.FromArgb(179, 255, 255, 255);
        private static readonly Brush CHECKBOX_OFF_DARK_BRUSH = new SolidBrush(CHECKBOX_OFF_DARK);
        private static readonly Color CHECKBOX_OFF_DISABLED_DARK = Color.FromArgb(77, 255, 255, 255);
        private static readonly Brush CHECKBOX_OFF_DISABLED_DARK_BRUSH = new SolidBrush(CHECKBOX_OFF_DISABLED_DARK);

        //Raised button
        private static readonly Color RAISED_BUTTON_BACKGROUND = Color.FromArgb(255, 255, 255, 255);
        private static readonly Brush RAISED_BUTTON_BACKGROUND_BRUSH = new SolidBrush(RAISED_BUTTON_BACKGROUND);
        private static readonly Color RAISED_BUTTON_TEXT_LIGHT = PRIMARY_TEXT_WHITE;
        private static readonly Brush RAISED_BUTTON_TEXT_LIGHT_BRUSH = new SolidBrush(RAISED_BUTTON_TEXT_LIGHT);
        private static readonly Color RAISED_BUTTON_TEXT_DARK = PRIMARY_TEXT_BLACK;
        private static readonly Brush RAISED_BUTTON_TEXT_DARK_BRUSH = new SolidBrush(RAISED_BUTTON_TEXT_DARK);

        //Flat button
        private static readonly Color FLAT_BUTTON_BACKGROUND_HOVER_LIGHT = Color.FromArgb(20.PercentageToColorComponent(), 0x999999.ToColor());
        private static readonly Brush FLAT_BUTTON_BACKGROUND_HOVER_LIGHT_BRUSH = new SolidBrush(FLAT_BUTTON_BACKGROUND_HOVER_LIGHT);
        private static readonly Color FLAT_BUTTON_BACKGROUND_PRESSED_LIGHT = Color.FromArgb(40.PercentageToColorComponent(), 0x999999.ToColor());
        private static readonly Brush FLAT_BUTTON_BACKGROUND_PRESSED_LIGHT_BRUSH = new SolidBrush(FLAT_BUTTON_BACKGROUND_PRESSED_LIGHT);
        private static readonly Color FLAT_BUTTON_DISABLEDTEXT_LIGHT = Color.FromArgb(26.PercentageToColorComponent(), 0x000000.ToColor());
        private static readonly Brush FLAT_BUTTON_DISABLEDTEXT_LIGHT_BRUSH = new SolidBrush(FLAT_BUTTON_DISABLEDTEXT_LIGHT);

        private static readonly Color FLAT_BUTTON_BACKGROUND_HOVER_DARK = Color.FromArgb(15.PercentageToColorComponent(), 0xCCCCCC.ToColor());
        private static readonly Brush FLAT_BUTTON_BACKGROUND_HOVER_DARK_BRUSH = new SolidBrush(FLAT_BUTTON_BACKGROUND_HOVER_DARK);
        private static readonly Color FLAT_BUTTON_BACKGROUND_PRESSED_DARK = Color.FromArgb(25.PercentageToColorComponent(), 0xCCCCCC.ToColor());
        private static readonly Brush FLAT_BUTTON_BACKGROUND_PRESSED_DARK_BRUSH = new SolidBrush(FLAT_BUTTON_BACKGROUND_PRESSED_DARK);
        private static readonly Color FLAT_BUTTON_DISABLEDTEXT_DARK = Color.FromArgb(30.PercentageToColorComponent(), 0xFFFFFF.ToColor());
        private static readonly Brush FLAT_BUTTON_DISABLEDTEXT_DARK_BRUSH = new SolidBrush(FLAT_BUTTON_DISABLEDTEXT_DARK);

        //ContextMenuStrip
        private static readonly Color CMS_BACKGROUND_LIGHT_HOVER = Color.FromArgb(255, 238, 238, 238);
        private static readonly Brush CMS_BACKGROUND_HOVER_LIGHT_BRUSH = new SolidBrush(CMS_BACKGROUND_LIGHT_HOVER);

        private static readonly Color CMS_BACKGROUND_DARK_HOVER = Color.FromArgb(38, 204, 204, 204);
        private static readonly Brush CMS_BACKGROUND_HOVER_DARK_BRUSH = new SolidBrush(CMS_BACKGROUND_DARK_HOVER);

        //Application background
        private static readonly Color BACKGROUND_LIGHT = Color.FromArgb(255, 255, 255, 255);
        private static Brush BACKGROUND_LIGHT_BRUSH = new SolidBrush(BACKGROUND_LIGHT);

        private static readonly Color BACKGROUND_DARK = Color.FromArgb(255, 51, 51, 51);
        private static Brush BACKGROUND_DARK_BRUSH = new SolidBrush(BACKGROUND_DARK);

        //Application action bar
        public readonly Color ACTION_BAR_TEXT = Color.FromArgb(255, 255, 255, 255);
        public readonly Brush ACTION_BAR_TEXT_BRUSH = new SolidBrush(Color.FromArgb(255, 255, 255, 255));
        public readonly Color ACTION_BAR_TEXT_SECONDARY = Color.FromArgb(153, 255, 255, 255);
        public readonly Brush ACTION_BAR_TEXT_SECONDARY_BRUSH = new SolidBrush(Color.FromArgb(153, 255, 255, 255));

        public Color GetPrimaryTextColor()
        {
            return (Theme == Themes.LIGHT ? PRIMARY_TEXT_BLACK : PRIMARY_TEXT_WHITE);
        }

        public Brush GetPrimaryTextBrush()
        {
            return (Theme == Themes.LIGHT ? PRIMARY_TEXT_BLACK_BRUSH : PRIMARY_TEXT_WHITE_BRUSH);
        }

        public Color GetSecondaryTextColor()
        {
            return (Theme == Themes.LIGHT ? SECONDARY_TEXT_BLACK : SECONDARY_TEXT_WHITE);
        }

        public Brush GetSecondaryTextBrush()
        {
            return (Theme == Themes.LIGHT ? SECONDARY_TEXT_BLACK_BRUSH : SECONDARY_TEXT_WHITE_BRUSH);
        }

        public Color GetDisabledOrHintColor()
        {
            return (Theme == Themes.LIGHT ? DISABLED_OR_HINT_TEXT_BLACK : DISABLED_OR_HINT_TEXT_WHITE);
        }

        public Brush GetDisabledOrHintBrush()
        {
            return (Theme == Themes.LIGHT ? DISABLED_OR_HINT_TEXT_BLACK_BRUSH : DISABLED_OR_HINT_TEXT_WHITE_BRUSH);
        }

        public Color GetDividersColor()
        {
            return (Theme == Themes.LIGHT ? DIVIDERS_BLACK : DIVIDERS_WHITE);
        }

        public Brush GetDividersBrush()
        {
            return (Theme == Themes.LIGHT ? DIVIDERS_BLACK_BRUSH : DIVIDERS_WHITE_BRUSH);
        }

        public Color GetCheckboxOffColor()
        {
            return (Theme == Themes.LIGHT ? CHECKBOX_OFF_LIGHT : CHECKBOX_OFF_DARK);
        }

        public Brush GetCheckboxOffBrush()
        {
            return (Theme == Themes.LIGHT ? CHECKBOX_OFF_LIGHT_BRUSH : CHECKBOX_OFF_DARK_BRUSH);
        }

        public Color GetCheckBoxOffDisabledColor()
        {
            return (Theme == Themes.LIGHT ? CHECKBOX_OFF_DISABLED_LIGHT : CHECKBOX_OFF_DISABLED_DARK);
        }

        public Brush GetCheckBoxOffDisabledBrush()
        {
            return (Theme == Themes.LIGHT ? CHECKBOX_OFF_DISABLED_LIGHT_BRUSH : CHECKBOX_OFF_DISABLED_DARK_BRUSH);
        }

        public Brush GetRaisedButtonBackgroundBrush()
        {
            return RAISED_BUTTON_BACKGROUND_BRUSH;
        }

        public Brush GetRaisedButtonTextBrush(bool primary)
        {
            return (primary ? RAISED_BUTTON_TEXT_LIGHT_BRUSH : RAISED_BUTTON_TEXT_DARK_BRUSH);
        }

        public Color GetFlatButtonHoverBackgroundColor()
        {
            return (Theme == Themes.LIGHT ? FLAT_BUTTON_BACKGROUND_HOVER_LIGHT : FLAT_BUTTON_BACKGROUND_HOVER_DARK);
        }

        public Brush GetFlatButtonHoverBackgroundBrush()
        {
            return (Theme == Themes.LIGHT ? FLAT_BUTTON_BACKGROUND_HOVER_LIGHT_BRUSH : FLAT_BUTTON_BACKGROUND_HOVER_DARK_BRUSH);
        }

        public Color GetFlatButtonPressedBackgroundColor()
        {
            return (Theme == Themes.LIGHT ? FLAT_BUTTON_BACKGROUND_PRESSED_LIGHT : FLAT_BUTTON_BACKGROUND_PRESSED_DARK);
        }

        public Brush GetFlatButtonPressedBackgroundBrush()
        {
            return (Theme == Themes.LIGHT ? FLAT_BUTTON_BACKGROUND_PRESSED_LIGHT_BRUSH : FLAT_BUTTON_BACKGROUND_PRESSED_DARK_BRUSH);
        }

        public Brush GetFlatButtonDisabledTextBrush()
        {
            return (Theme == Themes.LIGHT ? FLAT_BUTTON_DISABLEDTEXT_LIGHT_BRUSH : FLAT_BUTTON_DISABLEDTEXT_DARK_BRUSH);
        }

        public Brush GetCmsSelectedItemBrush()
        {
            return (Theme == Themes.LIGHT ? CMS_BACKGROUND_HOVER_LIGHT_BRUSH : CMS_BACKGROUND_HOVER_DARK_BRUSH);
        }

        public Color GetApplicationBackgroundColor()
        {
            return (Theme == Themes.LIGHT ? BACKGROUND_LIGHT : BACKGROUND_DARK);
        }

        //Arial font
        public Font ARIAL_MEDIUM_12;
        public Font ARIAL_REGULAR_11;
        public Font ARIAL_MEDIUM_11;
        public Font ARIAL_MEDIUM_10;

        //Other constants
        public int FORM_PADDING = 14;

        [DllImport("gdi32.dll")]
        private static extern IntPtr AddFontMemResourceEx(IntPtr pbFont, uint cbFont, IntPtr pvd, [In] ref uint pcFonts);

        private MaterialSkinManager()
        {
            ARIAL_MEDIUM_12 = new Font("Arial", 12f);
            ARIAL_MEDIUM_10 = new Font("Arial", 10f);
            ARIAL_REGULAR_11 = new Font("Arial", 11f);
            ARIAL_MEDIUM_11 = new Font("Arial", 11f);
            Theme = Themes.LIGHT;
            // This is the new color scheme.
            ColorScheme = Skins.skins(Skins.skinChoice);

            // This is the old color scheme.
            //ColorScheme = new ColorScheme(Primary.BlueGrey800, Primary.BlueGrey900, Primary.BlueGrey500, Accent.LightBlue200, TextShade.WHITE);
        }

        public static MaterialSkinManager Instance
        {
            get { return instance ?? (instance = new MaterialSkinManager()); }
        }

        public void AddFormToManage(MaterialForm materialForm)
        {
            formsToManage.Add(materialForm);
            UpdateBackgrounds();
        }

        public void AddFormToManage(Form form)
        {
            formsToManage.Add(form);
            UpdateBackgrounds();
        }

        public void RemoveFormToManage(MaterialForm materialForm)
        {
            formsToManage.Remove(materialForm);
        }

        public void RemoveFormToManage(Form form)
        {
            formsToManage.Remove(form);
        }

        private readonly PrivateFontCollection privateFontCollection = new PrivateFontCollection();

        private FontFamily LoadFont(byte[] fontResource)
        {
            int dataLength = fontResource.Length;
            IntPtr fontPtr = Marshal.AllocCoTaskMem(dataLength);
            Marshal.Copy(fontResource, 0, fontPtr, dataLength);

            uint cFonts = 0;
            AddFontMemResourceEx(fontPtr, (uint)fontResource.Length, IntPtr.Zero, ref cFonts);
            privateFontCollection.AddMemoryFont(fontPtr, dataLength);

            return privateFontCollection.Families.Last();
        }

        private void UpdateBackgrounds()
        {
            var newBackColor = GetApplicationBackgroundColor();
            foreach (var materialForm in formsToManage)
            {
                materialForm.BackColor = newBackColor;
                UpdateControl(materialForm, newBackColor);
            }
        }

        private void UpdateToolStrip(ToolStrip toolStrip, Color newBackColor)
        {
            if (toolStrip == null) return;

            toolStrip.BackColor = newBackColor;
            foreach (ToolStripItem control in toolStrip.Items)
            {
                control.BackColor = newBackColor;
                if (control is MaterialToolStripMenuItem && (control as MaterialToolStripMenuItem).HasDropDown)
                {

                    //recursive call
                    UpdateToolStrip((control as MaterialToolStripMenuItem).DropDown, newBackColor);
                }
            }
        }

        private void UpdateControl(Control controlToUpdate, Color newBackColor)
        {
            if (controlToUpdate == null) return;

            if (controlToUpdate.ContextMenuStrip != null)
            {
                UpdateToolStrip(controlToUpdate.ContextMenuStrip, newBackColor);
            }
            var tabControl = controlToUpdate as MaterialTabControl;
            if (tabControl != null)
            {
                foreach (TabPage tabPage in tabControl.TabPages)
                {
                    tabPage.BackColor = newBackColor;
                }
            }

            if (controlToUpdate is MaterialDivider)
            {
                controlToUpdate.BackColor = GetDividersColor();
            }

            if (controlToUpdate is MaterialListView)
            {
                controlToUpdate.BackColor = newBackColor;

            }

            //recursive call
            foreach (Control control in controlToUpdate.Controls)
            {
                UpdateControl(control, newBackColor);
            }

            controlToUpdate.Invalidate();
        }
    }

    #endregion
    #region IMaterialControl

    interface IMaterialControl
    {
        int Depth { get; set; }
        MaterialSkinManager SkinManager { get; }
        MouseState MouseState { get; set; }

    }

    public enum MouseState
    {
        HOVER,
        DOWN,
        OUT
    }

    #endregion
    #region DrawHelper

    static class DrawHelper
    {
        public static GraphicsPath CreateRoundRect(float x, float y, float width, float height, float radius)
        {
            GraphicsPath gp = new GraphicsPath();
            gp.AddLine(x + radius, y, x + width - (radius * 2), y);
            gp.AddArc(x + width - (radius * 2), y, radius * 2, radius * 2, 270, 90);
            gp.AddLine(x + width, y + radius, x + width, y + height - (radius * 2));
            gp.AddArc(x + width - (radius * 2), y + height - (radius * 2), radius * 2, radius * 2, 0, 90);
            gp.AddLine(x + width - (radius * 2), y + height, x + radius, y + height);
            gp.AddArc(x, y + height - (radius * 2), radius * 2, radius * 2, 90, 90);
            gp.AddLine(x, y + height - (radius * 2), x, y + radius);
            gp.AddArc(x, y, radius * 2, radius * 2, 180, 90);
            gp.CloseFigure();
            return gp;
        }

        public static GraphicsPath CreateRoundRect(Rectangle rect, float radius)
        {
            return CreateRoundRect(rect.X, rect.Y, rect.Width, rect.Height, radius);
        }

        public static Color BlendColor(Color backgroundColor, Color frontColor, double blend)
        {
            double ratio = blend / 255d;
            double invRatio = 1d - ratio;
            int r = (int)((backgroundColor.R * invRatio) + (frontColor.R * ratio));
            int g = (int)((backgroundColor.G * invRatio) + (frontColor.G * ratio));
            int b = (int)((backgroundColor.B * invRatio) + (frontColor.B * ratio));
            return Color.FromArgb(r, g, b);
        }

        public static Color BlendColor(Color backgroundColor, Color frontColor)
        {
            return BlendColor(backgroundColor, frontColor, frontColor.A);
        }
    }

    #endregion
    #region ColorScheme

    public class ColorScheme
    {
        public readonly Color PrimaryColor, SecondaryColor, AccentColor, TextColor;
        public readonly Pen PrimaryPen, SecondaryPen, AccentPen, TextPen;
        public readonly Brush PrimaryBrush, SecondaryBrush, AccentBrush, TextBrush;

        /// <summary>
        /// Defines the Color Scheme to be used for all forms.
        /// </summary>
        /// <param name="primary">The primary color, a -500 color is suggested here.</param>
        /// <param name="secondary">The secondary color for the tab selection indicator.</param>
        /// <param name="accent">The accent color, a -200 color is suggested here.</param>
        /// <param name="textShade">The text color, the one with the highest contrast is suggested.</param>
        public ColorScheme(Primary primary, Primary secondary, Accent accent, TextShade textShade)
        {
            //Color
            PrimaryColor = ((int)primary).ToColor();
            SecondaryColor = ((int)secondary).ToColor();
            AccentColor = ((int)accent).ToColor();
            TextColor = ((int)textShade).ToColor();

            //Pen
            PrimaryPen = new Pen(PrimaryColor);
            SecondaryPen = new Pen(SecondaryColor);
            AccentPen = new Pen(AccentColor);
            TextPen = new Pen(TextColor);

            //Brush
            PrimaryBrush = new SolidBrush(PrimaryColor);
            SecondaryBrush = new SolidBrush(SecondaryColor);
            AccentBrush = new SolidBrush(AccentColor);
            TextBrush = new SolidBrush(TextColor);
        }
    }

    public static class ColorExtension
    {
        /// <summary>
        /// Convert an integer number to a Color.
        /// </summary>
        /// <returns></returns>
        public static Color ToColor(this int argb)
        {
            return Color.FromArgb(
                (argb & 0xff0000) >> 16,
                (argb & 0xff00) >> 8,
                 argb & 0xff);
        }

        /// <summary>
        /// Removes the alpha component of a color.
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static Color RemoveAlpha(this Color color)
        {
            return Color.FromArgb(color.R, color.G, color.B);
        }

        /// <summary>
        /// Converts a 0-100 integer to a 0-255 color component.
        /// </summary>
        /// <param name="percentage"></param>
        /// <returns></returns>
        public static int PercentageToColorComponent(this int percentage)
        {
            return (int)((percentage / 100d) * 255d);
        }
    }

    //Color constantes
    public enum TextShade
    {
        WHITE = 0xFFFFFF,
        BLACK = 0x212121
    }

    public enum Primary
    {
        // Pastel 1 colors.
        PASTEL_STRAWBERRY = 0xFFB2B2,
        PASTEL_PEACH = 0xFFD0B5,
        PASTEL_CREAM = 0xFFEBBA,
        PASTEL_BANANA = 0xFFFACC,
        PASTEL_SEAFOAM = 0x6DC9C9,
        PASTEL_MINT = 0xA5D8D8,

        // Pastel 2 colors.
        PASTEL_GRAPEFRUIT = 0xdb7f8e,
        PASTEL_PLUM = 0xb27392,
        PASTEL_GRAPE = 0x786895,
        PASTEL_VIOLET = 0x465387,
        PASTEL_ORCHID = 0x202b58,

        // Pastel 3 colors.
        PASTEL_SUNSHINE = 0xffe577,
        PASTEL_GOLD = 0xfec051,
        PASTEL_ORANGE = 0xff8a67,
        PASTEL_BLOODORANGE = 0xfd6051,
        PASTEL_CRANBERRY = 0x392033,

        // All other colors.
        PRed50 = 0xFFEBEE,
        PRed100 = 0xFFCDD2,
        PRed200 = 0xEF9A9A,
        PRed300 = 0xE57373,
        PRed400 = 0xEF5350,
        PRed500 = 0xF44336,
        PRed600 = 0xE53935,
        PRed700 = 0xD32F2F,
        PRed800 = 0xC62828,
        PRed900 = 0xB71C1C,
        PPink50 = 0xFCE4EC,
        PPink100 = 0xF8BBD0,
        PPink200 = 0xF48FB1,
        PPink300 = 0xF06292,
        PPink400 = 0xEC407A,
        PPink500 = 0xE91E63,
        PPink600 = 0xD81B60,
        PPink700 = 0xC2185B,
        PPink800 = 0xAD1457,
        PPink900 = 0x880E4F,
        PPurple50 = 0xF3E5F5,
        PPurple100 = 0xE1BEE7,
        PPurple200 = 0xCE93D8,
        PPurple300 = 0xBA68C8,
        PPurple400 = 0xAB47BC,
        PPurple500 = 0x9C27B0,
        PPurple600 = 0x8E24AA,
        PPurple700 = 0x7B1FA2,
        PPurple800 = 0x6A1B9A,
        PPurple900 = 0x4A148C,
        PDeepPurple50 = 0xEDE7F6,
        PDeepPurple100 = 0xD1C4E9,
        PDeepPurple200 = 0xB39DDB,
        PDeepPurple300 = 0x9575CD,
        PDeepPurple400 = 0x7E57C2,
        PDeepPurple500 = 0x673AB7,
        PDeepPurple600 = 0x5E35B1,
        PDeepPurple700 = 0x512DA8,
        PDeepPurple800 = 0x4527A0,
        PDeepPurple900 = 0x311B92,
        PIndigo50 = 0xE8EAF6,
        PIndigo100 = 0xC5CAE9,
        PIndigo200 = 0x9FA8DA,
        PIndigo300 = 0x7986CB,
        PIndigo400 = 0x5C6BC0,
        PIndigo500 = 0x3F51B5,
        PIndigo600 = 0x3949AB,
        PIndigo700 = 0x303F9F,
        PIndigo800 = 0x283593,
        PIndigo900 = 0x1A237E,
        PBlue50 = 0xE3F2FD,
        PBlue100 = 0xBBDEFB,
        PBlue200 = 0x90CAF9,
        PBlue300 = 0x64B5F6,
        PBlue400 = 0x42A5F5,
        PBlue500 = 0x2196F3,
        PBlue600 = 0x1E88E5,
        PBlue700 = 0x1976D2,
        PBlue800 = 0x1565C0,
        PBlue900 = 0x0D47A1,
        PLightBlue50 = 0xE1F5FE,
        PLightBlue100 = 0xB3E5FC,
        PLightBlue200 = 0x81D4FA,
        PLightBlue300 = 0x4FC3F7,
        PLightBlue400 = 0x29B6F6,
        PLightBlue500 = 0x03A9F4,
        PLightBlue600 = 0x039BE5,
        PLightBlue700 = 0x0288D1,
        PLightBlue800 = 0x0277BD,
        PLightBlue900 = 0x01579B,
        PCyan50 = 0xE0F7FA,
        PCyan100 = 0xB2EBF2,
        PCyan200 = 0x80DEEA,
        PCyan300 = 0x4DD0E1,
        PCyan400 = 0x26C6DA,
        PCyan500 = 0x00BCD4,
        PCyan600 = 0x00ACC1,
        PCyan700 = 0x0097A7,
        PCyan800 = 0x00838F,
        PCyan900 = 0x006064,
        PTeal50 = 0xE0F2F1,
        PTeal100 = 0xB2DFDB,
        PTeal200 = 0x80CBC4,
        PTeal300 = 0x4DB6AC,
        PTeal400 = 0x26A69A,
        PTeal500 = 0x009688,
        PTeal600 = 0x00897B,
        PTeal700 = 0x00796B,
        PTeal800 = 0x00695C,
        PTeal900 = 0x004D40,
        PGreen50 = 0xE8F5E9,
        PGreen100 = 0xC8E6C9,
        PGreen200 = 0xA5D6A7,
        PGreen300 = 0x81C784,
        PGreen400 = 0x66BB6A,
        PGreen500 = 0x4CAF50,
        PGreen600 = 0x43A047,
        PGreen700 = 0x388E3C,
        PGreen800 = 0x2E7D32,
        PGreen900 = 0x1B5E20,
        PLightGreen50 = 0xF1F8E9,
        PLightGreen100 = 0xDCEDC8,
        PLightGreen200 = 0xC5E1A5,
        PLightGreen300 = 0xAED581,
        PLightGreen400 = 0x9CCC65,
        PLightGreen500 = 0x8BC34A,
        PLightGreen600 = 0x7CB342,
        PLightGreen700 = 0x689F38,
        PLightGreen800 = 0x558B2F,
        PLightGreen900 = 0x33691E,
        PLime50 = 0xF9FBE7,
        PLime100 = 0xF0F4C3,
        PLime200 = 0xE6EE9C,
        PLime300 = 0xDCE775,
        PLime400 = 0xD4E157,
        PLime500 = 0xCDDC39,
        PLime600 = 0xC0CA33,
        PLime700 = 0xAFB42B,
        PLime800 = 0x9E9D24,
        PLime900 = 0x827717,
        PYellow50 = 0xFFFDE7,
        PYellow100 = 0xFFF9C4,
        PYellow200 = 0xFFF59D,
        PYellow300 = 0xFFF176,
        PYellow400 = 0xFFEE58,
        PYellow500 = 0xFFEB3B,
        PYellow600 = 0xFDD835,
        PYellow700 = 0xFBC02D,
        PYellow800 = 0xF9A825,
        PYellow900 = 0xF57F17,
        PAmber50 = 0xFFF8E1,
        PAmber100 = 0xFFECB3,
        PAmber200 = 0xFFE082,
        PAmber300 = 0xFFD54F,
        PAmber400 = 0xFFCA28,
        PAmber500 = 0xFFC107,
        PAmber600 = 0xFFB300,
        PAmber700 = 0xFFA000,
        PAmber800 = 0xFF8F00,
        PAmber900 = 0xFF6F00,
        POrange50 = 0xFFF3E0,
        POrange100 = 0xFFE0B2,
        POrange200 = 0xFFCC80,
        POrange300 = 0xFFB74D,
        POrange400 = 0xFFA726,
        POrange500 = 0xFF9800,
        POrange600 = 0xFB8C00,
        POrange700 = 0xF57C00,
        POrange800 = 0xEF6C00,
        POrange900 = 0xE65100,
        PDeepOrange50 = 0xFBE9E7,
        PDeepOrange100 = 0xFFCCBC,
        PDeepOrange200 = 0xFFAB91,
        PDeepOrange300 = 0xFF8A65,
        PDeepOrange400 = 0xFF7043,
        PDeepOrange500 = 0xFF5722,
        PDeepOrange600 = 0xF4511E,
        PDeepOrange700 = 0xE64A19,
        PDeepOrange800 = 0xD84315,
        PDeepOrange900 = 0xBF360C,
        PBrown50 = 0xEFEBE9,
        PBrown100 = 0xD7CCC8,
        PBrown200 = 0xBCAAA4,
        PBrown300 = 0xA1887F,
        PBrown400 = 0x8D6E63,
        PBrown500 = 0x795548,
        PBrown600 = 0x6D4C41,
        PBrown700 = 0x5D4037,
        PBrown800 = 0x4E342E,
        PBrown900 = 0x3E2723,
        PGrey50 = 0xFAFAFA,
        PGrey100 = 0xF5F5F5,
        PGrey200 = 0xEEEEEE,
        PGrey300 = 0xE0E0E0,
        PGrey400 = 0xBDBDBD,
        PGrey500 = 0x9E9E9E,
        PGrey600 = 0x757575,
        PGrey700 = 0x616161,
        PGrey800 = 0x424242,
        PGrey900 = 0x212121,
        PBlueGrey50 = 0xECEFF1,
        PBlueGrey100 = 0xCFD8DC,
        PBlueGrey200 = 0xB0BEC5,
        PBlueGrey300 = 0x90A4AE,
        PBlueGrey400 = 0x78909C,
        PBlueGrey500 = 0x607D8B,
        PBlueGrey600 = 0x546E7A,
        PBlueGrey700 = 0x455A64,
        PBlueGrey800 = 0x37474F,
        PBlueGrey900 = 0x263238,

        ARed100 = 0xFF8A80,
        ARed200 = 0xFF5252,
        ARed400 = 0xFF1744,
        ARed700 = 0xD50000,
        APink100 = 0xFF80AB,
        APink200 = 0xFF4081,
        APink400 = 0xF50057,
        APink700 = 0xC51162,
        APurple100 = 0xEA80FC,
        APurple200 = 0xE040FB,
        APurple400 = 0xD500F9,
        APurple700 = 0xAA00FF,
        ADeepPurple100 = 0xB388FF,
        ADeepPurple200 = 0x7C4DFF,
        ADeepPurple400 = 0x651FFF,
        ADeepPurple700 = 0x6200EA,
        AIndigo100 = 0x8C9EFF,
        AIndigo200 = 0x536DFE,
        AIndigo400 = 0x3D5AFE,
        AIndigo700 = 0x304FFE,
        ABlue100 = 0x82B1FF,
        ABlue200 = 0x448AFF,
        ABlue400 = 0x2979FF,
        ABlue700 = 0x2962FF,
        ALightBlue100 = 0x80D8FF,
        ALightBlue200 = 0x40C4FF,
        ALightBlue400 = 0x00B0FF,
        ALightBlue700 = 0x0091EA,
        ACyan100 = 0x84FFFF,
        ACyan200 = 0x18FFFF,
        ACyan400 = 0x00E5FF,
        ACyan700 = 0x00B8D4,
        ATeal100 = 0xA7FFEB,
        ATeal200 = 0x64FFDA,
        ATeal400 = 0x1DE9B6,
        ATeal700 = 0x00BFA5,
        AGreen100 = 0xB9F6CA,
        AGreen200 = 0x69F0AE,
        AGreen400 = 0x00E676,
        AGreen700 = 0x00C853,
        ALightGreen100 = 0xCCFF90,
        ALightGreen200 = 0xB2FF59,
        ALightGreen400 = 0x76FF03,
        ALightGreen700 = 0x64DD17,
        ALime100 = 0xF4FF81,
        ALime200 = 0xEEFF41,
        ALime400 = 0xC6FF00,
        ALime700 = 0xAEEA00,
        AYellow100 = 0xFFFF8D,
        AYellow200 = 0xFFFF00,
        AYellow400 = 0xFFEA00,
        AYellow700 = 0xFFD600,
        AAmber100 = 0xFFE57F,
        AAmber200 = 0xFFD740,
        AAmber400 = 0xFFC400,
        AAmber700 = 0xFFAB00,
        AOrange100 = 0xFFD180,
        AOrange200 = 0xFFAB40,
        AOrange400 = 0xFF9100,
        AOrange700 = 0xFF6D00,
        ADeepOrange100 = 0xFF9E80,
        ADeepOrange200 = 0xFF6E40,
        ADeepOrange400 = 0xFF3D00,
        ADeepOrange700 = 0xDD2C00,
        White = 0xFFFFFF,
        Default = 0x2892ff
    }

    public enum Accent
    {
        // Pastel 1 colors.
        PASTEL_STRAWBERRY = 0xFFB2B2,
        PASTEL_PEACH = 0xFFD0B5,
        PASTEL_CREAM = 0xFFEBBA,
        PASTEL_BANANA = 0xFFFACC,
        PASTEL_SEAFOAM = 0x6DC9C9,
        PASTEL_MINT = 0xA5D8D8,

        // Pastel 2 colors.
        PASTEL_GRAPEFRUIT = 0xdb7f8e,
        PASTEL_PLUM = 0xb27392,
        PASTEL_GRAPE = 0x786895,
        PASTEL_VIOLET = 0x465387,
        PASTEL_ORCHID = 0x202b58,

        // Pastel 3 colors.
        PASTEL_SUNSHINE = 0xffe577,
        PASTEL_GOLD = 0xfec051,
        PASTEL_ORANGE = 0xff8a67,
        PASTEL_BLOODORANGE = 0xfd6051,
        PASTEL_CRANBERRY = 0x392033,

        // All other colors.
        PRed50 = 0xFFEBEE,
        PRed100 = 0xFFCDD2,
        PRed200 = 0xEF9A9A,
        PRed300 = 0xE57373,
        PRed400 = 0xEF5350,
        PRed500 = 0xF44336,
        PRed600 = 0xE53935,
        PRed700 = 0xD32F2F,
        PRed800 = 0xC62828,
        PRed900 = 0xB71C1C,
        PPink50 = 0xFCE4EC,
        PPink100 = 0xF8BBD0,
        PPink200 = 0xF48FB1,
        PPink300 = 0xF06292,
        PPink400 = 0xEC407A,
        PPink500 = 0xE91E63,
        PPink600 = 0xD81B60,
        PPink700 = 0xC2185B,
        PPink800 = 0xAD1457,
        PPink900 = 0x880E4F,
        PPurple50 = 0xF3E5F5,
        PPurple100 = 0xE1BEE7,
        PPurple200 = 0xCE93D8,
        PPurple300 = 0xBA68C8,
        PPurple400 = 0xAB47BC,
        PPurple500 = 0x9C27B0,
        PPurple600 = 0x8E24AA,
        PPurple700 = 0x7B1FA2,
        PPurple800 = 0x6A1B9A,
        PPurple900 = 0x4A148C,
        PDeepPurple50 = 0xEDE7F6,
        PDeepPurple100 = 0xD1C4E9,
        PDeepPurple200 = 0xB39DDB,
        PDeepPurple300 = 0x9575CD,
        PDeepPurple400 = 0x7E57C2,
        PDeepPurple500 = 0x673AB7,
        PDeepPurple600 = 0x5E35B1,
        PDeepPurple700 = 0x512DA8,
        PDeepPurple800 = 0x4527A0,
        PDeepPurple900 = 0x311B92,
        PIndigo50 = 0xE8EAF6,
        PIndigo100 = 0xC5CAE9,
        PIndigo200 = 0x9FA8DA,
        PIndigo300 = 0x7986CB,
        PIndigo400 = 0x5C6BC0,
        PIndigo500 = 0x3F51B5,
        PIndigo600 = 0x3949AB,
        PIndigo700 = 0x303F9F,
        PIndigo800 = 0x283593,
        PIndigo900 = 0x1A237E,
        PBlue50 = 0xE3F2FD,
        PBlue100 = 0xBBDEFB,
        PBlue200 = 0x90CAF9,
        PBlue300 = 0x64B5F6,
        PBlue400 = 0x42A5F5,
        PBlue500 = 0x2196F3,
        PBlue600 = 0x1E88E5,
        PBlue700 = 0x1976D2,
        PBlue800 = 0x1565C0,
        PBlue900 = 0x0D47A1,
        PLightBlue50 = 0xE1F5FE,
        PLightBlue100 = 0xB3E5FC,
        PLightBlue200 = 0x81D4FA,
        PLightBlue300 = 0x4FC3F7,
        PLightBlue400 = 0x29B6F6,
        PLightBlue500 = 0x03A9F4,
        PLightBlue600 = 0x039BE5,
        PLightBlue700 = 0x0288D1,
        PLightBlue800 = 0x0277BD,
        PLightBlue900 = 0x01579B,
        PCyan50 = 0xE0F7FA,
        PCyan100 = 0xB2EBF2,
        PCyan200 = 0x80DEEA,
        PCyan300 = 0x4DD0E1,
        PCyan400 = 0x26C6DA,
        PCyan500 = 0x00BCD4,
        PCyan600 = 0x00ACC1,
        PCyan700 = 0x0097A7,
        PCyan800 = 0x00838F,
        PCyan900 = 0x006064,
        PTeal50 = 0xE0F2F1,
        PTeal100 = 0xB2DFDB,
        PTeal200 = 0x80CBC4,
        PTeal300 = 0x4DB6AC,
        PTeal400 = 0x26A69A,
        PTeal500 = 0x009688,
        PTeal600 = 0x00897B,
        PTeal700 = 0x00796B,
        PTeal800 = 0x00695C,
        PTeal900 = 0x004D40,
        PGreen50 = 0xE8F5E9,
        PGreen100 = 0xC8E6C9,
        PGreen200 = 0xA5D6A7,
        PGreen300 = 0x81C784,
        PGreen400 = 0x66BB6A,
        PGreen500 = 0x4CAF50,
        PGreen600 = 0x43A047,
        PGreen700 = 0x388E3C,
        PGreen800 = 0x2E7D32,
        PGreen900 = 0x1B5E20,
        PLightGreen50 = 0xF1F8E9,
        PLightGreen100 = 0xDCEDC8,
        PLightGreen200 = 0xC5E1A5,
        PLightGreen300 = 0xAED581,
        PLightGreen400 = 0x9CCC65,
        PLightGreen500 = 0x8BC34A,
        PLightGreen600 = 0x7CB342,
        PLightGreen700 = 0x689F38,
        PLightGreen800 = 0x558B2F,
        PLightGreen900 = 0x33691E,
        PLime50 = 0xF9FBE7,
        PLime100 = 0xF0F4C3,
        PLime200 = 0xE6EE9C,
        PLime300 = 0xDCE775,
        PLime400 = 0xD4E157,
        PLime500 = 0xCDDC39,
        PLime600 = 0xC0CA33,
        PLime700 = 0xAFB42B,
        PLime800 = 0x9E9D24,
        PLime900 = 0x827717,
        PYellow50 = 0xFFFDE7,
        PYellow100 = 0xFFF9C4,
        PYellow200 = 0xFFF59D,
        PYellow300 = 0xFFF176,
        PYellow400 = 0xFFEE58,
        PYellow500 = 0xFFEB3B,
        PYellow600 = 0xFDD835,
        PYellow700 = 0xFBC02D,
        PYellow800 = 0xF9A825,
        PYellow900 = 0xF57F17,
        PAmber50 = 0xFFF8E1,
        PAmber100 = 0xFFECB3,
        PAmber200 = 0xFFE082,
        PAmber300 = 0xFFD54F,
        PAmber400 = 0xFFCA28,
        PAmber500 = 0xFFC107,
        PAmber600 = 0xFFB300,
        PAmber700 = 0xFFA000,
        PAmber800 = 0xFF8F00,
        PAmber900 = 0xFF6F00,
        POrange50 = 0xFFF3E0,
        POrange100 = 0xFFE0B2,
        POrange200 = 0xFFCC80,
        POrange300 = 0xFFB74D,
        POrange400 = 0xFFA726,
        POrange500 = 0xFF9800,
        POrange600 = 0xFB8C00,
        POrange700 = 0xF57C00,
        POrange800 = 0xEF6C00,
        POrange900 = 0xE65100,
        PDeepOrange50 = 0xFBE9E7,
        PDeepOrange100 = 0xFFCCBC,
        PDeepOrange200 = 0xFFAB91,
        PDeepOrange300 = 0xFF8A65,
        PDeepOrange400 = 0xFF7043,
        PDeepOrange500 = 0xFF5722,
        PDeepOrange600 = 0xF4511E,
        PDeepOrange700 = 0xE64A19,
        PDeepOrange800 = 0xD84315,
        PDeepOrange900 = 0xBF360C,
        PBrown50 = 0xEFEBE9,
        PBrown100 = 0xD7CCC8,
        PBrown200 = 0xBCAAA4,
        PBrown300 = 0xA1887F,
        PBrown400 = 0x8D6E63,
        PBrown500 = 0x795548,
        PBrown600 = 0x6D4C41,
        PBrown700 = 0x5D4037,
        PBrown800 = 0x4E342E,
        PBrown900 = 0x3E2723,
        PGrey50 = 0xFAFAFA,
        PGrey100 = 0xF5F5F5,
        PGrey200 = 0xEEEEEE,
        PGrey300 = 0xE0E0E0,
        PGrey400 = 0xBDBDBD,
        PGrey500 = 0x9E9E9E,
        PGrey600 = 0x757575,
        PGrey700 = 0x616161,
        PGrey800 = 0x424242,
        PGrey900 = 0x212121,
        PBlueGrey50 = 0xECEFF1,
        PBlueGrey100 = 0xCFD8DC,
        PBlueGrey200 = 0xB0BEC5,
        PBlueGrey300 = 0x90A4AE,
        PBlueGrey400 = 0x78909C,
        PBlueGrey500 = 0x607D8B,
        PBlueGrey600 = 0x546E7A,
        PBlueGrey700 = 0x455A64,
        PBlueGrey800 = 0x37474F,
        PBlueGrey900 = 0x263238,

        ARed100 = 0xFF8A80,
        ARed200 = 0xFF5252,
        ARed400 = 0xFF1744,
        ARed700 = 0xD50000,
        APink100 = 0xFF80AB,
        APink200 = 0xFF4081,
        APink400 = 0xF50057,
        APink700 = 0xC51162,
        APurple100 = 0xEA80FC,
        APurple200 = 0xE040FB,
        APurple400 = 0xD500F9,
        APurple700 = 0xAA00FF,
        ADeepPurple100 = 0xB388FF,
        ADeepPurple200 = 0x7C4DFF,
        ADeepPurple400 = 0x651FFF,
        ADeepPurple700 = 0x6200EA,
        AIndigo100 = 0x8C9EFF,
        AIndigo200 = 0x536DFE,
        AIndigo400 = 0x3D5AFE,
        AIndigo700 = 0x304FFE,
        ABlue100 = 0x82B1FF,
        ABlue200 = 0x448AFF,
        ABlue400 = 0x2979FF,
        ABlue700 = 0x2962FF,
        ALightBlue100 = 0x80D8FF,
        ALightBlue200 = 0x40C4FF,
        ALightBlue400 = 0x00B0FF,
        ALightBlue700 = 0x0091EA,
        ACyan100 = 0x84FFFF,
        ACyan200 = 0x18FFFF,
        ACyan400 = 0x00E5FF,
        ACyan700 = 0x00B8D4,
        ATeal100 = 0xA7FFEB,
        ATeal200 = 0x64FFDA,
        ATeal400 = 0x1DE9B6,
        ATeal700 = 0x00BFA5,
        AGreen100 = 0xB9F6CA,
        AGreen200 = 0x69F0AE,
        AGreen400 = 0x00E676,
        AGreen700 = 0x00C853,
        ALightGreen100 = 0xCCFF90,
        ALightGreen200 = 0xB2FF59,
        ALightGreen400 = 0x76FF03,
        ALightGreen700 = 0x64DD17,
        ALime100 = 0xF4FF81,
        ALime200 = 0xEEFF41,
        ALime400 = 0xC6FF00,
        ALime700 = 0xAEEA00,
        AYellow100 = 0xFFFF8D,
        AYellow200 = 0xFFFF00,
        AYellow400 = 0xFFEA00,
        AYellow700 = 0xFFD600,
        AAmber100 = 0xFFE57F,
        AAmber200 = 0xFFD740,
        AAmber400 = 0xFFC400,
        AAmber700 = 0xFFAB00,
        AOrange100 = 0xFFD180,
        AOrange200 = 0xFFAB40,
        AOrange400 = 0xFF9100,
        AOrange700 = 0xFF6D00,
        ADeepOrange100 = 0xFF9E80,
        ADeepOrange200 = 0xFF6E40,
        ADeepOrange400 = 0xFF3D00,
        ADeepOrange700 = 0xDD2C00,
        White = 0xFFFFFF,
        Default = 0x2892ff
    }

    #endregion
    #region Skins

    public static class Skins
    {
        public static int skinChoice { get; set; }

        public static ColorScheme skins(int choice)
        {
            ColorScheme skin = new ColorScheme(Primary.PBlueGrey800, Primary.PBlueGrey400, Accent.Default, TextShade.WHITE); // The original colors.
            switch (choice)
            {
                case 1: // Default
                    skin = new ColorScheme(Primary.PBlueGrey800, Primary.Default, Accent.Default, TextShade.WHITE);
                    break;
                case 2: // Red
                    skin = new ColorScheme(Primary.PRed800, Primary.PRed100, Accent.PRed400, TextShade.WHITE);
                    break;
                case 3: // Orange
                    skin = new ColorScheme(Primary.POrange800, Primary.POrange100, Accent.POrange400, TextShade.WHITE);
                    break;
                case 4: // Yellow
                    skin = new ColorScheme(Primary.PYellow800, Primary.PYellow100, Accent.PAmber300, TextShade.WHITE);
                    break;
                case 5: // Green
                    skin = new ColorScheme(Primary.PGreen800, Primary.PGreen100, Accent.PGreen400, TextShade.WHITE);
                    break;
                case 6: // Blue
                    skin = new ColorScheme(Primary.PBlue800, Primary.PBlue100, Accent.PBlue400, TextShade.WHITE);
                    break;
                case 7: // Purple
                    skin = new ColorScheme(Primary.PPurple800, Primary.PPurple100, Accent.PPurple400, TextShade.WHITE);
                    break;
                case 8: // Space
                    skin = new ColorScheme(Primary.PGrey900, Primary.White, Accent.PGrey900, TextShade.WHITE);
                    break;
                case 9: // Earth
                    skin = new ColorScheme(Primary.PBrown800, Primary.PGreen400, Accent.PGreen400, TextShade.WHITE);
                    break;
                case 10: // Cotton Candy
                    skin = new ColorScheme(Primary.APink100, Primary.PPink50, Accent.APink100, TextShade.WHITE);
                    break;
                case 11: // Pastel 1
                    skin = new ColorScheme(Primary.PASTEL_SEAFOAM, Primary.PASTEL_MINT, Accent.PASTEL_SEAFOAM, TextShade.WHITE);
                    break;
                case 12: // Pastel 2
                    skin = new ColorScheme(Primary.PASTEL_PLUM, Primary.White, Accent.PASTEL_GRAPEFRUIT, TextShade.WHITE);
                    break;
                case 13: // Pastel 3
                    skin = new ColorScheme(Primary.PBlueGrey800, Primary.PASTEL_GOLD, Accent.PASTEL_BLOODORANGE, TextShade.WHITE);
                    break;
                default:
                    skin = new ColorScheme(Primary.PBlueGrey800, Primary.Default, Accent.Default, TextShade.WHITE);
                    break;
            }

            return skin;
        }
    }

    #endregion
}

namespace MaterialSkin.Animations
{
    #region AnimationDirection

    enum AnimationDirection
    {
        In, //In. Stops if finished.
        Out, //Out. Stops if finished.
        InOutIn, //Same as In, but changes to InOutOut if finished.
        InOutOut, //Same as Out.
        InOutRepeatingIn, // Same as In, but changes to InOutRepeatingOut if finished.
        InOutRepeatingOut // Same as Out, but changes to InOutRepeatingIn if finished.
    }

    #endregion
    #region AnimationManager

    class AnimationManager
    {
        public bool InterruptAnimation { get; set; }
        public double Increment { get; set; }
        public double SecondaryIncrement { get; set; }
        public AnimationType AnimationType { get; set; }
        public bool Singular { get; set; }

        public delegate void AnimationFinished(object sender);
        public event AnimationFinished OnAnimationFinished;

        public delegate void AnimationProgress(object sender);
        public event AnimationProgress OnAnimationProgress;

        private readonly List<double> animationProgresses;
        private readonly List<Point> animationSources;
        private readonly List<AnimationDirection> animationDirections;
        private readonly List<object[]> animationDatas;

        private const double MIN_VALUE = 0.00;
        private const double MAX_VALUE = 1.00;

        private readonly Timer animationTimer = new Timer { Interval = 5, Enabled = false };

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="singular">If true, only one animation is supported. The current animation will be replaced with the new one. If false, a new animation is added to the list.</param>
        public AnimationManager(bool singular = true)
        {
            animationProgresses = new List<double>();
            animationSources = new List<Point>();
            animationDirections = new List<AnimationDirection>();
            animationDatas = new List<object[]>();

            Increment = 0.03;
            SecondaryIncrement = 0.03;
            AnimationType = AnimationType.Linear;
            InterruptAnimation = true;
            Singular = singular;

            if (Singular)
            {
                animationProgresses.Add(0);
                animationSources.Add(new Point(0, 0));
                animationDirections.Add(AnimationDirection.In);
            }

            animationTimer.Tick += AnimationTimerOnTick;
        }

        private void AnimationTimerOnTick(object sender, EventArgs eventArgs)
        {
            for (int i = 0; i < animationProgresses.Count; i++)
            {
                UpdateProgress(i);

                if (!Singular)
                {
                    if ((animationDirections[i] == AnimationDirection.InOutIn && animationProgresses[i] == MAX_VALUE))
                    {
                        animationDirections[i] = AnimationDirection.InOutOut;
                    }
                    else if ((animationDirections[i] == AnimationDirection.InOutRepeatingIn && animationProgresses[i] == MIN_VALUE))
                    {
                        animationDirections[i] = AnimationDirection.InOutRepeatingOut;
                    }
                    else if ((animationDirections[i] == AnimationDirection.InOutRepeatingOut && animationProgresses[i] == MIN_VALUE))
                    {
                        animationDirections[i] = AnimationDirection.InOutRepeatingIn;
                    }
                    else if (
                        (animationDirections[i] == AnimationDirection.In && animationProgresses[i] == MAX_VALUE) ||
                        (animationDirections[i] == AnimationDirection.Out && animationProgresses[i] == MIN_VALUE) ||
                        (animationDirections[i] == AnimationDirection.InOutOut && animationProgresses[i] == MIN_VALUE))
                    {
                        animationProgresses.RemoveAt(i);
                        animationSources.RemoveAt(i);
                        animationDirections.RemoveAt(i);
                        animationDatas.RemoveAt(i);
                    }
                }
                else
                {
                    if ((animationDirections[i] == AnimationDirection.InOutIn && animationProgresses[i] == MAX_VALUE))
                    {
                        animationDirections[i] = AnimationDirection.InOutOut;
                    }
                    else if ((animationDirections[i] == AnimationDirection.InOutRepeatingIn && animationProgresses[i] == MAX_VALUE))
                    {
                        animationDirections[i] = AnimationDirection.InOutRepeatingOut;
                    }
                    else if ((animationDirections[i] == AnimationDirection.InOutRepeatingOut && animationProgresses[i] == MIN_VALUE))
                    {
                        animationDirections[i] = AnimationDirection.InOutRepeatingIn;
                    }
                }
            }

            if (OnAnimationProgress != null)
            {
                OnAnimationProgress(this);
            }
        }

        public bool IsAnimating()
        {
            return animationTimer.Enabled;
        }

        public void StartNewAnimation(AnimationDirection animationDirection, object[] data = null)
        {
            StartNewAnimation(animationDirection, new Point(0, 0), data);
        }

        public void StartNewAnimation(AnimationDirection animationDirection, Point animationSource, object[] data = null)
        {
            if (!IsAnimating() || InterruptAnimation)
            {
                if (Singular && animationDirections.Count > 0)
                {
                    animationDirections[0] = animationDirection;
                }
                else
                {
                    animationDirections.Add(animationDirection);
                }

                if (Singular && animationSources.Count > 0)
                {
                    animationSources[0] = animationSource;
                }
                else
                {
                    animationSources.Add(animationSource);
                }

                if (!(Singular && animationProgresses.Count > 0))
                {
                    switch (animationDirections[animationDirections.Count - 1])
                    {
                        case AnimationDirection.InOutRepeatingIn:
                        case AnimationDirection.InOutIn:
                        case AnimationDirection.In:
                            animationProgresses.Add(MIN_VALUE);
                            break;
                        case AnimationDirection.InOutRepeatingOut:
                        case AnimationDirection.InOutOut:
                        case AnimationDirection.Out:
                            animationProgresses.Add(MAX_VALUE);
                            break;
                        default:
                            throw new Exception("Invalid AnimationDirection");
                    }
                }

                if (Singular && animationDatas.Count > 0)
                {
                    animationDatas[0] = data ?? new object[] { };
                }
                else
                {
                    animationDatas.Add(data ?? new object[] { });
                }

            }

            animationTimer.Start();
        }

        public void UpdateProgress(int index)
        {
            switch (animationDirections[index])
            {
                case AnimationDirection.InOutRepeatingIn:
                case AnimationDirection.InOutIn:
                case AnimationDirection.In:
                    IncrementProgress(index);
                    break;
                case AnimationDirection.InOutRepeatingOut:
                case AnimationDirection.InOutOut:
                case AnimationDirection.Out:
                    DecrementProgress(index);
                    break;
                default:
                    throw new Exception("No AnimationDirection has been set");
            }
        }

        private void IncrementProgress(int index)
        {
            animationProgresses[index] += Increment;
            if (animationProgresses[index] > MAX_VALUE)
            {
                animationProgresses[index] = MAX_VALUE;

                for (int i = 0; i < GetAnimationCount(); i++)
                {
                    if (animationDirections[i] == AnimationDirection.InOutIn) return;
                    if (animationDirections[i] == AnimationDirection.InOutRepeatingIn) return;
                    if (animationDirections[i] == AnimationDirection.InOutRepeatingOut) return;
                    if (animationDirections[i] == AnimationDirection.InOutOut && animationProgresses[i] != MAX_VALUE) return;
                    if (animationDirections[i] == AnimationDirection.In && animationProgresses[i] != MAX_VALUE) return;
                }

                animationTimer.Stop();
                if (OnAnimationFinished != null) OnAnimationFinished(this);
            }
        }

        private void DecrementProgress(int index)
        {
            animationProgresses[index] -= (animationDirections[index] == AnimationDirection.InOutOut || animationDirections[index] == AnimationDirection.InOutRepeatingOut) ? SecondaryIncrement : Increment;
            if (animationProgresses[index] < MIN_VALUE)
            {
                animationProgresses[index] = MIN_VALUE;

                for (int i = 0; i < GetAnimationCount(); i++)
                {
                    if (animationDirections[i] == AnimationDirection.InOutIn) return;
                    if (animationDirections[i] == AnimationDirection.InOutRepeatingIn) return;
                    if (animationDirections[i] == AnimationDirection.InOutRepeatingOut) return;
                    if (animationDirections[i] == AnimationDirection.InOutOut && animationProgresses[i] != MIN_VALUE) return;
                    if (animationDirections[i] == AnimationDirection.Out && animationProgresses[i] != MIN_VALUE) return;
                }

                animationTimer.Stop();
                if (OnAnimationFinished != null) OnAnimationFinished(this);
            }
        }

        public double GetProgress()
        {
            if (!Singular)
                throw new Exception("Animation is not set to Singular.");

            if (animationProgresses.Count == 0)
                throw new Exception("Invalid animation");

            return GetProgress(0);
        }

        public double GetProgress(int index)
        {
            if (!(index < GetAnimationCount()))
                throw new IndexOutOfRangeException("Invalid animation index");

            switch (AnimationType)
            {
                case AnimationType.Linear:
                    return AnimationLinear.CalculateProgress(animationProgresses[index]);
                case AnimationType.EaseInOut:
                    return AnimationEaseInOut.CalculateProgress(animationProgresses[index]);
                case AnimationType.EaseOut:
                    return AnimationEaseOut.CalculateProgress(animationProgresses[index]);
                case AnimationType.CustomQuadratic:
                    return AnimationCustomQuadratic.CalculateProgress(animationProgresses[index]);
                default:
                    throw new NotImplementedException("The given AnimationType is not implemented");
            }

        }

        public Point GetSource(int index)
        {
            if (!(index < GetAnimationCount()))
                throw new IndexOutOfRangeException("Invalid animation index");

            return animationSources[index];
        }

        public Point GetSource()
        {
            if (!Singular)
                throw new Exception("Animation is not set to Singular.");

            if (animationSources.Count == 0)
                throw new Exception("Invalid animation");

            return animationSources[0];
        }

        public AnimationDirection GetDirection()
        {
            if (!Singular)
                throw new Exception("Animation is not set to Singular.");

            if (animationDirections.Count == 0)
                throw new Exception("Invalid animation");

            return animationDirections[0];
        }

        public AnimationDirection GetDirection(int index)
        {
            if (!(index < animationDirections.Count))
                throw new IndexOutOfRangeException("Invalid animation index");

            return animationDirections[index];
        }

        public object[] GetData()
        {
            if (!Singular)
                throw new Exception("Animation is not set to Singular.");

            if (animationDatas.Count == 0)
                throw new Exception("Invalid animation");

            return animationDatas[0];
        }

        public object[] GetData(int index)
        {
            if (!(index < animationDatas.Count))
                throw new IndexOutOfRangeException("Invalid animation index");

            return animationDatas[index];
        }

        public int GetAnimationCount()
        {
            return animationProgresses.Count;
        }

        public void SetProgress(double progress)
        {
            if (!Singular)
                throw new Exception("Animation is not set to Singular.");

            if (animationProgresses.Count == 0)
                throw new Exception("Invalid animation");

            animationProgresses[0] = progress;
        }

        public void SetDirection(AnimationDirection direction)
        {
            if (!Singular)
                throw new Exception("Animation is not set to Singular.");

            if (animationProgresses.Count == 0)
                throw new Exception("Invalid animation");

            animationDirections[0] = direction;
        }

        public void SetData(object[] data)
        {
            if (!Singular)
                throw new Exception("Animation is not set to Singular.");

            if (animationDatas.Count == 0)
                throw new Exception("Invalid animation");

            animationDatas[0] = data;
        }
    }

    #endregion
    #region Animations

    enum AnimationType
    {
        Linear,
        EaseInOut,
        EaseOut,
        CustomQuadratic
    }

    static class AnimationLinear
    {
        public static double CalculateProgress(double progress)
        {
            return progress;
        }
    }

    static class AnimationEaseInOut
    {
        public static double PI = Math.PI;
        public static double PI_HALF = Math.PI / 2;

        public static double CalculateProgress(double progress)
        {
            return EaseInOut(progress);
        }

        private static double EaseInOut(double s)
        {
            return s - Math.Sin(s * 2 * PI) / (2 * PI);
        }
    }

    public static class AnimationEaseOut
    {
        public static double CalculateProgress(double progress)
        {
            return -1 * progress * (progress - 2);
        }
    }

    public static class AnimationCustomQuadratic
    {
        public static double CalculateProgress(double progress)
        {
            double kickoff = 0.6;
            return 1 - Math.Cos((Math.Max(progress, kickoff) - kickoff) * Math.PI / (2 - (2 * kickoff)));
        }
    }

    #endregion
}

namespace MaterialSkin.Controls
{
    #region MaterialCheckbox

    public class MaterialCheckBox : CheckBox, IMaterialControl
    {

        public ToolTip tip = new ToolTip();
        [Description("Specifies the text shown on the ToolTip.")]
        public string ToolTipText { get; set; }

        [Browsable(false)]
        public int Depth { get; set; }
        [Browsable(false)]
        public MaterialSkinManager SkinManager { get { return MaterialSkinManager.Instance; } }
        [Browsable(false)]
        public MouseState MouseState { get; set; }
        [Browsable(false)]
        public Point MouseLocation { get; set; }

        private bool ripple;
        [Category("Behavior")]
        public bool Ripple
        {
            get { return ripple; }
            set
            {
                ripple = value;
                AutoSize = AutoSize; //Make AutoSize directly set the bounds.

                if (value)
                {
                    Margin = new Padding(0);
                }

                Invalidate();
            }
        }

        private readonly AnimationManager animationManager;
        private readonly AnimationManager rippleAnimationManager;

        private const int CHECKBOX_SIZE = 18;
        private const int CHECKBOX_SIZE_HALF = CHECKBOX_SIZE / 2;
        private const int CHECKBOX_INNER_BOX_SIZE = CHECKBOX_SIZE - 4;

        private int boxOffset;
        private Rectangle boxRectangle;

        public MaterialCheckBox()
        {
            animationManager = new AnimationManager
            {
                AnimationType = AnimationType.EaseInOut,
                Increment = 0.05
            };

            rippleAnimationManager = new AnimationManager(false)
            {
                AnimationType = AnimationType.Linear,
                Increment = 0.10,
                SecondaryIncrement = 0.08
            };
            animationManager.OnAnimationProgress += sender => Invalidate();
            rippleAnimationManager.OnAnimationProgress += sender => Invalidate();

            CheckedChanged += (sender, args) =>
            {
                animationManager.StartNewAnimation(Checked ? AnimationDirection.In : AnimationDirection.Out);
            };

            Ripple = true;
            MouseLocation = new Point(-1, -1);
        }

        protected override void OnMouseHover(EventArgs e)
        {
            base.OnMouseHover(e);

            if (ToolTipText != "")
            {
                tip.Show(ToolTipText, this);
            }
        }
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);

            boxOffset = Height / 2 - 9;
            boxRectangle = new Rectangle(boxOffset, boxOffset, CHECKBOX_SIZE - 1, CHECKBOX_SIZE - 1);
        }

        public override Size GetPreferredSize(Size proposedSize)
        {
            int w = boxOffset + CHECKBOX_SIZE + 2 + (int)CreateGraphics().MeasureString(Text, SkinManager.ARIAL_MEDIUM_10).Width;
            return Ripple ? new Size(w, 30) : new Size(w, 20);
        }

        private static readonly Point[] CHECKMARK_LINE = { new Point(3, 8), new Point(7, 12), new Point(14, 5) };
        private const int TEXT_OFFSET = 22;
        protected override void OnPaint(PaintEventArgs pevent)
        {
            var g = pevent.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.AntiAlias;

            // clear the control
            g.Clear(Parent.BackColor);

            var CHECKBOX_CENTER = boxOffset + CHECKBOX_SIZE_HALF - 1;

            double animationProgress = animationManager.GetProgress();

            int colorAlpha = Enabled ? (int)(animationProgress * 255.0) : SkinManager.GetCheckBoxOffDisabledColor().A;
            int backgroundAlpha = Enabled ? (int)(SkinManager.GetCheckboxOffColor().A * (1.0 - animationProgress)) : SkinManager.GetCheckBoxOffDisabledColor().A;

            var brush = new SolidBrush(Color.FromArgb(colorAlpha, Enabled ? SkinManager.ColorScheme.AccentColor : SkinManager.GetCheckBoxOffDisabledColor()));
            var brush3 = new SolidBrush(Enabled ? SkinManager.ColorScheme.AccentColor : SkinManager.GetCheckBoxOffDisabledColor());
            var pen = new Pen(brush.Color);

            // draw ripple animation
            if (Ripple && rippleAnimationManager.IsAnimating())
            {
                for (int i = 0; i < rippleAnimationManager.GetAnimationCount(); i++)
                {
                    var animationValue = rippleAnimationManager.GetProgress(i);
                    var animationSource = new Point(CHECKBOX_CENTER, CHECKBOX_CENTER);
                    var rippleBrush = new SolidBrush(Color.FromArgb((int)((animationValue * 40)), ((bool)rippleAnimationManager.GetData(i)[0]) ? Color.Black : brush.Color));
                    var rippleHeight = (Height % 2 == 0) ? Height - 3 : Height - 2;
                    var rippleSize = (rippleAnimationManager.GetDirection(i) == AnimationDirection.InOutIn) ? (int)(rippleHeight * (0.8d + (0.2d * animationValue))) : rippleHeight;
                    using (var path = DrawHelper.CreateRoundRect(animationSource.X - rippleSize / 2, animationSource.Y - rippleSize / 2, rippleSize, rippleSize, rippleSize / 2))
                    {
                        g.FillPath(rippleBrush, path);
                    }

                    rippleBrush.Dispose();
                }
            }

            brush3.Dispose();

            var checkMarkLineFill = new Rectangle(boxOffset, boxOffset, (int)(17.0 * animationProgress), 17);
            using (var checkmarkPath = DrawHelper.CreateRoundRect(boxOffset, boxOffset, 17, 17, 1f))
            {
                SolidBrush brush2 = new SolidBrush(DrawHelper.BlendColor(Parent.BackColor, Enabled ? SkinManager.GetCheckboxOffColor() : SkinManager.GetCheckBoxOffDisabledColor(), backgroundAlpha));
                Pen pen2 = new Pen(brush2.Color);
                g.FillPath(brush2, checkmarkPath);
                g.DrawPath(pen2, checkmarkPath);

                g.FillRectangle(new SolidBrush(Parent.BackColor), boxOffset + 2, boxOffset + 2, CHECKBOX_INNER_BOX_SIZE - 1, CHECKBOX_INNER_BOX_SIZE - 1);
                g.DrawRectangle(new Pen(Parent.BackColor), boxOffset + 2, boxOffset + 2, CHECKBOX_INNER_BOX_SIZE - 1, CHECKBOX_INNER_BOX_SIZE - 1);

                brush2.Dispose();
                pen2.Dispose();

                if (Enabled)
                {
                    g.FillPath(brush, checkmarkPath);
                    g.DrawPath(pen, checkmarkPath);
                }
                else if (Checked)
                {
                    g.SmoothingMode = SmoothingMode.None;
                    g.FillRectangle(brush, boxOffset + 2, boxOffset + 2, CHECKBOX_INNER_BOX_SIZE, CHECKBOX_INNER_BOX_SIZE);
                    g.SmoothingMode = SmoothingMode.AntiAlias;
                }

                g.DrawImageUnscaledAndClipped(DrawCheckMarkBitmap(), checkMarkLineFill);
            }

            // draw checkbox text
            SizeF stringSize = g.MeasureString(Text, new Font("Arial", 8));
            g.DrawString(
                Text,
                new Font("Arial", 8),
                Enabled ? SkinManager.GetPrimaryTextBrush() : SkinManager.GetDisabledOrHintBrush(),
                boxOffset + TEXT_OFFSET, Height / 2 - stringSize.Height / 2 + 2);

            // dispose used paint objects
            pen.Dispose();
            brush.Dispose();
        }

        private Bitmap DrawCheckMarkBitmap()
        {
            var checkMark = new Bitmap(CHECKBOX_SIZE, CHECKBOX_SIZE);
            var g = Graphics.FromImage(checkMark);

            // clear everything, transparent
            g.Clear(Color.Transparent);

            // draw the checkmark lines
            using (var pen = new Pen(Parent.BackColor, 2))
            {
                g.DrawLines(pen, CHECKMARK_LINE);
            }

            return checkMark;
        }

        public override bool AutoSize
        {
            get { return base.AutoSize; }
            set
            {
                base.AutoSize = value;
                if (value)
                {
                    Size = new Size(10, 10);
                }
            }
        }

        private bool IsMouseInCheckArea()
        {
            return boxRectangle.Contains(MouseLocation);
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            Font = SkinManager.ARIAL_MEDIUM_10;

            if (DesignMode) return;

            MouseState = MouseState.OUT;
            MouseEnter += (sender, args) =>
            {
                MouseState = MouseState.HOVER;
            };
            MouseLeave += (sender, args) =>
            {
                MouseLocation = new Point(-1, -1);
                MouseState = MouseState.OUT;
            };
            MouseDown += (sender, args) =>
            {
                MouseState = MouseState.DOWN;

                if (Ripple && args.Button == MouseButtons.Left && IsMouseInCheckArea())
                {
                    rippleAnimationManager.SecondaryIncrement = 0;
                    rippleAnimationManager.StartNewAnimation(AnimationDirection.InOutIn, new object[] { Checked });
                }
            };
            MouseUp += (sender, args) =>
            {
                MouseState = MouseState.HOVER;
                rippleAnimationManager.SecondaryIncrement = 0.08;
            };
            MouseMove += (sender, args) =>
            {
                MouseLocation = args.Location;
                Cursor = IsMouseInCheckArea() ? Cursors.Hand : Cursors.Default;
            };
        }

    }

    #endregion
    #region MaterialContextMenu

    public class MaterialContextMenuStrip : ContextMenuStrip, IMaterialControl
    {
        //Properties for managing the material design properties
        [Browsable(false)]
        public int Depth { get; set; }
        [Browsable(false)]
        public MaterialSkinManager SkinManager { get { return MaterialSkinManager.Instance; } }
        [Browsable(false)]
        public MouseState MouseState { get; set; }


        internal AnimationManager animationManager;
        internal Point animationSource;

        public delegate void ItemClickStart(object sender, ToolStripItemClickedEventArgs e);
        public event ItemClickStart OnItemClickStart;

        public MaterialContextMenuStrip()
        {
            Renderer = new MaterialToolStripRender();

            animationManager = new AnimationManager(false)
            {
                Increment = 0.07,
                AnimationType = AnimationType.Linear
            };
            animationManager.OnAnimationProgress += sender => Invalidate();
            animationManager.OnAnimationFinished += sender => OnItemClicked(delayesArgs);

            BackColor = SkinManager.GetApplicationBackgroundColor();
        }

        protected override void OnMouseUp(MouseEventArgs mea)
        {
            base.OnMouseUp(mea);

            animationSource = mea.Location;
        }

        private ToolStripItemClickedEventArgs delayesArgs;
        protected override void OnItemClicked(ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem != null && !(e.ClickedItem is ToolStripSeparator))
            {
                if (e == delayesArgs)
                {
                    //The event has been fired manualy because the args are the ones we saved for delay
                    base.OnItemClicked(e);
                }
                else
                {
                    //Interrupt the default on click, saving the args for the delay which is needed to display the animaton
                    delayesArgs = e;

                    //Fire custom event to trigger actions directly but keep cms open
                    if (OnItemClickStart != null) OnItemClickStart(this, e);

                    //Start animation
                    animationManager.StartNewAnimation(AnimationDirection.In);
                }
            }
        }
    }

    public class MaterialToolStripMenuItem : ToolStripMenuItem
    {
        public MaterialToolStripMenuItem()
        {
            AutoSize = false;
            Size = new Size(120, 30);
        }

        protected override ToolStripDropDown CreateDefaultDropDown()
        {
            var baseDropDown = base.CreateDefaultDropDown();
            if (DesignMode) return baseDropDown;

            var defaultDropDown = new MaterialContextMenuStrip();
            defaultDropDown.Items.AddRange(baseDropDown.Items);

            return defaultDropDown;
        }
    }

    internal class MaterialToolStripRender : ToolStripProfessionalRenderer, IMaterialControl
    {
        //Properties for managing the material design properties
        public int Depth { get; set; }
        public MaterialSkinManager SkinManager { get { return MaterialSkinManager.Instance; } }
        public MouseState MouseState { get; set; }


        protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
        {
            var g = e.Graphics;
            g.TextRenderingHint = TextRenderingHint.AntiAlias;

            var itemRect = GetItemRect(e.Item);
            var textRect = new Rectangle(24, itemRect.Y, itemRect.Width - (24 + 16), itemRect.Height);
            g.DrawString(
                e.Text,
                new Font("Arial", 8),
                e.Item.Enabled ? SkinManager.GetPrimaryTextBrush() : SkinManager.GetDisabledOrHintBrush(),
                textRect,
                new StringFormat { LineAlignment = StringAlignment.Center });
        }

        protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
        {
            var g = e.Graphics;
            g.Clear(SkinManager.GetApplicationBackgroundColor());

            //Draw background
            var itemRect = GetItemRect(e.Item);
            g.FillRectangle(e.Item.Selected && e.Item.Enabled ? SkinManager.GetCmsSelectedItemBrush() : new SolidBrush(SkinManager.GetApplicationBackgroundColor()), itemRect);

            //Ripple animation
            var toolStrip = e.ToolStrip as MaterialContextMenuStrip;
            if (toolStrip != null)
            {
                var animationManager = toolStrip.animationManager;
                var animationSource = toolStrip.animationSource;
                if (toolStrip.animationManager.IsAnimating() && e.Item.Bounds.Contains(animationSource))
                {
                    for (int i = 0; i < animationManager.GetAnimationCount(); i++)
                    {
                        var animationValue = animationManager.GetProgress(i);
                        var rippleBrush = new SolidBrush(Color.FromArgb((int)(51 - (animationValue * 50)), Color.Black));
                        var rippleSize = (int)(animationValue * itemRect.Width * 2.5);
                        g.FillEllipse(rippleBrush, new Rectangle(animationSource.X - rippleSize / 2, itemRect.Y - itemRect.Height, rippleSize, itemRect.Height * 3));
                    }
                }
            }
        }

        protected override void OnRenderImageMargin(ToolStripRenderEventArgs e)
        {
            //base.OnRenderImageMargin(e);
        }

        protected override void OnRenderSeparator(ToolStripSeparatorRenderEventArgs e)
        {
            var g = e.Graphics;

            g.FillRectangle(new SolidBrush(SkinManager.GetApplicationBackgroundColor()), e.Item.Bounds);
            g.DrawLine(
                new Pen(SkinManager.GetDividersColor()),
                new Point(e.Item.Bounds.Left, e.Item.Bounds.Height / 2),
                new Point(e.Item.Bounds.Right, e.Item.Bounds.Height / 2));
        }

        protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
        {
            var g = e.Graphics;

            g.DrawRectangle(
                new Pen(SkinManager.GetDividersColor()),
                new Rectangle(e.AffectedBounds.X, e.AffectedBounds.Y, e.AffectedBounds.Width - 1, e.AffectedBounds.Height - 1));
        }

        protected override void OnRenderArrow(ToolStripArrowRenderEventArgs e)
        {
            var g = e.Graphics;
            const int ARROW_SIZE = 4;

            var arrowMiddle = new Point(e.ArrowRectangle.X + e.ArrowRectangle.Width / 2, e.ArrowRectangle.Y + e.ArrowRectangle.Height / 2);
            var arrowBrush = e.Item.Enabled ? SkinManager.GetPrimaryTextBrush() : SkinManager.GetDisabledOrHintBrush();
            using (var arrowPath = new GraphicsPath())
            {
                arrowPath.AddLines(
                    new[] {
                        new Point(arrowMiddle.X - ARROW_SIZE, arrowMiddle.Y - ARROW_SIZE),
                        new Point(arrowMiddle.X, arrowMiddle.Y),
                        new Point(arrowMiddle.X - ARROW_SIZE, arrowMiddle.Y + ARROW_SIZE) });
                arrowPath.CloseFigure();

                g.FillPath(arrowBrush, arrowPath);
            }
        }

        private Rectangle GetItemRect(ToolStripItem item)
        {
            return new Rectangle(0, item.ContentRectangle.Y, item.ContentRectangle.Width + 4, item.ContentRectangle.Height);
        }
    }

    #endregion
    #region MaterialDivider

    public sealed class MaterialDivider : Control, IMaterialControl
    {
        [Browsable(false)]
        public int Depth { get; set; }
        [Browsable(false)]
        public MaterialSkinManager SkinManager { get { return MaterialSkinManager.Instance; } }
        [Browsable(false)]
        public MouseState MouseState { get; set; }

        public MaterialDivider()
        {
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            Height = 1;
            BackColor = SkinManager.GetDividersColor();
        }
    }

    #endregion
    #region MaterialFlatButton

    public class MaterialFlatButton : Button, IMaterialControl
    {
        [Browsable(false)]
        public int Depth { get; set; }
        [Browsable(false)]
        public MaterialSkinManager SkinManager { get { return MaterialSkinManager.Instance; } }
        [Browsable(false)]
        public MouseState MouseState { get; set; }
        public bool Primary { get; set; }
        public bool ForceUpper { get; set; } = true;

        private readonly AnimationManager animationManager;
        private readonly AnimationManager hoverAnimationManager;

        private SizeF textSize;

        public MaterialFlatButton()
        {
            Primary = false;
            ForceUpper = true;

            animationManager = new AnimationManager(false)
            {
                Increment = 0.03,
                AnimationType = AnimationType.EaseOut
            };
            hoverAnimationManager = new AnimationManager
            {
                Increment = 0.07,
                AnimationType = AnimationType.Linear
            };

            hoverAnimationManager.OnAnimationProgress += sender => Invalidate();
            animationManager.OnAnimationProgress += sender => Invalidate();

            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            AutoSize = false;
            Margin = new Padding(4, 6, 4, 6);
            Padding = new Padding(0);
        }

        public override string Text
        {
            get { return base.Text; }
            set
            {
                string val = "";
                if (ForceUpper) { val = value.ToUpper(); }
                else { val = value; }
                base.Text = value;
                textSize = CreateGraphics().MeasureString(val, new Font("Arial", 9));
                if (AutoSize)
                    Size = GetPreferredSize();
                Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            var g = pevent.Graphics;
            g.TextRenderingHint = TextRenderingHint.AntiAlias;

            g.Clear(Parent.BackColor);

            //Hover
            Color c = SkinManager.GetFlatButtonHoverBackgroundColor();
            using (Brush b = new SolidBrush(Color.FromArgb((int)(hoverAnimationManager.GetProgress() * c.A), c.RemoveAlpha())))
                g.FillRectangle(b, ClientRectangle);

            //Ripple
            if (animationManager.IsAnimating())
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                for (int i = 0; i < animationManager.GetAnimationCount(); i++)
                {
                    var animationValue = animationManager.GetProgress(i);
                    var animationSource = animationManager.GetSource(i);

                    using (Brush rippleBrush = new SolidBrush(Color.FromArgb((int)(101 - (animationValue * 100)), Color.Black)))
                    {
                        var rippleSize = (int)(animationValue * Width * 2);
                        g.FillEllipse(rippleBrush, new Rectangle(animationSource.X - rippleSize / 2, animationSource.Y - rippleSize / 2, rippleSize, rippleSize));
                    }
                }
                g.SmoothingMode = SmoothingMode.None;
            }
            string val = "";
            if (ForceUpper) { val = Text.ToUpper(); }
            else { val = Text; }
            g.DrawString(val, new Font("Arial", 9), Enabled ? (Primary ? SkinManager.ColorScheme.PrimaryBrush : SkinManager.GetPrimaryTextBrush()) : SkinManager.GetFlatButtonDisabledTextBrush(), ClientRectangle, new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
        }

        private Size GetPreferredSize()
        {
            return GetPreferredSize(new Size(0, 0));
        }

        public override Size GetPreferredSize(Size proposedSize)
        {
            return new Size((int)textSize.Width + 8, 36);
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            if (DesignMode) return;

            MouseState = MouseState.OUT;
            MouseEnter += (sender, args) =>
            {
                MouseState = MouseState.HOVER;
                hoverAnimationManager.StartNewAnimation(AnimationDirection.In);
                Invalidate();
            };
            MouseLeave += (sender, args) =>
            {
                MouseState = MouseState.OUT;
                hoverAnimationManager.StartNewAnimation(AnimationDirection.Out);
                Invalidate();
            };
            MouseDown += (sender, args) =>
            {
                if (args.Button == MouseButtons.Left)
                {
                    MouseState = MouseState.DOWN;

                    animationManager.StartNewAnimation(AnimationDirection.In, args.Location);
                    Invalidate();
                }
            };
            MouseUp += (sender, args) =>
            {
                MouseState = MouseState.HOVER;

                Invalidate();
            };
        }
    }

    #endregion
    #region MaterialForm

    public class MaterialForm : Form, IMaterialControl
    {
        [Browsable(false)]
        public int Depth { get; set; }
        [Browsable(false)]
        public MaterialSkinManager SkinManager { get { return MaterialSkinManager.Instance; } }
        [Browsable(false)]
        public MouseState MouseState { get; set; }
        public new FormBorderStyle FormBorderStyle { get { return base.FormBorderStyle; } set { base.FormBorderStyle = value; } }
        public bool Sizable { get; set; }

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("user32.dll")]
        public static extern int TrackPopupMenuEx(IntPtr hmenu, uint fuFlags, int x, int y, IntPtr hwnd, IntPtr lptpm);

        [DllImport("user32.dll")]
        public static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("user32.dll")]
        public static extern IntPtr MonitorFromWindow(IntPtr hwnd, uint dwFlags);

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern bool GetMonitorInfo(HandleRef hmonitor, [In, Out] MONITORINFOEX info);

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        public const int WM_MOUSEMOVE = 0x0200;
        public const int WM_LBUTTONDOWN = 0x0201;
        public const int WM_LBUTTONUP = 0x0202;
        public const int WM_LBUTTONDBLCLK = 0x0203;
        public const int WM_RBUTTONDOWN = 0x0204;
        private const int HTBOTTOMLEFT = 16;
        private const int HTBOTTOMRIGHT = 17;
        private const int HTLEFT = 10;
        private const int HTRIGHT = 11;
        private const int HTBOTTOM = 15;
        private const int HTTOP = 12;
        private const int HTTOPLEFT = 13;
        private const int HTTOPRIGHT = 14;
        private const int BORDER_WIDTH = 7;
        private ResizeDirection resizeDir;
        private ButtonState buttonState = ButtonState.None;

        private const int WMSZ_TOP = 3;
        private const int WMSZ_TOPLEFT = 4;
        private const int WMSZ_TOPRIGHT = 5;
        private const int WMSZ_LEFT = 1;
        private const int WMSZ_RIGHT = 2;
        private const int WMSZ_BOTTOM = 6;
        private const int WMSZ_BOTTOMLEFT = 7;
        private const int WMSZ_BOTTOMRIGHT = 8;

        private readonly Dictionary<int, int> resizingLocationsToCmd = new Dictionary<int, int>
        {
            {HTTOP,         WMSZ_TOP},
            {HTTOPLEFT,     WMSZ_TOPLEFT},
            {HTTOPRIGHT,    WMSZ_TOPRIGHT},
            {HTLEFT,        WMSZ_LEFT},
            {HTRIGHT,       WMSZ_RIGHT},
            {HTBOTTOM,      WMSZ_BOTTOM},
            {HTBOTTOMLEFT,  WMSZ_BOTTOMLEFT},
            {HTBOTTOMRIGHT, WMSZ_BOTTOMRIGHT}
        };

        private const int STATUS_BAR_BUTTON_WIDTH = STATUS_BAR_HEIGHT;
        private const int STATUS_BAR_HEIGHT = 24;
        private const int ACTION_BAR_HEIGHT = 40;

        private const uint TPM_LEFTALIGN = 0x0000;
        private const uint TPM_RETURNCMD = 0x0100;

        private const int WM_SYSCOMMAND = 0x0112;
        private const int WS_MINIMIZEBOX = 0x20000;
        private const int WS_SYSMENU = 0x00080000;

        private const int MONITOR_DEFAULTTONEAREST = 2;

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto, Pack = 4)]
        public class MONITORINFOEX
        {
            public int cbSize = Marshal.SizeOf(typeof(MONITORINFOEX));
            public RECT rcMonitor = new RECT();
            public RECT rcWork = new RECT();
            public int dwFlags = 0;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public char[] szDevice = new char[32];
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;

            public int Width()
            {
                return right - left;
            }

            public int Height()
            {
                return bottom - top;
            }
        }

        private enum ResizeDirection
        {
            BottomLeft,
            Left,
            Right,
            BottomRight,
            Bottom,
            None
        }

        private enum ButtonState
        {
            XOver,
            MaxOver,
            MinOver,
            XDown,
            MaxDown,
            MinDown,
            None
        }

        private readonly Cursor[] resizeCursors = { Cursors.SizeNESW, Cursors.SizeWE, Cursors.SizeNWSE, Cursors.SizeWE, Cursors.SizeNS };

        private Rectangle minButtonBounds;
        private Rectangle maxButtonBounds;
        private Rectangle xButtonBounds;
        private Rectangle actionBarBounds;
        private Rectangle statusBarBounds;

        private bool Maximized;
        private Size previousSize;
        private Point previousLocation;
        private bool headerMouseDown;

        public MaterialForm()
        {
            FormBorderStyle = FormBorderStyle.None;
            Sizable = true;
            DoubleBuffered = true;
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);

            // This enables the form to trigger the MouseMove event even when mouse is over another control
            Application.AddMessageFilter(new MouseMessageFilter());
            MouseMessageFilter.MouseMove += OnGlobalMouseMove;
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (DesignMode || IsDisposed) return;

            if (m.Msg == WM_LBUTTONDBLCLK)
            {
                MaximizeWindow(!Maximized);
            }
            else if (m.Msg == WM_MOUSEMOVE && Maximized &&
                (statusBarBounds.Contains(PointToClient(Cursor.Position)) || actionBarBounds.Contains(PointToClient(Cursor.Position))) &&
                !(minButtonBounds.Contains(PointToClient(Cursor.Position)) || maxButtonBounds.Contains(PointToClient(Cursor.Position)) || xButtonBounds.Contains(PointToClient(Cursor.Position))))
            {
                if (headerMouseDown)
                {
                    Maximized = false;
                    headerMouseDown = false;

                    Point mousePoint = PointToClient(Cursor.Position);
                    if (mousePoint.X < Width / 2)
                        Location = mousePoint.X < previousSize.Width / 2 ?
                            new Point(Cursor.Position.X - mousePoint.X, Cursor.Position.Y - mousePoint.Y) :
                            new Point(Cursor.Position.X - previousSize.Width / 2, Cursor.Position.Y - mousePoint.Y);
                    else
                        Location = Width - mousePoint.X < previousSize.Width / 2 ?
                            new Point(Cursor.Position.X - previousSize.Width + Width - mousePoint.X, Cursor.Position.Y - mousePoint.Y) :
                            new Point(Cursor.Position.X - previousSize.Width / 2, Cursor.Position.Y - mousePoint.Y);

                    Size = previousSize;
                    ReleaseCapture();
                    SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
                }
            }
            else if (m.Msg == WM_LBUTTONDOWN &&
                (statusBarBounds.Contains(PointToClient(Cursor.Position)) || actionBarBounds.Contains(PointToClient(Cursor.Position))) &&
                !(minButtonBounds.Contains(PointToClient(Cursor.Position)) || maxButtonBounds.Contains(PointToClient(Cursor.Position)) || xButtonBounds.Contains(PointToClient(Cursor.Position))))
            {
                if (!Maximized)
                {
                    ReleaseCapture();
                    SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
                }
                else
                {
                    headerMouseDown = true;
                }
            }
            else if (m.Msg == WM_RBUTTONDOWN)
            {
                Point cursorPos = PointToClient(Cursor.Position);

                if (statusBarBounds.Contains(cursorPos) && !minButtonBounds.Contains(cursorPos) &&
                    !maxButtonBounds.Contains(cursorPos) && !xButtonBounds.Contains(cursorPos))
                {
                    // Show default system menu when right clicking titlebar
                    int id = TrackPopupMenuEx(
                        GetSystemMenu(Handle, false),
                        TPM_LEFTALIGN | TPM_RETURNCMD,
                        Cursor.Position.X, Cursor.Position.Y, Handle, IntPtr.Zero);

                    // Pass the command as a WM_SYSCOMMAND message
                    SendMessage(Handle, WM_SYSCOMMAND, id, 0);
                }
            }
            else if (m.Msg == WM_NCLBUTTONDOWN)
            {
                // This re-enables resizing by letting the application know when the
                // user is trying to resize a side. This is disabled by default when using WS_SYSMENU.
                if (!Sizable) return;

                byte bFlag = 0;

                // Get which side to resize from
                if (resizingLocationsToCmd.ContainsKey((int)m.WParam))
                    bFlag = (byte)resizingLocationsToCmd[(int)m.WParam];

                if (bFlag != 0)
                    SendMessage(Handle, WM_SYSCOMMAND, 0xF000 | bFlag, (int)m.LParam);
            }
            else if (m.Msg == WM_LBUTTONUP)
            {
                headerMouseDown = false;
            }
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams par = base.CreateParams;
                // WS_SYSMENU: Trigger the creation of the system menu
                // WS_MINIMIZEBOX: Allow minimizing from taskbar
                par.Style = par.Style | WS_MINIMIZEBOX | WS_SYSMENU; // Turn on the WS_MINIMIZEBOX style flag
                return par;
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (DesignMode) return;
            UpdateButtons(e);

            if (e.Button == MouseButtons.Left && !Maximized)
                ResizeForm(resizeDir);
            base.OnMouseDown(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            if (DesignMode) return;
            buttonState = ButtonState.None;
            Invalidate();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (DesignMode) return;

            if (Sizable)
            {
                //True if the mouse is hovering over a child control
                bool isChildUnderMouse = GetChildAtPoint(e.Location) != null;

                if (e.Location.X < BORDER_WIDTH && e.Location.Y > Height - BORDER_WIDTH && !isChildUnderMouse && !Maximized)
                {
                    resizeDir = ResizeDirection.BottomLeft;
                    Cursor = Cursors.SizeNESW;
                }
                else if (e.Location.X < BORDER_WIDTH && !isChildUnderMouse && !Maximized)
                {
                    resizeDir = ResizeDirection.Left;
                    Cursor = Cursors.SizeWE;
                }
                else if (e.Location.X > Width - BORDER_WIDTH && e.Location.Y > Height - BORDER_WIDTH && !isChildUnderMouse && !Maximized)
                {
                    resizeDir = ResizeDirection.BottomRight;
                    Cursor = Cursors.SizeNWSE;
                }
                else if (e.Location.X > Width - BORDER_WIDTH && !isChildUnderMouse && !Maximized)
                {
                    resizeDir = ResizeDirection.Right;
                    Cursor = Cursors.SizeWE;
                }
                else if (e.Location.Y > Height - BORDER_WIDTH && !isChildUnderMouse && !Maximized)
                {
                    resizeDir = ResizeDirection.Bottom;
                    Cursor = Cursors.SizeNS;
                }
                else
                {
                    resizeDir = ResizeDirection.None;

                    //Only reset the cursor when needed, this prevents it from flickering when a child control changes the cursor to its own needs
                    if (resizeCursors.Contains(Cursor))
                    {
                        Cursor = Cursors.Default;
                    }
                }
            }

            UpdateButtons(e);
        }

        protected void OnGlobalMouseMove(object sender, MouseEventArgs e)
        {
            if (!IsDisposed)
            {
                // Convert to client position and pass to Form.MouseMove
                Point clientCursorPos = PointToClient(e.Location);
                MouseEventArgs new_e = new MouseEventArgs(MouseButtons.None, 0, clientCursorPos.X, clientCursorPos.Y, 0);
                OnMouseMove(new_e);
            }
        }

        private void UpdateButtons(MouseEventArgs e, bool up = false)
        {
            if (DesignMode) return;
            ButtonState oldState = buttonState;
            bool showMin = MinimizeBox && ControlBox;
            bool showMax = MaximizeBox && ControlBox;

            if (e.Button == MouseButtons.Left && !up)
            {
                if (showMin && !showMax && maxButtonBounds.Contains(e.Location))
                    buttonState = ButtonState.MinDown;
                else if (showMin && showMax && minButtonBounds.Contains(e.Location))
                    buttonState = ButtonState.MinDown;
                else if (showMax && maxButtonBounds.Contains(e.Location))
                    buttonState = ButtonState.MaxDown;
                else if (ControlBox && xButtonBounds.Contains(e.Location))
                    buttonState = ButtonState.XDown;
                else
                    buttonState = ButtonState.None;
            }
            else
            {
                if (showMin && !showMax && maxButtonBounds.Contains(e.Location))
                {
                    buttonState = ButtonState.MinOver;

                    if (oldState == ButtonState.MinDown)
                        WindowState = FormWindowState.Minimized;
                }
                else if (showMin && showMax && minButtonBounds.Contains(e.Location))
                {
                    buttonState = ButtonState.MinOver;

                    if (oldState == ButtonState.MinDown)
                        WindowState = FormWindowState.Minimized;
                }
                else if (MaximizeBox && ControlBox && maxButtonBounds.Contains(e.Location))
                {
                    buttonState = ButtonState.MaxOver;

                    if (oldState == ButtonState.MaxDown)
                        MaximizeWindow(!Maximized);

                }
                else if (ControlBox && xButtonBounds.Contains(e.Location))
                {
                    buttonState = ButtonState.XOver;

                    if (oldState == ButtonState.XDown)
                        Close();
                }
                else buttonState = ButtonState.None;
            }

            if (oldState != buttonState) Invalidate();
        }

        private void MaximizeWindow(bool maximize)
        {
            if (!MaximizeBox || !ControlBox) return;

            Maximized = maximize;

            if (maximize)
            {
                IntPtr monitorHandle = MonitorFromWindow(Handle, MONITOR_DEFAULTTONEAREST);
                MONITORINFOEX monitorInfo = new MONITORINFOEX();
                GetMonitorInfo(new HandleRef(null, monitorHandle), monitorInfo);
                previousSize = Size;
                previousLocation = Location;
                Size = new Size(monitorInfo.rcWork.Width(), monitorInfo.rcWork.Height());
                Location = new Point(monitorInfo.rcWork.left, monitorInfo.rcWork.top);
            }
            else
            {
                Size = previousSize;
                Location = previousLocation;
            }

        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (DesignMode) return;
            UpdateButtons(e, true);

            base.OnMouseUp(e);
            ReleaseCapture();
        }

        private void ResizeForm(ResizeDirection direction)
        {
            if (DesignMode) return;
            int dir = -1;
            switch (direction)
            {
                case ResizeDirection.BottomLeft:
                    dir = HTBOTTOMLEFT;
                    break;
                case ResizeDirection.Left:
                    dir = HTLEFT;
                    break;
                case ResizeDirection.Right:
                    dir = HTRIGHT;
                    break;
                case ResizeDirection.BottomRight:
                    dir = HTBOTTOMRIGHT;
                    break;
                case ResizeDirection.Bottom:
                    dir = HTBOTTOM;
                    break;
            }

            ReleaseCapture();
            if (dir != -1)
            {
                SendMessage(Handle, WM_NCLBUTTONDOWN, dir, 0);
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            minButtonBounds = new Rectangle((Width - SkinManager.FORM_PADDING / 2) - 3 * STATUS_BAR_BUTTON_WIDTH, 0, STATUS_BAR_BUTTON_WIDTH, STATUS_BAR_HEIGHT);
            maxButtonBounds = new Rectangle((Width - SkinManager.FORM_PADDING / 2) - 2 * STATUS_BAR_BUTTON_WIDTH, 0, STATUS_BAR_BUTTON_WIDTH, STATUS_BAR_HEIGHT);
            xButtonBounds = new Rectangle((Width - SkinManager.FORM_PADDING / 2) - STATUS_BAR_BUTTON_WIDTH, 0, STATUS_BAR_BUTTON_WIDTH, STATUS_BAR_HEIGHT);
            statusBarBounds = new Rectangle(0, 0, Width, STATUS_BAR_HEIGHT);
            actionBarBounds = new Rectangle(0, STATUS_BAR_HEIGHT, Width, ACTION_BAR_HEIGHT);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.TextRenderingHint = TextRenderingHint.AntiAlias;

            g.Clear(SkinManager.GetApplicationBackgroundColor());
            g.FillRectangle(SkinManager.ColorScheme.SecondaryBrush, statusBarBounds);
            g.FillRectangle(SkinManager.ColorScheme.PrimaryBrush, actionBarBounds);

            //Draw border
            using (var borderPen = new Pen(SkinManager.GetDividersColor(), 1))
            {
                g.DrawLine(borderPen, new Point(0, actionBarBounds.Bottom), new Point(0, Height - 2));
                g.DrawLine(borderPen, new Point(Width - 1, actionBarBounds.Bottom), new Point(Width - 1, Height - 2));
                g.DrawLine(borderPen, new Point(0, Height - 1), new Point(Width - 1, Height - 1));
            }

            // Determine whether or not we even should be drawing the buttons.
            bool showMin = MinimizeBox && ControlBox;
            bool showMax = MaximizeBox && ControlBox;
            var hoverBrush = SkinManager.GetFlatButtonHoverBackgroundBrush();
            var downBrush = SkinManager.GetFlatButtonPressedBackgroundBrush();

            // When MaximizeButton == false, the minimize button will be painted in its place
            if (buttonState == ButtonState.MinOver && showMin)
                g.FillRectangle(hoverBrush, showMax ? minButtonBounds : maxButtonBounds);

            if (buttonState == ButtonState.MinDown && showMin)
                g.FillRectangle(downBrush, showMax ? minButtonBounds : maxButtonBounds);

            if (buttonState == ButtonState.MaxOver && showMax)
                g.FillRectangle(hoverBrush, maxButtonBounds);

            if (buttonState == ButtonState.MaxDown && showMax)
                g.FillRectangle(downBrush, maxButtonBounds);

            if (buttonState == ButtonState.XOver && ControlBox)
                g.FillRectangle(hoverBrush, xButtonBounds);

            if (buttonState == ButtonState.XDown && ControlBox)
                g.FillRectangle(downBrush, xButtonBounds);

            using (var formButtonsPen = new Pen(SkinManager.ACTION_BAR_TEXT_SECONDARY, 2))
            {
                // Minimize button.
                if (showMin)
                {
                    int x = showMax ? minButtonBounds.X : maxButtonBounds.X;
                    int y = showMax ? minButtonBounds.Y : maxButtonBounds.Y;

                    g.DrawLine(
                        formButtonsPen,
                        x + (int)(minButtonBounds.Width * 0.33),
                        y + (int)(minButtonBounds.Height * 0.66),
                        x + (int)(minButtonBounds.Width * 0.66),
                        y + (int)(minButtonBounds.Height * 0.66)
                   );
                }

                // Maximize button
                if (showMax)
                {
                    g.DrawRectangle(
                        formButtonsPen,
                        maxButtonBounds.X + (int)(maxButtonBounds.Width * 0.33),
                        maxButtonBounds.Y + (int)(maxButtonBounds.Height * 0.36),
                        (int)(maxButtonBounds.Width * 0.39),
                        (int)(maxButtonBounds.Height * 0.31)
                   );
                }

                // Close button
                if (ControlBox)
                {
                    g.DrawLine(
                        formButtonsPen,
                        xButtonBounds.X + (int)(xButtonBounds.Width * 0.33),
                        xButtonBounds.Y + (int)(xButtonBounds.Height * 0.33),
                        xButtonBounds.X + (int)(xButtonBounds.Width * 0.66),
                        xButtonBounds.Y + (int)(xButtonBounds.Height * 0.66)
                   );

                    g.DrawLine(
                        formButtonsPen,
                        xButtonBounds.X + (int)(xButtonBounds.Width * 0.66),
                        xButtonBounds.Y + (int)(xButtonBounds.Height * 0.33),
                        xButtonBounds.X + (int)(xButtonBounds.Width * 0.33),
                        xButtonBounds.Y + (int)(xButtonBounds.Height * 0.66));
                }
            }

            //Form title
            g.DrawString(Text, SkinManager.ARIAL_MEDIUM_12, SkinManager.ColorScheme.TextBrush, new Rectangle(SkinManager.FORM_PADDING, STATUS_BAR_HEIGHT, Width, ACTION_BAR_HEIGHT), new StringFormat { LineAlignment = StringAlignment.Center });
        }
    }

    public class MouseMessageFilter : IMessageFilter
    {
        private const int WM_MOUSEMOVE = 0x0200;

        public static event MouseEventHandler MouseMove;

        public bool PreFilterMessage(ref Message m)
        {

            if (m.Msg == WM_MOUSEMOVE)
            {
                if (MouseMove != null)
                {
                    int x = Control.MousePosition.X, y = Control.MousePosition.Y;

                    MouseMove(null, new MouseEventArgs(MouseButtons.None, 0, x, y, 0));
                }
            }
            return false;
        }
    }

    #endregion
    #region MaterialLabel

    public class MaterialLabel : Label, IMaterialControl
    {
        [Browsable(false)]
        public int Depth { get; set; }
        [Browsable(false)]
        public MaterialSkinManager SkinManager { get { return MaterialSkinManager.Instance; } }
        [Browsable(false)]
        public MouseState MouseState { get; set; }
        protected override void OnCreateControl()
        {
            base.OnCreateControl();

            ForeColor = SkinManager.GetPrimaryTextColor();

            BackColorChanged += (sender, args) => ForeColor = SkinManager.GetPrimaryTextColor();
        }
    }

    #endregion
    #region MaterialListview

    public class MaterialListView : ListView, IMaterialControl
    {
        [Browsable(false)]
        public int Depth { get; set; }
        [Browsable(false)]
        public MaterialSkinManager SkinManager { get { return MaterialSkinManager.Instance; } }
        [Browsable(false)]
        public MouseState MouseState { get; set; }
        [Browsable(false)]
        public Point MouseLocation { get; set; }

        public MaterialListView()
        {
            GridLines = false;
            FullRowSelect = true;
            HeaderStyle = ColumnHeaderStyle.Nonclickable;
            View = View.Details;
            OwnerDraw = true;
            ResizeRedraw = true;
            BorderStyle = BorderStyle.None;
            SetStyle(ControlStyles.DoubleBuffer | ControlStyles.OptimizedDoubleBuffer, true);

            //Fix for hovers, by default it doesn't redraw
            //TODO: should only redraw when the hovered line changed, this to reduce unnecessary redraws
            MouseLocation = new Point(-1, -1);
            MouseState = MouseState.OUT;
            MouseEnter += delegate
            {
                MouseState = MouseState.HOVER;
            };
            MouseLeave += delegate
            {
                MouseState = MouseState.OUT;
                MouseLocation = new Point(-1, -1);
                Invalidate();
            };
            MouseDown += delegate { MouseState = MouseState.DOWN; };
            MouseUp += delegate { MouseState = MouseState.HOVER; };
            MouseMove += delegate (object sender, MouseEventArgs args)
            {
                MouseLocation = args.Location;
                Invalidate();
            };
        }

        protected override void OnDrawColumnHeader(DrawListViewColumnHeaderEventArgs e)
        {
            e.Graphics.FillRectangle(new SolidBrush(SkinManager.GetApplicationBackgroundColor()), new Rectangle(e.Bounds.X, e.Bounds.Y, Width, e.Bounds.Height));
            e.Graphics.DrawString(e.Header.Text,
                SkinManager.ARIAL_MEDIUM_10,
                SkinManager.GetSecondaryTextBrush(),
                new Rectangle(e.Bounds.X + ITEM_PADDING, e.Bounds.Y + ITEM_PADDING, e.Bounds.Width - ITEM_PADDING * 2, e.Bounds.Height - ITEM_PADDING * 2),
                getStringFormat());
        }

        private const int ITEM_PADDING = 12;
        protected override void OnDrawItem(DrawListViewItemEventArgs e)
        {
            //We draw the current line of items (= item with subitems) on a temp bitmap, then draw the bitmap at once. This is to reduce flickering.
            var b = new Bitmap(e.Item.Bounds.Width, e.Item.Bounds.Height);
            var g = Graphics.FromImage(b);

            //always draw default background
            g.FillRectangle(new SolidBrush(SkinManager.GetApplicationBackgroundColor()), new Rectangle(new Point(e.Bounds.X, 0), e.Bounds.Size));

            if (e.State.HasFlag(ListViewItemStates.Selected))
            {
                //selected background
                g.FillRectangle(SkinManager.GetFlatButtonPressedBackgroundBrush(), new Rectangle(new Point(e.Bounds.X, 0), e.Bounds.Size));
            }
            else if (e.Bounds.Contains(MouseLocation) && MouseState == MouseState.HOVER)
            {
                //hover background
                g.FillRectangle(SkinManager.GetFlatButtonHoverBackgroundBrush(), new Rectangle(new Point(e.Bounds.X, 0), e.Bounds.Size));
            }


            //Draw separator
            g.DrawLine(new Pen(SkinManager.GetDividersColor()), e.Bounds.Left, 0, e.Bounds.Right, 0);

            foreach (ListViewItem.ListViewSubItem subItem in e.Item.SubItems)
            {
                //Draw text
                g.DrawString(subItem.Text, SkinManager.ARIAL_MEDIUM_10, SkinManager.GetPrimaryTextBrush(),
                                 new Rectangle(subItem.Bounds.Location.X + ITEM_PADDING, ITEM_PADDING, subItem.Bounds.Width - 2 * ITEM_PADDING, subItem.Bounds.Height - 2 * ITEM_PADDING),
                                 getStringFormat());
            }

            e.Graphics.DrawImage((Image)b.Clone(), e.Item.Bounds.Location);
            g.Dispose();
            b.Dispose();
        }

        private StringFormat getStringFormat()
        {
            return new StringFormat
            {
                FormatFlags = StringFormatFlags.LineLimit,
                Trimming = StringTrimming.EllipsisCharacter,
                Alignment = StringAlignment.Near,
                LineAlignment = StringAlignment.Center
            };
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();

            //This is a hax for the needed padding.
            //Another way would be intercepting all ListViewItems and changing the sizes, but really, that will be a lot of work
            //This will do for now.
            Font = new Font(SkinManager.ARIAL_MEDIUM_12.FontFamily, 24);
        }
    }

    #endregion
    #region MaterialProgressBar

    /// <summary>
    /// Material design-like progress bar
    /// </summary>
    public class MaterialProgressBar : ProgressBar, IMaterialControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MaterialProgressBar"/> class.
        /// </summary>
        public MaterialProgressBar()
        {
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }

        /// <summary>
        /// Gets or sets the depth.
        /// </summary>
        /// <value>
        /// The depth.
        /// </value>
        [Browsable(false)]
        public int Depth { get; set; }

        /// <summary>
        /// Gets the skin manager.
        /// </summary>
        /// <value>
        /// The skin manager.
        /// </value>
        [Browsable(false)]
        public MaterialSkinManager SkinManager
        {
            get { return MaterialSkinManager.Instance; }
        }

        /// <summary>
        /// Gets or sets the state of the mouse.
        /// </summary>
        /// <value>
        /// The state of the mouse.
        /// </value>
        [Browsable(false)]
        public MouseState MouseState { get; set; }

        /// <summary>
        /// Performs the work of setting the specified bounds of this control.
        /// </summary>
        /// <param name="x">The new <see cref="P:System.Windows.Forms.Control.Left" /> property value of the control.</param>
        /// <param name="y">The new <see cref="P:System.Windows.Forms.Control.Top" /> property value of the control.</param>
        /// <param name="width">The new <see cref="P:System.Windows.Forms.Control.Width" /> property value of the control.</param>
        /// <param name="height">The new <see cref="P:System.Windows.Forms.Control.Height" /> property value of the control.</param>
        /// <param name="specified">A bitwise combination of the <see cref="T:System.Windows.Forms.BoundsSpecified" /> values.</param>
        protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
        {
            base.SetBoundsCore(x, y, width, 5, specified);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.Paint" /> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs" /> that contains the event data.</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            var doneProgress = (int)(e.ClipRectangle.Width * ((double)Value / Maximum));
            e.Graphics.FillRectangle(SkinManager.ColorScheme.PrimaryBrush, 0, 0, doneProgress, e.ClipRectangle.Height);
            e.Graphics.FillRectangle(SkinManager.GetDisabledOrHintBrush(), doneProgress, 0, e.ClipRectangle.Width, e.ClipRectangle.Height);
        }
    }

    #endregion
    #region MaterialRadioButton

    public class MaterialRadioButton : RadioButton, IMaterialControl
    {
        ToolTip tip = new ToolTip();
        [Description("Specifies the text shown on the ToolTip.")]
        public string ToolTipText { get; set; }

        [Browsable(false)]
        public int Depth { get; set; }
        [Browsable(false)]
        public MaterialSkinManager SkinManager { get { return MaterialSkinManager.Instance; } }
        [Browsable(false)]
        public MouseState MouseState { get; set; }
        [Browsable(false)]
        public Point MouseLocation { get; set; }

        private bool ripple;
        [Category("Behavior")]
        public bool Ripple
        {
            get { return ripple; }
            set
            {
                ripple = value;
                AutoSize = AutoSize; //Make AutoSize directly set the bounds.

                if (value)
                {
                    Margin = new Padding(0);
                }

                Invalidate();
            }
        }

        // animation managers
        private readonly AnimationManager animationManager;
        private readonly AnimationManager rippleAnimationManager;

        // size related variables which should be recalculated onsizechanged
        private Rectangle radioButtonBounds;
        private int boxOffset;

        // size constants
        private const int RADIOBUTTON_SIZE = 19;
        private const int RADIOBUTTON_SIZE_HALF = RADIOBUTTON_SIZE / 2;
        private const int RADIOBUTTON_OUTER_CIRCLE_WIDTH = 2;
        private const int RADIOBUTTON_INNER_CIRCLE_SIZE = RADIOBUTTON_SIZE - (2 * RADIOBUTTON_OUTER_CIRCLE_WIDTH);

        public MaterialRadioButton()
        {
            SetStyle(ControlStyles.DoubleBuffer | ControlStyles.OptimizedDoubleBuffer, true);

            animationManager = new AnimationManager
            {
                AnimationType = AnimationType.EaseInOut,
                Increment = 0.06
            };
            rippleAnimationManager = new AnimationManager(false)
            {
                AnimationType = AnimationType.Linear,
                Increment = 0.10,
                SecondaryIncrement = 0.08
            };
            animationManager.OnAnimationProgress += sender => Invalidate();
            rippleAnimationManager.OnAnimationProgress += sender => Invalidate();

            CheckedChanged += (sender, args) => animationManager.StartNewAnimation(Checked ? AnimationDirection.In : AnimationDirection.Out);

            SizeChanged += OnSizeChanged;

            Ripple = true;
            MouseLocation = new Point(-1, -1);
        }

        protected override void OnMouseHover(EventArgs e)
        {
            base.OnMouseHover(e);

            if (ToolTipText != "")
            {
                tip.Show(ToolTipText, this);
            }
        }

        private void OnSizeChanged(object sender, EventArgs eventArgs)
        {
            boxOffset = Height / 2 - (int)Math.Ceiling(RADIOBUTTON_SIZE / 2d);
            radioButtonBounds = new Rectangle(boxOffset, boxOffset, RADIOBUTTON_SIZE, RADIOBUTTON_SIZE);
        }

        public override Size GetPreferredSize(Size proposedSize)
        {
            int width = boxOffset + 20 + (int)CreateGraphics().MeasureString(Text, SkinManager.ARIAL_MEDIUM_10).Width;
            return Ripple ? new Size(width, 30) : new Size(width, 20);
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            var g = pevent.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.AntiAlias;

            // clear the control
            g.Clear(Parent.BackColor);

            var RADIOBUTTON_CENTER = boxOffset + RADIOBUTTON_SIZE_HALF;

            var animationProgress = animationManager.GetProgress();

            int colorAlpha = Enabled ? (int)(animationProgress * 255.0) : SkinManager.GetCheckBoxOffDisabledColor().A;
            int backgroundAlpha = Enabled ? (int)(SkinManager.GetCheckboxOffColor().A * (1.0 - animationProgress)) : SkinManager.GetCheckBoxOffDisabledColor().A;
            float animationSize = (float)(animationProgress * 8f);
            float animationSizeHalf = animationSize / 2;
            animationSize = (float)(animationProgress * 9f);

            var brush = new SolidBrush(Color.FromArgb(colorAlpha, Enabled ? SkinManager.ColorScheme.AccentColor : SkinManager.GetCheckBoxOffDisabledColor()));
            var pen = new Pen(brush.Color);

            // draw ripple animation
            if (Ripple && rippleAnimationManager.IsAnimating())
            {
                for (int i = 0; i < rippleAnimationManager.GetAnimationCount(); i++)
                {
                    var animationValue = rippleAnimationManager.GetProgress(i);
                    var animationSource = new Point(RADIOBUTTON_CENTER, RADIOBUTTON_CENTER);
                    var rippleBrush = new SolidBrush(Color.FromArgb((int)((animationValue * 40)), ((bool)rippleAnimationManager.GetData(i)[0]) ? Color.Black : brush.Color));
                    var rippleHeight = (Height % 2 == 0) ? Height - 3 : Height - 2;
                    var rippleSize = (rippleAnimationManager.GetDirection(i) == AnimationDirection.InOutIn) ? (int)(rippleHeight * (0.8d + (0.2d * animationValue))) : rippleHeight;
                    using (var path = DrawHelper.CreateRoundRect(animationSource.X - rippleSize / 2, animationSource.Y - rippleSize / 2, rippleSize, rippleSize, rippleSize / 2))
                    {
                        g.FillPath(rippleBrush, path);
                    }

                    rippleBrush.Dispose();
                }
            }

            // draw radiobutton circle
            Color uncheckedColor = DrawHelper.BlendColor(Parent.BackColor, Enabled ? SkinManager.GetCheckboxOffColor() : SkinManager.GetCheckBoxOffDisabledColor(), backgroundAlpha);

            using (var path = DrawHelper.CreateRoundRect(boxOffset, boxOffset, RADIOBUTTON_SIZE, RADIOBUTTON_SIZE, 9f))
            {
                g.FillPath(new SolidBrush(uncheckedColor), path);

                if (Enabled)
                {
                    g.FillPath(brush, path);
                }
            }

            g.FillEllipse(
                new SolidBrush(Parent.BackColor),
                RADIOBUTTON_OUTER_CIRCLE_WIDTH + boxOffset,
                RADIOBUTTON_OUTER_CIRCLE_WIDTH + boxOffset,
                RADIOBUTTON_INNER_CIRCLE_SIZE,
                RADIOBUTTON_INNER_CIRCLE_SIZE);

            if (Checked)
            {
                using (var path = DrawHelper.CreateRoundRect(RADIOBUTTON_CENTER - animationSizeHalf, RADIOBUTTON_CENTER - animationSizeHalf, animationSize, animationSize, 4f))
                {
                    g.FillPath(brush, path);
                }
            }
            SizeF stringSize = g.MeasureString(Text, new Font("Arial", 8));
            g.DrawString(Text, new Font("Arial", 8), Enabled ? SkinManager.GetPrimaryTextBrush() : SkinManager.GetDisabledOrHintBrush(), boxOffset + 22, Height / 2 - stringSize.Height / 2 + 2);

            brush.Dispose();
            pen.Dispose();
        }

        private bool IsMouseInCheckArea()
        {
            return radioButtonBounds.Contains(MouseLocation);
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            Font = SkinManager.ARIAL_MEDIUM_10;

            if (DesignMode) return;

            MouseState = MouseState.OUT;
            MouseEnter += (sender, args) =>
            {
                MouseState = MouseState.HOVER;
            };
            MouseLeave += (sender, args) =>
            {
                MouseLocation = new Point(-1, -1);
                MouseState = MouseState.OUT;
            };
            MouseDown += (sender, args) =>
            {
                MouseState = MouseState.DOWN;

                if (Ripple && args.Button == MouseButtons.Left && IsMouseInCheckArea())
                {
                    rippleAnimationManager.SecondaryIncrement = 0;
                    rippleAnimationManager.StartNewAnimation(AnimationDirection.InOutIn, new object[] { Checked });
                }
            };
            MouseUp += (sender, args) =>
            {
                MouseState = MouseState.HOVER;
                rippleAnimationManager.SecondaryIncrement = 0.08;
            };
            MouseMove += (sender, args) =>
            {
                MouseLocation = args.Location;
                Cursor = IsMouseInCheckArea() ? Cursors.Hand : Cursors.Default;
            };
        }
    }

    #endregion
    #region MaterialRaisedButton

    public class MaterialRaisedButton : Button, IMaterialControl
    {
        [Browsable(false)]
        public int Depth { get; set; }
        [Browsable(false)]
        public MaterialSkinManager SkinManager { get { return MaterialSkinManager.Instance; } }
        [Browsable(false)]
        public MouseState MouseState { get; set; }
        public bool Primary { get; set; }

        private readonly AnimationManager animationManager;

        public MaterialRaisedButton()
        {
            Primary = true;

            animationManager = new AnimationManager(false)
            {
                Increment = 0.03,
                AnimationType = AnimationType.EaseOut
            };
            animationManager.OnAnimationProgress += sender => Invalidate();
        }

        protected override void OnMouseUp(MouseEventArgs mevent)
        {
            base.OnMouseUp(mevent);

            animationManager.StartNewAnimation(AnimationDirection.In, mevent.Location);
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            var g = pevent.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.AntiAlias;

            g.Clear(Parent.BackColor);

            using (var backgroundPath = DrawHelper.CreateRoundRect(ClientRectangle.X,
                ClientRectangle.Y,
                ClientRectangle.Width - 1,
                ClientRectangle.Height - 1,
                1f))
            {
                g.FillPath(Primary ? SkinManager.ColorScheme.PrimaryBrush : SkinManager.GetRaisedButtonBackgroundBrush(), backgroundPath);
            }

            if (animationManager.IsAnimating())
            {
                for (int i = 0; i < animationManager.GetAnimationCount(); i++)
                {
                    var animationValue = animationManager.GetProgress(i);
                    var animationSource = animationManager.GetSource(i);
                    var rippleBrush = new SolidBrush(Color.FromArgb((int)(51 - (animationValue * 50)), Color.White));
                    var rippleSize = (int)(animationValue * Width * 2);
                    g.FillEllipse(rippleBrush, new Rectangle(animationSource.X - rippleSize / 2, animationSource.Y - rippleSize / 2, rippleSize, rippleSize));
                }
            }

            g.DrawString(
                Text.ToUpper(),
                new Font("Arial", 9),
                SkinManager.GetRaisedButtonTextBrush(Primary),
                ClientRectangle,
                new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
        }
    }

    #endregion
    #region MaterialSingleLineTextField

    public class MaterialSingleLineTextField : Control, IMaterialControl
    {
        //Properties for managing the material design properties
        [Browsable(false)]
        public int Depth { get; set; }
        [Browsable(false)]
        public MaterialSkinManager SkinManager { get { return MaterialSkinManager.Instance; } }
        [Browsable(false)]
        public MouseState MouseState { get; set; }

        public override string Text { get { return baseTextBox.Text; } set { baseTextBox.Text = value; } }
        public new object Tag { get { return baseTextBox.Tag; } set { baseTextBox.Tag = value; } }
        public int MaxLength { get { return baseTextBox.MaxLength; } set { baseTextBox.MaxLength = value; } }

        public string SelectedText { get { return baseTextBox.SelectedText; } set { baseTextBox.SelectedText = value; } }
        public string Hint { get { return baseTextBox.Hint; } set { baseTextBox.Hint = value; } }

        public int SelectionStart { get { return baseTextBox.SelectionStart; } set { baseTextBox.SelectionStart = value; } }
        public int SelectionLength { get { return baseTextBox.SelectionLength; } set { baseTextBox.SelectionLength = value; } }
        public int TextLength { get { return baseTextBox.TextLength; } }

        public bool UseSystemPasswordChar { get { return baseTextBox.UseSystemPasswordChar; } set { baseTextBox.UseSystemPasswordChar = value; } }
        public char PasswordChar { get { return baseTextBox.PasswordChar; } set { baseTextBox.PasswordChar = value; } }
    
        public void SelectAll() { baseTextBox.SelectAll(); }
        public void Clear() { baseTextBox.Clear(); }
        public void ScrollToCaret() { baseTextBox.ScrollToCaret(); }
        # region Forwarding events to baseTextBox
        public event EventHandler AcceptsTabChanged
        {
            add
            {
                baseTextBox.AcceptsTabChanged += value;
            }
            remove
            {
                baseTextBox.AcceptsTabChanged -= value;
            }
        }

        public new event EventHandler AutoSizeChanged
        {
            add
            {
                baseTextBox.AutoSizeChanged += value;
            }
            remove
            {
                baseTextBox.AutoSizeChanged -= value;
            }
        }

        public new event EventHandler BackgroundImageChanged
        {
            add
            {
                baseTextBox.BackgroundImageChanged += value;
            }
            remove
            {
                baseTextBox.BackgroundImageChanged -= value;
            }
        }

        public new event EventHandler BackgroundImageLayoutChanged
        {
            add
            {
                baseTextBox.BackgroundImageLayoutChanged += value;
            }
            remove
            {
                baseTextBox.BackgroundImageLayoutChanged -= value;
            }
        }

        public new event EventHandler BindingContextChanged
        {
            add
            {
                baseTextBox.BindingContextChanged += value;
            }
            remove
            {
                baseTextBox.BindingContextChanged -= value;
            }
        }

        public event EventHandler BorderStyleChanged
        {
            add
            {
                baseTextBox.BorderStyleChanged += value;
            }
            remove
            {
                baseTextBox.BorderStyleChanged -= value;
            }
        }

        public new event EventHandler CausesValidationChanged
        {
            add
            {
                baseTextBox.CausesValidationChanged += value;
            }
            remove
            {
                baseTextBox.CausesValidationChanged -= value;
            }
        }

        public new event UICuesEventHandler ChangeUICues
        {
            add
            {
                baseTextBox.ChangeUICues += value;
            }
            remove
            {
                baseTextBox.ChangeUICues -= value;
            }
        }

        public new event EventHandler Click
        {
            add
            {
                baseTextBox.Click += value;
            }
            remove
            {
                baseTextBox.Click -= value;
            }
        }

        public new event EventHandler ClientSizeChanged
        {
            add
            {
                baseTextBox.ClientSizeChanged += value;
            }
            remove
            {
                baseTextBox.ClientSizeChanged -= value;
            }
        }

        public new event EventHandler ContextMenuChanged
        {
            add
            {
                baseTextBox.ContextMenuChanged += value;
            }
            remove
            {
                baseTextBox.ContextMenuChanged -= value;
            }
        }

        public new event EventHandler ContextMenuStripChanged
        {
            add
            {
                baseTextBox.ContextMenuStripChanged += value;
            }
            remove
            {
                baseTextBox.ContextMenuStripChanged -= value;
            }
        }

        public new event ControlEventHandler ControlAdded
        {
            add
            {
                baseTextBox.ControlAdded += value;
            }
            remove
            {
                baseTextBox.ControlAdded -= value;
            }
        }

        public new event ControlEventHandler ControlRemoved
        {
            add
            {
                baseTextBox.ControlRemoved += value;
            }
            remove
            {
                baseTextBox.ControlRemoved -= value;
            }
        }

        public new event EventHandler CursorChanged
        {
            add
            {
                baseTextBox.CursorChanged += value;
            }
            remove
            {
                baseTextBox.CursorChanged -= value;
            }
        }

        public new event EventHandler Disposed
        {
            add
            {
                baseTextBox.Disposed += value;
            }
            remove
            {
                baseTextBox.Disposed -= value;
            }
        }

        public new event EventHandler DockChanged
        {
            add
            {
                baseTextBox.DockChanged += value;
            }
            remove
            {
                baseTextBox.DockChanged -= value;
            }
        }

        public new event EventHandler DoubleClick
        {
            add
            {
                baseTextBox.DoubleClick += value;
            }
            remove
            {
                baseTextBox.DoubleClick -= value;
            }
        }

        public new event DragEventHandler DragDrop
        {
            add
            {
                baseTextBox.DragDrop += value;
            }
            remove
            {
                baseTextBox.DragDrop -= value;
            }
        }

        public new event DragEventHandler DragEnter
        {
            add
            {
                baseTextBox.DragEnter += value;
            }
            remove
            {
                baseTextBox.DragEnter -= value;
            }
        }

        public new event EventHandler DragLeave
        {
            add
            {
                baseTextBox.DragLeave += value;
            }
            remove
            {
                baseTextBox.DragLeave -= value;
            }
        }

        public new event DragEventHandler DragOver
        {
            add
            {
                baseTextBox.DragOver += value;
            }
            remove
            {
                baseTextBox.DragOver -= value;
            }
        }

        public new event EventHandler EnabledChanged
        {
            add
            {
                baseTextBox.EnabledChanged += value;
            }
            remove
            {
                baseTextBox.EnabledChanged -= value;
            }
        }

        public new event EventHandler Enter
        {
            add
            {
                baseTextBox.Enter += value;
            }
            remove
            {
                baseTextBox.Enter -= value;
            }
        }

        public new event EventHandler FontChanged
        {
            add
            {
                baseTextBox.FontChanged += value;
            }
            remove
            {
                baseTextBox.FontChanged -= value;
            }
        }

        public new event EventHandler ForeColorChanged
        {
            add
            {
                baseTextBox.ForeColorChanged += value;
            }
            remove
            {
                baseTextBox.ForeColorChanged -= value;
            }
        }

        public new event GiveFeedbackEventHandler GiveFeedback
        {
            add
            {
                baseTextBox.GiveFeedback += value;
            }
            remove
            {
                baseTextBox.GiveFeedback -= value;
            }
        }

        public new event EventHandler GotFocus
        {
            add
            {
                baseTextBox.GotFocus += value;
            }
            remove
            {
                baseTextBox.GotFocus -= value;
            }
        }

        public new event EventHandler HandleCreated
        {
            add
            {
                baseTextBox.HandleCreated += value;
            }
            remove
            {
                baseTextBox.HandleCreated -= value;
            }
        }

        public new event EventHandler HandleDestroyed
        {
            add
            {
                baseTextBox.HandleDestroyed += value;
            }
            remove
            {
                baseTextBox.HandleDestroyed -= value;
            }
        }

        public new event HelpEventHandler HelpRequested
        {
            add
            {
                baseTextBox.HelpRequested += value;
            }
            remove
            {
                baseTextBox.HelpRequested -= value;
            }
        }

        public event EventHandler HideSelectionChanged
        {
            add
            {
                baseTextBox.HideSelectionChanged += value;
            }
            remove
            {
                baseTextBox.HideSelectionChanged -= value;
            }
        }

        public new event EventHandler ImeModeChanged
        {
            add
            {
                baseTextBox.ImeModeChanged += value;
            }
            remove
            {
                baseTextBox.ImeModeChanged -= value;
            }
        }

        public new event InvalidateEventHandler Invalidated
        {
            add
            {
                baseTextBox.Invalidated += value;
            }
            remove
            {
                baseTextBox.Invalidated -= value;
            }
        }

        public new event KeyEventHandler KeyDown
        {
            add
            {
                baseTextBox.KeyDown += value;
            }
            remove
            {
                baseTextBox.KeyDown -= value;
            }
        }

        public new event KeyPressEventHandler KeyPress
        {
            add
            {
                baseTextBox.KeyPress += value;
            }
            remove
            {
                baseTextBox.KeyPress -= value;
            }
        }

        public new event KeyEventHandler KeyUp
        {
            add
            {
                baseTextBox.KeyUp += value;
            }
            remove
            {
                baseTextBox.KeyUp -= value;
            }
        }

        public new event LayoutEventHandler Layout
        {
            add
            {
                baseTextBox.Layout += value;
            }
            remove
            {
                baseTextBox.Layout -= value;
            }
        }

        public new event EventHandler Leave
        {
            add
            {
                baseTextBox.Leave += value;
            }
            remove
            {
                baseTextBox.Leave -= value;
            }
        }

        public new event EventHandler LocationChanged
        {
            add
            {
                baseTextBox.LocationChanged += value;
            }
            remove
            {
                baseTextBox.LocationChanged -= value;
            }
        }

        public new event EventHandler LostFocus
        {
            add
            {
                baseTextBox.LostFocus += value;
            }
            remove
            {
                baseTextBox.LostFocus -= value;
            }
        }

        public new event EventHandler MarginChanged
        {
            add
            {
                baseTextBox.MarginChanged += value;
            }
            remove
            {
                baseTextBox.MarginChanged -= value;
            }
        }

        public event EventHandler ModifiedChanged
        {
            add
            {
                baseTextBox.ModifiedChanged += value;
            }
            remove
            {
                baseTextBox.ModifiedChanged -= value;
            }
        }

        public new event EventHandler MouseCaptureChanged
        {
            add
            {
                baseTextBox.MouseCaptureChanged += value;
            }
            remove
            {
                baseTextBox.MouseCaptureChanged -= value;
            }
        }

        public new event MouseEventHandler MouseClick
        {
            add
            {
                baseTextBox.MouseClick += value;
            }
            remove
            {
                baseTextBox.MouseClick -= value;
            }
        }

        public new event MouseEventHandler MouseDoubleClick
        {
            add
            {
                baseTextBox.MouseDoubleClick += value;
            }
            remove
            {
                baseTextBox.MouseDoubleClick -= value;
            }
        }

        public new event MouseEventHandler MouseDown
        {
            add
            {
                baseTextBox.MouseDown += value;
            }
            remove
            {
                baseTextBox.MouseDown -= value;
            }
        }

        public new event EventHandler MouseEnter
        {
            add
            {
                baseTextBox.MouseEnter += value;
            }
            remove
            {
                baseTextBox.MouseEnter -= value;
            }
        }

        public new event EventHandler MouseHover
        {
            add
            {
                baseTextBox.MouseHover += value;
            }
            remove
            {
                baseTextBox.MouseHover -= value;
            }
        }

        public new event EventHandler MouseLeave
        {
            add
            {
                baseTextBox.MouseLeave += value;
            }
            remove
            {
                baseTextBox.MouseLeave -= value;
            }
        }

        public new event MouseEventHandler MouseMove
        {
            add
            {
                baseTextBox.MouseMove += value;
            }
            remove
            {
                baseTextBox.MouseMove -= value;
            }
        }

        public new event MouseEventHandler MouseUp
        {
            add
            {
                baseTextBox.MouseUp += value;
            }
            remove
            {
                baseTextBox.MouseUp -= value;
            }
        }

        public new event MouseEventHandler MouseWheel
        {
            add
            {
                baseTextBox.MouseWheel += value;
            }
            remove
            {
                baseTextBox.MouseWheel -= value;
            }
        }

        public new event EventHandler Move
        {
            add
            {
                baseTextBox.Move += value;
            }
            remove
            {
                baseTextBox.Move -= value;
            }
        }

        public event EventHandler MultilineChanged
        {
            add
            {
                baseTextBox.MultilineChanged += value;
            }
            remove
            {
                baseTextBox.MultilineChanged -= value;
            }
        }

        public new event EventHandler PaddingChanged
        {
            add
            {
                baseTextBox.PaddingChanged += value;
            }
            remove
            {
                baseTextBox.PaddingChanged -= value;
            }
        }

        public new event PaintEventHandler Paint
        {
            add
            {
                baseTextBox.Paint += value;
            }
            remove
            {
                baseTextBox.Paint -= value;
            }
        }

        public new event EventHandler ParentChanged
        {
            add
            {
                baseTextBox.ParentChanged += value;
            }
            remove
            {
                baseTextBox.ParentChanged -= value;
            }
        }

        public new event PreviewKeyDownEventHandler PreviewKeyDown
        {
            add
            {
                baseTextBox.PreviewKeyDown += value;
            }
            remove
            {
                baseTextBox.PreviewKeyDown -= value;
            }
        }

        public new event QueryAccessibilityHelpEventHandler QueryAccessibilityHelp
        {
            add
            {
                baseTextBox.QueryAccessibilityHelp += value;
            }
            remove
            {
                baseTextBox.QueryAccessibilityHelp -= value;
            }
        }

        public new event QueryContinueDragEventHandler QueryContinueDrag
        {
            add
            {
                baseTextBox.QueryContinueDrag += value;
            }
            remove
            {
                baseTextBox.QueryContinueDrag -= value;
            }
        }

        public event EventHandler ReadOnlyChanged
        {
            add
            {
                baseTextBox.ReadOnlyChanged += value;
            }
            remove
            {
                baseTextBox.ReadOnlyChanged -= value;
            }
        }

        public new event EventHandler RegionChanged
        {
            add
            {
                baseTextBox.RegionChanged += value;
            }
            remove
            {
                baseTextBox.RegionChanged -= value;
            }
        }

        public new event EventHandler Resize
        {
            add
            {
                baseTextBox.Resize += value;
            }
            remove
            {
                baseTextBox.Resize -= value;
            }
        }

        public new event EventHandler RightToLeftChanged
        {
            add
            {
                baseTextBox.RightToLeftChanged += value;
            }
            remove
            {
                baseTextBox.RightToLeftChanged -= value;
            }
        }

        public new event EventHandler SizeChanged
        {
            add
            {
                baseTextBox.SizeChanged += value;
            }
            remove
            {
                baseTextBox.SizeChanged -= value;
            }
        }

        public new event EventHandler StyleChanged
        {
            add
            {
                baseTextBox.StyleChanged += value;
            }
            remove
            {
                baseTextBox.StyleChanged -= value;
            }
        }

        public new event EventHandler SystemColorsChanged
        {
            add
            {
                baseTextBox.SystemColorsChanged += value;
            }
            remove
            {
                baseTextBox.SystemColorsChanged -= value;
            }
        }

        public new event EventHandler TabIndexChanged
        {
            add
            {
                baseTextBox.TabIndexChanged += value;
            }
            remove
            {
                baseTextBox.TabIndexChanged -= value;
            }
        }

        public new event EventHandler TabStopChanged
        {
            add
            {
                baseTextBox.TabStopChanged += value;
            }
            remove
            {
                baseTextBox.TabStopChanged -= value;
            }
        }

        public event EventHandler TextAlignChanged
        {
            add
            {
                baseTextBox.TextAlignChanged += value;
            }
            remove
            {
                baseTextBox.TextAlignChanged -= value;
            }
        }

        public new event EventHandler TextChanged
        {
            add
            {
                baseTextBox.TextChanged += value;
            }
            remove
            {
                baseTextBox.TextChanged -= value;
            }
        }

        public new event EventHandler Validated
        {
            add
            {
                baseTextBox.Validated += value;
            }
            remove
            {
                baseTextBox.Validated -= value;
            }
        }

        public new event CancelEventHandler Validating
        {
            add
            {
                baseTextBox.Validating += value;
            }
            remove
            {
                baseTextBox.Validating -= value;
            }
        }

        public new event EventHandler VisibleChanged
        {
            add
            {
                baseTextBox.VisibleChanged += value;
            }
            remove
            {
                baseTextBox.VisibleChanged -= value;
            }
        }
        # endregion

        private readonly AnimationManager animationManager;

        private readonly BaseTextBox baseTextBox;
        public MaterialSingleLineTextField()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.DoubleBuffer, true);

            animationManager = new AnimationManager
            {
                Increment = 0.06,
                AnimationType = AnimationType.EaseInOut,
                InterruptAnimation = false
            };
            animationManager.OnAnimationProgress += sender => Invalidate();

            baseTextBox = new BaseTextBox
            {
                BorderStyle = BorderStyle.None,
                ForeColor = SkinManager.GetPrimaryTextColor(),
                BackColor = Color.White,
                Location = new Point(0, 0),
                Width = Width,
                Height = Height - 5
            };

            if (!Controls.Contains(baseTextBox) && !DesignMode)
            {
                Controls.Add(baseTextBox);
            }

            baseTextBox.GotFocus += (sender, args) => animationManager.StartNewAnimation(AnimationDirection.In);
            baseTextBox.LostFocus += (sender, args) => animationManager.StartNewAnimation(AnimationDirection.Out);
            BackColorChanged += (sender, args) =>
            {
                baseTextBox.BackColor = BackColor;
                baseTextBox.ForeColor = SkinManager.GetPrimaryTextColor();
            };

            //Fix for tabstop
            baseTextBox.TabStop = true;
            this.TabStop = false;
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            var g = pevent.Graphics;
            g.Clear(Parent.BackColor);

            int lineY = baseTextBox.Bottom + 3;

            if (!animationManager.IsAnimating())
            {
                //No animation
                g.FillRectangle(baseTextBox.Focused ? SkinManager.ColorScheme.PrimaryBrush : SkinManager.GetDividersBrush(), baseTextBox.Location.X, lineY, baseTextBox.Width, baseTextBox.Focused ? 2 : 1);
            }
            else
            {
                //Animate
                int animationWidth = (int)(baseTextBox.Width * animationManager.GetProgress());
                int halfAnimationWidth = animationWidth / 2;
                int animationStart = baseTextBox.Location.X + baseTextBox.Width / 2;

                //Unfocused background
                g.FillRectangle(SkinManager.GetDividersBrush(), baseTextBox.Location.X, lineY, baseTextBox.Width, 1);

                //Animated focus transition
                g.FillRectangle(SkinManager.ColorScheme.PrimaryBrush, animationStart - halfAnimationWidth, lineY, animationWidth, 2);
            }
            
            baseTextBox.Refresh();
        }

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wp, IntPtr lp);
        
        private void OnClick(object sender, EventArgs e)
        {
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            baseTextBox.Location = new Point(0, 0);
            baseTextBox.Width = Width;

            Height = baseTextBox.Height + 5;
            
            baseTextBox.Refresh();
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();

            baseTextBox.BackColor = Parent.BackColor;
            baseTextBox.ForeColor = SkinManager.GetPrimaryTextColor();
        }

        private class BaseTextBox : TextBox
        {
            [DllImport("user32.dll", CharSet = CharSet.Auto)]
            private static extern IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, string lParam);

            private const int EM_SETCUEBANNER = 0x1501;
            private const char EmptyChar = (char)0;
            private const char VisualStylePasswordChar = '\u25CF';
            private const char NonVisualStylePasswordChar = '\u002A';

            private string hint = string.Empty;
            public string Hint
            {
                get { return hint; }
                set
                {
                    hint = value;
                    SendMessage(Handle, EM_SETCUEBANNER, (int)IntPtr.Zero, Hint);
                }
            }

            private char passwordChar = EmptyChar;
            public new char PasswordChar
            {
                get { return passwordChar; }
                set
                {
                    passwordChar = value;
                    SetBasePasswordChar();
                }
            }

            public new void SelectAll()
            {
                BeginInvoke((MethodInvoker)delegate ()
                {
                    base.Focus();
                    base.SelectAll();
                });
            }


            private char useSystemPasswordChar = EmptyChar;
            public new bool UseSystemPasswordChar
            {
                get { return useSystemPasswordChar != EmptyChar; }
                set
                {
                    if (value)
                    {
                        useSystemPasswordChar = Application.RenderWithVisualStyles ? VisualStylePasswordChar : NonVisualStylePasswordChar;
                    }
                    else
                    {
                        useSystemPasswordChar = EmptyChar;
                    }

                    SetBasePasswordChar();
                }
            }

            private void SetBasePasswordChar()
            {
                base.PasswordChar = UseSystemPasswordChar ? useSystemPasswordChar : passwordChar;
            }

            public BaseTextBox()
            {
                MaterialContextMenuStrip cms = new TextBoxContextMenuStrip();
                cms.Opening += ContextMenuStripOnOpening;
                cms.OnItemClickStart += ContextMenuStripOnItemClickStart;

                ContextMenuStrip = cms;
            }

            private void ContextMenuStripOnItemClickStart(object sender, ToolStripItemClickedEventArgs toolStripItemClickedEventArgs)
            {
                switch (toolStripItemClickedEventArgs.ClickedItem.Text)
                {
                    case "Undo":
                        Undo();
                        break;
                    case "Cut":
                        Cut();
                        break;
                    case "Copy":
                        Copy();
                        break;
                    case "Paste":
                        Paste();
                        break;
                    case "Delete":
                        SelectedText = string.Empty;
                        break;
                    case "Select All":
                        SelectAll();
                        break;
                }
            }

            private void ContextMenuStripOnOpening(object sender, CancelEventArgs cancelEventArgs)
            {
                var strip = sender as TextBoxContextMenuStrip;
                if (strip != null)
                {
                    strip.undo.Enabled = CanUndo;
                    strip.cut.Enabled = !string.IsNullOrEmpty(SelectedText);
                    strip.copy.Enabled = !string.IsNullOrEmpty(SelectedText);
                    strip.paste.Enabled = Clipboard.ContainsText();
                    strip.delete.Enabled = !string.IsNullOrEmpty(SelectedText);
                    strip.selectAll.Enabled = !string.IsNullOrEmpty(Text);
                }
            }
        }

        private class TextBoxContextMenuStrip : MaterialContextMenuStrip
        {
            public readonly ToolStripItem undo = new MaterialToolStripMenuItem { Text = "Undo" };
            public readonly ToolStripItem seperator1 = new ToolStripSeparator();
            public readonly ToolStripItem cut = new MaterialToolStripMenuItem { Text = "Cut" };
            public readonly ToolStripItem copy = new MaterialToolStripMenuItem { Text = "Copy" };
            public readonly ToolStripItem paste = new MaterialToolStripMenuItem { Text = "Paste" };
            public readonly ToolStripItem delete = new MaterialToolStripMenuItem { Text = "Delete" };
            public readonly ToolStripItem seperator2 = new ToolStripSeparator();
            public readonly ToolStripItem selectAll = new MaterialToolStripMenuItem { Text = "Select All" };

            public TextBoxContextMenuStrip()
            {
                Items.AddRange(new[]
                {
                    undo,
                    seperator1,
                    cut,
                    copy,
                    paste,
                    delete,
                    seperator2,
                    selectAll
                });
            }
        }
    }

    #endregion
    #region MaterialTabControl

    public class MaterialTabControl : TabControl, IMaterialControl
    {
        [Browsable(false)]
        public int Depth { get; set; }
        [Browsable(false)]
        public MaterialSkinManager SkinManager { get { return MaterialSkinManager.Instance; } }
        [Browsable(false)]
        public MouseState MouseState { get; set; }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x1328 && !DesignMode) m.Result = (IntPtr)1;
            else base.WndProc(ref m);
        }
    }

    #endregion
    #region MaterialTabSelector

    public class MaterialTabSelector : Control, IMaterialControl
    {
        [Browsable(false)]
        public int Depth { get; set; }
        [Browsable(false)]
        public MaterialSkinManager SkinManager { get { return MaterialSkinManager.Instance; } }
        [Browsable(false)]
        public MouseState MouseState { get; set; }

        private MaterialTabControl baseTabControl;
        public MaterialTabControl BaseTabControl
        {
            get { return baseTabControl; }
            set
            {
                baseTabControl = value;
                if (baseTabControl == null) return;
                previousSelectedTabIndex = baseTabControl.SelectedIndex;
                baseTabControl.Deselected += (sender, args) =>
                {
                    previousSelectedTabIndex = baseTabControl.SelectedIndex;
                };
                baseTabControl.SelectedIndexChanged += (sender, args) =>
                {
                    animationManager.SetProgress(0);
                    animationManager.StartNewAnimation(AnimationDirection.In);
                };
                baseTabControl.ControlAdded += delegate
                {
                    Invalidate();
                };
                baseTabControl.ControlRemoved += delegate
                {
                    Invalidate();
                };
            }
        }

        private int previousSelectedTabIndex;
        private Point animationSource;
        private readonly AnimationManager animationManager;

        private List<Rectangle> tabRects;
        private const int TAB_HEADER_PADDING = 24;
        private const int TAB_INDICATOR_HEIGHT = 2;

        public MaterialTabSelector()
        {
            SetStyle(ControlStyles.DoubleBuffer | ControlStyles.OptimizedDoubleBuffer, true);
            Height = 48;

            animationManager = new AnimationManager
            {
                AnimationType = AnimationType.EaseOut,
                Increment = 0.04
            };
            animationManager.OnAnimationProgress += sender => Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.TextRenderingHint = TextRenderingHint.AntiAlias;

            g.Clear(SkinManager.ColorScheme.PrimaryColor);

            if (baseTabControl == null) return;

            if (!animationManager.IsAnimating() || tabRects == null || tabRects.Count != baseTabControl.TabCount)
                UpdateTabRects();

            double animationProgress = animationManager.GetProgress();

            //Click feedback
            if (animationManager.IsAnimating())
            {
                var rippleBrush = new SolidBrush(Color.FromArgb((int)(51 - (animationProgress * 50)), Color.White));
                var rippleSize = (int)(animationProgress * tabRects[baseTabControl.SelectedIndex].Width * 1.75);

                g.SetClip(tabRects[baseTabControl.SelectedIndex]);
                g.FillEllipse(rippleBrush, new Rectangle(animationSource.X - rippleSize / 2, animationSource.Y - rippleSize / 2, rippleSize, rippleSize));
                g.ResetClip();
                rippleBrush.Dispose();
            }

            //Draw tab headers
            foreach (TabPage tabPage in baseTabControl.TabPages)
            {
                int currentTabIndex = baseTabControl.TabPages.IndexOf(tabPage);
                Brush textBrush = new SolidBrush(Color.FromArgb(CalculateTextAlpha(currentTabIndex, animationProgress), SkinManager.ColorScheme.TextColor));

                g.DrawString(
                    tabPage.Text.ToUpper(),
                    new Font("Arial", 9, FontStyle.Regular),
                    textBrush,
                    tabRects[currentTabIndex],
                    new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                textBrush.Dispose();
            }

            //Animate tab indicator
            int previousSelectedTabIndexIfHasOne = previousSelectedTabIndex == -1 ? baseTabControl.SelectedIndex : previousSelectedTabIndex;
            Rectangle previousActiveTabRect = tabRects[previousSelectedTabIndexIfHasOne];
            Rectangle activeTabPageRect = tabRects[baseTabControl.SelectedIndex];

            int y = activeTabPageRect.Bottom - 2;
            int x = previousActiveTabRect.X + (int)((activeTabPageRect.X - previousActiveTabRect.X) * animationProgress);
            int width = previousActiveTabRect.Width + (int)((activeTabPageRect.Width - previousActiveTabRect.Width) * animationProgress);

            g.FillRectangle(SkinManager.ColorScheme.SecondaryBrush, x, y, width, TAB_INDICATOR_HEIGHT);
        }

        private int CalculateTextAlpha(int tabIndex, double animationProgress)
        {
            int primaryA = SkinManager.ACTION_BAR_TEXT.A;
            int secondaryA = SkinManager.ACTION_BAR_TEXT_SECONDARY.A;

            if (tabIndex == baseTabControl.SelectedIndex && !animationManager.IsAnimating())
            {
                return primaryA;
            }
            if (tabIndex != previousSelectedTabIndex && tabIndex != baseTabControl.SelectedIndex)
            {
                return secondaryA;
            }
            if (tabIndex == previousSelectedTabIndex)
            {
                return primaryA - (int)((primaryA - secondaryA) * animationProgress);
            }
            return secondaryA + (int)((primaryA - secondaryA) * animationProgress);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            if (tabRects == null) UpdateTabRects();
            for (int i = 0; i < tabRects.Count; i++)
            {
                if (tabRects[i].Contains(e.Location))
                {
                    baseTabControl.SelectedIndex = i;
                }
            }

            animationSource = e.Location;
        }

        private void UpdateTabRects()
        {
            tabRects = new List<Rectangle>();

            //If there isn't a base tab control, the rects shouldn't be calculated
            //If there aren't tab pages in the base tab control, the list should just be empty which has been set already; exit the void
            if (baseTabControl == null || baseTabControl.TabCount == 0) return;

            //Calculate the bounds of each tab header specified in the base tab control
            using (var b = new Bitmap(1, 1))
            {
                using (var g = Graphics.FromImage(b))
                {
                    tabRects.Add(new Rectangle(SkinManager.FORM_PADDING, 0, TAB_HEADER_PADDING * 2 + (int)g.MeasureString(baseTabControl.TabPages[0].Text, SkinManager.ARIAL_MEDIUM_10).Width, Height));
                    for (int i = 1; i < baseTabControl.TabPages.Count; i++)
                    {
                        tabRects.Add(new Rectangle(tabRects[i - 1].Right, 0, TAB_HEADER_PADDING * 2 + (int)g.MeasureString(baseTabControl.TabPages[i].Text, SkinManager.ARIAL_MEDIUM_10).Width, Height));
                    }
                }
            }
        }
    }

    #endregion
}
