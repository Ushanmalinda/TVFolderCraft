using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace TVFolderCraft
{
    public static class DocumentationHelper
    {
        //path to  PDF documentation file
        private static readonly string DocumentationPath = Path.Combine(Application.StartupPath, "Documentation", "TVFolderCraft_Documentation.pdf");
        
        // Open PDF 
        public static void OpenDocumentation()
        {
            try
            {
                //check if PDF file exists
                if (File.Exists(DocumentationPath))
                {
                    //try to open with default PDF viewer
                    ProcessStartInfo startInfo = new ProcessStartInfo
                    {
                        FileName = DocumentationPath,
                        UseShellExecute = true
                    };

                    Process.Start(startInfo);
                }
                else
                {
                    //show message if PDF not found and offer to download
                    DialogResult result = MessageBox.Show(
                        "Documentation file not found!\n\n" +
                        "The PDF documentation is not available in the expected location:\n" +
                        DocumentationPath + "\n\n" +
                        "Would you like to:\n" +
                        "• YES - Open online documentation\n" +
                        "• NO - Show quick help info\n" +
                        "• CANCEL - Close this dialog",
                        "Documentation Not Found",
                        MessageBoxButtons.YesNoCancel,
                        MessageBoxIcon.Question
                    );

                    //check user click button
                    switch (result)
                    {
                        case DialogResult.Yes:
                            OpenOnlineDocumentation(); // if yes open browser and go to my drive
                            break;
                        case DialogResult.No:
                            ShowQuickHelp(); // else show default informations
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                // error handling pdf viewer in install
                MessageBox.Show(
                    $"Unable to open documentation file.\n\n" +
                    $"Error: {ex.Message}\n\n" +
                    "Please ensure you have a PDF viewer installed (like Adobe Reader, Edge, or Chrome).",
                    "Documentation Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                //offer alternative
                ShowQuickHelp();
            }
        }
        
        // Opens online documentation in browser
        private static void OpenOnlineDocumentation()
        {
            try
            {
                //google drive path to opent document
                string onlineDocsUrl = "https://drive.google.com/file/d/1gzmA2UPCeYTDfANtZ8WR8cEenyL3GJuM/view?usp=drive_link";

                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = onlineDocsUrl,
                    UseShellExecute = true
                };

                Process.Start(startInfo);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Unable to open online documentation.\n\nError: {ex.Message}",
                    "Browser Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
            }
        }

        // Shows quick help information in a message box
        private static void ShowQuickHelp()
        {
            string quickHelp =
                "TVFolderCraft - Quick Help\n\n" +
                "🎬 MAIN FEATURES:\n" +
                "• Automatic TV series folder organization\n" +
                "• Internet metadata fetching\n" +
                "• Batch processing of multiple series\n" +
                "• Customizable folder naming patterns\n\n" +
                "🚀 GETTING STARTED:\n" +
                "1. Click 'Get Started' or 'New Project'\n" +
                "2. Select your TV series files/folders\n" +
                "3. Choose organization options\n" +
                "4. Let TVFolderCraft organize everything!\n\n" +
                "⚙️ SETTINGS:\n" +
                "• Configure folder structures\n" +
                "• Set file naming conventions\n" +
                "• Customize metadata sources\n\n" +
                "📧 NEED HELP?\n" +
                "Click 'Support' in the footer to contact us!";

            MessageBox.Show(
                quickHelp,
                "TVFolderCraft - Quick Help",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }

    }
}