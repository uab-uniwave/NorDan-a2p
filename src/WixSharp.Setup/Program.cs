// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;

using WixSharp_Setup1.Dialogs;

namespace WixSharp
{
    public class Program
    {
        static void Main()
        {
            var project = new ManagedProject("Aluminum2Prefsuite",
                             new Dir(@"%ProgramFiles%\Uniwave UAB\Aluminum2Prefsuite",
                                 new File("Program.cs")))
            {
                GUID = new Guid("9ab9ab76-dce9-448c-bb97-e365d4514522"),

                //custom set of standard UI dialogs
                ManagedUI = new ManagedUI()
            };

            _ = project.ManagedUI.InstallDialogs.Add<WelcomeDialog>()
                                            .Add<LicenceDialog>()
                                            .Add<SetupTypeDialog>()
                                            .Add<FeaturesDialog>()
                                            .Add<InstallDirDialog>()
                                            .Add<ProgressDialog>()
                                            .Add<ExitDialog>();

            _ = project.ManagedUI.ModifyDialogs.Add<MaintenanceTypeDialog>()
                                           .Add<FeaturesDialog>()
                                           .Add<ProgressDialog>()
                                           .Add<ExitDialog>();

            //project.SourceBaseDir = "<input dir path>";
            project.OutDir = "..\\..\\Output\\a2p.WinForm";

            ValidateAssemblyCompatibility();

            _ = project.BuildMsi();
        }

        static void ValidateAssemblyCompatibility()
        {
            var assembly = System.Reflection.Assembly.GetExecutingAssembly();

            if (!assembly.ImageRuntimeVersion.StartsWith("v2."))
            {
                Console.WriteLine("Warning: assembly '{0}' is compiled for {1} runtime, which may not be compatible with the CLR version hosted by MSI. " +
                                  "The incompatibility is particularly possible for the EmbeddedUI scenarios. " +
                                   "The safest way to solve the problem is to compile the assembly for v3.5 Target Framework.",
                                   assembly.GetName().Name, assembly.ImageRuntimeVersion);
            }
        }
    }
}
