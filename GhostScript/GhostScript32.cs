using System;
using System.Runtime.InteropServices;

namespace kli.ThumbsFunctions.GhostScript
{
    internal class GhostScript32
    {
        [DllImport("..\\ghostScript\\gsdll32.dll", EntryPoint = "gsapi_new_instance")]
        private static extern int CreateAPIInstance(out IntPtr pinstance, IntPtr caller_handle);

        [DllImport("..\\ghostScript\\gsdll32.dll", EntryPoint = "gsapi_init_with_args")]
        private static extern int InitAPI(IntPtr instance, int argc, string[] argv);

        [DllImport("..\\ghostScript\\gsdll32.dll", EntryPoint = "gsapi_exit")]
        private static extern int ExitAPI(IntPtr instance);

        [DllImport("..\\ghostScript\\gsdll32.dll", EntryPoint = "gsapi_delete_instance")]
        private static extern void DeleteAPIInstance(IntPtr instance);

        public static void CallAPI(string[] args)
        {
            IntPtr gsInstancePtr;
            lock (resourceLock)
            {
                CreateAPIInstance(out gsInstancePtr, IntPtr.Zero);
                try
                {
                    int result = InitAPI(gsInstancePtr, args.Length, args);
                    if (result < 0)
                        throw new ExternalException("Ghostscript conversion error", result);
                }
                finally
                {
                    Cleanup(gsInstancePtr);
                }
            }
        }

        private static void Cleanup(IntPtr gsInstancePtr)
        {
            ExitAPI(gsInstancePtr);
            DeleteAPIInstance(gsInstancePtr);
        }

        private static object resourceLock = new object();
    }
}
