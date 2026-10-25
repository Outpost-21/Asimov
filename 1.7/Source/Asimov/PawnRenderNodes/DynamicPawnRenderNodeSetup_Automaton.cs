using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Asimov
{
    public class DynamicPawnRenderNodeSetup_Automaton : DynamicPawnRenderNodeSetup
    {
        public override bool HumanlikeOnly => true;

        public override IEnumerable<(PawnRenderNode node, PawnRenderNode parent)> GetDynamicNodes(Pawn pawn, PawnRenderTree tree)
        {
            PawnDef pd = pawn.kindDef.race as PawnDef;
            if (pd == null) { yield break; }
            foreach(PawnRenderNodeProperties renderNodeProperty in pd.pawnSettings.RenderNodeProperties)
            {
                if (tree.ShouldAddNodeToTree(renderNodeProperty))
                {
                    PawnRenderNode pawnRenderNode = (PawnRenderNode)Activator.CreateInstance(renderNodeProperty.nodeClass, pawn, renderNodeProperty, tree);
                    yield return (pawnRenderNode, null);
                }
            }
        }
    }
}
