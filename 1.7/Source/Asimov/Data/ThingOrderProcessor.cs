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
    public class ThingOrderProcessor : IExposable
    {
        public ThingOwner thingHolder;

        public StorageSettings storageSettings;

        public List<ThingOrderRequest> requestedItems = new List<ThingOrderRequest>();

        public ThingOrderProcessor()
        {

        }

        public ThingOrderProcessor(ThingOwner thingHolder)
        {
            this.thingHolder = thingHolder;
        }

        public IEnumerable<ThingOrderRequest> PendingRequests()
        {
            foreach (ThingOrderRequest idealRequest in requestedItems)
            {

                float totalItemCount = thingHolder.TotalStackCountOfDef(idealRequest.thingDef);
                if (totalItemCount < idealRequest.amount)
                {
                    ThingOrderRequest request = new ThingOrderRequest();
                    request.thingDef = idealRequest.thingDef;
                    request.amount = idealRequest.amount - totalItemCount;

                    yield return request;
                }
            }
        }

        public void ExposeData()
        {
            Scribe_Collections.Look(ref requestedItems, "requestedItems", LookMode.Deep);
        }
    }
}
