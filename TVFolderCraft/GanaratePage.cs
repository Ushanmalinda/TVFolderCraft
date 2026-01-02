using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace TVFolderCraft
{
    public partial class GanaratePage : Form
    {
        private Panel headerPanel; 
        private Panel mainPanel;
        private Panel contentHeaderPanel;
        private Panel seasonsPanel;
        private Panel footerPanel;
        private TextBox seriesNameTextBox;
        private NumericUpDown yearNumericUpDown;
        private TextBox outputDirectoryTextBox;
        private Button addSeasonButton;
        private Button generateButton;
        private Button browseButton;
        private List<SeasonCard> seasonCards;
        private int seasonCounter = 1;

        public GanaratePage()
        {
            InitializeComponent();
            seasonCards = new List<SeasonCard>();
            SetupUI();
            this.Size = new Size(650, 800);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = Color.FromArgb(240, 242, 245);
            this.ClientSize = new Size(800, 800);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = true;
            this.Name = "GanaratePage";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Manual TV Series Creator";
            this.ResumeLayout(false);
        }
        // create ui
        private void SetupUI()
        {
            CreateHeaderPanel();
            CreateContentPanel();
            CreateFooterPanel();
        }

        // create header panal
        private void CreateHeaderPanel()
        {
            headerPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 70,
                BackColor = Color.FromArgb(207, 223, 252)
            };

            //logo
            var logo = new PictureBox
            {
                Size = new Size(70, 70),
                Location = new Point(10, 0),
                SizeMode = PictureBoxSizeMode.StretchImage,
                Image = Properties.Resources.Logo
            };

            // logo and app name
            var logoLabel = new Label
            {
                Text = "TVFolderCraft",
                Font = new Font("roboto", 20, FontStyle.Bold),
                ForeColor = Color.FromArgb(51, 51, 51),
                AutoSize = true,
                Location = new Point(80, 20)
            };

            // navigation buttons
            //home
            var homeButton = CreateNavButton("Home", false);
            homeButton.Location = new Point(this.Width - 320, 25);
            homeButton.Click += (s, e) => NavigateToHome();
            //internetPage
            var generateButton = CreateNavButton("Generate", true);
            generateButton.Location = new Point(this.Width - 240, 25);
            generateButton.Click += (s, e) => NavigateToInternet();
            //history
            var historyButton = CreateNavButton("History", false);
            historyButton.Location = new Point(this.Width - 160, 25);
            historyButton.Click += (s, e) => NavigateToHistoryPage();
            //setting
            var settingButton = CreateNavButton("Setting", false);
            settingButton.Location = new Point(this.Width - 80, 25);
            settingButton.Click += (s, e) => NavigateToSettingPage();

            headerPanel.Controls.AddRange(new Control[] { logo,logoLabel, homeButton, generateButton, historyButton, settingButton });
            this.Controls.Add(headerPanel);
        }
        // navigation button creation
        private Button CreateNavButton(string text, bool isActive)
        {
            var button = new Button
            {
                Text = text,
                Size = new Size(80, 30),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("roboto", 10,FontStyle.Bold),
                Cursor = Cursors.Hand
            };

            if (isActive)
            {
                button.ForeColor = Color.FromArgb(0,0,0);
                button.FlatAppearance.BorderSize = 0;
                button.BackColor = Color.Transparent;
            }
            else
            {
                button.ForeColor = Color.FromArgb(0,0,0);
                button.FlatAppearance.BorderSize = 0;
                button.BackColor = Color.Transparent;
            }

            button.FlatAppearance.MouseOverBackColor = Color.FromArgb(230, 235, 240);
            return button;
        }
        //create content area
        private void CreateContentPanel()
        {
            // main panel
            mainPanel = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                Padding = new Padding(20),
                BackColor = Color.FromArgb(248, 250, 252),
                BackgroundImage = Properties.Resources.backgrount,
                BackgroundImageLayout = ImageLayout.Stretch
            };

            // series info panal
            contentHeaderPanel = new Panel
            {
                Size = new Size(460, 80),
                Location = new Point(50, 100),
                BackColor = Color.White,
                BorderStyle = BorderStyle.None
            };
            // create component inside Series info panal
            CreateComponent();

            // seasons panel
            seasonsPanel = new Panel
            {
                Location = new Point(50, 200),
                Size = new Size(460, 300),
                BackColor = Color.Transparent,
                AutoSize = true
            };

            // add Seasons panal
            var controlsPanel = new Panel
            {
                Size = new Size(460, 120),
                BackColor = Color.White,
                BorderStyle = BorderStyle.None
                
            };
            CreateControlsPanelContent(controlsPanel);

            // add panels to main panel
            mainPanel.Controls.Add(contentHeaderPanel);
            mainPanel.Controls.Add(seasonsPanel);
            mainPanel.Controls.Add(controlsPanel);

            // rename the reference for layout updates
            footerPanel = controlsPanel;

            // add main panel to form
            this.Controls.Add(mainPanel);

            // add initial season cards
            AddSeasonCard();
            AddSeasonCard();

            UpdateLayout();
        }
        //create component for TV sereas info area
        private void CreateComponent()
        {
            // series name label and textbox
            Label seriesLabel = new Label
            {
                Text = "Series Name",
                Location = new Point(15, 15),
                Size = new Size(100, 20),
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.FromArgb(45, 55, 72)
            };

            seriesNameTextBox = new TextBox
            {
                Location = new Point(15, 35),
                Size = new Size(200, 25),
                Font = new Font("Segoe UI", 10F),
                Text = "Enter series name..."
            };

            // year label and numeric updown
            Label yearLabel = new Label
            {
                Text = "Year",
                Location = new Point(230, 15),
                Size = new Size(50, 20),
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.FromArgb(45, 55, 72)
            };

            yearNumericUpDown = new NumericUpDown
            {
                Location = new Point(230, 35),
                Size = new Size(80, 25),
                Font = new Font("Segoe UI", 10F),
                Minimum = 1900,
                Maximum = 2030,
                Value = DateTime.Now.Year
            };

            contentHeaderPanel.Controls.AddRange(new Control[] {seriesLabel, seriesNameTextBox, yearLabel, yearNumericUpDown});
        }

        // create control panal contents
        private void CreateControlsPanelContent(Panel panel)
        {
            // add Season button
            addSeasonButton = new Button
            {
                Text = "+ Add Season",
                Location = new Point(15, 15),
                Size = new Size(430, 35),
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                BackColor = Color.FromArgb(72, 187, 120),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            addSeasonButton.FlatAppearance.BorderSize = 0;
            addSeasonButton.Click += AddSeasonButton_Click;

            // output directory label and controls
            Label directoryLabel = new Label
            {
                Text = "Output Directory",
                Location = new Point(15, 65),
                Size = new Size(120, 20),
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.FromArgb(45, 55, 72)
            };

            outputDirectoryTextBox = new TextBox
            {
                Location = new Point(15, 85),
                Size = new Size(250, 25),
                Font = new Font("Segoe UI", 10F),
                Text = @"C:\TV Shows\"
            };

            //create button for open to select path user
            browseButton = new Button
            {
                Text = "Browse",
                Location = new Point(275, 85),
                Size = new Size(70, 25),
                Font = new Font("Segoe UI", 9F),
                BackColor = Color.FromArgb(113, 128, 150),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            browseButton.FlatAppearance.BorderSize = 0;
            browseButton.Click += BrowseButton_Click;

            //create generate button
            generateButton = new Button
            {
                Text = "Generate Structure",
                Location = new Point(355, 85),
                Size = new Size(120, 25),
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                BackColor = Color.FromArgb(99, 102, 241),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            generateButton.FlatAppearance.BorderSize = 0;
            generateButton.Click += GenerateButton_Click;

            panel.Controls.AddRange(new Control[] {addSeasonButton, directoryLabel, outputDirectoryTextBox, browseButton, generateButton});
        }

        // open browse to select path to save folders
        private void BrowseButton_Click(object sender, EventArgs e)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                dialog.Description = "Select output directory for TV series folders";
                dialog.SelectedPath = outputDirectoryTextBox.Text;

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    outputDirectoryTextBox.Text = dialog.SelectedPath;
                }
            }
        }

        //genarate folder button click event
        private void GenerateButton_Click(object sender, EventArgs e)
        {
            if (ValidateInput())
            {
                try
                {
                    CreateFolder(); // pass to create folders
                    HistoryManager.AddManualHistoryItem(seriesNameTextBox.Text.Trim(), (int)yearNumericUpDown.Value, outputDirectoryTextBox.Text.Trim()); // pass historManager class to save that created tv serias info

                    MessageBox.Show("Folder structure created successfully!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error creating folders: {ex.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        //check all filds user enter correctly
        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(seriesNameTextBox.Text) || seriesNameTextBox.Text == "Enter series name...")
            {
                MessageBox.Show("Please enter a series name.", "Validation Error",MessageBoxButtons.OK, MessageBoxIcon.Warning);
                seriesNameTextBox.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(outputDirectoryTextBox.Text))
            {
                MessageBox.Show("Please select an output directory.", "Validation Error",MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (seasonCards.Count == 0)
            {
                MessageBox.Show("Please add at least one season.", "Validation Error",MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        //create folders
        private void CreateFolder()
        {
            string seriesName = seriesNameTextBox.Text.Trim();
            string year = yearNumericUpDown.Value.ToString();
            string basePath = outputDirectoryTextBox.Text.Trim();

            // Create main series folder
            string seriesFolderName = $"{seriesName} ({year})";
            string seriesPath = Path.Combine(basePath, seriesFolderName);
            Directory.CreateDirectory(seriesPath);

            // Create season folders
            foreach (var seasonCard in seasonCards)
            {
                var seasonInfo = seasonCard.GetSeasonInfo();
                if (seasonInfo.EpisodeCount > 0)
                {
                    string seasonFolderName = $"Season {seasonInfo.SeasonNumber:D2}";
                    string seasonPath = Path.Combine(seriesPath, seasonFolderName);
                    Directory.CreateDirectory(seasonPath);

                    // optionally create episode placeholders
                    for (int i = 1; i <= seasonInfo.EpisodeCount; i++)
                    {
                        string episodeFolder = $"S{seasonInfo.SeasonNumber:D2}E{i:D2}";
                        Directory.CreateDirectory(Path.Combine(seasonPath, episodeFolder));
                    }
                }
            }
        }

        //create footer panal
        private void CreateFooterPanel()
        {
            var formFooterPanel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 50,
                BackColor = Color.FromArgb(207, 223, 252)
            };

            var copyrightLabel = new Label
            {
                Text = "© 2024 TVFolderCraft. All rights reserved.",
                Font = new Font("roboto", 9),
                ForeColor = Color.FromArgb(0,0,0),
                AutoSize = true,
                Location = new Point(20, 20)
            };

            var versionLabel = new Label
            {
                Text = "Version 1.0.0",
                Font = new Font("roboto", 9),
                ForeColor = Color.FromArgb(128, 128, 128),
                AutoSize = true
            };
            versionLabel.Location = new Point(this.Width - versionLabel.PreferredWidth - 20, 20);

            var supportLabel = new Label
            {
                Text = "Support",
                Font = new Font("roboto", 9),
                ForeColor = Color.FromArgb(70, 130, 250),
                AutoSize = true,
                Cursor = Cursors.Hand
            };
            supportLabel.Location = new Point(this.Width - versionLabel.PreferredWidth - supportLabel.PreferredWidth - 40, 20);

            // handle the click event in support
            supportLabel.Click += (s, e) => EmailSupport.OpenSupportEmail();

            var documentationLabel = new Label
            {
                Text = "Documentation",
                Font = new Font("roboto", 9),
                ForeColor = Color.FromArgb(70, 130, 250),
                AutoSize = true,
                Cursor = Cursors.Hand
            };
            documentationLabel.Location = new Point(this.Width - versionLabel.PreferredWidth - supportLabel.PreferredWidth - documentationLabel.PreferredWidth - 70, 20);

            // handle the click event in documentation
            documentationLabel.Click += (s, e) => DocumentationHelper.OpenDocumentation();

            formFooterPanel.Controls.AddRange(new Control[] { copyrightLabel, documentationLabel, supportLabel, versionLabel });

            // add the footer panel directly to the form 
            this.Controls.Add(formFooterPanel);
        }
        //adding seasons panal when user adding
        private void AddSeasonCard()
        {
            var seasonCard = new SeasonCard(seasonCounter++); // create object to SeasonCard class
            seasonCard.OnRemoveRequested += RemoveSeasonCard;
            seasonCards.Add(seasonCard);
            seasonsPanel.Controls.Add(seasonCard);
            UpdateLayout();
        }
        //remove seasons panala if user close
        private void RemoveSeasonCard(SeasonCard card)
        {
            seasonCards.Remove(card);
            seasonsPanel.Controls.Remove(card);
            card.Dispose();
            UpdateLayout();
        }
        //update layout
        private void UpdateLayout()
        {
            int yPos = 0;
            foreach (var card in seasonCards)
            {
                card.Location = new Point(0, yPos);
                yPos += card.Height + 10;
            }

            seasonsPanel.Height = Math.Max(yPos, 100);
            footerPanel.Location = new Point(50, seasonsPanel.Location.Y + seasonsPanel.Height + 30);

            // update main panel scroll size
            mainPanel.AutoScrollMinSize = new Size(0, footerPanel.Location.Y + footerPanel.Height + 40);
        }
        //event acc season button
        private void AddSeasonButton_Click(object sender, EventArgs e)
        {
            AddSeasonCard();
        }

        // Navigation methods
        private void NavigateToHome()
        {
            this.Hide();
            var homeForm = new Form1();
            homeForm.Show();
            homeForm.FormClosed += (s, e) => this.Close();
        }
        private void NavigateToHistoryPage()
        {
            this.Hide();
            var historyForm = new HistoryPage();
            historyForm.Show();
            historyForm.FormClosed += (s, e) => this.Close();
        }
        private void NavigateToSettingPage()
        {
            this.Hide();
            var settingForm = new SettingPage();
            settingForm.Show();
            settingForm.FormClosed += (s, e) => this.Close();
        }
        private void NavigateToInternet()
        {
            this.Hide();
            var settingForm = new InternetPage();
            settingForm.Show();
            settingForm.FormClosed += (s, e) => this.Close();
        }
    }

    public class SeasonCard : Panel
    {
        private NumericUpDown episodeCountNumeric;
        private NumericUpDown yearNumeric;
        private Button removeButton;
        private int seasonNumber;

        public event Action<SeasonCard> OnRemoveRequested;

        public SeasonCard(int seasonNum)
        {
            seasonNumber = seasonNum;
            InitializeCard();
            CreateControls();
            StyleCard();
        }

        private void InitializeCard()
        {
            this.Size = new Size(460, 80);
            this.BackColor = Color.FromArgb(240, 147, 251);
            this.BorderStyle = BorderStyle.None;
            this.Margin = new Padding(0, 0, 0, 10);
        }

        //create seasons card component
        private void CreateControls()
        {
            // season label
            Label seasonLabel = new Label
            {
                Text = $"Season {seasonNumber}",
                Location = new Point(15, 15),
                Size = new Size(100, 20),
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Transparent
            };

            // episodes label and numeric
            Label episodesLabel = new Label
            {
                Text = "Episodes",
                Location = new Point(15, 40),
                Size = new Size(70, 15),
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.White,
                BackColor = Color.Transparent
            };

            episodeCountNumeric = new NumericUpDown
            {
                Location = new Point(90, 38),
                Size = new Size(60, 25),
                Font = new Font("Segoe UI", 9F),
                Minimum = 1,
                Maximum = 100,
                Value = seasonNumber == 1 ? 10 : 12
            };

            // Year label and numeric
            Label yearLabel = new Label
            {
                Text = "Year",
                Location = new Point(200, 40),
                Size = new Size(40, 15),
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.White,
                BackColor = Color.Transparent
            };

            yearNumeric = new NumericUpDown
            {
                Location = new Point(245, 38),
                Size = new Size(70, 25),
                Font = new Font("Segoe UI", 9F),
                Minimum = 1900,
                Maximum = 2030,
                Value = DateTime.Now.Year
            };

            // remove button
            removeButton = new Button
            {
                Text = "×",
                Location = new Point(420, 10),
                Size = new Size(25, 25),
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(245, 87, 108),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            removeButton.FlatAppearance.BorderSize = 0;
            removeButton.Click += RemoveButton_Click;

            this.Controls.AddRange(new Control[] {
                seasonLabel, episodesLabel, episodeCountNumeric, yearLabel, yearNumeric, removeButton
            });
        }

        private void StyleCard()
        {
            this.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                var rect = new Rectangle(0, 0, this.Width - 1, this.Height - 1);
            };
        }
        // event remove button click
        private void RemoveButton_Click(object sender, EventArgs e)
        {
            OnRemoveRequested?.Invoke(this);
        }

        // return to all deatils 
        public SeasonInfo GetSeasonInfo()
        {
            return new SeasonInfo
            {
                SeasonNumber = seasonNumber,
                EpisodeCount = (int)episodeCountNumeric.Value,
                Year = (int)yearNumeric.Value
            };
        }
    }

    //create data transfer object to get and set values
    public class SeasonInfo
    {
        public int SeasonNumber { get; set; }
        public int EpisodeCount { get; set; }
        public int Year { get; set; }
    }
}