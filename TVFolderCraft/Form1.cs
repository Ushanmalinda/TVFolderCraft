using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace TVFolderCraft
{
    public partial class Form1 : Form
    {
        private NavigationHeader navigationHeader;
        private FooterPanel footerPanel;
        private Panel contentPanel;
        private Label titleLabel;
        private Label subtitleLabel;
        private Button getStartButton;
        private Label quickActionsLabel;
        public Form1()
        {
            InitializeComponent();
            SetupUI();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout(); 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(242)))), ((int)(((byte)(245)))));
            this.ClientSize = new System.Drawing.Size(1024, 768);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TVFolderCraft";
            this.Load += new System.EventHandler(this.Form1_Load_1);
            this.ResumeLayout(false);

        }
        // create ui part
        private void SetupUI()
        {
            try
            {
                // create navigation header
                navigationHeader = new NavigationHeader(this, "Home");
                this.Controls.Add(navigationHeader);

                // create footer panel
                footerPanel = new FooterPanel(this);
                this.Controls.Add(footerPanel);

                CreateContentPanel();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        //create content area
        private void CreateContentPanel()
        {
            contentPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(240, 242, 245),
                Padding = new Padding(40),
                BackgroundImage = Image.FromFile("D:\\Campuse\\Mini Project\\Program\\WinForm\\TVFolderCraft\\TVFolderCraft\\TVFolderCraft\\Images\\backgrount.png"),
                BackgroundImageLayout = ImageLayout.Stretch
            };

            // Welcome title
            titleLabel = new Label
            {
                Text = "Welcome To TVFolderCraft",
                Font = new Font("roboto", 35, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 58, 138),
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.Transparent
            };
            titleLabel.Location = new Point((this.Width - titleLabel.PreferredWidth) / 2, 90);

            // Subtitle
            subtitleLabel = new Label
            {
                Text = "create your Tv series Collection effortlessly with automatic\nmetadata fetching and intelligent file organization",
                Font = new Font("Roboto", 10, FontStyle.Italic),
                ForeColor = Color.FromArgb(102, 102, 102),
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.Transparent
            };
            subtitleLabel.Location = new Point((this.Width - subtitleLabel.PreferredWidth) / 2, 160);

            // Get Start button
            getStartButton = CreateModernButton("Get Start", Color.FromArgb(70, 130, 250), Color.Black);
            getStartButton.Size = new Size(150, 45);
            getStartButton.Location = new Point((this.Width - getStartButton.Width) / 2, 230);
            getStartButton.Region = new Region(GetRoundedRectanglePath(new Rectangle(0, 0, getStartButton.Width, getStartButton.Height), 50)); //create round button
            getStartButton.Click += (s, e) => NavigateToIntrernetPage(); //click event

            // Quick Actions label
            quickActionsLabel = new Label
            {
                Text = "Quick Actions",
                Font = new Font("Acme Gothic", 22, FontStyle.Bold|FontStyle.Underline),
                ForeColor = Color.FromArgb(51, 51, 51),
                AutoSize = true,
                BackColor = Color.Transparent
            };
            quickActionsLabel.Location = new Point((this.Width - quickActionsLabel.PreferredWidth) / 2, 300);

            // Action cards with navigation
            var newProjectCard = CreateActionCard("➕", "New Project", "Start organizing a new Tv Series",
                new Point(this.Width / 2 - 310, 370), NavigateToNewProject);

            var recentFilesCard = CreateActionCard("🕒", "Recent Files", "Access your recently processed files",
                new Point(this.Width / 2 + 10, 370), NavigateToHistoryPage);

            var searchCard = CreateActionCard("🌐", "Search From Internet", "Searching number of seasons",
                new Point(this.Width / 2 - 310, 470), NavigateToIntrernetPage);

            var settingsCard = CreateActionCard("⚙️", "Setting", "Customize your preferences",
                new Point(this.Width / 2 + 10, 470), NavigateToSettingPage);

            contentPanel.Controls.AddRange(new Control[] {titleLabel, subtitleLabel, getStartButton, quickActionsLabel,newProjectCard, recentFilesCard, searchCard, settingsCard }
            );

            this.Controls.Add(contentPanel);
        }
        
        //create panal for quick action button
        private Panel CreateActionCard(string icon, string title, string description, Point location, Action clickAction = null)
        {
            var card = new Panel
            {
                Size = new Size(280, 80),
                Location = location,
                BackColor = Color.White,
                Cursor = Cursors.Hand
            };

            // add click event to the card
            if (clickAction != null)
            {
                card.Click += (s, e) => clickAction();
            }

            var iconLabel = new Label
            {
                Text = icon,
                Font = new Font("Segoe UI", 16),
                AutoSize = true,
                Location = new Point(20, 15),
                BackColor = Color.Transparent
            };

            var titleLabel = new Label
            {
                Text = title,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.FromArgb(51, 51, 51),
                AutoSize = true,
                Location = new Point(60, 12),
                BackColor = Color.Transparent
            };

            var descLabel = new Label
            {
                Text = description,
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.FromArgb(128, 128, 128),
                AutoSize = true,
                Location = new Point(60, 35),
                BackColor = Color.Transparent
            };

            // make labels clickable
            if (clickAction != null)
            {
                iconLabel.Click += (s, e) => clickAction();
                titleLabel.Click += (s, e) => clickAction();
                descLabel.Click += (s, e) => clickAction();

                // set cursor for labels
                iconLabel.Cursor = Cursors.Hand;
                titleLabel.Cursor = Cursors.Hand;
                descLabel.Cursor = Cursors.Hand;
            }

            // hover effects
            card.MouseEnter += (s, e) => card.BackColor = Color.FromArgb(228, 229, 230);
            card.MouseLeave += (s, e) => card.BackColor = Color.White;

            card.Controls.AddRange(new Control[] { iconLabel, titleLabel, descLabel });
            return card;
        }
        //create button
        private Button CreateModernButton(string text, Color backColor, Color foreColor)
        {
            var button = new Button
            {
                Text = text,
                Font = new Font("Verdana", 16, FontStyle.Bold | FontStyle.Italic),
                ForeColor = foreColor,
                BackColor = backColor,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                TextAlign = ContentAlignment.MiddleCenter
            };

            button.FlatAppearance.BorderSize = 0;
            button.FlatAppearance.MouseOverBackColor = Color.Transparent;

            // store the original colors for hover effects
            Color originalBackColor = backColor;
            Color hoverBackColor = Color.FromArgb(60, 120, 240);

            // add hover effects
            button.MouseEnter += (sender, e) => {
                button.BackColor = hoverBackColor;
                button.Invalidate(); // force repaint
            };

            button.MouseLeave += (sender, e) => {
                button.BackColor = originalBackColor;
                button.Invalidate(); // force repaint
            };

            return button;
        }
        private GraphicsPath GetRoundedRectanglePath(Rectangle rect, int radius)
        {
            var path = new GraphicsPath();
            path.AddArc(rect.X, rect.Y, radius, radius, 180, 90);
            path.AddArc(rect.X + rect.Width - radius, rect.Y, radius, radius, 270, 90);
            path.AddArc(rect.X + rect.Width - radius, rect.Y + rect.Height - radius, radius, radius, 0, 90);
            path.AddArc(rect.X, rect.Y + rect.Height - radius, radius, radius, 90, 90);
            path.CloseAllFigures();
            return path;
        }
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (contentPanel != null)
            {
                // recenter elements when form is resized
                if (titleLabel != null)
                    titleLabel.Location = new Point((this.Width - titleLabel.PreferredWidth) / 2, 80);
                if (subtitleLabel != null)
                    subtitleLabel.Location = new Point((this.Width - subtitleLabel.PreferredWidth) / 2, 130);
                if (getStartButton != null)
                    getStartButton.Location = new Point((this.Width - getStartButton.Width) / 2, 190);
                if (quickActionsLabel != null)
                    quickActionsLabel.Location = new Point((this.Width - quickActionsLabel.PreferredWidth) / 2, 280);
            }
        }
        private void Form1_Load_1(object sender, EventArgs e)
        {

        }
        
        //navigation methods
        private async void NavigateToIntrernetPage()
        {
            this.Hide();
            var mainForm = new InternetPage();
            mainForm.Show();
            mainForm.FormClosed += (s, e) => this.Close();

        }
        private void NavigateToHistoryPage()
        {
            this.Hide();
            var mainForm = new HistoryPage();
            mainForm.Show();
            mainForm.FormClosed += (s, e) => this.Close();

        }
        private void NavigateToSettingPage()
        {
            this.Hide();
            var mainForm = new SettingPage();
            mainForm.Show();
            mainForm.FormClosed += (s, e) => this.Close();

        }
        private void NavigateToNewProject()
        {
            this.Hide();


            var newProjectForm = new GanaratePage();
            newProjectForm.Show();
            newProjectForm.FormClosed += (s, e) => this.Close();

        }
    }
}
