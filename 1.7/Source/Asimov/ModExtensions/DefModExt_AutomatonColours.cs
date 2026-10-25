
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
    public class DefModExt_AutomatonColours : DefModExtension
    {
        public List<ColorPair> skinColorPairs = new List<ColorPair>();

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
