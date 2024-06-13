#if Unity_Editor
using UnityEngine;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using System.Linq;
using System.Collections.Generic;

public static class DependencyPackageInstaller
{
    static private AddRequest request;
    static private ListRequest listRequest;
    static private string defineDirective = "DATAGRID_DEPENDENCY_INSTALLED";

    [InitializeOnLoadMethod]
    static private void InstallPackage()
    {
        EditorApplication.update += CheckPackageList;
    }

    static private void CheckPackageList()
    {
        if (listRequest == null)
        {
            listRequest = Client.List();
            return;
        }

        if (!listRequest.IsCompleted)
            return;

        EditorApplication.update -= CheckPackageList;

        var isUniRxInstalled = listRequest.Result.Any(pkg => pkg.name == "com.neuecc.unirx");

        if (!isUniRxInstalled)
        {
            request = Client.Add("https://github.com/neuecc/UniRx.git?path=Assets/Plugins/UniRx/Scripts");
            EditorApplication.update += CheckAddPackage;
        }
        else
        {
            UpdateDefineSymbols(addDirective: true);
        }
    }

    static private void CheckAddPackage()
    {
        if (!request.IsCompleted)
            return;

        EditorApplication.update -= CheckAddPackage;

        if (request.Status == StatusCode.Success)
        {
            Debug.Log("UniRx installed successfully.");
            UpdateDefineSymbols(addDirective: true);
        }
        else if (request.Status >= StatusCode.Failure)
        {
            Debug.LogError(request.Error.message);
            UpdateDefineSymbols(addDirective: false);
        }
    }

    static private void UpdateDefineSymbols(bool addDirective)
    {
        BuildTargetGroup buildTargetGroup = BuildTargetGroup.Standalone; // 필요한 경우 다른 플랫폼으로 변경하세요
        var defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup);

        var definesList = new List<string>(defines.Split(';'));

        if (addDirective)
        {
            if (!definesList.Contains(defineDirective))
            {
                definesList.Add(defineDirective);
                PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTargetGroup, string.Join(";", definesList));
            }
        }
        else
        {
            if (definesList.Contains(defineDirective))
            {
                definesList.Remove(defineDirective);
                PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTargetGroup, string.Join(";", definesList));
            }
        }
    }
}
#endif