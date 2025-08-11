using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using AutoAppdater.Window;

namespace AutoAppdater.Mod
{
    internal class ExtensionModule
    {
        dynamic Mod;
        const string Mod_ExtensionName = "mod";
        const string Mod_Main_Namespace = "AutoAppdaterExtensionModule";
        const string Mod_Main_MainClass = "MainClass";
        const string Mod_Main_EntryPoint = "ModMain";
        const string Mod_Main_Message = "Message";
        const string Mod_Main_Exit = "Exit";
        const string Split = ".";
        internal ExtensionModule(string dllPath)
        {
            if (!File.Exists(dllPath)) throw new Exception("dll file not found.");
            if (Path.GetExtension(dllPath) != Mod_ExtensionName) throw new Exception("Unsupported type of Mod.");
            Assembly asm = Assembly.LoadFrom(dllPath);
            Module? mod = asm.GetModule(Path.GetFileName(dllPath));
            if (mod == null) throw new Exception("Cannot get module.");
            Type? typ = mod.GetType(Mod_Main_Namespace + Split + Mod_Main_MainClass);
            if (typ == null) throw new Exception("Cannot get type object.");
            Mod = typ;
            Setup();
            Message(new CopyData());
        }
        void Setup()
        {
            Mod.ModMain();
        }
        internal void Message(CopyData data)
        {
            string js = JsonSerializer.Serialize(data);
            Mod.Message(js);
        }
        internal void Exit()
        {
            try
            {
                Mod.Exit();
            }
            catch
            {
                //
            }
        }
    }
}