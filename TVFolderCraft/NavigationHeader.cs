using System;
using System.Drawing;
using System.Windows.Forms;

namespace TVFolderCraft
{
    public class NavigationHeader : Panel
    {
        private Form parentForm;
        private string currentPage;

        public NavigationHeader(Form parent, string currentPageName)
        {
            parentForm = parent;
            currentPage = currentPageName;
            InitializeHeader();
        }

        private void InitializeHeader()
        {
            //set panel properties
            this.Dock = DockStyle.Top;
            this.Height = 70;
            this.BackColor = Color.FromArgb(207, 223, 252);

            CreateHeaderElements();
        }

        //create hedder elements
        private void CreateHeaderElements()
        {
            //logo
            var logo = new PictureBox
            {
                Size = new Size(70, 70),
                Location = new Point(10, 0),
                SizeMode = PictureBoxSizeMode.StretchImage,
                Image = Properties.Resources.Logo
            };

            //app name
            var logoLabel = new Label
            {
                Text = "TVFolderCraft",
                Font = new Font("Roboto", 20, FontStyle.Bold),
                ForeColor = Color.FromArgb(10, 3, 43),
                AutoSize = true,
                Location = new Point(75, 23)
            };

            //navigation buttons
            var homeButton = CreateNavButton("Home", currentPage == "Home");
            homeButton.Location = new Point(parentForm.Width - 370, 25);
            homeButton.Click += (s, e) => NavigateToPage("Home");

            var generateButton = CreateNavButton("Generate", currentPage == "Generate");
            generateButton.Location = new Point(parentForm.Width - 290, 25);
            generateButton.Click += (s, e) => NavigateToPage("Generate");

            var historyButton = CreateNavButton("History", currentPage == "History");
            historyButton.Location = new Point(parentForm.Width - 200, 25);
            historyButton.Click += (s, e) => NavigateToPage("History");

            var settingButton = CreateNavButton("Setting", currentPage == "Setting");
            settingButton.Location = new Point(parentForm.Width - 120, 25);
            settingButton.Click += (s, e) => NavigateToPage("Setting");

            //add controls to panel
            this.Controls.AddRange(new Control[] {
                logo, logoLabel, homeButton, generateButton, historyButton, settingButton
            });
        }

        //create navigation buttons
        private Button CreateNavButton(string text, bool isActive)
        {
            var button = new Button
            {
                Text = text,
                Size = new Size(80, 30),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Roboto", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };

            if (isActive)
            {
                button.ForeColor = Color.FromArgb(70, 100, 250);
                button.FlatAppearance.BorderSize = 0;
                button.BackColor = Color.Transparent;
            }
            else
            {
                button.ForeColor = Color.FromArgb(0, 0, 0);
                button.FlatAppearance.BorderSize = 0;
                button.BackColor = Color.Transparent;
            }

            button.FlatAppearance.MouseOverBackColor = Color.FromArgb(230, 235, 240);
            return button;
        }

        //navigation to other pages
        private void NavigateToPage(string pageName)
        {
            try
            {
                parentForm.Hide();
                Form newForm = null;

                switch (pageName)
                {
                    case "Home":
                        newForm = new Form1();
                        break;
                    case "Generate":
                        newForm = new InternetPage();
                        break;
                    case "History":
                        newForm = new HistoryPage();
                        break;
                    case "Setting":
                        newForm = new SettingPage();
                        break;
                    default:
                        return;
                }

                if (newForm != null)
                {
                    newForm.Show();
                    newForm.FormClosed += (s, e) => parentForm.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Navigation error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //update button positions when parent form resizes
        public void UpdateButtonPositions()
        {
            if (this.Controls.Count >= 6) //check all controls have or not
            {
                var homeButton = this.Controls[2] as Button;
                var generateButton = this.Controls[3] as Button;
                var historyButton = this.Controls[4] as Button;
                var settingButton = this.Controls[5] as Button;

                if (homeButton != null) homeButton.Location = new Point(parentForm.Width - 370, 25);
                if (generateButton != null) generateButton.Location = new Point(parentForm.Width - 290, 25);
                if (historyButton != null) historyButton.Location = new Point(parentForm.Width - 200, 25);
                if (settingButton != null) settingButton.Location = new Point(parentForm.Width - 120, 25);
            }
        }
    }
}