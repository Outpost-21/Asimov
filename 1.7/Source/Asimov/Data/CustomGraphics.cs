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
    public class CustomGraphics
    {
        public Vector2 headScale;
        public string bodyPath = "Things/Pawn/Humanlike/Bodies/";
        public Vector2 bodyScale;
        public string skull = "Things/Pawn/Humanlike/Heads/None_Average_Skull";
        public string skeleton = "Things/Pawn/Humanlike/HumanoidDessicated";
        public string stump = "Things/Pawn/Humanlike/Heads/None_Average_Stump";
        public List<ColorPair> skinColorPairs = new List<ColorPair>();
        public List<ColorPair> hairColorPairs = new List<ColorPair>();
        public ShaderTypeDef shaderType;
        //public List<BodyAddon> bodyAddons = new List<BodyAddon>();

        public Shader Shader
        {
            get
            {
                if(shaderType == null)
                {
                    return ShaderDatabase.Cutout;
                }
                return shaderType.Shader;
            }
        }

        public ColorPair GetSkinColor
        {
            get
            {
                if (skinColorPairs.NullOrEmpty())
                {
                    return new ColorPair();
                }
                return skinColorPairs.RandomElementByWeight((ColorPair x) => x.weight);
            }
        }
    }
}
