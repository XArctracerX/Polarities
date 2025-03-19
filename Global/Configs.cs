using System;
using System.ComponentModel;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;
using Terraria.Graphics.CameraModifiers;

namespace Polarities.Global
{
    public class ClientConfigurations : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;

        [Label("Convective Wanderer Screenshake")]
        [Tooltip("Controls the amount of screenshake that Convective Wanderer applies.")]
        [DefaultValue(1f)]
        public float CW_SCREENSHAKE_MULTIPLIER;

        [Label("Railgun Screenshake")]
        [Tooltip("Controls the amount of screenshake that Railgun applies.")]
        [DefaultValue(1f)]
        public float RAILGUN_SCREENSHAKE_MULTIPLIER;

    }
}
