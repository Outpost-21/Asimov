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
    [StaticConstructorOnStartup]
    internal class TexUtil
    {
        public static readonly Texture2D Plus = ContentFinder<Texture2D>.Get("Asimov/UI/Plus", true);
        public static readonly Texture2D Minus = ContentFinder<Texture2D>.Get("Asimov/UI/Minus", true);
    }
}
