using System;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class TerminalInterface
{
   public void GetDiff()
   {
      Process process = new Process();
      process.Start();
   }

   public void Main()
   {
      try
      {
         using (Process myProcess = new Process())
         {
            myProcess.StartInfo.UseShellExecute = false;
            // You can start any process, HelloWorld is a do-nothing example.
            myProcess.StartInfo.FileName = "Assets/Scene_Git_Extension/Runtime/Scripts/Scene_Git/ShellScripts/get_diff.sh";
            myProcess.StartInfo.CreateNoWindow = true;
            myProcess.Start();
            // This code assumes the process you are starting will terminate itself.
            // Given that it is started without a window so you cannot terminate it
            // on the desktop, it must terminate itself or you can do it programmatically
            // from this application using the Kill method.
         }
      }
      catch (Exception e)
      {
         Debug.LogError($"error {e}");
      }
   }
}