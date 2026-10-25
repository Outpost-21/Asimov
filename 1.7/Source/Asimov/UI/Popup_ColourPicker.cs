using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace Asimov
{
    public class Popup_ColourPicker : Window
    {
        public Comp_RecolourablePawn comp;
        public Color color;
        public bool secondary = false;

        public float colorHue;
        public float colorSaturation;
        public float colorValue;

        public Color oldColor;

        public String bufferColorCode;

        public override Vector2 InitialSize
        {
            get
            {
                return new Vector2(500f, 380f);
            }
        }

        public Popup_ColourPicker(Comp_RecolourablePawn inComp, bool second)
        {
            comp = inComp;
            secondary = second;
            color = comp.GetSkinColor(second);
            Color.RGBToHSV(color, out colorHue, out colorSaturation, out colorValue);
            UpdateBufferColorCode();

            optionalTitle = "Asimov.ColorTitle".Translate();
            forcePause = true;
            absorbInputAroundWindow = true;
            closeOnClickedOutside = true;
        }

        public override void DoWindowContents(Rect inRect)
        {
            oldColor = color;

            Text.Font = GameFont.Medium;

            color.r = Widgets.HorizontalSlider(new Rect(160f, 0f, 200f, 30f), color.r, 0f, 1f, false, "R");
            color.g = Widgets.HorizontalSlider(new Rect(160f, 30f, 200f, 30f), color.g, 0f, 1f, false, "G");
            color.b = Widgets.HorizontalSlider(new Rect(160f, 60f, 200f, 30f), color.b, 0f, 1f, false, "B");
            if (color != oldColor)
            {
                Color.RGBToHSV(color, out colorHue, out colorSaturation, out colorValue);
            }

            colorHue = Widgets.HorizontalSlider(new Rect(160f, 110f, 200f, 30f), colorHue, 0f, 1f, false, "H");
            colorSaturation = Widgets.HorizontalSlider(new Rect(160f, 140f, 200f, 30f), colorSaturation, 0f, 1f, false, "S");
            colorValue = Widgets.HorizontalSlider(new Rect(160f, 170f, 200f, 30f), colorValue, 0f, 1f, false, "V");
            color = Color.HSVToRGB(colorHue, colorSaturation, colorValue);

            Text.Font = GameFont.Small;
            Widgets.Label(new Rect(160f, 220f, 120f, 25f), "Asimov.HexLabel".Translate());
            HexColorCodeField(new Rect(280f, 218f, 100f, 25f));

            DrawColourSquare(new Rect(13f, 36f, 128f, 128f));

            if (Widgets.ButtonText(new Rect(inRect.width / 2f - 50f, inRect.height - 40f, 100f, 40f), "Asimov.Apply".Translate(), true, false, true) || (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Return))
            {
                comp.SetSkinColor(color, secondary);
                Find.WindowStack.TryRemove(this, true);
            }
            Text.Font = GameFont.Medium;
        }

        private void DrawColourSquare(Rect rect)
        {
            GUI.color = color;
            Texture2D tex = ContentFinder<Texture2D>.Get("Asimov/UI/ColorPicker");
            GUI.DrawTexture(rect, tex);
            GUI.color = Color.white;
        }

        private void HexColorCodeField(Rect rect)
        {
            if (oldColor != color)
            {
                UpdateBufferColorCode();
            }

            bufferColorCode = Widgets.TextField(rect, bufferColorCode);
            Color outColor = Color.white;
            bool isColorFormat = ColorUtility.TryParseHtmlString(bufferColorCode, out outColor);
            Color textColor = isColorFormat ? Widgets.NormalOptionColor : new Color(0.5f, 0.5f, 0.5f);
            if (Widgets.ButtonText(new Rect(rect.xMax + 15, rect.y, 70f, rect.height), "Asimov.Apply".Translate(), false, false, textColor, true))
            {
                if (isColorFormat)
                {
                    color = outColor;
                    Color.RGBToHSV(color, out colorHue, out colorSaturation, out colorValue);
                }
                else
                {
                    Messages.Message("Asimov.HexColorCodeIsIllFormed".Translate(), MessageTypeDefOf.CautionInput);
                }
            }

        }

        private void UpdateBufferColorCode()
        {
            int r = (int)(color.r * 255);
            int g = (int)(color.g * 255);
            int b = (int)(color.b * 255);
            int code = r * 65536 + g * 256 + b;
            bufferColorCode = "#" + code.ToString("X6");
        }
    }
}
