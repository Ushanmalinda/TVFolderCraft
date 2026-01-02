# TVFolderCraft

A Windows desktop application for organizing and creating TV series folder structures.

## Features

- **Manual TV Series Creator** - Create organized folder structures for TV series with seasons and episodes
- **Series Name & Year** - Specify series information for proper naming
- **Season Management** - Add multiple seasons with customizable episode counts
- **Output Directory Selection** - Choose where to create your folder structure
- **History Tracking** - Keep track of previously created folder structures
- **Internet Integration** - Additional online features
- **Settings** - Customize application behavior

## Screenshots

*Coming soon*

## Requirements

- Windows OS
- .NET Framework 4.x

## Installation

1. Download the latest release
2. Extract the files
3. Run `TVFolderCraft.exe`

## Building from Source

1. Clone the repository

   ```bash
   git clone https://github.com/Ushanmalinda/TVFolderCraft.git
   ```
2. Open `TVFolderCraft.sln` in Visual Studio
3. Restore NuGet packages
4. Build the solution (Ctrl+Shift+B)
5. Run the application (F5)

## Project Structure

```
TVFolderCraft/
├── Form1.cs              # Main application form
├── GanaratePage.cs       # TV series folder generator page
├── HistoryPage.cs        # History tracking page
├── HistoryManager.cs     # History data management
├── InternetPage.cs       # Internet features page
├── SettingPage.cs        # Settings page
├── NavigationHeader.cs   # Navigation header component
├── FooterPanel.cs        # Footer panel component
├── EmailSupport.cs       # Email support functionality
├── DocumentationHelper.cs # Documentation utilities
└── Images/               # Application images
```

## Technologies Used

- C# (.NET Framework)
- Windows Forms
- iText7 (PDF generation)
- Newtonsoft.Json (JSON handling)

## License

*Add your license here*

## Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add some amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## Author

Ushan Malinda

## Acknowledgments

- iText7 for PDF functionality
- Newtonsoft.Json for JSON serialization
