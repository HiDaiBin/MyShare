using System.ComponentModel;
using System.Reflection;
using System;
using HarmonyLib;
using System.IO;
using System.Linq;
using Mono.CompilerServices.SymbolWriter;
using System.Xml;

class SpireOfficePatch
{
    static object licenseCache;
    [HarmonyPatch(typeof(Spire.License.LicenseProvider), "GetLicense")]
    [HarmonyPrefix]
    static bool Prefix_SpireOffice_LicenseProvider_GetLicense(LicenseContext context, Type type, object instance, bool allowExceptions, ref object __result, MethodInfo __originalMethod)
    {
        if (licenseCache != null)
        {
            //有构建过 的 直接拦截返回
            __result = licenseCache;
            return false;
        }
        var declaringType = __originalMethod.DeclaringType;
        MethodInfo tempMethod = AccessTools.FirstMethod(declaringType, m => m.GetParameters().Any() && m.GetParameters()[0].ParameterType == typeof(Stream) && m.ReturnType != typeof(void));
        if (tempMethod != null)
        {
            var licType = tempMethod.ReturnType;
            var o = Activator.CreateInstance(licType);
            var tempType = o.GetType();
            var fields = o.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance).ToList();
            fields[0].SetValue(o, "999.999");



            __result = licenseCache = o;
            return false;


        }
        return true;
    }

    internal static void Patch()
    {
        Harmony.CreateAndPatchAll(typeof(SpireOfficePatch), nameof(SpireOfficePatch));
    }
}