using UnityEngine;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using System.Linq;
using System.IO;
using System.Collections.Generic;

public static class DependencyPackageInstaller
{
    static private AddRequest request;
    static private ListRequest listRequest;
    static private string cscRspPath = Path.Combine(Application.dataPath, "csc.rsp");
    static private string defineDirective = "-define:DATAGRID_DEPENDENCY_INSTALLED";

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
            UpdateCscRspFile(addDirective: true);
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
            UpdateCscRspFile(addDirective: true);
        }
        else if (request.Status >= StatusCode.Failure)
        {
            Debug.LogError(request.Error.message);
            UpdateCscRspFile(addDirective: false);
        }
    }

    static private void UpdateCscRspFile(bool addDirective)
    {
        var directives = File.Exists(cscRspPath) ? File.ReadAllLines(cscRspPath).ToList() : new List<string>();

        if (addDirective)
        {
            if (!directives.Contains(defineDirective))
            {
                directives.Add(defineDirective);
                File.WriteAllLines(cscRspPath, directives);
            }
        }
        else
        {
            if (directives.Contains(defineDirective))
            {
                directives.Remove(defineDirective);
                File.WriteAllLines(cscRspPath, directives);
            }
        }
    }
}
