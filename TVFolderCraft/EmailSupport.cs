using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.Web;
using System.Drawing;

namespace TVFolderCraft
{
    public static class EmailSupport
    {
        // my  email address
        private static readonly string SupportEmail = "abesinhaushan@gmail.com"; 

        //open email 
        public static void OpenSupportEmail()
        {
            ShowEmailProviderDialog();
        }

        // create user to 3 option
        private static void ShowEmailProviderDialog()
        {
            Form dialog = new Form()
            {
                Width = 400,
                Height = 250,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = "Email Support Options",
                StartPosition = FormStartPosition.CenterScreen,
                MaximizeBox = false,
                MinimizeBox = false,
                BackColor = Color.FromArgb(240, 242, 245)
            };

            Label titleLabel = new Label()
            {
                Text = "Choose your preferred email option:",
                Font = new Font("Roboto", 12, FontStyle.Bold),
                Location = new Point(20, 20),
                Size = new Size(360, 30),
                ForeColor = Color.FromArgb(51, 51, 51)
            };

            //google button
            Button option1Button = CreateOptionButton("1. Gmail (gmail.com)", new Point(20, 60), () => {
                dialog.Close();
                OpenGmail();
            });

            //outlookweb button
            Button option2Button = CreateOptionButton("2. Outlook Web (outlook.com)", new Point(20, 110), () => {
                dialog.Close();
                OpenOutlookWeb();
            });

            //copyble my email 
            Button option3Button = CreateOptionButton("3. Copy Email Address", new Point(20, 160), () => {
                dialog.Close();
                CopyEmailToClipboard();
            });

            
            dialog.Controls.Add(titleLabel);
            dialog.Controls.Add(option1Button);
            dialog.Controls.Add(option2Button);
            dialog.Controls.Add(option3Button);

            dialog.ShowDialog();
        }

        //creates button
        private static Button CreateOptionButton(string text, Point location, Action clickAction)
        {
            Button button = new Button()
            {
                Text = text,
                Font = new Font("Roboto", 10),
                Size = new Size(350, 35),
                Location = location,
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.White,
                ForeColor = Color.FromArgb(51, 51, 51),
                TextAlign = ContentAlignment.MiddleLeft,
                Cursor = Cursors.Hand
            };

            button.FlatAppearance.BorderColor = Color.FromArgb(200, 200, 200);
            button.FlatAppearance.BorderSize = 1;
            button.FlatAppearance.MouseOverBackColor = Color.FromArgb(70, 130, 250);
            button.FlatAppearance.MouseDownBackColor = Color.FromArgb(60, 120, 240);

            //hover effects
            button.MouseEnter += (s, e) => {
                button.ForeColor = Color.White;
            };
            button.MouseLeave += (s, e) => {
                button.ForeColor = Color.FromArgb(51, 51, 51);
            };

            button.Click += (s, e) => clickAction();

            return button;
        }

        //open Gmail function
        private static void OpenGmail()
        {
            try
            {
                string subject = HttpUtility.UrlEncode("TVFolderCraft Support Request");
                string body = HttpUtility.UrlEncode($"Hello,\n\nI need help with TVFolderCraft.\n\nPlease describe your issue:\n\n\n\nSystem Information:\n- Application Version: 1.0.0\n- Operating System: {Environment.OSVersion}\n- .NET Framework: {Environment.Version}\n\nThank you!");

                string url = $"https://mail.google.com/mail/?view=cm&to={SupportEmail}&su={subject}&body={body}";

                OpenUrlInEdge(url);
            }
            catch (Exception ex)
            {
                ShowErrorMessage("Gmail", ex.Message);
            }
        }

        //open Outlook function
        private static void OpenOutlookWeb()
        {
            try
            {
                string subject = HttpUtility.UrlEncode("TVFolderCraft Support Request");
                string body = HttpUtility.UrlEncode($"Hello,\n\nI need help with TVFolderCraft.\n\nPlease describe your issue:\n\n\n\nSystem Information:\n- Application Version: 1.0.0\n- Operating System: {Environment.OSVersion}\n- .NET Framework: {Environment.Version}\n\nThank you!");

                string url = $"https://outlook.live.com/mail/0/deeplink/compose?to={SupportEmail}&subject={subject}&body={body}";

                OpenUrlInEdge(url);
            }
            catch (Exception ex)
            {
                ShowErrorMessage("Outlook Web", ex.Message);
            }
        }

        // Opens URL
        private static void OpenUrlInEdge(string url)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = "msedge.exe",
                Arguments = url,
                UseShellExecute = true
            };

            Process.Start(startInfo);
        }
        
        //  error handaling 
        private static void ShowErrorMessage(string provider, string error)
        {
            MessageBox.Show(
                $"Unable to open {provider} automatically.\n\n" +
                $"Please manually open your email and send to:\n{SupportEmail}\n\n" +
                $"Error: {error}",
                "Email Support",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }

        //  copies to clipboard emil 
        public static void CopyEmailToClipboard()
        {
            try
            {
                //get email to clipbord
                Clipboard.SetText(SupportEmail);
                MessageBox.Show(
                    $"Support email address copied to clipboard:\n{SupportEmail}\n\n" +
                    "You can now paste it in your email client.",
                    "Email Copied",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Unable to copy to clipboard.\n\n" +
                    $"Support email: {SupportEmail}\n\n" +
                    $"Error: {ex.Message}",
                    "Copy Failed",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
            }
        }
    }
}