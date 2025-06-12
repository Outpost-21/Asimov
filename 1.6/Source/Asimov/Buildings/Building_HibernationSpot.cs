using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;
using Verse.AI;

namespace Asimov
{
    public class Building_HibernationSpot : Building
    {
        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);
            EnergyUtil.GetEnergyNeedWorldComp.AddHibernationSpot(this);
        }

        public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
        {
            base.DeSpawn(mode);
            EnergyUtil.GetEnergyNeedWorldComp.RemoveHibernationSpot(this);
        }

        public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
        {
            base.Destroy(mode);
            EnergyUtil.GetEnergyNeedWorldComp.RemoveHibernationSpot(this);
        }

        public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Pawn selPawn)
        {
            foreach(FloatMenuOption option in base.GetFloatMenuOptions(selPawn))
            {
                yield return option;
            }
            if (selPawn.def.HasModExtension<DefModExt_EnergyNeed>())
            {
                Comp_Hibernation hibernates = selPawn.TryGetComp<Comp_Hibernation>();
                if (hibernates != null)
                {
                    if (selPawn.CanReserveAndReach(this, PathEndMode.OnCell, Danger.Deadly))
                    {
                        if (this.TryGetComp<CompPowerTrader>() is CompPowerTrader power)
                        {
                            if (power.PowerOn)
                            {
                                {
                                    FloatMenuOption option = new FloatMenuOption("Asimov.Hibernate".Translate(selPawn.Name.ToStringShort),
                                    delegate ()
                                    {
                                        selPawn.jobs.TryTakeOrderedJob(new Job(AsimovDefOf.Asimov_Hibernate, this), JobTag.Misc);
                                    });
                                    yield return option;
                                }
                                if (selPawn.health.hediffSet.HasTendableHediff()) 
                                {
                                    FloatMenuOption option = new FloatMenuOption("Asimov.HibernateTillRepaired".Translate(selPawn.Name.ToStringShort),
                                    delegate ()
                                    {
                                        selPawn.jobs.TryTakeOrderedJob(new Job(AsimovDefOf.Asimov_HibernateTillRepaired, this), JobTag.Misc);
                                    });
                                    yield return option;
                                }
                            }
                            else
                            {
                                FloatMenuOption option = new FloatMenuOption("Asimov.HibernateFailNoPower".Translate(selPawn.Name.ToStringShort, LabelCap), null);
                                option.Disabled = true;
                                yield return option;
                            }
                        }
                        else
                        {
                            FloatMenuOption option = new FloatMenuOption("Asimov.Hibernate".Translate(selPawn.Name.ToStringShort),
                            delegate ()
                            {
                                selPawn.jobs.TryTakeOrderedJob(new Job(AsimovDefOf.Asimov_Hibernate, this), JobTag.Misc);
                            });
                            yield return option;
                        }
                    }
                    else
                    {
                        FloatMenuOption option = new FloatMenuOption("Asimov.HibernateFailReserveOrReach".Translate(selPawn.Name.ToStringShort, LabelCap), null);
                        option.Disabled = true;
                        yield return option;
                    }
                }
            }
            else
            {
                FloatMenuOption option = new FloatMenuOption("Asimov.HibernateFail".Translate(selPawn.Name.ToStringShort), null);
                option.Disabled = true;
                yield return option;
            }
        }
    }
}
