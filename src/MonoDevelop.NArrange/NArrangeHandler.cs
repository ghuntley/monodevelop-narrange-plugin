using System;

using MonoDevelop.Components.Commands;
using MonoDevelop.Core;
using MonoDevelop.Core.Execution;
using MonoDevelop.Ide;
using MonoDevelop.Ide.Gui;

namespace MonoDevelop.NArrange
{
    public class NArrangeHandler : CommandHandler
    {
        #region Methods

        protected override void Run()
        {
            IProgressMonitor progressMonitor = GetRunProcessMonitor();

            try
            {
                IdeApp.Workbench.AutoReloadDocuments = true;
                IdeApp.Workbench.LockGui ();

                var solutionDirectory = IdeApp.ProjectOperations.CurrentSelectedSolution.BaseDirectory;

                var command = "/Users/ghuntley/.dotfiles/bin/narrange";
                var commandArgs = String.Format("{0}/. -c:/Users/ghuntley/.dotfiles/bin/narrange.xml", solutionDirectory);

                Runtime.ProcessService.StartConsoleProcess(
                    command,
                    commandArgs,
                    solutionDirectory,
                    progressMonitor as IConsole,
                    (e, sender) => progressMonitor.Dispose()
                );
            }
            finally
            {
                IdeApp.Workbench.AutoReloadDocuments = false;

// HACK: Attempt at getting open files to reload automatically
// as AutoReloadDocuments isn't detecting that the files
// have been modified.
//                var openfiles = IdeApp.Workbench.Documents;
//                foreach (var document in openfiles)
//                {
//                    document.Reload();
//                }

                IdeApp.Workbench.UnlockGui ();
            }
        }

        IProgressMonitor GetRunProcessMonitor()
        {
            return IdeApp.Workbench.ProgressMonitors.GetOutputProgressMonitor(
                "NArrange",
                Stock.RunProgramIcon,
                true,
                true);
        }

        #endregion Methods
    }
}