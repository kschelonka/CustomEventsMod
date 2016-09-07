using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Inheritance;
using StardewValley;
using StardewValley.Characters;
using StardewValley.Monsters;
using StardewValley.Objects;
using StardewValley.Quests;
using StardewValley.TerrainFeatures;
using System;
using System.Diagnostics;
using System.Collections.Generic;

namespace CustomEventsMod
{ 
    public class MyNewEvents
  {
    // add events here with unique keys
    public static int Hospital_SampleEvent = 938492;
    public static Dictionary<int, string> Events = new Dictionary<int, string>();


    // input the event text (like what's in the .xnb files)
    // remove the animate and stopAnimation lines to get the event to work
    // -----------------------------------
    // animate farmer false true 350 104 105 (line 36)
    // stopAnimate farmer (line 38)
    public static void InitializeEvents()
    {
        AddEvent(Hospital_SampleEvent, @"playful/-1000 -1000/farmer 4 5 2 Harvey 5 5 3 Sebastian 3 5 1/skippable/changeLocation Hospital/viewport 4 5 true
                /pause 1000
                /speak Harvey ""You're pregnant!$h""
                /pause 500
                /speak Sebastian ""I'm not ready to be a father...$s""
                /pause 500
                /animate farmer false true 350 104 105
                /pause 1500
                /stopAnimate farmer
                /pause 500
                /end");
    }
    public static bool CheckForEvent()
    {
        if (Game1.player == null || Game1.player.currentLocation == null || Game1.CurrentEvent != null)
            return false;
        bool startedEvent = false;

        // logic for starting event
        // will need separate statements for other events
        // but you could also make a hash of event names with location keys
        if (!Game1.player.eventsSeen.Contains(Hospital_SampleEvent)
            && Game1.player.currentLocation.Name == "BusStop"
            )
        {
            Game1.currentLocation.currentEvent = new CustomEvent(Events[Hospital_SampleEvent], Hospital_SampleEvent);   // queue event
            Game1.player.eventsSeen.Add(Hospital_SampleEvent);      // record event has been seen
            startedEvent = true;
        }
        // only need one of this part
        if (!startedEvent)
            return false;
        if (Game1.player.getMount() != null)
        {
            Game1.currentLocation.currentEvent.playerWasMounted = true;
            Game1.player.getMount().dismount();
        }
        foreach (NPC npc in Game1.currentLocation.characters)
            npc.clearTextAboveHead();
        Game1.eventUp = true;
        Game1.displayHUD = false;
        Game1.player.CanMove = false;
        Game1.player.showNotCarrying();
        return true;
    }
        // methods related to parsing event text
    public static bool IsCustomEvent()
    {
        if (Game1.CurrentEvent == null)
            return false;
        if (!(Game1.CurrentEvent is CustomEvent))
            return false;
        return (Events.ContainsKey((Game1.CurrentEvent as CustomEvent).EventID));
    }
    public static void CheckCustomCommands()
    {
        if (Game1.CurrentEvent == null)
            return;
        if (Game1.CurrentEvent.eventCommands == null || Game1.CurrentEvent.eventCommands.Length == 0)
            return;
        if (Game1.CurrentEvent.skipped || Game1.CurrentEvent.currentCommand >= Game1.CurrentEvent.eventCommands.Length)
            return;
        string[] split = Game1.CurrentEvent.eventCommands[Game1.CurrentEvent.currentCommand].Split(' ');
        if (split[0].Equals("cFork"))
        {
            if (split.Length > 2)
            {
                int result;
                if (Game1.player.mailReceived.Contains(split[1]) || int.TryParse(split[1], out result) && Game1.player.dialogueQuestionsAnswered.Contains(result))
                {
                    Game1.CurrentEvent.eventCommands = Events[Convert.ToInt32(split[2])].Split('/');
                    Game1.CurrentEvent.currentCommand = 0;
                    Game1.CurrentEvent.forked = !Game1.CurrentEvent.forked;
                }
                else
                    Game1.CurrentEvent.currentCommand++;
            }
            else if (Game1.CurrentEvent.specialEventVariable1)
            {
                Game1.CurrentEvent.eventCommands = Events[Convert.ToInt32(split[1])].Split('/');
                Game1.CurrentEvent.currentCommand = 0;
                Game1.CurrentEvent.forked = !Game1.CurrentEvent.forked;
            }
            else
            {
                Game1.CurrentEvent.currentCommand++;
            }
        }
    }
        // add the events to the game
    static void AddEvent(int id, string eventString)
    {
        Events.Add(id, eventString.Replace(System.Environment.NewLine, ""));
    }

  }
}
