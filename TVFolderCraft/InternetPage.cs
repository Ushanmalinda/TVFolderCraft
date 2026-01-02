using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.NetworkInformation;
using System.Net.Http;
using Newtonsoft.Json;
using System.IO;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;

namespace TVFolderCraft
{

    public class TMDBShowSearchResult
    {
        public List<TMDBShow> results { get; set; }
    }
    public class TMDBShow
    {
        public int id { get; set; }
        public string name { get; set; }
        public string[] genres { get; set; }
        public string status { get; set; }
        public string first_air_date { get; set; }
        public string last_air_date { get; set; }
        public TMDBShowImage poster_path { get; set; }
        public string overview { get; set; }
        public int number_of_seasons { get; set; }
        public int number_of_episodes { get; set; }
        public List<TMDBSeason> seasons { get; set; }
    }
    public class TMDBShowImage
    {
        public string medium { get; set; } // For poster_path
        public string original { get; set; }
    }
    public class TMDBSeason
    {
        public int id { get; set; }
        public int season_number { get; set; }
        public string name { get; set; }
        public int episode_count { get; set; }
        public string air_date { get; set; }
        public string poster_path { get; set; }
        public string overview { get; set; }
    }
    public class TMDBEpisode
    {
        public int id { get; set; }
        public string name { get; set; }
        public int season_number { get; set; }
        public int episode_number { get; set; }
        public string air_date { get; set; }
        public int runtime { get; set; }
        public string overview { get; set; }
        public string still_path { get; set; }
    }
    public class TMDBSeasonDetails
    {
        public int id { get; set; }
        public string name { get; set; }
        public string overview { get; set; }
        public int season_number { get; set; }
        public string air_date { get; set; }
        public List<TMDBEpisode> episodes { get; set; }
    }
    public class TVShowSearchResult
    {
        public TVShow show { get; set; }
    }
    public class TVShow
    {
        public int id { get; set; }
        public string name { get; set; }
        public string[] genres { get; set; }
        public string status { get; set; }
        public string premiered { get; set; }
        public string ended { get; set; }
        public TVShowImage image { get; set; }
        public string summary { get; set; }
    }
    public class TVShowImage
    {
        public string medium { get; set; }
        public string original { get; set; }
    }
    public class Season
    {
        public int id { get; set; }
        public int number { get; set; }
        public string name { get; set; }
        public int episodeOrder { get; set; }
        public string premiereDate { get; set; }
        public string endDate { get; set; }
        public TVShowImage image { get; set; }
        public string summary { get; set; }
        public Episode[] episodes { get; set; } 
    }
    public class Episode
    {
        public int id { get; set; }
        public string name { get; set; }
        public int season { get; set; }
        public int number { get; set; }
        public string airdate { get; set; }
        public int runtime { get; set; }
        public string summary { get; set; }
        public TVShowImage image { get; set; }
    }
    public class TVSeriesData
    {
        public string Name { get; set; }
        public int TotalSeasons { get; set; }
        public int TotalEpisodes { get; set; }
        public string Status { get; set; }
        public string[] Genres { get; set; }
        public string Summary { get; set; }
        public string Premiered { get; set; }
        
        public Season[] Seasons { get; set; }
    }
    public partial class InternetPage : Form
    {
        private NavigationHeader navigationHeader;
        private FooterPanel footerPanel;
        private Panel contentPanel;
        private Label titleLabel;
        private Label subtitleLabel;
        private Panel searchPanel;
        private TextBox searchTextBox;
        private Button searchButton;
        private Label trendingLabel;
        private Panel trendingSeriesPanel;
        private Panel searchTipsPanel;
        private static readonly HttpClient httpClient = new HttpClient();

