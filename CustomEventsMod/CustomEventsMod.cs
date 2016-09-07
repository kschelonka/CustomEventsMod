using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Inheritance;
using StardewValley;
using StardewValley.Characters;
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace CustomEventsMod
{
    public class CustomEventsMod : Mod
    {
        public override void Entry(params object[] objects)
        {
            GameEvents.FirstUpdateTick += GameEvents_FirstUpdateTick;   // add and update api methods
            GameEvents.UpdateTick += GameEvents_UpdateTick;
            LocationEvents.CurrentLocationChanged += LocationEvents_CurrentLocationChanged;
            TimeEvents.OnNewDay += TimeEvents_OnNewDay;
        }

        private void GameEvents_FirstUpdateTick(object sender, EventArgs e)  // insert events when the game starts
        {
            MyNewEvents.InitializeEvents();
        }

        private void LocationEvents_CurrentLocationChanged(object sender, EventArgsCurrentLocationChanged e) // look for events when you change locations
        {
            MyNewEvents.CheckForEvent();
        }

        private void TimeEvents_OnNewDay(object sender, EventArgs e) // (optional) look for events when you wake up
        {
            MyNewEvents.CheckForEvent();
        }
        private void GameEvents_UpdateTick(object sender, EventArgs e)  // hoping to be able to get rid of this to make it faster
        {
            if (MyNewEvents.IsCustomEvent())
            {
                MyNewEvents.CheckCustomCommands();
            }


            /* here you can also modify character dialogue */


        }
    }
}
