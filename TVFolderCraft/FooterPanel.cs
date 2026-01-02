using System;
using System.Drawing;
using System.Windows.Forms;

namespace TVFolderCraft
{
    public class FooterPanel : Panel
    {
        private Form parentForm;
        private Label versionLabel;
        private Label supportLabel;
        private Label documentationLabel;

        public FooterPanel(Form parent)
        {
            parentForm = parent;
            InitializeFooter();
        }

        private void InitializeFooter()
        {
            //set panel properties
            this.Dock = DockStyle.Bottom;
            this.Height = 50;
            this.BackColor = Color.FromArgb(207, 223, 252);

            CreateFooterElements();
        }

        //create footer elements
        private void CreateFooterElements()
        {
            //copyright label
            var copyrightLabel = new Label
            {
                Text = "© 2024 TVFolderCraft. All rights reserved.",
                Font = new Font("Roboto", 9),
                ForeColor = Color.FromArgb(41, 38, 48),
                AutoSize = true,
                Location = new Point(20, 18)
            };

            //version label
            versionLabel = new Label
            {
                Text = "Version 1.0.0",
                Font = new Font("Roboto", 9),
                ForeColor = Color.FromArgb(128, 128, 128),
                AutoSize = true
            };

            //support label
            supportLabel = new Label
            {
                Text = "Support",
                Font = new Font("Roboto", 9),
                ForeColor = Color.FromArgb(70, 130, 250),
                AutoSize = true,
                Cursor = Cursors.Hand
            };
            supportLabel.Click += (s, e) => OpenSupportEmail();

            //documentation label
            documentationLabel = new Label
            {
                Text = "Documentation",
                Font = new Font("Roboto", 9),
                ForeColor = Color.FromArgb(70, 130, 250),
                AutoSize = true,
                Cursor = Cursors.Hand
            };
            documentationLabel.Click += (s, e) => OpenDocumentation();

            //add hover effects for clickable labels
            AddHoverEffects(supportLabel);
            AddHoverEffects(documentationLabel);

            //add controls to panel
            this.Controls.AddRange(new Control[] {
                copyrightLabel, documentationLabel, supportLabel, versionLabel
            });

            //position labels
            UpdateLabelPositions();
        }

        // event if user hover that lables
        private void AddHoverEffects(Label label)
        {
            Color originalColor = label.ForeColor;
            Color hoverColor = Color.FromArgb(50, 100, 220);

            label.MouseEnter += (s, e) => label.ForeColor = hoverColor;
            label.MouseLeave += (s, e) => label.ForeColor = originalColor;
        }

        //if user click support lable pass to emailsupport class
        private void OpenSupportEmail()
        {
            try
            {
                //check if EmailSupport class exists, otherwise provide fallback
                if (System.Reflection.Assembly.GetExecutingAssembly()
                    .GetType("TVFolderCraft.EmailSupport") != null)
                {
                    EmailSupport.OpenSupportEmail();
                }
                else
                {
                    //fallback implementation
                    System.Diagnostics.Process.Start("mailto:support@tvfoldercraft.com?subject=TVFolderCraft Support");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unable to open email client: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        //if user click document lable pass DocumentationHelper class
        private void OpenDocumentation()
        {
            try
            {
                //check if DocumentationHelper class exists, otherwise provide fallback
                if (System.Reflection.Assembly.GetExecutingAssembly()
                    .GetType("TVFolderCraft.DocumentationHelper") != null)
                {
                    DocumentationHelper.OpenDocumentation();
                }
                else
                {
                    //fallback implementation
                    System.Diagnostics.Process.Start("https://github.com/yourproject/tvfoldercraft/wiki");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unable to open documentation: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        //update label positions when parent form resizes
        public void UpdateLabelPositions()
        {
            if (parentForm != null && versionLabel != null && supportLabel != null && documentationLabel != null)
            {
                //update positions based on parent form width
                versionLabel.Location = new Point(parentForm.Width - versionLabel.PreferredWidth - 20, 18);
                supportLabel.Location = new Point(parentForm.Width - versionLabel.PreferredWidth - supportLabel.PreferredWidth - 40, 18);
                documentationLabel.Location = new Point(parentForm.Width - versionLabel.PreferredWidth - supportLabel.PreferredWidth - documentationLabel.PreferredWidth - 70, 18);
            }
        }

        //set custom version text
        public void SetVersion(string version)
        {
            if (versionLabel != null)
            {
                versionLabel.Text = $"Version {version}";
                UpdateLabelPositions();
            }
        }

        //handle parent form resize
        protected override void OnParentChanged(EventArgs e)
        {
            base.OnParentChanged(e);
            if (Parent != null)
            {
                Parent.Resize += (s, args) => UpdateLabelPositions();
            }
        }
    }
}