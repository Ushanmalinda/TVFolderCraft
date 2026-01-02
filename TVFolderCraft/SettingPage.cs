using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http;
using System.Net;


namespace TVFolderCraft
{
    public class ApiValidationResult
    {
        public bool IsValid { get; set; }
        public string Message { get; set; }
        public string Details { get; set; }
    }
    public partial class SettingPage : Form
    {
        private string downloads;
        private string apiKey;

        private Panel headerPanel;
        private Panel sidebarPanel;
        private Panel contentPanel;
        private Panel footerPanel;
        private Button hamburgerButton;
        private bool isSidebarVisible = false;

        public SettingPage()
        {
            InitializeComponent();
            LoadSettings();
            SetupUI();
            ShowGeneralSettings();
        }

        //load setting if user change past
        private void LoadSettings()
        {
            // Load the user save location
            downloads = Properties.Settings.Default.DefaultDownloadPath;

            //if that location empy gest default as dounload path
            if (string.IsNullOrEmpty(downloads))
            {
                downloads = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
                //save the default path
                Properties.Settings.Default.DefaultDownloadPath = downloads;
                Properties.Settings.Default.Save();
            }

            //load the saved API key
            apiKey = Properties.Settings.Default.ApiKey;
            //if apikey is null set defaul as '';
            if (string.IsNullOrEmpty(apiKey))
            {
                apiKey = "";
            }
        }
        private void SetupUI()
        {
            CreateHeaderPanel();//create header panal
            CreateContentPanel();//create content area
            CreateSidebarPanel();//create slide panal
            CreateFooterPanel();//create footer panal
        }
        //create header
        private void CreateHeaderPanel()
        {
            headerPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 70,
                BackColor = Color.FromArgb(207, 223, 252)
            };

            //hamburger menu button (3 lines)
            hamburgerButton = new Button
            {
                Size = new Size(40, 40),
                Location = new Point(20, 20),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.Transparent,
                Cursor = Cursors.Hand,
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                Text = "☰", //hmburger icon
                ForeColor = Color.FromArgb(51, 51, 51)
            };
            hamburgerButton.FlatAppearance.BorderSize = 0;
            hamburgerButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(230, 235, 240);
            hamburgerButton.Click += HamburgerButton_Click;

            //logo
            var logo = new PictureBox
            {
                Size = new Size(70, 70),
                Location = new Point(50, 0),
                SizeMode = PictureBoxSizeMode.StretchImage, // Adjust how the image fits
                Image = Properties.Resources.Logo
            };

            //app name
            var logoLabel = new Label
            {
                Text = "TVFolderCraft",
                Font = new Font("roboto", 20, FontStyle.Bold),
                ForeColor = Color.FromArgb(51, 51, 51),
                AutoSize = true,
                Location = new Point(120, 23)
            };

            //navigation buttons
            var homeButton = CreateNavButton("Home", false);
            homeButton.Location = new Point(this.Width - 360, 25);
            homeButton.Click += (s, e) => NavigateToHome();

            var generateButton = CreateNavButton("Generate", false);
            generateButton.Location = new Point(this.Width - 280, 27);
            generateButton.Click += (s, e) => NavigateToInternetPage();

            var historyButton = CreateNavButton("History", false);
            historyButton.Location = new Point(this.Width - 200, 25);
            historyButton.Click += (s, e) => NavigateToHistory();

            var settingButton = CreateNavButton("Setting", true);
            settingButton.Location = new Point(this.Width - 120, 25);