        string url = "https://api.themoviedb.org/3/trending/tv/day?language=en-US"; // get info current day trending list 
        string bearerToken = "eyJhbGciOiJIUzI1NiJ9.eyJhdWQiOiIwNGRiYjg5YTA5ODcwZjJiNjUxOTc4ZjBkZjI1MjZkYSIsIm5iZiI6MTc0OTMyNzU4MS4xNDMwMDAxLCJzdWIiOiI2ODQ0OWVkZDViOGQyYTE4NDViYjJhZDQiLCJzY29wZXMiOlsiYXBpX3JlYWQiXSwidmVyc2lvbiI6MX0.vLQn5hsq6cO-aVONYtlFzCGAYi-b1yFZsjjLMpfKw3s"; // my personal tocken
        string[] seriesNames = new string[5]; // get last 5 Tv series names 
        string[] imagePath = new string[5]; // get last 5 Tv series image path 
        public InternetPage()
        {
            InitializeComponent();
            SetupUI();
        }
        private void InitializeComponent()
        {
            //create page
            this.SuspendLayout();
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(242)))), ((int)(((byte)(245)))));
            this.ClientSize = new System.Drawing.Size(1250, 800);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "InternetPage";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TVFolderCraft - Find Series";
            this.Load += new System.EventHandler(this.SearchForm_Load);
            this.ResumeLayout(false);
        }
        private async void SetupUI()
        {

            await FindData(); // wating for find data using internet 

            // create navigation header
            navigationHeader = new NavigationHeader(this, "InternetPage");
            this.Controls.Add(navigationHeader);

            // create footer panel
            footerPanel = new FooterPanel(this);
            this.Controls.Add(footerPanel);

            // create content panel
            CreateContentPanel();
        }
        //check api key
        private bool HasValidTMDBKey()
        {
            return !string.IsNullOrWhiteSpace(Properties.Settings.Default.ApiKey);
        }
        //create content area
        private void CreateContentPanel()
        {
            contentPanel = new Panel
            {
                BackColor = Color.FromArgb(240, 242, 245),
                Padding = new Padding(40),
                Location = new Point(0, 40),
                Size = new Size(1000, 600),
                BackgroundImage = Image.FromFile("D:\\Campuse\\Mini Project\\Program\\WinForm\\TVFolderCraft\\TVFolderCraft\\TVFolderCraft\\Images\\backgrount.png"),
                BackgroundImageLayout = ImageLayout.Stretch
            };

            // Main title
            titleLabel = new Label
            {
                Text = "Find Your Favorite TV Series",
                Font = new Font("Segoe UI", 28, FontStyle.Bold),
                ForeColor = Color.FromArgb(51, 51, 51),
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.Transparent
            };
            titleLabel.Location = new Point((this.Width - titleLabel.PreferredWidth) / 2, 60);

            // Subtitle
            subtitleLabel = new Label
            {
                Text = "Get detailed information about any TV series instantly",
                Font = new Font("Segoe UI", 11),
                ForeColor = Color.FromArgb(102, 102, 102),
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.Transparent
            };
            subtitleLabel.Location = new Point((this.Width - subtitleLabel.PreferredWidth) / 2, 110);

            // Search Panel
            CreateSearchPanel();

            // Trending Series Section
            trendingLabel = new Label
            {
                Text = "Trending Series 🔥",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = Color.FromArgb(51, 51, 51),
                AutoSize = true
            };
            trendingLabel.Location = new Point((this.Width - trendingLabel.PreferredWidth) / 2, 220);

            // Create trending series cards
            CreateTrendingSeriesPanel();

            // Create search tips panel
            CreateSearchTipsPanel();

            // add component to content panal
            contentPanel.Controls.AddRange(new Control[] {
                titleLabel, subtitleLabel, searchPanel, trendingLabel, trendingSeriesPanel, searchTipsPanel
            });

            this.Controls.Add(contentPanel);
        }
        //create searchPanal
        private void CreateSearchPanel()
        {
            searchPanel = new Panel
            {
                Size = new Size(600, 50),
                BackColor = Color.Transparent
            };
            searchPanel.Location = new Point((this.Width - searchPanel.Width) / 2, 150);

            // Search TextBox
            searchTextBox = new TextBox
            {
                Font = new Font("Segoe UI", 12),
                Size = new Size(450, 35),
                Location = new Point(0, 8),
                BorderStyle = BorderStyle.FixedSingle,
                Text = "Enter series title to begin...",
                ForeColor = Color.Gray
            };

            // Search Button
            searchButton = CreateModernButton("🔍 Find Series", Color.FromArgb(70, 130, 250), Color.White);
            searchButton.Size = new Size(130, 35);
            searchButton.Location = new Point(460, 8);

            // Add event handlers
            searchTextBox.Enter += SearchTextBox_Enter;
            searchTextBox.Leave += SearchTextBox_Leave;
            searchTextBox.KeyPress += SearchTextBox_KeyPress;
            searchButton.Click += SearchButton_Click;

            searchPanel.Controls.AddRange(new Control[] { searchTextBox, searchButton });
        }
        // find cuttent trading tv info
        private async Task FindData()
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                try
                {
                    string response = await client.GetStringAsync(url);
                    JObject data = JObject.Parse(response);
                    JArray results = (JArray)data["results"];

                    int count = results.Count;
                    int j = 0;

                    for (int i = count - 5; i < count; i++)
                    {
                        string name = results[i]["name"]?.ToString();
                        string posterPath = results[i]["poster_path"]?.ToString();
                        string imageUrl = posterPath == null
                            ? "https://via.placeholder.com/200x300?text=No+Image"
                            : "https://image.tmdb.org/t/p/w200" + posterPath;

                        seriesNames[j] = name;
                        imagePath[j] = imageUrl;
                        j++;
                    }
                }
                catch (HttpRequestException)
                {
                    DialogResult check = MessageBox.Show("Please  Check your internet connection \n Do you want Enter Manualy", "Internet Error",MessageBoxButtons.YesNo,MessageBoxIcon.Hand);

                    if(check == DialogResult.Yes)
                    {
                        NavigateToGanaratePage();
                    }
                }
            }
        }
        // filling trending tv series data  
        private async void CreateTrendingSeriesPanel()
        {
            
            trendingSeriesPanel = new Panel
            {
                Size = new Size(900, 150),
                BackColor = Color.Transparent
            };
            trendingSeriesPanel.Location = new Point((this.Width - trendingSeriesPanel.Width) / 2, 260);

            Color[] cardColors = {
                Color.Transparent,
                Color.Transparent,
                Color.Transparent,
                Color.Transparent,
                Color.Transparent
            };

            for (int i = 0; i < 5; i++)
            {
                var card = CreateSeriesCard(seriesNames[i], cardColors[i], new Point(i * 175, 10), imagePath[i]);
                trendingSeriesPanel.Controls.Add(card);
            }
        }
        //create seriesCard for dispaling Images
        private Panel CreateSeriesCard(string seriesName, Color cardColor, Point location, string imagePath)
        {
            var card = new Panel
            {
                Size = new Size(160, 160),
                Location = location,
                BackColor = cardColor,
                Cursor = Cursors.Hand,
                BorderStyle = BorderStyle.FixedSingle,
                BackgroundImageLayout = ImageLayout.Stretch
            };

            // load image asynchronously to avoid blocking UI
            LoadImageAsync(card, imagePath);

            var nameLabel = new Label
            {
                Text = seriesName,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = cardColor == Color.FromArgb(66, 66, 66) ? Color.White : Color.Black,
                AutoSize = false,
                Size = new Size(0, 0),
                Location = new Point(5, 55),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.Transparent
            };

            // Hover effects
            card.MouseEnter += (s, e) =>
            {
                card.BackColor = Color.FromArgb(Math.Min(255, cardColor.R + 20),
                                              Math.Min(255, cardColor.G + 20),
                                              Math.Min(255, cardColor.B + 20));
                card.Invalidate();
            };
            card.MouseLeave += (s, e) =>
            {
                card.BackColor = cardColor;
                card.Invalidate();
            };

            // Click event
            card.Click += (s, e) => SearchSeries(seriesName);
            nameLabel.Click += (s, e) => SearchSeries(seriesName);

            card.Controls.Add(nameLabel);
            return card;
        }
        // helper method to load images asynchronously
        private async void LoadImageAsync(Panel card, string imageUrl)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    byte[] imageBytes = await client.GetByteArrayAsync(imageUrl);
                    using (var ms = new MemoryStream(imageBytes))
                    {
                        card.BackgroundImage = Image.FromStream(ms);
                    }
                }
            }
            catch (Exception ex)
            {
                //loading error 
                Console.WriteLine($"Failed to load image: {ex.Message}");
            }
        }
        //create search tips panal
        private void CreateSearchTipsPanel()
        {
            searchTipsPanel = new Panel
            {
                Size = new Size(800, 100),
                BackColor = Color.FromArgb(165, 243, 252),
            };
            searchTipsPanel.Location = new Point((this.Width - searchTipsPanel.Width) / 2, 450);

            var tipsHeaderLabel = new Label
            {
                Text = "Search Tips",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.FromArgb(51, 51, 51),
                Location = new Point(20, 15),
                AutoSize = true,
                BackColor = Color.Transparent
            };

            var tip1 = new Label
            {
                Text = "• Enter the complete series name for best results",
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.FromArgb(51, 51, 51),
                Location = new Point(20, 45),
                AutoSize = true,
                BackColor = Color.Transparent
            };

            var tip2 = new Label
            {
                Text = "• Try both original and international titles",
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.FromArgb(51, 51, 51),
                Location = new Point(20, 65),
                AutoSize = true,
                BackColor = Color.Transparent
            };

            var tip3 = new Label
            {
                Text = "• Check spelling if no results appear",
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.FromArgb(51, 51, 51),
                Location = new Point(450, 45),
                AutoSize = true,
                BackColor = Color.Transparent
            };

            var tip4 = new Label
            {
                Text = "• Check Your Internet connection",
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.FromArgb(51, 51, 51),
                Location = new Point(450, 65),
                AutoSize = true,
                BackColor = Color.Transparent
            };

            searchTipsPanel.Controls.AddRange(new Control[] { tipsHeaderLabel, tip1, tip2, tip3, tip4 });
        }
        //create buttons 
        private Button CreateModernButton(string text, Color backColor, Color foreColor)
        {
            var button = new Button
            {
                Text = text,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = foreColor,
                BackColor = backColor,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                TextAlign = ContentAlignment.MiddleCenter
            };

            button.FlatAppearance.BorderSize = 0;
            button.FlatAppearance.MouseOverBackColor = Color.FromArgb(60, 120, 240);

            return button;
        }

        // check user enter or not series name
        private void SearchTextBox_Enter(object sender, EventArgs e)
        {
            if (searchTextBox.Text == "Enter series title to begin...")
            {
                searchTextBox.Text = "";
                searchTextBox.ForeColor = Color.Black;
            }
        }
        private void SearchTextBox_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(searchTextBox.Text))
            {
                searchTextBox.Text = "Enter series title to begin...";
                searchTextBox.ForeColor = Color.Gray;
            }
        }
        private void SearchTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                SearchButton_Click(sender, e);
            }
        }

        //search button click to search
        private void SearchButton_Click(object sender, EventArgs e)
        {
            if (searchTextBox.Text != "Enter series title to begin..." && !string.IsNullOrWhiteSpace(searchTextBox.Text))
            {
                SearchSeries(searchTextBox.Text);
            }
        }
        //searching time
        private async void SearchSeries(string seriesName)
        {
            try
            {
                // show loading message
                this.Cursor = Cursors.WaitCursor;
                searchButton.Text = "Searching...";
                searchButton.Enabled = false;

                // check internet connectivity
                if (!await IsInternetConnectedAsync())
                {
                    DialogResult dialogResult = MessageBox.Show("❌ No Internet Connection\n\nPlease check your internet connection and try again.\n\nDo You Want Enter Details Manually.",
                                                "Connection Error", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);

                    if (dialogResult == DialogResult.Yes)
                    {
                        NavigateToGanaratePage();
                    }
                    return;
                }

                //try TMDB api first if key exists,else TVMaze as default
                TVSeriesData tvSeriesData = null;

                if (HasValidTMDBKey())
                {
                    tvSeriesData = await SearchTMDBAPI(seriesName);

                    // If TMDB search fails pass to TVMaze search
                    if (tvSeriesData == null)
                    {
                        tvSeriesData = await SearchTVMazeAPI(seriesName);
                    }
                }
                else
                {
                    // No TMDB key, use TVMaze key 
                    tvSeriesData = await SearchTVMazeAPI(seriesName);
                }
                // display error message if cant find user enter name
                if (tvSeriesData == null)
                {
                    MessageBox.Show($"❌ No Results Found\n\nNo TV series found matching '{seriesName}'.\n\nPlease check the spelling and try again.",
                                   "Search Results", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                //display results
                await DisplayEnhancedSearchResults(tvSeriesData);
            }
            //display error of interner connection
            catch (HttpRequestException)
            {
                MessageBox.Show("❌ Network Error\n\nFailed to connect to the TV database.\nPlease check your internet connection and try again.",
                               "Network Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            //display error some other thing 
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Error\n\nAn unexpected error occurred:\n{ex.Message}",
                               "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            //reset ui
            finally
            {
                this.Cursor = Cursors.Default;
                searchButton.Text = "🔍 Find Series";
                searchButton.Enabled = true;
            }
        }
        //searching default api
        private async Task<TVSeriesData> SearchTVMazeAPI(string seriesName)
        {
            try
            {
                string encodedName = Uri.EscapeDataString(seriesName);
                string apiUrl = $"https://api.tvmaze.com/search/shows?q={encodedName}";

                var response = await httpClient.GetStringAsync(apiUrl);
                var searchResults = JsonConvert.DeserializeObject<TVShowSearchResult[]>(response);

                if (searchResults?.Length > 0)
                {
                    var show = searchResults[0].show;

                    // try multiple time to get episode count
                    var seasons = await GetSeasonsWithEpisodesAsync(show.id);
                    int totalEpisodes = 0;
                    int totalSeasons = seasons?.Length ?? 0;

                    //count episodes from seasons
                    if (seasons != null)
                    {
                        totalEpisodes = seasons.Sum(s => s.episodes?.Length ?? 0);

                        // if no episodes found via seasons, try episode order
                        if (totalEpisodes == 0)
                        {
                            totalEpisodes = seasons.Sum(s => s.episodeOrder);
                        }
                    }

                    //if still count = 0,try getting episodes directly
                    if (totalEpisodes == 0)
                    {
                        try
                        {
                            string directEpisodesUrl = $"https://api.tvmaze.com/shows/{show.id}/episodes";
                            var directEpisodesResponse = await httpClient.GetStringAsync(directEpisodesUrl);
                            var directEpisodes = JsonConvert.DeserializeObject<Episode[]>(directEpisodesResponse);

                            if (directEpisodes != null)
                            {
                                totalEpisodes = directEpisodes.Length;

                                // update season count based on episodes if needed
                                if (totalSeasons == 0)
                                {
                                    totalSeasons = directEpisodes.GroupBy(e => e.season).Count();
                                }

                                // update seasons array with episodes grouped by season
                                if (seasons == null || seasons.Length == 0)
                                {
                                    var seasonGroups = directEpisodes.GroupBy(e => e.season).OrderBy(g => g.Key);
                                    seasons = seasonGroups.Select(g => new Season
                                    {
                                        number = g.Key,
                                        episodes = g.ToArray(),
                                        episodeOrder = g.Count()
                                    }).ToArray();
                                }
                            }
                        }
                        //error if cant find episodes
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Could not fetch direct episodes: {ex.Message}");
                        }
                    }
                    //return to TVSeriesData class to that data
                    return new TVSeriesData
                    {
                        Name = show.name,
                        TotalSeasons = totalSeasons,
                        TotalEpisodes = totalEpisodes,
                        Status = show.status,
                        Genres = show.genres,
                        Summary = show.summary,
                        Premiered = show.premiered,
                        Seasons = seasons
                    };
                }
            }
            //dispaly api error
            catch (Exception ex)
            {
                Console.WriteLine($"TVMaze API error: {ex.Message}");
            }
            return null;
        }
        //display more information about searched tv sereas name
        private async Task DisplayEnhancedSearchResults(TVSeriesData seriesData)
        {
            //create form
            var resultForm = new Form
            {
                Text = $"Enhanced Search Results - {seriesData.Name}",
                Size = new Size(800, 700), 
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false,
                BackColor = Color.FromArgb(240, 242, 245)
            };

            var panel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(20),
                AutoScroll = true
            };

            int yPos = 0;

            // title
            var titleLabel = new Label
            {
                Text = $"📺 {seriesData.Name}",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.FromArgb(51, 51, 51),
                AutoSize = true,
                Location = new Point(0, yPos)
            };
            panel.Controls.Add(titleLabel);
            yPos += 35;

            // data source info
            var sourceLabel = new Label
            {
                Text = HasValidTMDBKey() ? "📊 Data Source: TMDB API" : "📊 Data Source: TVMaze API",
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.FromArgb(70, 130, 250),
                AutoSize = true,
                Location = new Point(0, yPos)
            };
            panel.Controls.Add(sourceLabel);
            yPos += 25;

            // enhanced season/episode info
            var seasonEpisodeLabel = new Label
            {
                Text = $"🎬 Total Seasons: {seriesData.TotalSeasons} | Total Episodes: {seriesData.TotalEpisodes}",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.FromArgb(70, 130, 250),
                AutoSize = true,
                Location = new Point(0, yPos)
            };
            panel.Controls.Add(seasonEpisodeLabel);
            yPos += 35;

            // status
            if (!string.IsNullOrEmpty(seriesData.Status))
            {
                var statusLabel = new Label
                {
                    Text = $"Status: {seriesData.Status}",
                    Font = new Font("Segoe UI", 10),
                    ForeColor = Color.FromArgb(102, 102, 102),
                    AutoSize = true,
                    Location = new Point(0, yPos)
                };
                panel.Controls.Add(statusLabel);
                yPos += 20;
            }

            //Genres
            if (seriesData.Genres?.Length > 0)
            {
                var genresLabel = new Label
                {
                    Text = $"Genres: {string.Join(", ", seriesData.Genres)}",
                    Font = new Font("Segoe UI", 10),
                    ForeColor = Color.FromArgb(102, 102, 102),
                    AutoSize = true,
                    Location = new Point(0, yPos)
                };
                panel.Controls.Add(genresLabel);
                yPos += 20;
            }

            //premiered
            if (!string.IsNullOrEmpty(seriesData.Premiered))
            {
                var premieredLabel = new Label
                {
                    Text = $"Premiered: {seriesData.Premiered}",
                    Font = new Font("Segoe UI", 10),
                    ForeColor = Color.FromArgb(102, 102, 102),
                    AutoSize = true,
                    Location = new Point(0, yPos)
                };
                panel.Controls.Add(premieredLabel);
                yPos += 25;
            }

            // summary 
            if (!string.IsNullOrEmpty(seriesData.Summary))
            {
                var summaryLabel = new Label
                {
                    Text = "Summary:",
                    Font = new Font("Segoe UI", 12, FontStyle.Bold),
                    ForeColor = Color.FromArgb(51, 51, 51),
                    AutoSize = true,
                    Location = new Point(0, yPos)
                };
                panel.Controls.Add(summaryLabel);
                yPos += 25;

                string cleanSummary = System.Text.RegularExpressions.Regex.Replace(seriesData.Summary, "<.*?>", "");
                if (cleanSummary.Length > 400)
                {
                    cleanSummary = cleanSummary.Substring(0, 400) + "...";
                }

                var summaryText = new Label
                {
                    Text = cleanSummary,
                    Font = new Font("Segoe UI", 10),
                    ForeColor = Color.FromArgb(102, 102, 102),
                    Size = new Size(720, 100),
                    Location = new Point(0, yPos)
                };
                panel.Controls.Add(summaryText);
                yPos += 110;
            }

            //enhanced season and episode details
            if (seriesData.Seasons?.Length > 0)
            {
                var seasonDetailsLabel = new Label
                {
                    Text = "📋 Detailed Season & Episode Information:",
                    Font = new Font("Segoe UI", 12, FontStyle.Bold),
                    ForeColor = Color.FromArgb(51, 51, 51),
                    AutoSize = true,
                    Location = new Point(0, yPos)
                };
                panel.Controls.Add(seasonDetailsLabel);
                yPos += 30;

                foreach (var season in seriesData.Seasons.Take(10)) // limit to first 10 seasons
                {
                    // season Header
                    var seasonHeader = new Label
                    {
                        Text = $"🎭 Season {season.number}",
                        Font = new Font("Segoe UI", 11, FontStyle.Bold),
                        ForeColor = Color.FromArgb(70, 130, 250),
                        AutoSize = true,
                        Location = new Point(0, yPos)
                    };
                    panel.Controls.Add(seasonHeader);
                    yPos += 25;

                    // season info
                    string seasonInfo = "";
                    if (season.episodes?.Length > 0)
                    {
                        seasonInfo = $"Episodes: {season.episodes.Length}";
                    }
                    else if (season.episodeOrder > 0)
                    {
                        seasonInfo = $"Episodes: {season.episodeOrder}";
                    }

                    if (!string.IsNullOrEmpty(season.premiereDate))
                    {
                        seasonInfo += $" | Aired: {season.premiereDate}";
                    }

                    if (!string.IsNullOrEmpty(seasonInfo))
                    {
                        var seasonInfoLabel = new Label
                        {
                            Text = $"   {seasonInfo}",
                            Font = new Font("Segoe UI", 9),
                            ForeColor = Color.FromArgb(102, 102, 102),
                            AutoSize = true,
                            Location = new Point(10, yPos)
                        };
                        panel.Controls.Add(seasonInfoLabel);
                        yPos += 18;
                    }

                    // episode details if available
                    if (season.episodes?.Length > 0)
                    {
                        var episodeHeaderLabel = new Label
                        {
                            Text = "   📺 Episodes:",
                            Font = new Font("Segoe UI", 10, FontStyle.Bold),
                            ForeColor = Color.FromArgb(85, 85, 85),
                            AutoSize = true,
                            Location = new Point(10, yPos)
                        };
                        panel.Controls.Add(episodeHeaderLabel);
                        yPos += 20;

                        // show first 5 episodes per season
                        foreach (var episode in season.episodes.Take(5))
                        {
                            string episodeText = $"      • Episode {episode.number}: {episode.name}";
                            if (!string.IsNullOrEmpty(episode.airdate))
                            {
                                episodeText += $" (Aired: {episode.airdate})";
                            }
                            if (episode.runtime > 0)
                            {
                                episodeText += $" - {episode.runtime} min";
                            }

                            var episodeLabel = new Label
                            {
                                Text = episodeText,
                                Font = new Font("Segoe UI", 8),
                                ForeColor = Color.FromArgb(119, 119, 119),
                                Size = new Size(700, 15),
                                Location = new Point(20, yPos)
                            };
                            panel.Controls.Add(episodeLabel);
                            yPos += 16;
                        }

                        // show "more episodes" if there are more than 5
                        if (season.episodes.Length > 5)
                        {
                            var moreEpisodesLabel = new Label
                            {
                                Text = $"      ... and {season.episodes.Length - 5} more episodes",
                                Font = new Font("Segoe UI", 8, FontStyle.Italic),
                                ForeColor = Color.FromArgb(150, 150, 150),
                                AutoSize = true,
                                Location = new Point(20, yPos)
                            };
                            panel.Controls.Add(moreEpisodesLabel);
                            yPos += 16;
                        }
                    }

                    yPos += 10; // maintens space between seasons
                }

                // show "more seasons" if there are more than 10
                if (seriesData.Seasons.Length > 10)
                {
                    var moreSeasonsLabel = new Label
                    {
                        Text = $"... and {seriesData.Seasons.Length - 10} more seasons",
                        Font = new Font("Segoe UI", 10, FontStyle.Italic),
                        ForeColor = Color.FromArgb(150, 150, 150),
                        AutoSize = true,
                        Location = new Point(0, yPos)
                    };
                    panel.Controls.Add(moreSeasonsLabel);
                    yPos += 25;
                }
            }

            // add a question label above buttons
            var questionLabel = new Label
            {
                Text = "Do you want to proceed with this TV series data?",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.FromArgb(51, 51, 51),
                AutoSize = true,
                Location = new Point(0, yPos + 10)
            };
            panel.Controls.Add(questionLabel);

            // oK Button
            var okButton = new Button
            {
                Text = "✅ OK",
                Size = new Size(100, 35),
                Location = new Point(520, yPos + 40),
                BackColor = Color.FromArgb(76, 175, 80),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            okButton.FlatAppearance.BorderSize = 0;
            okButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(56, 155, 60);

            // no Button
            var noButton = new Button
            {
                Text = "❌ No",
                Size = new Size(100, 35),
                Location = new Point(630, yPos + 40),
                BackColor = Color.FromArgb(244, 67, 54),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            noButton.FlatAppearance.BorderSize = 0;
            noButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(224, 47, 34);

            // button event handlers
            okButton.Click += async (s, e) => {
                try
                {
                    // show loading cursor
                    this.Cursor = Cursors.WaitCursor;
                    okButton.Enabled = false;
                    okButton.Text = "Creating Folders...";

                    // create folder structure
                    string result1 = await CreateTVSeriesFolderStructure(seriesData);

                    if (!string.IsNullOrEmpty(result1))
                    {
                        HistoryManager.AddInternetHistoryItem(seriesData.Name, result1);

                        // success message with folder path
                        MessageBox.Show($"✅ Success!\n\nTV Series folder structure created successfully!\n\nLocation: {result1}\n\nYou can now organize your {seriesData.Name} episodes in the created folders.",
                                       "Folders Created Successfully", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("❌ Error creating folder structure. Please check permissions and try again.",
                                       "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    resultForm.DialogResult = DialogResult.OK;
                    resultForm.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"❌ Error creating folders:\n\n{ex.Message}",
                                   "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    //reset UI
                    this.Cursor = Cursors.Default;
                    okButton.Enabled = true;
                    okButton.Text = "✅ OK";
                }
            };

            noButton.Click += (s, e) => {
                //handle No button
                MessageBox.Show("❌ Operation cancelled. You can search for another TV series.",
                               "Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information);

                resultForm.DialogResult = DialogResult.No;
                resultForm.Close();
            };

            panel.Controls.Add(okButton);
            panel.Controls.Add(noButton);

            resultForm.Controls.Add(panel);

            //show the dialog and get the result
            DialogResult result = resultForm.ShowDialog(this);

        }
        //create folders
        private async Task<string> CreateTVSeriesFolderStructure(TVSeriesData seriesData)
        {
            return await Task.Run(() =>
            {
                try
                {
                    // get downloads folder path
                    string downloadsPath = Properties.Settings.Default.DefaultDownloadPath;

                    // remove invalid characters in series name
                    string cleanSeriesName = CleanFileName(seriesData.Name);

                    // create main tV series folder
                    string mainFolderPath = Path.Combine(downloadsPath, cleanSeriesName);
                    Directory.CreateDirectory(mainFolderPath);

                    // create seasons and episodes folders
                    if (seriesData.Seasons != null && seriesData.Seasons.Length > 0)
                    {
                        // create folders based on detailed season data
                        foreach (var season in seriesData.Seasons)
                        {
                            string seasonFolderName = $"Season {season.number:D2}";
                            string seasonPath = Path.Combine(mainFolderPath, seasonFolderName);
                            Directory.CreateDirectory(seasonPath);

                            // create episode folders
                            if (season.episodes != null && season.episodes.Length > 0)
                            {
                                // create folders for each episode
                                foreach (var episode in season.episodes)
                                {
                                    string cleanEpisodeName = CleanFileName(episode.name ?? $"Episode {episode.number}");
                                    string episodeFolderName = $"E{episode.number:D2} - {cleanEpisodeName}";
                                    string episodePath = Path.Combine(seasonPath, episodeFolderName);
                                    Directory.CreateDirectory(episodePath);
                                }
                            }
                            else if (season.episodeOrder > 0)
                            {
                                // create episode folders based on episode order count
                                for (int i = 1; i <= season.episodeOrder; i++)
                                {
                                    string episodeFolderName = $"E{i:D2} - Episode {i}";
                                    string episodePath = Path.Combine(seasonPath, episodeFolderName);
                                    Directory.CreateDirectory(episodePath);
                                }
                            }
                        }
                    }
                    else if (seriesData.TotalSeasons > 0)
                    {
                        // default create folders based on total season count 
                        for (int seasonNum = 1; seasonNum <= seriesData.TotalSeasons; seasonNum++)
                        {
                            string seasonFolderName = $"Season {seasonNum:D2}";
                            string seasonPath = Path.Combine(mainFolderPath, seasonFolderName);
                            Directory.CreateDirectory(seasonPath);

                            //estimate episodes per season if total episodes available
                            int episodesPerSeason = seriesData.TotalEpisodes > 0 ?
                                (int)Math.Ceiling((double)seriesData.TotalEpisodes / seriesData.TotalSeasons) : 10;

                            // create episode folders
                            for (int episodeNum = 1; episodeNum <= episodesPerSeason; episodeNum++)
                            {
                                string episodeFolderName = $"E{episodeNum:D2} - Episode {episodeNum}";
                                string episodePath = Path.Combine(seasonPath, episodeFolderName);
                                Directory.CreateDirectory(episodePath);
                            }
                        }
                    }

                    // create a readme text file with series information
                    CreateSeriesInfoFile(mainFolderPath, seriesData);

                    return mainFolderPath;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error creating folder structure: {ex.Message}");
                    return null;
                }
            });
        }
        //remove unnesesey thing in filename
        private string CleanFileName(string filename)
        {
            if (string.IsNullOrEmpty(filename))
                return "Unknown";

            //remove HTML tags if present
            filename = System.Text.RegularExpressions.Regex.Replace(filename, "<.*?>", "");

            //remove or replace invalid characters
            char[] invalidChars = Path.GetInvalidFileNameChars();
            foreach (char c in invalidChars)
            {
                filename = filename.Replace(c, '_');
            }

            //remove additional problematic characters
            filename = filename.Replace(":", " -")
                             .Replace("?", "")
                             .Replace("*", "")
                             .Replace("\"", "'")
                             .Replace("|", "-")
                             .Replace("/", "-")
                             .Replace("\\", "-");

            //trim and limit length
            filename = filename.Trim();
            if (filename.Length > 100)
            {
                filename = filename.Substring(0, 100).Trim();
            }

            return filename;
        }
        //create series info
        private void CreateSeriesInfoFile(string folderPath, TVSeriesData seriesData)
        {
            try
            {
                string infoFilePath = Path.Combine(folderPath, "Series_Info.txt");

                var infoContent = new StringBuilder();
                infoContent.AppendLine($"TV Series: {seriesData.Name}");
                infoContent.AppendLine($"Created: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                infoContent.AppendLine(new string('=', 50));
                infoContent.AppendLine($"Total Seasons: {seriesData.TotalSeasons}");
                infoContent.AppendLine($"Total Episodes: {seriesData.TotalEpisodes}");

                if (!string.IsNullOrEmpty(seriesData.Status))
                    infoContent.AppendLine($"Status: {seriesData.Status}");

                if (seriesData.Genres?.Length > 0)
                    infoContent.AppendLine($"Genres: {string.Join(", ", seriesData.Genres)}");

                if (!string.IsNullOrEmpty(seriesData.Premiered))
                    infoContent.AppendLine($"Premiered: {seriesData.Premiered}");

                if (!string.IsNullOrEmpty(seriesData.Summary))
                {
                    string cleanSummary = System.Text.RegularExpressions.Regex.Replace(seriesData.Summary, "<.*?>", "");
                    infoContent.AppendLine($"\nSummary:\n{cleanSummary}");
                }

                if (seriesData.Seasons?.Length > 0)
                {
                    infoContent.AppendLine("\nSeason Details:");
                    infoContent.AppendLine(new string('-', 30));

                    foreach (var season in seriesData.Seasons)
                    {
                        infoContent.AppendLine($"\nSeason {season.number}:");
                        if (season.episodes?.Length > 0)
                        {
                            infoContent.AppendLine($"  Episodes: {season.episodes.Length}");
                            foreach (var episode in season.episodes.Take(5)) // show first 5 episodes
                            {
                                infoContent.AppendLine($"    E{episode.number:D2}: {episode.name}");
                            }
                            if (season.episodes.Length > 5)
                            {
                                infoContent.AppendLine($"    ... and {season.episodes.Length - 5} more episodes");
                            }
                        }
                        else if (season.episodeOrder > 0)
                        {
                            infoContent.AppendLine($"  Episodes: {season.episodeOrder}");
                        }
                    }
                }

                File.WriteAllText(infoFilePath, infoContent.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating info file: {ex.Message}");
            }
        }
        //if intenret connection oky
        private async Task<bool> IsInternetConnectedAsync()
        {
            try
            {
                //method 1 ping google DNS
                using (var ping = new Ping())
                {
                    var reply = await ping.SendPingAsync("8.8.8.8", 3000);
                    if (reply.Status == IPStatus.Success)
                        return true;
                }

                //method 2 try HTTP request as backup
                using (var client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromSeconds(5);
                    var response = await client.GetAsync("https://www.google.com");
                    return response.IsSuccessStatusCode;
                }
            }
            catch
            {
                return false;
            }
        }
        //get seasons with episodes 
        private async Task<Season[]> GetSeasonsWithEpisodesAsync(int showId)
        {
            try
            {
                // get seasons
                string seasonsApiUrl = $"https://api.tvmaze.com/shows/{showId}/seasons";
                var seasonsResponse = await httpClient.GetStringAsync(seasonsApiUrl);
                var seasons = JsonConvert.DeserializeObject<Season[]>(seasonsResponse) ?? new Season[0];

                // get episodes for each season
                foreach (var season in seasons)
                {
                    try
                    {
                        string episodesApiUrl = $"https://api.tvmaze.com/seasons/{season.id}/episodes";
                        var episodesResponse = await httpClient.GetStringAsync(episodesApiUrl);
                        season.episodes = JsonConvert.DeserializeObject<Episode[]>(episodesResponse) ?? new Episode[0];
                    }
                    catch
                    {
                        // if episodes fail for this season, continue with others
                        season.episodes = new Episode[0];
                    }
                }

                return seasons;
            }
            catch
            {
                return new Season[0];
            }
        }
        //make sure to dispose HttpClient when form closes
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            httpClient?.Dispose();
            base.OnFormClosed(e);
        }
        //resize when form resize
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (contentPanel != null)
            {
                //recenter elements when form is resized
                if (titleLabel != null)
                    titleLabel.Location = new Point((this.Width - titleLabel.PreferredWidth) / 2, 60);
                if (subtitleLabel != null)
                    subtitleLabel.Location = new Point((this.Width - subtitleLabel.PreferredWidth) / 2, 110);
                if (searchPanel != null)
                    searchPanel.Location = new Point((this.Width - searchPanel.Width) / 2, 150);
                if (trendingLabel != null)
                    trendingLabel.Location = new Point((this.Width - trendingLabel.PreferredWidth) / 2, 220);
                if (trendingSeriesPanel != null)
                    trendingSeriesPanel.Location = new Point((this.Width - trendingSeriesPanel.Width) / 2, 260);
                if (searchTipsPanel != null)
                    searchTipsPanel.Location = new Point((this.Width - searchTipsPanel.Width) / 2, 400);
            }
        }
        //insernetPage load
        private async void SearchForm_Load(object sender, EventArgs e) {}
        //navigate manuwal entry if internet connection lost
        private void NavigateToGanaratePage()
        {
            this.Hide();
            var mainForm = new GanaratePage();
            mainForm.Show();
            mainForm.FormClosed += (s, e) => this.Close();

        }
        //TMDB methods
        private async Task<TVSeriesData> SearchTMDBAPI(string seriesName)
        {
            try
            {
                //get my default api 
                string apiKey = Properties.Settings.Default.ApiKey;
                if (string.IsNullOrEmpty(apiKey))
                {
                    return null; // pass to TVMaze
                }
                string encodedName = Uri.EscapeDataString(seriesName);
                string searchUrl = $"https://api.themoviedb.org/3/search/tv?api_key={apiKey}&query={encodedName}";
                var response = await httpClient.GetStringAsync(searchUrl);
                var searchResults = JsonConvert.DeserializeObject<TMDBShowSearchResult>(response);

                if (searchResults?.results?.Count > 0)
                {
                    var show = searchResults.results[0];

                    //get detailed show information including seasons
                    string detailsUrl = $"https://api.themoviedb.org/3/tv/{show.id}?api_key={apiKey}";
                    var detailsResponse = await httpClient.GetStringAsync(detailsUrl);
                    var detailedShow = JsonConvert.DeserializeObject<TMDBShow>(detailsResponse);

                    //get seasons with episodes
                    var tmdbSeasons = await GetTMDBSeasonsWithEpisodesAsync(show.id);

                    //convert TMDB data to your TVSeriesData format
                    return new TVSeriesData
                    {
                        Name = detailedShow.name,
                        TotalSeasons = detailedShow.number_of_seasons,
                        TotalEpisodes = detailedShow.number_of_episodes,
                        Status = detailedShow.status,
                        Genres = detailedShow.genres,
                        Summary = detailedShow.overview,
                        Premiered = detailedShow.first_air_date,
                        Seasons = tmdbSeasons
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"TMDB API error: {ex.Message}");
            }
            return null;
        }
        private async Task<Season[]> GetTMDBSeasonsWithEpisodesAsync(int showId)
        {
            try
            {
                string apiKey = Properties.Settings.Default.ApiKey;
                if (string.IsNullOrEmpty(apiKey))
                {
                    return new Season[0];
                }
                var seasonsList = new List<Season>();

                //first get the show details to know how many seasons there are
                string showDetailsUrl = $"https://api.themoviedb.org/3/tv/{showId}?api_key={apiKey}";
                var showResponse = await httpClient.GetStringAsync(showDetailsUrl);
                var show = JsonConvert.DeserializeObject<TMDBShow>(showResponse);

                if (show.seasons != null)
                {
                    foreach (var tmdbSeason in show.seasons)
                    {
                        try
                        {

                            //get episodes for each season
                            string seasonUrl = $"https://api.themoviedb.org/3/tv/{showId}/season/{tmdbSeason.season_number}?api_key={apiKey}";
                            var seasonResponse = await httpClient.GetStringAsync(seasonUrl);
                            var seasonDetails = JsonConvert.DeserializeObject<TMDBSeasonDetails>(seasonResponse);

                            //convert TMDB episodes to Episode format
                            var episodes = seasonDetails.episodes?.Select(e => new Episode
                            {
                                id = e.id,
                                name = e.name,
                                season = e.season_number,
                                number = e.episode_number,
                                airdate = e.air_date,
                                runtime = e.runtime,
                                summary = e.overview
                            }).ToArray() ?? new Episode[0];

                            var season = new Season
                            {
                                id = tmdbSeason.id,
                                number = tmdbSeason.season_number,
                                name = tmdbSeason.name,
                                episodeOrder = tmdbSeason.episode_count,
                                premiereDate = tmdbSeason.air_date,
                                summary = tmdbSeason.overview,
                                episodes = episodes
                            };

                            seasonsList.Add(season);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error fetching TMDB season {tmdbSeason.season_number}: {ex.Message}");
                            //add season without episodes if episode fetch fails
                            seasonsList.Add(new Season
                            {
                                id = tmdbSeason.id,
                                number = tmdbSeason.season_number,
                                name = tmdbSeason.name,
                                episodeOrder = tmdbSeason.episode_count,
                                premiereDate = tmdbSeason.air_date,
                                summary = tmdbSeason.overview,
                                episodes = new Episode[0]
                            });
                        }
                    }
                }

                return seasonsList.OrderBy(s => s.number).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching TMDB seasons: {ex.Message}");
                return new Season[0];
            }
        }

    }
}
