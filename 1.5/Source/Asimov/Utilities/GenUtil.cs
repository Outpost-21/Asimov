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
    public static class GenUtil
    {
        public static BodyTypeDef GetBodyType(Pawn pawn)
        {
            BodyTypeDef bodyTypeDef = pawn?.story?.bodyType ?? null;
            if(pawn.def is PawnDef def)
            {
                if(def.pawnSettings != null && !def.pawnSettings.bodyTypeWhitelist.NullOrEmpty())
                {
                    List<BodyTypeDef> whitelist = def.pawnSettings.bodyTypeWhitelist;
                    if (pawn.gender == Gender.Male && whitelist.Contains(BodyTypeDefOf.Female) && whitelist.Count > 1)
                    {
                        whitelist.Remove(BodyTypeDefOf.Female);
                    }
                    if (pawn.gender == Gender.Female && whitelist.Contains(BodyTypeDefOf.Male) && whitelist.Count > 1)
                    {
                        whitelist.Remove(BodyTypeDefOf.Male);
                    }
                    if (!whitelist.Contains(bodyTypeDef))
                    {
                        bodyTypeDef = whitelist.RandomElement();
                    }
                }
            }
            return bodyTypeDef;
        }
    }
}
