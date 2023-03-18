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
    public class BodyAddon
    {
        public string texPath;
        public string texPathDestroyed;
        public string texPathDessicated;
        public BodyPartDef bodyPart;
        public ShaderTypeDef shaderType;
        public List<ColorPair> colorPairs = new List<ColorPair>();
        public GeneDrawLoc drawLocation;
        public GeneDrawLayer drawLayer;
        public bool drawDessicated;
        public bool drawInBed;
    }
}
