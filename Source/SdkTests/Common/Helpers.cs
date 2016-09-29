namespace SdkTests.Common
{
  using System;
  using System.IO;
  using System.Reflection;

  internal class Helpers
  {
    private const string ConfigResourceAssembly = "SdkTests.dll";
    private const string ConfigResourceNamespace = "SdkTests.Resources.";

    public static string GetAssemblyResource(string relativeResourceName)
    {
      string absoluteResourceName = string.Concat(ConfigResourceNamespace, relativeResourceName);

      string resSrc = string.Empty;

      try
      {
        using (Stream s = Assembly.LoadFile(Path.Combine(Environment.CurrentDirectory, ConfigResourceAssembly)).GetManifestResourceStream(absoluteResourceName))
        {
          if (s == null)
          {
            throw new InvalidOperationException("Resource not found: " + absoluteResourceName);
          }

          using (var reader = new StreamReader(s))
          {
            resSrc = reader.ReadToEnd();
            reader.Close();
            s.Close();
          }
        }
      }
      catch
      {
        throw;
      }

      return resSrc;
    }
  }
}
