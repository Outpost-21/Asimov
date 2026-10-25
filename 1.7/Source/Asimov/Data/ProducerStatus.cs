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
    public enum ProducerStatus
    {
        /// <summary>
        /// Idle means it has no recipe.
        /// </summary>
        idle,
        /// <summary>
        /// AwaitingResources means it has a recipe but currently doesn't have the resources to craft.
        /// </summary>
        awaitingResources,
        /// <summary>
        /// Working means it has both a recipe and the resources to craft and is currently processing.
        /// Resources are stored inside a buffer once this stage is active, cancelling will drop the resources.
        /// </summary>
        working,
        /// <summary>
        /// Likely never seen as it happens quickly, Producing means it is currently spitting out the resulting products.
        /// </summary>
        producing
    }
}
