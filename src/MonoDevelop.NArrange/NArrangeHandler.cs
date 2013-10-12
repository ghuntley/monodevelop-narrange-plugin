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

                // HACK: Still not working..
                //                var openfiles = IdeApp.Workspace.Items;
                //
                //                foreach (var file in openfiles)
                //                {
                //                    FileService.NotifyFileChanged(file.FileName);
                //                }

                //                [23:13:07] <Therzok>     ghuntley: check monodevelop's codeissuepad
                //                [23:13:28] <Therzok>     it's used for batch fixing of source analysis issues.
                //                [23:14:14] <ghuntley>    the git plugin uses IdeApp.Workbench.AutoReloadDocuments = true and locks the gui as part of a branch switch.
                //                [23:14:50] <ghuntley>    lifted the pattern from there. Still quite new to monodevelop addons.
                //                [23:19:28] <ghuntley>    @Therzok: If I run the https://github.com/ghuntley/monodevelop-narrange-plugin/blob/master/src/MonoDevelop.NArrange/NArrangeHandler.cs#L26 to #L27 in a consle/terminal then it is a different story. File changes are detected and monodevelop notifies me that the files have changed and asks to reload them.
                //                [23:29:33] <Therzok>     Also, I don't think that you should lock/unlock the gui, but use DispatchService.
                //                [23:33:17] <Therzok>     ghuntley: Also, on each file you modify, you need to call FileService.NotifyFilesChanged (myFiles);

                // HACK: Attempt at getting open files to reload automatically
                // as AutoReloadDocuments isn't detecting that the files
                // have been modified.
                //                var openfiles = IdeApp.Workbench.Documents;
                //                foreach (var document in openfiles)
                //                {
                //                    document.Reload();
                //                }
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