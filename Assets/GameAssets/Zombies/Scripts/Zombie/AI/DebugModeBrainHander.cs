﻿using Assets.UnityFoundation.DebugHelper;
using UnityEngine;

namespace Assets.GameAssets.Zombies
{
    public class DebugModeBrainHander : BaseDecisionHandler<SimpleBrainContext>
    {
        private readonly SimpleBrain.Settings settings;

        public DebugModeBrainHander(SimpleBrain.Settings settings)
        {
            this.settings = settings;
        }
        public override bool OnHandle(SimpleBrainContext context)
        {
            if(!context.DebugMode)
                return true;

            DebugDraw.DrawSphere(
                context.Body.position, settings.MinAttackDistance, Color.red);
            DebugDraw.DrawSphere(
                context.Body.position, settings.MinDistanceForChasePlayer, Color.blue);

            return true;
        }
    }
}