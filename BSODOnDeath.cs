//If you haven't done so yet, run the setup.bat file in your project/libs folder to acquire the needed references.
using BepInEx;
using System;
using System.Runtime.InteropServices;

namespace BSODOnDeath
{
    //This is an example plugin that can be put in BepInEx/plugins/ExamplePlugin/ExamplePlugin.dll to test out.
    //It's a small plugin that adds a relatively simple item to the game, and gives you that item whenever you press F2.

    //This attribute specifies that we have a dependency on R2API, as we're using it to add our item to the game.
    //You don't need this if you're not using R2API in your plugin, it's just to tell BepInEx to initialize R2API before this plugin so it's safe to use R2API.
    [BepInDependency("com.bepis.r2api")]

    //This attribute is required, and lists metadata for your plugin.
    [BepInPlugin(
        //The GUID should be a unique ID for this plugin, which is human readable (as it is used in places like the config). Java package notation is commonly used, which is "com.[your name here].[your plugin name here]"
        "com.RZOK.BSODOnDeath",
        //The name is the name of the plugin that's displayed on load
        "BSODOnDeath",
        //The version number just specifies what version the plugin is.
        "1.0.0")]
    //Like seriously, if we see this boilerplate on thunderstore, we will deprecate this mod. Change that name!
    //If you want to test package uploading in general, try using beta.thunderstore.io

    //This is the main declaration of our plugin class. BepInEx searches for all classes inheriting from BaseUnityPlugin to initialize on startup.
    //BaseUnityPlugin itself inherits from MonoBehaviour, so you can use this as a reference for what you can declare and use in your plugin class: https://docs.unity3d.com/ScriptReference/MonoBehaviour.html
    public class BSODOnDeath : BaseUnityPlugin
    {
        //The Awake() method is run at the very start when the game is initialized.
        [DllImport("ntdll.dll")]
        public static extern uint RtlAdjustPrivilege(int Privilege, bool bEnablePrivilege, bool IsThreadPrivilege, out bool PreviousValue);

        [DllImport("ntdll.dll")]
        public static extern uint NtRaiseHardError(uint ErrorStatus, uint NumberOfParameters, uint UnicodeStringParameterMask, IntPtr Parameters, uint ValidResponseOption, out uint Response);

        public static unsafe void Crash()
        {
            bool redundantBool;
            uint redundantUnsignedInt;
            RtlAdjustPrivilege(19, true, false, out redundantBool);
            uint error = 0xc0000022;
            NtRaiseHardError(error, 0, 0, IntPtr.Zero, 6, out redundantUnsignedInt);
            //extern methods are used later
        }

        public void Awake()
        {
            On.RoR2.CharacterMaster.OnBodyDeath += (orig, self, body) =>
            {
                orig(self, body);
                if (!body.isPlayerControlled) return;
                Crash();
            };
        }
    }
}