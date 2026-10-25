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
    public static class PawnGraphicUtil
    {
        public static readonly Dictionary<GraphicRequest, Graphic> graphics = new Dictionary<GraphicRequest, Graphic>();

        public static T GetInner<T>(GraphicRequest req) where T : Graphic, new()
        {
            if(!graphics.TryGetValue(req, out Graphic graphic))
            {
                graphic = new T();
                graphic.Init(req);
                graphics.Add(req, graphic);
            }
            return (T)graphic;
        }

        public static string GetHeadPath(Pawn pawn)
        {
            return pawn.story.headType.graphicPath;
        }

        public static string GetBodyPath(Pawn pawn, string path)
        {
            if(path == null)
            {
                return null;
            }
            return path + "Naked_" + pawn.story.bodyType;
        }
    }
}
