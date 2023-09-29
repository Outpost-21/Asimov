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
    public class CompProperties_RecolourablePawn : CompProperties
    {
        public bool channelOne = false;

        public bool channelTwo = false;

        public List<ColorPair> skinColorPairs = new List<ColorPair>();

        public CompProperties_RecolourablePawn()
        {
            compClass = typeof(Comp_RecolourablePawn);
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
