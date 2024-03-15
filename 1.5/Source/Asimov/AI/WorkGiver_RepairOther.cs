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
    public class WorkGiver_RepairOther : WorkGiver_RepairPawn
	{
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (base.HasJobOnThing(pawn, t, forced))
			{
				return pawn != t;
			}
			return false;
		}
	}
}