            headerPanel.Controls.AddRange(new Control[] { hamburgerButton, logo, logoLabel, homeButton, generateButton, historyButton, settingButton });
            this.Controls.Add(headerPanel);
        }
        //content area
        private void CreateContentPanel()
        {
            contentPanel = new Panel
            {
                BackColor = Color.FromArgb(240, 242, 245),
                Padding = new Padding(30, 20, 30, 20),
                Location = new Point(0, 70), 
                Size = new Size(1024, 609),
                BackgroundImage = Properties.Resources.backgrount,
                BackgroundImageLayout = ImageLayout.Stretch,
            };
            this.Controls.Add(contentPanel);
        }
        //create slidePanal
        private void CreateSidebarPanel()
        {
            sidebarPanel = new Panel
            {
                Width = 200,
                Height = this.Height - 160,
                Location = new Point(0, 80),
                BackColor = Color.FromArgb(250, 251, 252),
                Padding = new Padding(0, 20, 0, 0),
                Visible = false
            };

            //general settings button
            var generalButton = CreateSidebarButton("⚙️ General Settings", true);
            generalButton.Location = new Point(0, 20);
            generalButton.Click += (s, e) => {
                UpdateSidebarSelection(generalButton);
                ShowGeneralSettings();
                ToggleSidebar(); //close sidebar after selection
            };

            //privacy button
            var privacyButton = CreateSidebarButton("🔒 Privacy", false);
            privacyButton.Location = new Point(0, 70);
            privacyButton.Click += (s, e) => {
                UpdateSidebarSelection(privacyButton);
                ShowPrivacySettings();
                ToggleSidebar(); //close sidebar after selection
            };

            //logout button
            var logoutButton = CreateSidebarButton("🚪 LogOut", false);
            logoutButton.Location = new Point(0, 120);
            logoutButton.Click += (s, e) => {
                UpdateSidebarSelection(logoutButton);
                ShowLogoutSettings();
                ToggleSidebar(); //close sidebar after selection
            };

            sidebarPanel.Controls.AddRange(new Control[] { generalButton, privacyButton, logoutButton });
            this.Controls.Add(sidebarPanel);
        }
        //create footer panal
        private void CreateFooterPanel()
        {
            footerPanel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 50,
                BackColor = Color.FromArgb(207, 223, 252),
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
                AutoSize = true,
                Location = new Point(50, 20)
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

            footerPanel.Controls.AddRange(new Control[] { copyrightLabel, documentationLabel, supportLabel, versionLabel });
            this.Controls.Add(footerPanel);
        }
        //create navigation button
        private Button CreateNavButton(string text, bool isActive)
        {
            var button = new Button
            {
                Text = text,
                Size = new Size(70, 30),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("roboto", 10,FontStyle.Bold),
                Cursor = Cursors.Hand
            };

            if (isActive)
            {
                button.ForeColor = Color.FromArgb(70, 130, 250);
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
        
        // save user enter or default value to properties setting
        private void SaveSettings()
        {
            Properties.Settings.Default.DefaultDownloadPath = downloads;
            Properties.Settings.Default.ApiKey = apiKey;
            Properties.Settings.Default.Save();
        }
        //general setting panal 
        private void ShowGeneralSettings()
        {
            contentPanel.Controls.Clear();

            //add main page title at the top
            var pageTitle = new Label
            {
                Text = "General Settings",
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                ForeColor = Color.FromArgb(51, 51, 51),
                Location = new Point(20, 20),
                AutoSize = true,
                BackColor = Color.Transparent
            };

            //default Creation Location Section
            var locationPanel = CreateSettingsSection("Default Creation Location");
            locationPanel.Location = new Point(20, 70);
            locationPanel.Height = 100;
            locationPanel.Width = 700;

            var locationTextBox = new TextBox
            {
                Text = downloads, //show the saved location
                Size = new Size(400, 30),
                Font = new Font("Segoe UI", 9),
                Location = new Point(20, 40)
            };

            var browseButton = CreateModernButton("Browse", Color.FromArgb(70, 130, 250), Color.White);
            browseButton.Size = new Size(80, 30);
            browseButton.Location = new Point(440, 40);
            browseButton.Click += (s, e) => BrowseButton_Click(s, e, locationTextBox);

            var saveLocationButton = CreateModernButton("Save Location", Color.FromArgb(70, 130, 250), Color.White);
            saveLocationButton.Size = new Size(120, 30);
            saveLocationButton.Location = new Point(530, 40);
            //click event if user save location
            saveLocationButton.Click += (s, e) =>
            {
                if (!string.IsNullOrEmpty(locationTextBox.Text) && Directory.Exists(locationTextBox.Text))
                {
                    downloads = locationTextBox.Text;
                    SaveSettings();
                    MessageBox.Show("Default location saved successfully!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Please select a valid location first.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            };

            locationPanel.Controls.AddRange(new Control[] { locationTextBox, browseButton, saveLocationButton });

            //api settings section
            var apiPanel = CreateSettingsSection("API Settings (TMDB)");
            apiPanel.Location = new Point(20, 210);
            apiPanel.Height = 140; 
            apiPanel.Width = 700;


            //api key input textbox
            var apiTextBox = new TextBox
            {
                Text = string.IsNullOrEmpty(apiKey) ? "Enter your TMDB API key here..." : apiKey,
                Size = new Size(300, 30),
                Font = new Font("Segoe UI", 9),
                Location = new Point(20, 40),
                ForeColor = string.IsNullOrEmpty(apiKey) ? Color.Gray : Color.Black
            };

            //add placeholder behavior
            apiTextBox.Enter += (s, e) => {
                if (apiTextBox.Text == "Enter your TMDB API key here..." && apiTextBox.ForeColor == Color.Gray)
                {
                    apiTextBox.Text = "";
                    apiTextBox.ForeColor = Color.Black;
                }
            };

            apiTextBox.Leave += (s, e) => {
                if (string.IsNullOrWhiteSpace(apiTextBox.Text))
                {
                    apiTextBox.Text = "Enter your TMDB API key here...";
                    apiTextBox.ForeColor = Color.Gray;
                }
            };



            //test api button
            var testApiButton = CreateModernButton("Test API", Color.FromArgb(40, 167, 69), Color.White);
            testApiButton.Size = new Size(80, 30);
            testApiButton.Location = new Point(330, 40);
            //event test button 
            testApiButton.Click += async (s, e) => await TestApiButton_Click(s, e, apiTextBox);

            //save api button
            var saveApiButton = CreateModernButton("Save API", Color.FromArgb(70, 130, 250), Color.White);
            saveApiButton.Size = new Size(100, 30);
            saveApiButton.Location = new Point(440, 40);
            //event pass value if user click save button
            saveApiButton.Click += (s, e) => SaveApiButton_Click(s, e, apiTextBox);

            //reset api button
            var resetApiButton = CreateModernButton("Reset", Color.FromArgb(220, 53, 69), Color.White);
            resetApiButton.Size = new Size(80, 30);
            resetApiButton.Location = new Point(560, 40);
            resetApiButton.Click += (s, e) => ResetApiButton_Click(s, e, apiTextBox);

            //description label
            var apiDescLabel = new Label
            {
                Text = "Get your free API key from: https://www.themoviedb.org/",
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.FromArgb(128, 128, 128),
                Location = new Point(20, 75),
                AutoSize = true
            };

            //status label for showing test results
            var apiStatusLabel = new Label
            {
                Text = string.IsNullOrEmpty(apiKey) ? "No API key configured" : "API key saved (click Test to verify)",
                Font = new Font("Segoe UI", 9, FontStyle.Italic),
                ForeColor = string.IsNullOrEmpty(apiKey) ? Color.FromArgb(220, 53, 69) : Color.FromArgb(128, 128, 128),
                Location = new Point(20, 95),
                AutoSize = true,
                Name = "apiStatusLabel"
            };

            apiPanel.Controls.AddRange(new Control[] { apiTextBox, testApiButton, saveApiButton, resetApiButton, apiDescLabel, apiStatusLabel });


            //history Section
            var historyPanel = CreateSettingsSection("History");
            historyPanel.Location = new Point(20, 400);
            historyPanel.Width = 500;

            var clearFolderButton = CreateActionButton("Clear Folder Creation History", Color.FromArgb(220, 53, 69));
            clearFolderButton.Location = new Point(20, 50);
            clearFolderButton.Click += (s, e) => ShowConfirmation("Clear Folder Creation History");

            var resetAllButton = CreateActionButton("Reset All", Color.FromArgb(220, 53, 69));
            resetAllButton.Location = new Point(20, 140);
            resetAllButton.Click += (s, e) => ShowConfirmation("Reset All Settings");

            historyPanel.Controls.AddRange(new Control[] { clearFolderButton, resetAllButton });

            //add all controls to content panel
            contentPanel.Controls.AddRange(new Control[] { pageTitle, locationPanel, apiPanel, historyPanel });
        }
        //creating buttons for clear history
        private Button CreateActionButton(string text, Color backColor)
        {
            var button = new Button
            {
                Text = text + " ➤",
                Size = new Size(300, 35),
                Font = new Font("roboto", 10, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = backColor,
                FlatStyle = FlatStyle.Flat,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(10, 0, 0, 0),
                Cursor = Cursors.Hand
            };

            button.FlatAppearance.BorderSize = 0;

            return button;
        }
        //create button for general setting area
        private Button CreateModernButton(string text, Color backColor, Color foreColor)
        {
            var button = new Button
            {
                Text = text,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = foreColor,
                BackColor = backColor,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                TextAlign = ContentAlignment.MiddleCenter
            };

            button.FlatAppearance.BorderSize = 0;

            

            return button;
        }
        //event textapibutton click
        private async Task TestApiButton_Click(object sender, EventArgs e, TextBox apiTextBox)
        {
            var testButton = sender as Button;
            var statusLabel = contentPanel.Controls[2].Controls.OfType<Control>().FirstOrDefault(c => c.Name == "apiStatusLabel") as Label;

            //if user didnt enter api
            if (string.IsNullOrEmpty(apiTextBox.Text.Trim()))
            {
                MessageBox.Show("Please enter an API key first.", "No API Key",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            //event disable the test button and show testing status
            testButton.Enabled = false;
            testButton.Text = "Testing...";
            if (statusLabel != null)
            {
                statusLabel.Text = "Testing API key...";
                statusLabel.ForeColor = Color.FromArgb(255, 193, 7);
            }

            try
            {
                //test the api key
                var validationResult = await ValidateApiKeyWithDetailsAsync(apiTextBox.Text.Trim());

                //if api valid
                if (validationResult.IsValid)
                {
                    //success
                    MessageBox.Show($"{validationResult.Message}\n\n{validationResult.Details}",
                        "API Key Valid", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    if (statusLabel != null)
                    {
                        statusLabel.Text = "✓ API key is valid and working";
                        statusLabel.ForeColor = Color.FromArgb(40, 167, 69);
                    }
                }
                else
                {
                    //failed
                    MessageBox.Show($"{validationResult.Message}\n\n{validationResult.Details}",
                        "API Key Invalid", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    if (statusLabel != null)
                    {
                        statusLabel.Text = "✗ API key validation failed";
                        statusLabel.ForeColor = Color.FromArgb(220, 53, 69);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error testing API key: {ex.Message}", "Test Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);

                if (statusLabel != null)
                {
                    statusLabel.Text = "Error testing API key";
                    statusLabel.ForeColor = Color.FromArgb(220, 53, 69);
                }
            }
            finally
            {
                //re-enable the test button
                testButton.Enabled = true;
                testButton.Text = "Test API";
            }
        }
        // event savebutton in api area
        private async void SaveApiButton_Click(object sender, EventArgs e, TextBox apiTextBox)
        {
            if (apiTextBox == null || string.IsNullOrEmpty(apiTextBox.Text.Trim()))
            {
                MessageBox.Show("Please enter a valid API key.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            //show testing message
            var statusLabel = contentPanel.Controls[2].Controls.OfType<Control>().FirstOrDefault(c => c.Name == "apiStatusLabel") as Label;
            if (statusLabel != null)
            {
                statusLabel.Text = "Testing API key...";
                statusLabel.ForeColor = Color.FromArgb(255, 193, 7);
            }

            //test the api key first
            var validationResult = await ValidateApiKeyWithDetailsAsync(apiTextBox.Text.Trim());

            if (!validationResult.IsValid)
            {
                //api key is invalid show error and don't save
                if (statusLabel != null)
                {
                    statusLabel.Text = "✗ API key validation failed";
                    statusLabel.ForeColor = Color.FromArgb(220, 53, 69);
                }

                MessageBox.Show($"API key validation failed:\n{validationResult.Message}\n\nPlease check your API key and try again.",
                    "Validation Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //if api key is valid - save it
            apiKey = apiTextBox.Text.Trim();
            SaveSettings();

            if (statusLabel != null)
            {
                statusLabel.Text = "✓ API key saved and verified";
                statusLabel.ForeColor = Color.FromArgb(40, 167, 69);
            }

            MessageBox.Show("API key tested and saved successfully!", "Success",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        //event resetapi button
        private void ResetApiButton_Click(object sender, EventArgs e, TextBox apiTextBox)
        {
            var result = MessageBox.Show("Are you sure you want to reset the API key?\nThis will clear the saved API key.",
                "Confirm Reset", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    //clear the api key from settings and Properties
                    apiKey = "";
                    Properties.Settings.Default.ApiKey = "";
                    Properties.Settings.Default.Save();

                    //reset the textbox to placeholder state
                    apiTextBox.Text = "Enter your TMDB API key here...";
                    apiTextBox.ForeColor = Color.Gray;

                    //update the status label
                    var statusLabel = contentPanel.Controls[2].Controls.OfType<Control>()
                        .FirstOrDefault(c => c.Name == "apiStatusLabel") as Label;

                    //display no api 
                    if (statusLabel != null)
                    {
                        statusLabel.Text = "No API key configured";
                        statusLabel.ForeColor = Color.FromArgb(220, 53, 69);
                    }
                    //display message if api reset oky
                    MessageBox.Show("API key has been reset successfully!", "Reset Complete",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error resetting API key: {ex.Message}", "Reset Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        //check api key is valide or not 
        private async Task<ApiValidationResult> ValidateApiKeyWithDetailsAsync(string apiKey)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.Timeout = TimeSpan.FromSeconds(10);

                    //test api key
                    string testUrl = $"https://api.themoviedb.org/3/configuration?api_key={apiKey}";

                    var response = await httpClient.GetAsync(testUrl);
                    string responseContent = await response.Content.ReadAsStringAsync();

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        return new ApiValidationResult
                        {
                            IsValid = true,
                            Message = "API key is valid and working!",
                            Details = "Successfully connected to TMDB API"
                        };
                    }
                    else if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        return new ApiValidationResult
                        {
                            IsValid = false,
                            Message = "Invalid API key",
                            Details = "The provided API key is not valid. Please check your key and try again."
                        };
                    }
                    else
                    {
                        return new ApiValidationResult
                        {
                            IsValid = false,
                            Message = "API validation failed",
                            Details = $"Server responded with: {response.StatusCode}"
                        };
                    }
                }
            }
            catch (HttpRequestException)
            {
                return new ApiValidationResult
                {
                    IsValid = false,
                    Message = "Network error",
                    Details = "Unable to connect to the internet. Please check your connection."
                };
            }
            catch (TaskCanceledException)
            {
                return new ApiValidationResult
                {
                    IsValid = false,
                    Message = "Request timeout",
                    Details = "The request took too long to complete. Please try again."
                };
            }
            catch (Exception ex)
            {
                return new ApiValidationResult
                {
                    IsValid = false,
                    Message = "Validation error",
                    Details = $"An error occurred: {ex.Message}"
                };
            }
        }
        //load setting page
        private void SettingPage_Load(object sender, EventArgs e)
        {

        }
        
        // event user clikc slidebar button
        private void HamburgerButton_Click(object sender, EventArgs e)
        {
            ToggleSidebar();
        }

        // visible and hide slide bar
        private void ToggleSidebar()
        {
            //make ass visible
            isSidebarVisible = !isSidebarVisible;

            if (isSidebarVisible)
            {
                //show sidebar instantly
                sidebarPanel.Visible = true;
                sidebarPanel.BringToFront();

                //adjust content panel
                contentPanel.Location = new Point(200, 81);
                contentPanel.Size = new Size(820, 605);
            }
            else
            {
                //hide sidebar instantly
                sidebarPanel.Visible = false;

                //adjust content panel to full width
                contentPanel.Location = new Point(0, 81);
                contentPanel.Size = new Size(1024, 605);
            }
        }
        
        //create slibebar button
        private Button CreateSidebarButton(string text, bool isSelected)
        {
            var button = new Button
            {
                Text = text,
                Size = new Size(200, 40),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(20, 0, 0, 0),
                Cursor = Cursors.Hand,
                Tag = isSelected
            };

            UpdateButtonStyle(button, isSelected);
            return button;
        }

        //update button color for current open panal
        private void UpdateButtonStyle(Button button, bool isSelected)
        {
            if (isSelected)
            {
                button.BackColor = Color.FromArgb(70, 130, 250);
                button.ForeColor = Color.White;
                button.FlatAppearance.BorderSize = 0;
            }
            else
            {
                button.BackColor = Color.Transparent;
                button.ForeColor = Color.FromArgb(51, 51, 51);
                button.FlatAppearance.BorderSize = 0;
                button.FlatAppearance.MouseOverBackColor = Color.FromArgb(240, 242, 245);
            }
        }

        //update sidebar buttons
        private void UpdateSidebarSelection(Button selectedButton)
        {
            //first reset all buttons
            foreach (Control control in sidebarPanel.Controls)
            {
                if (control is Button btn)
                {
                    UpdateButtonStyle(btn, false);
                }
            }

            //only highlight selected button
            UpdateButtonStyle(selectedButton, true);
        }

        // privacy area
        private void ShowPrivacySettings()
        {
            contentPanel.Controls.Clear();

            //page title
            var privacyLabel = new Label
            {
                Text = "Privacy Policy",
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                ForeColor = Color.FromArgb(51, 51, 51),
                Location = new Point(20, 20),
                AutoSize = true,
                BackColor = Color.Transparent
            };

            //create a scrollable panel for privacy content
            var scrollablePanel = new Panel
            {
                Location = new Point(20, 70),
                Size = new Size(contentPanel.Width, contentPanel.Height - 100),
                BackColor = Color.White,
                AutoScroll = true,
                BorderStyle = BorderStyle.None
            };

            //create privacy content with proper formatting
            var privacyContent = new RichTextBox
            {
                Location = new Point(20, 20),
                Size = new Size(scrollablePanel.Width - 60, 2000), //large height for content
                ReadOnly = true,
                BorderStyle = BorderStyle.None,
                BackColor = Color.White,
                Font = new Font("Segoe UI", 10),
                ScrollBars = RichTextBoxScrollBars.None //panel handles scrolling
            };

            //set the privacy policy content
            privacyContent.Rtf = GetPrivacyPolicyRTF();

             //add privacy content to scrollable panel
            scrollablePanel.Controls.Add(privacyContent);

            //add all controls to content panel
            contentPanel.Controls.AddRange(new Control[] { privacyLabel, scrollablePanel });
        }

        // privacy data
        private string GetPrivacyPolicyRTF()
        {
            return @"{\rtf1\ansi\deff0 {\fonttbl {\f0 Segoe UI;}}
{\colortbl;\red0\green0\blue0;\red70\green130\blue250;\red51\green51\blue51;\red40\green167\blue69;\red220\green53\blue69;}

\f0\fs24\cf2\b TVFolderCraft Privacy Policy\b0\par
\fs16\cf3\i Last updated: " + DateTime.Now.ToString("MMMM dd, yyyy") + @"\i0\par\par

\fs20\cf2\b 1. Your Privacy is Protected\b0\par
\fs16\cf3 TVFolderCraft is committed to protecting your privacy. This policy explains how we handle your information when you use our TV series organization application.\par\par

\fs18\cf2\b 2. Information We Collect\b0\par
\fs16\cf3\b Local File Information:\b0\par
\bullet File paths and names (to organize your TV series files)\par
\bullet File metadata (sizes, creation dates, media properties)\par
\bullet Folder structures and organization patterns\par
\bullet Your preferences and settings\par\par

\b Internet-Based Data:\b0\par
\bullet Series names you search for metadata\par
\bullet API requests sent to TMDB, TVDB databases\par
\bullet Downloaded series information and artwork\par\par

\b System Information:\b0\par
\bullet Operating system version (for compatibility)\par
\bullet Application version (for updates and support)\par
\bullet Error logs (technical information only, no personal data)\par\par

\cf4\b What We DO NOT Collect:\b0\cf3\par
\bullet \b Personal content:\b0 We never access your actual video files\par
\bullet \b Viewing history:\b0 We don't track what you watch\par
\bullet \b Personal identification:\b0 No names, emails, or contact info\par
\bullet \b Location data:\b0 No GPS or location tracking\par
\bullet \b Browsing history:\b0 No web activity tracking\par\par

\fs18\cf2\b 3. How We Use Your Information\b0\par
\fs16\cf3\b Primary Functions:\b0\par
\bullet File organization according to your preferences\par
\bullet Metadata fetching from online databases\par
\bullet Application improvement and bug fixes\par
\bullet User support when you contact us\par\par

\b Data Storage:\b0\par
\bullet \cf4 Local storage only:\cf3 All your file information stays on YOUR computer\par
\bullet \cf4 No cloud storage:\cf3 We don't upload your data to our servers\par
\bullet Temporary cache for performance (stored locally)\par\par

\fs18\cf2\b 4. Information Sharing\b0\par
\fs16\cf3\b Third-Party Services:\b0\par
We only integrate with these external services:\par
\bullet TMDB (The Movie Database) - for series information\par
\bullet TVDB (TheTVDB) - for episode details\par
\bullet IMDb - for additional series information\par\par

\b What we share:\b0 Only series names and search queries (no personal information)\par
\b What we receive:\b0 Public metadata about TV series only\par\par

\cf4\b We Never Share:\b0\cf3\par
\bullet Your file paths or folder structures\par
\bullet Personal information or contact details\par
\bullet Usage patterns or preferences\par
\bullet Any data that could identify you\par\par

\fs18\cf2\b 5. Data Security\b0\par
\fs16\cf3\b Protection Measures:\b0\par
\bullet \cf4 Local processing:\cf3 All file operations happen on your computer\par
\bullet \cf4 Encrypted connections:\cf3 All internet requests use HTTPS\par
\bullet \cf4 No data transmission:\cf3 Your file information never leaves your device\par
\bullet \cf4 Secure APIs:\cf3 We only use reputable metadata services\par\par

\b Your Data Control:\b0\par
\bullet \cf4 Full control:\cf3 You own and control all your data\par
\bullet \cf4 Easy removal:\cf3 Uninstalling removes all stored data\par
\bullet \cf4 No tracking:\cf3 We can't access your data remotely\par
\bullet \cf4 Optional features:\cf3 All internet features can be disabled\par\par

\fs18\cf2\b 6. Your Privacy Rights\b0\par
\fs16\cf3\b What You Can Do:\b0\par
\bullet \b Access:\b0 View all data stored by the application in Settings\par
\bullet \b Delete:\b0 Clear all application data through Settings or uninstallation\par
\bullet \b Control:\b0 Enable/disable internet features at any time\par
\bullet \b Export:\b0 Backup your settings and preferences\par\par

\b Data Retention:\b0\par
\bullet User settings: Stored until you delete them\par
\bullet Cache data: Automatically cleaned after 30 days\par
\bullet Error logs: Automatically deleted after 7 days\par
\bullet \cf4 No server data:\cf3 We don't store your data on our servers\par\par

\fs18\cf2\b 7. Technical Details\b0\par
\fs16\cf3\b Data Storage Locations:\b0\par
\bullet Windows: %AppData%\\TVFolderCraft\\\par
\bullet Settings: Local application data folder\par
\bullet Cache: Temporary folder, auto-cleaned\par
\bullet Logs: Auto-deleted after 7 days\par\par

\b Network Communications:\b0\par
\bullet HTTPS only: All internet requests are encrypted\par
\bullet API endpoints: Only official metadata service APIs\par
\bullet No background transmission: Internet features only work when actively used\par
\bullet \cf4 Offline mode available:\cf3 Full functionality without internet\par\par

\fs18\cf2\b 8. Contact Information\b0\par
\fs16\cf3 For privacy questions or data requests:\par
\bullet Click \b 'Privacy Questions'\b0 button above to contact us\par
\bullet We respond to privacy inquiries within 48 hours\par
\bullet Most requests can be handled through application Settings\par\par

\fs18\cf2\b Summary of Key Protections\b0\par
\fs16\cf4
\bullet Your files stay on your computer\par
\bullet No personal data collection\par
\bullet Local processing only\par
\bullet Encrypted internet requests\par
\bullet Optional internet features\par
\bullet Easy data deletion\par
\bullet No tracking or analytics\par
\bullet Full user control\par
\cf3\par

This privacy policy reflects our actual practices and is designed to protect your privacy while using TVFolderCraft.
}";
        }

        // logout area
        private void ShowLogoutSettings()
        {
            contentPanel.Controls.Clear();

            var logoutLabel = new Label
            {
                Text = "Account & Logout",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = Color.FromArgb(51, 51, 51),
                Location = new Point(20, 20),
                AutoSize = true
            };

            var logoutButton = CreateModernButton("Logout", Color.FromArgb(220, 53, 69), Color.White);
            logoutButton.Size = new Size(120, 40);
            logoutButton.Location = new Point(20, 80);
            //event if user click logout button
            logoutButton.Click += (s, e) => {
                var result = MessageBox.Show("Are you sure you want to logout?", "Confirm Logout",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    Application.Exit();
                }
            };

            contentPanel.Controls.AddRange(new Control[] { logoutLabel, logoutButton });
        }

        //create setting section panals
        private Panel CreateSettingsSection(string title)
        {
            var panel = new Panel
            {
                Size = new Size(contentPanel.Width - 60, 120),
                BackColor = Color.White
            };

            var titleLabel = new Label
            {
                Text = title,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(51, 51, 51),
                Location = new Point(20, 15),
                AutoSize = true,
                BackColor = Color.Transparent
            };

            panel.Controls.Add(titleLabel);
            return panel;
        }

        //get downloads path if default path null
        public static string GetDefaultDownloadPath()
        {
            var savedPath = Properties.Settings.Default.DefaultDownloadPath;
            if (string.IsNullOrEmpty(savedPath))
            {
                return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
            }
            return savedPath;
        }

        //event if user click BrowseButton to get path
        private void BrowseButton_Click(object sender, EventArgs e, TextBox locationTextBox)
        {
            using (var folderDialog = new FolderBrowserDialog())
            {
                folderDialog.SelectedPath = downloads; //start with current location
                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    locationTextBox.Text = folderDialog.SelectedPath;
                }
            }
        }
        
        //show message box if user click clear history
        private void ShowConfirmation(string action)
        {
            var result = MessageBox.Show($"Are you sure you want to {action.ToLower()}?",
                "Confirm Action", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    if (action == "Clear Folder Creation History")
                    {
                        HistoryManager.ClearHistory();
                        MessageBox.Show("Folder creation history cleared successfully!", "Success",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error performing action: {ex.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // Navigation methods
        private void NavigateToHome()
        {
            this.Hide();
            var mainForm = new Form1();
            mainForm.Show();
            mainForm.FormClosed += (s, e) => this.Close();
        }
        private void NavigateToHistory()
        {
            this.Hide();
            var historyForm = new HistoryPage();
            historyForm.Show();
            historyForm.FormClosed += (s, e) => this.Close();
        }
        private void NavigateToInternetPage()
        {
            this.Hide();
            var historyForm = new InternetPage();
            historyForm.Show();
            historyForm.FormClosed += (s, e) => this.Close();
        }
    }
}