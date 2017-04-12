using System;
using System.IO;
using System.Text;
using System.Security.Policy;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using System.Security;
using System.Security.Permissions;

namespace BoatReplayLib {
  // Sandboxed, wao
  public class Depickler {
    private static AppDomain domain = null;

    private static AppDomain GetDomain() {
      if(domain == null) {
        PermissionSet permSet = new PermissionSet(PermissionState.None);
        permSet.AddPermission(new SecurityPermission(SecurityPermissionFlag.Execution));
        StrongName trust = typeof(SandboxedDepickler).Assembly.Evidence.GetHostEvidence<StrongName>();
        AppDomainSetup ads = new AppDomainSetup();
        ads.ApplicationBase = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Untrusted");
        domain = AppDomain.CreateDomain("SandboxedDepickler", null, ads, permSet, trust);
      }
      return domain;
    }

    private static SandboxedDepickler SandboxedInstance = null;

    private class SandboxedDepickler {
      private static string source = @"""
import cPickle as pickle
import sys
data = pickle.load(sys.stdin)
""";

      public dynamic load(Stream data) {
        ScriptEngine engine = Python.CreateEngine();
        engine.Runtime.IO.SetInput(data, Encoding.Default);
        ScriptScope scope = engine.CreateScope();
        engine.CreateScriptSourceFromString(source, Microsoft.Scripting.SourceCodeKind.File).Execute(scope);
        dynamic result = engine.Execute("data", scope);
        return result;
      }
    }

    public static dynamic load(Stream data) {
      if(SandboxedInstance == null) {
        object instance = GetDomain().CreateInstanceAndUnwrap(typeof(SandboxedDepickler).Assembly.ManifestModule.FullyQualifiedName, typeof(SandboxedDepickler).FullName);
        if(instance == null) {
          return null;
        }
        SandboxedInstance = instance as SandboxedDepickler;
      }
      return SandboxedInstance.load(data);
    }
  }
}
